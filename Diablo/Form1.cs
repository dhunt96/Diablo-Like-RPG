using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diablo
{
    public partial class Form1 : Form
    {

        // Note: keyword Events used by C# and/or .Net
        class Event // Action
        {
            string name; // name of event
            int t; // time of event
            Creature c;
            int n; // generic parameter
            // location
            public Event(string Name, int Time, Creature Who, int Value)
            {
                name = Name;
                t = Time;
                c = Who;
                n = Value;
            }
            public string Name
            {
                get { return name; }
            }
            public int Time
            {
                get { return t; }
            }
            public Creature Who // Actor, Participant
            {
                get { return c; }
            }
            public int Value
            {
                get { return n; }
            }
        }



        // Examples: Blessed, Cursed, Flashing/Red, Constrained, Blinded, Frozen, Burning
        class Effect // Condition, Phenomenon, Force, Consequence, State
        {
            string n; // condition name
            //Creature c; // who it is affecting
            int e; // expiration
            public Effect(string Name, int Expiration)
            {
                n = Name;
                //c = Affected;
                e = Expiration;
            }
            public string Name
            {
                get { return n; }
            }
            //public Creature Affected
            //{
            //    get { return c; }
            //}
            public int Expiration
            {
                get { return e; }
            }
        }



        class Ability // Training, Craft, Talent, Competence, Skill
        {
            string skill;
            int proficiency;
            public Ability(string Skill, int Proficiency)
            {
                skill = Skill;
                proficiency = Proficiency;
            }
            public string Skill // Name
            {
                get { return skill; }
            }
            public int Proficiency
            {
                get { return proficiency; }
                set { proficiency = value; }
            }
            // Override ToString?
        }



        class Possession // Item
        {
            string name;
            string category;
            public Possession(string Name, string Category)
            {
                name = Name;
                category = Category;
            }
            public string Name
            {
                get { return name; }
            }
            public string Category
            {
                get { return category; }
            }
        }



        class Armor : Possession
        {
            int protection;
            int encumbrance;
            public Armor(string Name, int Protection, int Encumbrance)
                : base(Name, "Armor")
            {
                protection = Protection;
                encumbrance = Encumbrance;
            }
            public int Protection
            {
                get { return protection; }
            }
            public int Encumbrance
            {
                get { return encumbrance; }
            }
        }



        class Weapon : Possession
        {
            int ld; // low damage
            int hd; // high damage
            int bth; // base to hit = ease of use
            int ws; // weapon speed
            // range?
            // encumbrance
            public Weapon(string Name, int LowDamage, int HighDamage, int BaseToHit, int WeaponSpeed)
                : base(Name, "Weapon")
            {
                ld = LowDamage;
                hd = HighDamage;
                bth = BaseToHit;
                ws = WeaponSpeed;
            }
            public int GetDamage(Random R)
            {
                return R.Next(hd - ld + 1) + ld;
            }
            public int LowDamage
            {
                get { return ld; }
            }
            public int HighDamage
            {
                get { return hd; }
            }
            public int BaseToHit
            {
                get { return bth; }
            }
            public int WeaponSpeed
            {
                get { return ws; }
            }
        }



        class Shield : Possession
        {
            int ease;
            int maxd;
            int encumbrance;
            public Shield(string Name, int EaseOfUse, int MaxParried, int Encumbrance)
                : base(Name, "Shield")
            {
                ease = EaseOfUse;
                maxd = MaxParried;
                encumbrance = Encumbrance;
            }
            public int GetParried(Random R, int QOP)
            {
                return R.Next(maxd - QOP) + QOP + 1;
            }
            public int EaseOfUse
            {
                get { return ease; }
            }
            public int MaxParried
            {
                get { return maxd; }
            }
            public int Encumbrance
            {
                get { return encumbrance; }
            }
        }



        class Object
        {
            Possession p;
            float x;
            float y;
            bool show;
            bool visible;
            public Object(Possession Possession, float X, float Y)
            {
                p = Possession;
                x = X;
                y = Y;
                show = false;
                visible = false;
            }
            public Possession Possession
            {
                get { return p; }
            }
            public float X
            {
                get { return x; }
            }
            public float Y
            {
                get { return y; }
            }
            public bool Show
            {
                get { return show; }
                set { show = value; }
            }
            public bool Visible
            {
                get { return visible; }
                set { visible = value; }
            }
            public void Draw(Graphics g, int Clock)
            {
                // Draw rectangle
                g.FillRectangle(new SolidBrush(Color.Gray), x - 2, y - 2, 5, 5);
                // Show additional information?
                if (show)
                {
                    //System.Drawing.Font font = new System.Drawing.Font("Courier New", 6.0F); // Sans Serif not bad
                    System.Drawing.Font font = new System.Drawing.Font("New Times Roman", 5.0F);
                    SizeF NameSize = g.MeasureString(p.Name, font);
                    float bx = x - NameSize.Width / 2.0F;
                    float by = y - NameSize.Height - 5;
                    g.DrawString(p.Name, font, new SolidBrush(Color.White), new PointF(bx, by));
                }
            } // end method Draw
        }



        class Creature
        {
            string name;
            float x;
            float y;
            Color c;
            float dx;
            float dy;
            float size; // radius of creature
            //float oldx;
            //float oldy;
            bool show;
            float speed;
            bool visible; // true if seen by player
            System.Collections.ArrayList possessions;
            System.Collections.ArrayList skills; // abilities
            System.Collections.ArrayList effects; // auras?
            Creature target;
            Weapon w;
            Shield s;
            Armor a;
            int life;
            int maxlife;
            int alliance;
            // strength
            // agility
            // Constructor
            public Creature(string Name, int Life, int MaxLife, float X, float Y, Color Color, float Speed, int Alliance)
            {
                name = Name;
                x = X;
                y = Y;
                c = Color;
                dx = x;
                dy = y;
                size = 5.0F;
                show = false;
                speed = Speed;
                visible = false;
                possessions = new System.Collections.ArrayList();
                skills = new System.Collections.ArrayList();
                effects = new System.Collections.ArrayList();
                target = null;
                w = null;
                s = null;
                life = Life;
                maxlife = life;
                alliance = Alliance;
            }
            // Interfaces
            public string Name // DEBUG
            {
                get { return name; }
            }
            public float X
            {
                get { return x; }
            }
            public float Y
            {
                get { return y; }
            }
            public Color Color
            {
                get { return c; }
            }
            // DEBUG
            public float DestX
            {
                get { return dx; }
            }
            public float DestY
            {
                get { return dy; }
            }
            // END DEBUG
            public float Size
            {
                get { return size; }
            }
            public bool Show
            {
                get { return show; }
                set { show = value; }
            }
            public bool Visible
            {
                get { return visible; }
                set { visible = value; }
            }
            public float Velocity
            {
                get { return speed; }
            }
            public System.Collections.ArrayList Possessions
            {
                get { return possessions; }
            }
            public System.Collections.ArrayList Skills
            {
                get { return skills; }
            }
            public Armor Armor
            {
                get { return a; }
                set { a = value; }
            }
            public Weapon Weapon
            {
                get { return w; }
                set { w = value; }
            }
            public Shield Shield
            {
                get { return s; }
                set { s = value; }
            }
            public Creature Target
            {
                get { return target; }
                set { target = value; }
            }
            public int Life
            {
                get { return life; }
                set { life = value; }
            }
            public int MaxLife
            {
                get { return maxlife; }
                set { maxlife = value; }
            }
            public int Alliance
            {
                get { return alliance; }
            }
            // Public methods
            public void AddPossession(Possession Item)
            {
                possessions.Add(Item);
                if (Item.Category == "Armor")
                {
                    if (a == null)
                        a = (Armor)Item;
                }
                if (Item.Category == "Weapon")
                {
                    if (w == null)
                        w = (Weapon)Item;
                }
                if (Item.Category == "Shield")
                {
                    if (s == null)
                        s = (Shield)Item;
                }
            }
            public void AddSkill(Ability ability)
            {
                skills.Add(ability);
            }
            public void AddEffect(Effect Effect)
            {
                // check and see if effect already active, then just extend
                effects.Add(Effect);
            }
            public void ClearEffects(int Clock)
            {
                System.Collections.ArrayList ExpiredEffects = new System.Collections.ArrayList();
                foreach (Effect E in effects)
                    if (E.Expiration <= Clock)
                        ExpiredEffects.Add(E);
                foreach (Effect E in ExpiredEffects)
                    effects.Remove(E);
            }
            public int GetProficiency(string Skill)
            {
                foreach (Ability ability in skills)
                {
                    if (ability.Skill == Skill) return ability.Proficiency;
                }
                return 0;
            }
            public void SetDestination(float X, float Y)
            {
                dx = X;
                dy = Y;
            }
            public void SetLocation(float X, float Y)
            {
                x = X;
                y = Y;
            }
            /*
            public void Move() // Move Towards
            {
                oldx = x;
                oldy = y;
                if (target != null)
                {
                    dx = target.X;
                    dy = target.Y;
                }
                double d = Math.Sqrt((x - dx) * (x - dx) + (y - dy) * (y - dy));
                if (d >= speed)
                {
                    float deltax = dx - x;
                    float deltay = dy - y;
                    float m = (float)Math.Sqrt(deltax * deltax + deltay * deltay);
                    deltax /= m;
                    deltay /= m;
                    deltax *= speed;
                    deltay *= speed;
                    x += deltax;
                    y += deltay;
                }
                else
                {
                    x = dx;
                    y = dy;
                }
            }
            */

            private double Distance(float px, float py, Obstacle obstacle)
            {
                // Compute distance from line segment P1 to P2 and creature position = d
                // https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line
                // Vector a-p
                float apx = obstacle.P1.X - px;
                float apy = obstacle.P1.Y - py;
                // Vector nx, ny is vector representing segment
                float nx = obstacle.P2.X - obstacle.P1.X;
                float ny = obstacle.P2.Y - obstacle.P1.Y;
                float m = (float)Math.Sqrt(nx * nx + ny * ny); // length of segment
                // Make n a unit vector
                nx = nx / m;
                ny = ny / m;
                // (a-p)*n
                float dot = apx * nx + apy * ny;
                // projection vector
                float dx = apx - dot * nx;
                float dy = apy - dot * ny;

                float d1 = (float)Math.Sqrt((obstacle.P1.X - px) * (obstacle.P1.X - px) + (obstacle.P1.Y - py) * (obstacle.P1.Y - py));
                float d2 = (float)Math.Sqrt((obstacle.P2.X - px) * (obstacle.P2.X - px) + (obstacle.P2.Y - py) * (obstacle.P2.Y - py));

                if ((d1 < m) && (d2 < m))
                {
                    return Math.Sqrt(dx * dx + dy * dy);
                }
                else
                {
                    return Math.Min(d1, d2);
                }

            }

            private bool LegalMove(float px, float py, System.Collections.ArrayList Infrastructure, System.Collections.ArrayList Creatures)
            {
                // Disallow movement onto obstacles
                foreach (Obstacle obstacle in Infrastructure)
                {
                    if (Distance(px,py, obstacle) < size)
                    {
                        return false;
                    }
                }
                // Disallow movement onto other creatures
                foreach (Creature c1 in Creatures)
                {
                    if (c1 != this)
                    {
                        if (Math.Sqrt((c1.X - px) * (c1.X - px) + (c1.Y - py) * (c1.Y - py)) <= (size + c1.Size))
                        {
                            return false;
                        }
                    }
                }
                // px, py not blocked therefore is legal
                return true;
            }

            public void Move(System.Collections.ArrayList Infrastructure, System.Collections.ArrayList Creatures)
            {
                bool Blocked = true;
                if (target != null)
                {
                    dx = target.X;
                    dy = target.Y;
                }
                double d = Math.Sqrt((x - dx) * (x - dx) + (y - dy) * (y - dy));
                if (d >= speed)
                {
                    float deltax = dx - x;
                    float deltay = dy - y;
                    float m = (float)Math.Sqrt(deltax * deltax + deltay * deltay);
                    deltax /= m;
                    deltay /= m;
                    deltax *= speed;
                    deltay *= speed;
                    float px = x + deltax;
                    float py = y + deltay;
                    if (LegalMove(px, py, Infrastructure, Creatures))
                    {
                        x = px;
                        y = py;
                        Blocked = false;
                    }
                }
                else
                {
                    if (LegalMove(dx, dy, Infrastructure, Creatures))
                    {
                        x = dx;
                        y = dy;
                        Blocked = false;
                    }
                }
                if (!LegalMove(x, y, Infrastructure, Creatures))
                {
                    MessageBox.Show("Illegal Location!");
                }
                if (Blocked)
                {
                    double Course = Math.Atan2(y - dy, dx - x);
                    double ProposedCourse;
                    for (int i = 1; i <= 8; i++)
                    {
                        ProposedCourse = Course + i * 11.0 * Math.PI / 180.0;
                        float px = x + speed * (float)Math.Cos(ProposedCourse);
                        float py = y + speed * (float)Math.Sin(ProposedCourse);
                        if (LegalMove(px, py, Infrastructure, Creatures))
                        {
                            x = px;
                            y = py;
                            return;
                        }
                        ProposedCourse = Course - i * 11.0 * Math.PI / 180.0;
                        px = x + speed * (float)Math.Cos(ProposedCourse);
                        py = y + speed * (float)Math.Sin(ProposedCourse);
                        if (LegalMove(px, py, Infrastructure, Creatures))
                        {
                            x = px;
                            y = py;
                            return;
                        }
                    }
                }
            }


            public void Move(double Course) // Move Course
            {
                float deltax = speed * (float)Math.Cos(Course);
                float deltay = speed * (float)Math.Sin(Course);
                x += deltax;
                y += deltay;
            }
            public void Draw(Graphics g, int Clock, int HeadsUpX, int HeadsUpY)
            {
                // See if dead
                if (life <= 0)
                    c = Color.FromArgb(32, 32, 32);
                // Check for flashing effect
                bool Flash = false;
                foreach (Effect effect in effects)
                {
                    if (effect.Name == "Flashing")
                    {
                        Flash = true;
                        break;
                    }
                }
                // Draw circle
                //if ((Flash == true) && (Clock % 2 == 0))
                if (Flash==true)
                {
                    g.FillEllipse(new SolidBrush(Color.Red), x - size, y - size, 2 * size, 2 * size);
                }
                else
                    g.FillEllipse(new SolidBrush(c), x - size, y - size, 2 * size, 2 * size);
                // Show additional information?
                if (show)
                {
                    //System.Drawing.Font font = new System.Drawing.Font("Courier New", 6.0F); // Sans Serif not bad
                    System.Drawing.Font font = new System.Drawing.Font("New Times Roman", 5.0F);
                    SizeF NameSize = g.MeasureString(name, font);
                    float bx = x - NameSize.Width / 2.0F;
                    float by = y - NameSize.Height - size;
                    g.DrawString(name, font, new SolidBrush(Color.White), new PointF(bx, by));
                    // Draw creature data in heads up display
                    SolidBrush b = new SolidBrush(Color.White);
                    int VerticalSpacing = 20;
                    System.Drawing.Font font1 = new System.Drawing.Font("New Times Roman", 10.0F);
                    g.DrawString(name, font1, b, new PointF(HeadsUpX, HeadsUpY));
                    HeadsUpY += VerticalSpacing;
                    g.DrawString(this.life.ToString() + " / " + this.maxlife.ToString(),font1,b,new PointF(HeadsUpX,HeadsUpY));
                    HeadsUpY += VerticalSpacing;
                    if (w != null)
                    {
                        g.DrawString(w.Name, font1, b, new PointF(HeadsUpX, HeadsUpY));
                        HeadsUpY += VerticalSpacing;
                    }
                    if (s != null)
                    {
                        g.DrawString(s.Name, font1, b, new PointF(HeadsUpX, HeadsUpY));
                        HeadsUpY += VerticalSpacing;
                    }
                    if (a != null)
                    {
                        g.DrawString(a.Name, font1, b, new PointF(HeadsUpX, HeadsUpY));
                        HeadsUpY += VerticalSpacing;
                    }
                }
            } // end method Draw
        }



        class Obstacle // Impediment, Hindrance, Obstruction, Restriction, Blockage, Hazard, Impedance
        {
            PointF p1;
            PointF p2;
            bool known; // seen?
            // transparent?
            // impassable/passable?
            public Obstacle(float X1, float Y1, float X2, float Y2)
            {
                p1 = new PointF(X1, Y1);
                p2 = new PointF(X2, Y2);
                known = false;
            }
            public Obstacle(float X1, float Y1, float X2, float Y2, bool Known)
            {
                p1 = new PointF(X1, Y1);
                p2 = new PointF(X2, Y2);
                known = Known;
            }
            public PointF P1
            {
                get { return p1; }
            }
            public PointF P2
            {
                get { return p2; }
            }
            public bool Known
            {
                get { return known; }
                set { known = value; }
            }
        }



        class Portal // Gate, Gateway, Passage
        {
            float x; // portal location
            float y;
            float r; // portal size
            string nl; // next level
            float lx; // lx,ly is where player will appear in the new level
            float ly;
            string t; // text description
            float tx; // text offset x
            float ty; // text offset y
            bool show; // mouse over portal?
            bool known; // been discovered?
            public Portal(float X, float Y, float Radius, string NextLevel, float LevelX, float LevelY, string Description, float TextX, float TextY)
            {
                x = X;
                y = Y;
                r = Radius;
                nl = NextLevel;
                lx = LevelX;
                ly = LevelY;
                t = Description;
                tx = TextX;
                ty = TextY;
                show = false;
                known = false;
            }
            public float X
            {
                get { return x; }
            }
            public float Y
            {
                get { return y; }
            }
            public float Radius
            {
                get { return r; }
            }
            public string NextLevel
            {
                get { return nl; }
            }
            public string Text
            {
                get { return t; }
            }
            public float LevelX
            {
                get { return lx; }
            }
            public float LevelY
            {
                get { return ly; }
            }
            public float TextOffsetX
            {
                get { return tx; }
            }
            public float TextOffsetY
            {
                get { return ty; }
            }
            public bool Show
            {
                get { return show; }
                set { show = value; }
            }
            public bool Known
            {
                get { return known; }
                set { known = value; }
            }
            public void Draw(Graphics g)
            {
                Pen PortalPen = new Pen(Color.Yellow);
                g.DrawLine(PortalPen, x, y - 5, x - 5, y);
                g.DrawLine(PortalPen, x-5, y, x, y+5);
                g.DrawLine(PortalPen, x, y + 5, x + 5, y);
                g.DrawLine(PortalPen, x + 5, y, x, y-5);
                if (show)
                {
                    System.Drawing.Font font = new System.Drawing.Font("New Times Roman", 5.0F);
                    g.DrawString(t, font, new SolidBrush(Color.White), new PointF(x + tx, y + ty));
                }
            }
        }



        System.Collections.ArrayList Infrastructure;
        System.Collections.ArrayList Creatures;
        System.Collections.ArrayList Corpses;
        System.Collections.ArrayList Events; // Hides .Net Events
        System.Collections.ArrayList Objects;
        System.Collections.ArrayList Portals;

        Creature Player;
        Object PickingUp;
        string Level;

        int MouseX;
        int MouseY;
        int Clock;
        Random R;
        int HeadsUpX;
        int HeadsUpY;

        Bitmap b;
        Graphics g;



        public Form1()
        {
            InitializeComponent();
            this.Text = "Diablo";

            R = new Random();

            // Clear debug document at beginning of each program execution
            System.IO.StreamWriter sw = new System.IO.StreamWriter("debug.txt");
            sw.Close();

            //System.IO.StreamReader sr = new System.IO.StreamReader("Location.txt");
            //Level = sr.ReadLine();
            //sr.Close();

            Player = LoadPlayer("Player.txt");
            LoadLevel(Level);

            pictureBox1.Width = this.Width - 16;
            pictureBox1.Height = this.Height - 38;

            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);

            Clock = 0;

            timer1.Interval = 25; // 40 frames per second
            timer1.Enabled = true;
        }



        private void LoadLevel(string LevelName)
        {
            PickingUp = null;

            Infrastructure = new System.Collections.ArrayList();
            Creatures = new System.Collections.ArrayList();
            Corpses = new System.Collections.ArrayList();
            Events = new System.Collections.ArrayList();
            Objects = new System.Collections.ArrayList();
            Portals = new System.Collections.ArrayList();

            Creatures.Add(Player);

            System.IO.StreamReader sr = new System.IO.StreamReader(LevelName + ".txt");
            HeadsUpX = Convert.ToInt32(sr.ReadLine());
            HeadsUpY = Convert.ToInt32(sr.ReadLine());
           
            string line = sr.ReadLine();
            while (line != null)
            {
                int i = line.IndexOf(" ");
                string RecordType = line.Substring(0, i);
                if (RecordType == "Creature")
                {
                    // Get Location
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    float x = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    float y = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    // Get Alliance
                    i = line.IndexOf(" ");
                    int Alliance = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    // Get Life points
                    i = line.IndexOf(" ");
                    int hp = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    // Get Max Life points
                    i = line.IndexOf(" ");
                    int mhp = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    // Get Red Intensity
                    i = line.IndexOf(" ");
                    int red = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    // Get Green Intensity
                    i = line.IndexOf(" ");
                    int green = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    // Get Blue Intensity
                    i = line.IndexOf(" ");
                    int blue = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    // Get Speed
                    i = line.IndexOf(" ");
                    float speed = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim(); // Name
                    Creature NewCreature = new Creature(line, hp, mhp, x, y, Color.FromArgb(red, green, blue), speed, Alliance);

                    line = sr.ReadLine();
                    while ((line != null) && (line.Substring(0, 1) == "+"))
                    {
                        if (line != null)
                        {
                            line = line.Trim();
                            if (line.Substring(0, 1) == "+")
                            {
                                line = line.Substring(1).Trim();
                                i = line.IndexOf(" ");
                                string AttributeType = line.Substring(0, i);
                                if (AttributeType == "Weapon")
                                {
                                    line = line.Substring(i + 1).Trim();
                                    i = line.IndexOf(" ");
                                    int LowDamage = Convert.ToInt32(line.Substring(0, i));
                                    line = line.Substring(i + 1).Trim();
                                    i = line.IndexOf(" ");
                                    int HighDamage = Convert.ToInt32(line.Substring(0, i));
                                    line = line.Substring(i + 1).Trim();
                                    i = line.IndexOf(" ");
                                    int BaseToHit = Convert.ToInt32(line.Substring(0, i));
                                    line = line.Substring(i + 1).Trim();
                                    i = line.IndexOf(" ");
                                    int WeaponSpeed = Convert.ToInt32(line.Substring(0, i));
                                    line = line.Substring(i + 1).Trim();
                                    NewCreature.AddPossession(new Weapon(line, LowDamage, HighDamage, BaseToHit, WeaponSpeed));
                                }
                                if (AttributeType == "Shield")
                                {
                                    line = line.Substring(i + 1).Trim();
                                    i = line.IndexOf(" ");
                                    int EaseOfUse = Convert.ToInt32(line.Substring(0, i));
                                    line = line.Substring(i + 1).Trim();
                                    i = line.IndexOf(" ");
                                    int MaxDeflected = Convert.ToInt32(line.Substring(0, i));
                                    line = line.Substring(i + 1).Trim();
                                    i = line.IndexOf(" ");
                                    int Encumbrance = Convert.ToInt32(line.Substring(0, i));
                                    line = line.Substring(i + 1).Trim();
                                    NewCreature.AddPossession(new Shield(line, EaseOfUse, MaxDeflected, Encumbrance));
                                }
                                if (AttributeType == "Potion")
                                {
                                    line = line.Substring(i + 1).Trim();
                                    NewCreature.AddPossession(new Possession(line, "Potion"));
                                }
                                if (AttributeType == "Armor")
                                {
                                    line = line.Substring(i + 1).Trim();
                                    i = line.IndexOf(" ");
                                    int Absorbed = Convert.ToInt32(line.Substring(0, i));
                                    line = line.Substring(i + 1).Trim();
                                    i = line.IndexOf(" ");
                                    int Encumbrance = Convert.ToInt32(line.Substring(0, i));
                                    line = line.Substring(i + 1).Trim();
                                    NewCreature.AddPossession(new Armor(line, Absorbed, Encumbrance));
                                }
                                if (AttributeType == "Skill")
                                {
                                    line = line.Substring(i + 1).Trim();
                                    i = line.IndexOf(" ");
                                    int Proficiency = Convert.ToInt32(line.Substring(0, i));
                                    line = line.Substring(i + 1).Trim();
                                    NewCreature.AddSkill(new Ability(line, Proficiency));
                                }
                            }
                        }
                        line = sr.ReadLine();
                    }
                    Creatures.Add(NewCreature);
                }
                if (RecordType == "Obstacle")
                {
                    line = line.Substring(i + 1);
                    i = line.IndexOf(" ");
                    float x1 = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1);
                    i = line.IndexOf(" ");
                    float y1 = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1);
                    i = line.IndexOf(" ");
                    float x2 = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1);
                    i = line.IndexOf(" ");
                    float y2 = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1); // line is 'True' or 'False'
                    Infrastructure.Add(new Obstacle(x1, y1, x2, y2, Convert.ToBoolean(line)));
                    line = sr.ReadLine();
                }
                if (RecordType == "Object")
                {
                    // Get Location
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    float x = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    float y = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    // Get Object Type
                    i = line.IndexOf(" ");
                    string ObjectType = line.Substring(0, i);
                    if (ObjectType == "Armor")
                    {
                        // Get Armor Protection
                        line = line.Substring(i + 1).Trim();
                        i = line.IndexOf(" ");
                        int Protection = Convert.ToInt32(line.Substring(0, i));
                        // Get Armor Encumbrance
                        line = line.Substring(i + 1).Trim();
                        i = line.IndexOf(" ");
                        int Encumbrance = Convert.ToInt32(line.Substring(0, i));
                        line = line.Substring(i + 1).Trim(); // Armor Name
                        Objects.Add(new Object(new Armor(line, Protection, Encumbrance), x, y));
                    }
                    if (ObjectType == "Shield")
                    {
                        line = line.Substring(i + 1).Trim();
                        i = line.IndexOf(" ");
                        int EaseOfUse = Convert.ToInt32(line.Substring(0, i));
                        line = line.Substring(i + 1).Trim();
                        i = line.IndexOf(" ");
                        int MaxDeflected = Convert.ToInt32(line.Substring(0, i));
                        line = line.Substring(i + 1).Trim();
                        i = line.IndexOf(" ");
                        int Encumbrance = Convert.ToInt32(line.Substring(0, i));
                        line = line.Substring(i + 1).Trim();
                        Objects.Add(new Object(new Shield(line, EaseOfUse, MaxDeflected, Encumbrance), x, y));
                    }
                    if (ObjectType == "Potion")
                    {
                        line = line.Substring(i + 1).Trim();
                        Objects.Add(new Object(new Possession(line,"Potion"),x,y));
                    }
                    if (ObjectType == "Weapon")
                    {
                        // Get Low Damage
                        line = line.Substring(i + 1).Trim();
                        i = line.IndexOf(" ");
                        int LowDamage = Convert.ToInt32(line.Substring(0, i));
                        // Get High Damage
                        line = line.Substring(i + 1).Trim();
                        i = line.IndexOf(" ");
                        int HighDamage = Convert.ToInt32(line.Substring(0, i));
                        // Get Ease Of Use
                        line = line.Substring(i + 1).Trim();
                        i = line.IndexOf(" ");
                        int EaseOfUse = Convert.ToInt32(line.Substring(0, i));
                        // Get Weapon Speed
                        line = line.Substring(i + 1).Trim();
                        i = line.IndexOf(" ");
                        int WeaponSpeed = Convert.ToInt32(line.Substring(0, i));
                        line = line.Substring(i + 1).Trim(); // Weapon Name
                        Objects.Add(new Object(new Weapon(line, LowDamage, HighDamage, EaseOfUse, WeaponSpeed), x, y));
                    }
                    line = sr.ReadLine();
                }
                if (RecordType == "Portal")
                {
                    // Get Location
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    float x = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    float y = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    // Get Radius
                    i = line.IndexOf(" ");
                    float r = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    // Get Player's new location
                    i = line.IndexOf(" ");
                    float px = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    float py = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    // Get Description offset
                    i = line.IndexOf(" ");
                    float dx = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    float dy = (float)Convert.ToDouble(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    // Get New Level Name
                    i = line.IndexOf('\t');
                    string NewLevel = line.Substring(0, i);
                    line = line.Substring(i + 1).Trim();
                    // Note: line = Description
                    Portals.Add(new Portal(x, y, r, NewLevel, px, py, line, dx, dy));
                    line = sr.ReadLine();
                }
            }
            sr.Close();
        }



        private void SaveLevel(string Filename)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(Filename + ".txt");
            sw.WriteLine(HeadsUpX);
            sw.WriteLine(HeadsUpY);
            foreach (Obstacle obstacle in Infrastructure)
            {
                sw.Write("Obstacle ");
                sw.Write(obstacle.P1.X.ToString() + " ");
                sw.Write(obstacle.P1.Y.ToString() + " ");
                sw.Write(obstacle.P2.X.ToString() + " ");
                sw.Write(obstacle.P2.Y.ToString() + " ");
                sw.WriteLine(obstacle.Known.ToString());
            }
            foreach (Portal p in Portals)
            {
                sw.Write("Portal ");
                sw.Write(p.X.ToString() + " ");
                sw.Write(p.Y.ToString() + " ");
                sw.Write(p.Radius.ToString() + " ");
                sw.Write(p.LevelX.ToString() + " ");
                sw.Write(p.LevelY.ToString() + " ");
                sw.Write(p.TextOffsetX.ToString() + " ");
                sw.Write(p.TextOffsetY.ToString() + " ");
                sw.Write(p.NextLevel + "\t");
                sw.WriteLine(p.Text);
            }
            foreach (Object obj in Objects)
            {
                sw.Write("Object ");
                sw.Write(obj.X.ToString() + " ");
                sw.Write(obj.Y.ToString() + " ");
                sw.Write(obj.Possession.Category + " ");
                if (obj.Possession.Category == "Armor")
                {
                    sw.Write(((Armor)obj.Possession).Protection.ToString() + " ");
                    sw.Write(((Armor)obj.Possession).Encumbrance.ToString() + " ");
                    sw.WriteLine(obj.Possession.Name);
                }
                if (obj.Possession.Category == "Potion")
                {
                    sw.WriteLine(obj.Possession.Name);
                }
                if (obj.Possession.Category == "Shield")
                {
                    sw.Write(((Shield)obj.Possession).EaseOfUse.ToString() + " ");
                    sw.Write(((Shield)obj.Possession).MaxParried.ToString() + " ");
                    sw.Write(((Shield)obj.Possession).Encumbrance.ToString() + " ");
                    sw.WriteLine(obj.Possession.Name);
                }
                if (obj.Possession.Category == "Weapon")
                {
                    sw.Write(((Weapon)obj.Possession).LowDamage.ToString() + " ");
                    sw.Write(((Weapon)obj.Possession).HighDamage.ToString() + " ");
                    sw.Write(((Weapon)obj.Possession).BaseToHit.ToString() + " ");
                    sw.Write(((Weapon)obj.Possession).WeaponSpeed.ToString() + " ");
                    sw.WriteLine(obj.Possession.Name);
                }
            }
            foreach (Creature Critter in Creatures)
            {
                if (Critter != Player)
                {
                    sw.Write("Creature ");
                    sw.Write(Critter.X.ToString() + " ");
                    sw.Write(Critter.Y.ToString() + " ");
                    sw.Write(Critter.Alliance.ToString() + " ");
                    sw.Write(Critter.Life.ToString() + " ");
                    sw.Write(Critter.MaxLife.ToString() + " ");
                    sw.Write(Critter.Color.R.ToString() + " " + Critter.Color.G.ToString() + " " + Critter.Color.B.ToString() + " ");
                    sw.Write(Critter.Velocity.ToString() + " ");
                    sw.WriteLine(Critter.Name);
                    foreach (Possession p in Critter.Possessions)
                    {
                        sw.Write("+ " + p.Category + " ");
                        if (p.Category == "Armor")
                        {
                            sw.Write(((Armor)p).Protection.ToString() + " ");
                            sw.Write(((Armor)p).Encumbrance.ToString() + " ");
                            sw.WriteLine(p.Name);
                        }
                        if (p.Category == "Potion")
                        {
                            sw.WriteLine(p.Name);
                        }
                        if (p.Category == "Shield")
                        {
                            sw.Write(((Shield)p).EaseOfUse.ToString() + " ");
                            sw.Write(((Shield)p).MaxParried.ToString() + " ");
                            sw.Write(((Shield)p).Encumbrance.ToString() + " ");
                            sw.WriteLine(p.Name);
                        }
                        if (p.Category == "Weapon")
                        {
                            sw.Write(((Weapon)p).LowDamage.ToString() + " ");
                            sw.Write(((Weapon)p).HighDamage.ToString() + " ");
                            sw.Write(((Weapon)p).BaseToHit.ToString() + " ");
                            sw.Write(((Weapon)p).WeaponSpeed.ToString() + " ");
                            sw.WriteLine(p.Name);
                        }
                    }
                    foreach (Ability Skill in Critter.Skills)
                    {
                        sw.Write("+ Skill ");
                        sw.Write(Skill.Proficiency.ToString() + " ");
                        sw.WriteLine(Skill.Skill);
                    }
                }
            }
            sw.Close();
        }



        private Creature LoadPlayer(string Filename)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(Filename);
            string Name = sr.ReadLine();
            Level = sr.ReadLine(); // global variable
            float X = (float)Convert.ToDouble(sr.ReadLine());
            float Y = (float)Convert.ToDouble(sr.ReadLine());
            int Life = Convert.ToInt32(sr.ReadLine());
            int MaxLife = Convert.ToInt32(sr.ReadLine());
            int Red = Convert.ToInt32(sr.ReadLine());
            int Green = Convert.ToInt32(sr.ReadLine());
            int Blue = Convert.ToInt32(sr.ReadLine());
            float Speed = (float)Convert.ToDouble(sr.ReadLine());
            Creature Critter = new Creature(Name, Life, MaxLife, X, Y, Color.FromArgb(Red, Green, Blue), Speed, 1);
            string line = sr.ReadLine();
            while (line != null)
            {
                int i = line.IndexOf(" ");
                string RecordType = line.Substring(0, i);
                if (RecordType == "Weapon")
                {
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    int LowDamage = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    int HighDamage = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    int BaseToHit = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    int WeaponSpeed = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    Critter.AddPossession(new Weapon(line, LowDamage, HighDamage, BaseToHit, WeaponSpeed));
                }
                if (RecordType == "Shield")
                {
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    int EaseOfUse = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    int MaxDeflected = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    int Encumbrance = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    Critter.AddPossession(new Shield(line, EaseOfUse, MaxDeflected, Encumbrance));
                }
                if (RecordType == "Potion")
                {
                    line = line.Substring(i + 1).Trim();
                    Critter.AddPossession(new Possession(line, "Potion"));
                }
                if (RecordType == "Armor")
                {
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    int Absorbed = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    int Encumbrance = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    Critter.AddPossession(new Armor(line, Absorbed, Encumbrance));
                }
                if (RecordType == "Skill")
                {
                    line = line.Substring(i + 1).Trim();
                    i = line.IndexOf(" ");
                    int Proficiency = Convert.ToInt32(line.Substring(0, i));
                    line = line.Substring(i + 1).Trim();
                    Critter.AddSkill(new Ability(line, Proficiency));
                }
                line = sr.ReadLine();
            }
            sr.Close();
            return Critter;
        }



        private void SavePlayer(Creature Player)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter("Player.txt");
            sw.WriteLine(Player.Name);
            sw.WriteLine(Level);
            sw.WriteLine(Player.X);
            sw.WriteLine(Player.Y);
            sw.WriteLine(Player.Life);
            sw.WriteLine(Player.MaxLife);
            sw.WriteLine(Player.Color.R);
            sw.WriteLine(Player.Color.G);
            sw.WriteLine(Player.Color.B);
            sw.WriteLine(Player.Velocity);
            foreach (Possession Item in Player.Possessions)
            {
                if (Item.Category == "Armor")
                {
                    sw.WriteLine("Armor " + ((Armor)Item).Protection.ToString() + " " + ((Armor)Item).Encumbrance.ToString() + " " + Item.Name);
                }
                if (Item.Category == "Potion")
                {
                    sw.WriteLine("Potion " + Item.Name);
                }
                if (Item.Category == "Shield")
                {
                    sw.WriteLine("Shield " + ((Shield)Item).EaseOfUse.ToString() + " " + ((Shield)Item).MaxParried.ToString() + " " + ((Shield)Item).Encumbrance.ToString() + " " + Item.Name);
                }
                if (Item.Category == "Weapon")
                {
                    sw.WriteLine("Weapon " + ((Weapon)Item).LowDamage.ToString() + " " + ((Weapon)Item).HighDamage.ToString() + " " + ((Weapon)Item).BaseToHit.ToString() + " " + ((Weapon)Item).WeaponSpeed.ToString() + " " + Item.Name);
                }
            }
            foreach (Ability Skill in Player.Skills)
            {
                sw.WriteLine("Skill " + Skill.Proficiency.ToString() + " " + Skill.Skill);
            }
            sw.Close();
        }



        private double Distance(Creature C, PointF P)
        {
            return Math.Sqrt((C.X - P.X) * (C.X - P.X) + (C.Y - P.Y) * (C.Y - P.Y));
        }



        private double Distance(Creature C1, Creature C2)
        {
            return Math.Sqrt((C1.X - C2.X) * (C1.X - C2.X) + (C1.Y - C2.Y) * (C1.Y - C2.Y));
        }



        private double Distance(Creature C, Obstacle obstacle)
        {
            // Compute distance from line segment P1 to P2 and creature position = d
            // https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line
            // Vector a-p
            float apx = obstacle.P1.X - C.X;
            float apy = obstacle.P1.Y - C.Y;
            // Vector nx, ny is vector representing segment
            float nx = obstacle.P2.X - obstacle.P1.X;
            float ny = obstacle.P2.Y - obstacle.P1.Y;
            float m = (float)Math.Sqrt(nx * nx + ny * ny); // length of segment
            // Make n a unit vector
            nx = nx / m;
            ny = ny / m;
            // (a-p)*n
            float dot = apx * nx + apy * ny;
            // projection vector
            float dx = apx - dot * nx;
            float dy = apy - dot * ny;

            float d1 = (float)Math.Sqrt((obstacle.P1.X - C.X) * (obstacle.P1.X - C.X) + (obstacle.P1.Y - C.Y) * (obstacle.P1.Y - C.Y));
            float d2 = (float)Math.Sqrt((obstacle.P2.X - C.X) * (obstacle.P2.X - C.X) + (obstacle.P2.Y - C.Y) * (obstacle.P2.Y - C.Y));

            if ((d1 < m) && (d2 < m))
            {
                return Math.Sqrt(dx * dx + dy * dy);
            }
            else
            {
                return Math.Min(d1, d2);
            }

        }



        // http://geomalgorithms.com/a05-_intersect-1.html

        private bool Intersection(PointF P0, PointF P1, PointF Q0, PointF Q1)
        {
            float u1 = P1.X - P0.X;
            float u2 = P1.Y - P0.Y;
            float v1 = Q1.X - Q0.X;
            float v2 = Q1.Y - Q0.Y;
            if ((u1 * v2 - u2 * v1) == 0) return false; // line segments are parallel
            float w1 = P0.X-Q0.X;
            float w2 = P0.Y-Q0.Y;
            float s1 = (v2 * w1 - v1 * w2) / (v1 * u2 - v2 * u1);
            if (s1 <= 0) return false; // <= excludes endpoints from intersections
            if (s1 >= 1) return false;
            float t1 = (u1 * w2 - u2 * w1) / (u1 * v2 - u2 * v1);
            if (t1 <= 0) return false;
            if (t1 >= 1) return false;
            return true;
        }



        private void ExecuteMeleeAttack(Creature Attacker, Creature Defender, System.Collections.ArrayList NewEvents)
        {
            // Code assumes no events can occur for null creatures
            Log(Attacker.Name + " attacks " + Defender.Name);
            int DefenderEncumbrance = 0;
            if (Defender.Shield != null) DefenderEncumbrance += Defender.Shield.Encumbrance;
            if (Defender.Armor != null) DefenderEncumbrance += Defender.Armor.Encumbrance;
            int AttackerEncumbrance = 0;
            if (Attacker.Shield != null) AttackerEncumbrance += Attacker.Shield.Encumbrance;
            if (Attacker.Armor != null) AttackerEncumbrance += Attacker.Armor.Encumbrance;
            // Attacker armor encumbrance
            int ToHit = Attacker.Weapon.BaseToHit + Attacker.GetProficiency(Attacker.Weapon.Name) + DefenderEncumbrance - AttackerEncumbrance;
            Log("ToHit = " + ToHit.ToString() + " (" + Attacker.Weapon.BaseToHit.ToString() + " + " + Attacker.GetProficiency(Attacker.Weapon.Name).ToString() + " + " + DefenderEncumbrance.ToString() + " - " + AttackerEncumbrance.ToString() + ")");
            int Roll = R.Next(10) + 1; // random 1 to 10
            Log("Roll = " + Roll.ToString());
            if (Roll <= ToHit)
            {
                int Damage = Attacker.Weapon.GetDamage(R);
                Log("Hit! Initial Damage = " + Damage.ToString());
                if (Defender.Shield != null)
                {
                    int QOA = ToHit - Roll;
                    Log("QOA = " + QOA.ToString());
                    int ToParry = Defender.Shield.EaseOfUse + Defender.GetProficiency(Defender.Shield.Name) - QOA;
                    Log("ToParry = " + ToParry.ToString() + " (" + Defender.Shield.EaseOfUse.ToString() + " + " + Defender.GetProficiency(Defender.Shield.Name).ToString() + " - " + QOA.ToString() + ")");
                    Roll = R.Next(10) + 1; // random 1 to 10
                    Log("Roll = " + Roll.ToString());
                    if (Roll <= ToParry)
                    {
                        int QOP = ToParry - Roll;
                        Log("QOP = " + QOP.ToString());
                        int Blocked = Defender.Shield.GetParried(R, QOP);
                        Log("Blocked = " + Blocked.ToString() + " (" + (QOP + 1).ToString() + " to " + Defender.Shield.MaxParried.ToString() + ")");
                        Damage -= Blocked;
                        if (Damage < 0) Damage = 0;
                        Log("Damage after parry = " + Damage.ToString());
                    }
                } // end if (Defender.Shield != null)
                if (Defender.Armor != null)
                {
                    Log(Defender.Armor.Name + " absorbs " + Defender.Armor.Protection.ToString());
                    Damage -= Defender.Armor.Protection;
                    if (Damage < 0)
                        Damage = 0;
                    Log("Damage after armor = "+Damage.ToString());
                }
                if (Damage > 0)
                {
                    NewEvents.Add(new Event("Damage", Clock, Defender, Damage));
                }
            } // end if (Roll <= ToHit)
        }



        private void ProcessEvents() // ProcessTick, ExecuteTick
        {
            System.Collections.ArrayList NewEvents = new System.Collections.ArrayList();
            System.Collections.ArrayList ExpiredEvents = new System.Collections.ArrayList();

            // Execute all Melee Attacks
            foreach (Event E in Events)
            {
                if ((E.Time == Clock) && (E.Name == "Melee Attack"))
                {
                    Creature Attacker = E.Who;
                    Creature Defender = E.Who.Target;
                    ExecuteMeleeAttack(Attacker, Defender,NewEvents);
                    NewEvents.Add(new Event("Melee Attack", Clock + Attacker.Weapon.WeaponSpeed, Attacker, 0));
                    Log("Event Created (" + (Clock + Attacker.Weapon.WeaponSpeed).ToString() + ", " + Attacker.Name + ", Melee Attack)");
                    ExpiredEvents.Add(E);
                    // If defender has not target or not "engaged" current target, then switch target to current attacker
                    if ((Defender.Target == null) || (Distance(Defender, Defender.Target) > (Defender.Size + Defender.Target.Size + Defender.Velocity)))
                        E.Who.Target.Target = E.Who;
                } // end if (Time and 'Melee Attack')
            } // end foreach (Event ...

            foreach (Event E in ExpiredEvents) // Melee Attack events
                Events.Remove(E);
            foreach (Event E in NewEvents) // Damage events
                Events.Add(E);
            ExpiredEvents.Clear();
            NewEvents.Clear();


            // Apply Healing
            foreach (Event E in Events)
            {
                if ((E.Time == Clock) && (E.Name == "Healing"))
                {
                    E.Who.Life += E.Value;
                    if (E.Who.Life > E.Who.MaxLife)
                        E.Who.Life = E.Who.MaxLife;
                    Log("Healing applied to " + E.Who.Name + " = " + E.Value.ToString());
                    //if (E.Who.Life > 0) // Creature still alive
                    //    E.Who.AddEffect(new Effect("Flashing", Clock + 10));
                    ExpiredEvents.Add(E);
                } // end if (Time and 'Damage')
            } // end foreach (Event ...
            foreach (Event E in ExpiredEvents)
                Events.Remove(E);
            ExpiredEvents.Clear();


            // Apply Damage
            foreach (Event E in Events)
            {
                if ((E.Time == Clock) && (E.Name == "Damage"))
                {
                    E.Who.Life -= E.Value;
                    Log("Damage applied to " + E.Who.Name + " = " + E.Value.ToString());
                    if (E.Who.Life > 0) // Creature still alive
                        E.Who.AddEffect(new Effect("Flashing", Clock + 10));
                    ExpiredEvents.Add(E);
                } // end if (Time and 'Damage')
            } // end foreach (Event ...

            foreach (Event E in ExpiredEvents)
                Events.Remove(E);
            ExpiredEvents.Clear();


            // Identify/Process newly Dead Creatures
            System.Collections.ArrayList DeadCreatures = new System.Collections.ArrayList();
            foreach (Creature c in Creatures)
            {
                if (c.Life <= 0) // Creature Died during current tick
                {
                    DeadCreatures.Add(c);
                }
            }
            foreach (Creature c in DeadCreatures)
            {
                Log(c.Name + " was killed");
                Creatures.Remove(c);
                Corpses.Add(c);
                // Remove all events for dead creature
                foreach (Event E in Events)
                    if (E.Who == c)
                    {
                        ExpiredEvents.Add(E);
                        Log("Event Deleted (" + E.Time + ", " + E.Who.Name + ", " + E.Name + ")");
                    }
                // Free anyone targeting newly dead creature
                foreach (Creature c1 in Creatures)
                    if (c1.Target == c)
                    {
                        c1.Target = null;
                        // Remove c1 events attacking target
                        foreach (Event E in Events)
                            if (E.Who == c1)
                            {
                                ExpiredEvents.Add(E);
                                Log("Event Deleted (" + E.Time + ", " + E.Who.Name + ", " + E.Name + ")");
                            }
                    }
                // Drop possessions
                foreach (Possession p in c.Possessions)
                {
                    int x = R.Next(11) - 5;
                    int y = R.Next(11) - 5;
                    Objects.Add(new Object(p, c.X + x, c.Y + y));
                }
                
            }
            // Remove any future events for newly dead creature
            foreach (Event E in ExpiredEvents)
                Events.Remove(E);

        }



        private void ClearEvents()
        {
            System.Collections.ArrayList ExpiredEvents = new System.Collections.ArrayList();
            foreach (Event E in Events)
                if (E.Time <= Clock)
                    ExpiredEvents.Add(E);
            foreach (Event E in ExpiredEvents)
                Events.Remove(E);
        }



        private void PrintEvents(Graphics g)
        {
            if (Events.Count > 0)
            {
                System.Drawing.Font font = new System.Drawing.Font("New Times Roman", 10.0F);
                float Y = 50;
                foreach (Event E in Events)
                {
                    g.DrawString("(" + E.Time.ToString() + ", " + E.Who.Name + ", " + E.Name + ")", font, new SolidBrush(Color.White), new PointF(25, Y));
                    Y += 20;
                }
            }
        }



        private void Log(string Action)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter("debug.txt", true);
            sw.WriteLine(Clock.ToString() + "\t" + Action);
            sw.Close();
        }

 

        private void timer1_Tick(object sender, EventArgs e)
        {
            ++Clock;

            // Move all creatures
            foreach (Creature c in Creatures)
            {
                c.Move(Infrastructure,Creatures);
                // Melee Attack adjacent enemies
                if (c.Target != null)
                {
                    // See if adjacent to target
                    if (Math.Sqrt((c.X - c.Target.X) * (c.X - c.Target.X) + (c.Y - c.Target.Y) * (c.Y - c.Target.Y)) <= (c.Size + c.Target.Size + c.Velocity))
                    {
                        c.SetDestination(c.X, c.Y);
                        bool Attacking = false;
                        foreach (Event E in Events)
                        {
                            if ((E.Who == c) && (E.Name == "Melee Attack"))
                            {
                                Attacking = true;
                                break;
                            }
                        }
                        if (Attacking==false)
                        {
                            Events.Add(new Event("Melee Attack", Clock + c.Weapon.WeaponSpeed, c, 0));
                            Log("Event Created ("+(Clock+c.Weapon.WeaponSpeed).ToString()+", "+c.Name +", Melee Attack)");
                        }
                    }
                } // end if (c.Target != null)
            } // end foreach (Creature c ...

            // Move Through Portal?
            Portal NewLevel = null;
            foreach (Portal p in Portals)
            {
                if (Math.Sqrt((Player.X - p.X) * (Player.X - p.X) + (Player.Y - p.Y) * (Player.Y - p.Y)) <= p.Radius)
                {
                    NewLevel = p;
                    break;
                }
            }
            if (NewLevel != null)
            {
                SaveLevel(Level);
                Player.SetLocation(NewLevel.LevelX, NewLevel.LevelY);
                Player.Target = null;
                Player.SetDestination(Player.X, Player.Y);
                Level = NewLevel.NextLevel;
                LoadLevel(Level);
                return;
            }

            // Pick up object?
            if (PickingUp != null)
            {
                if (Math.Sqrt((Player.X - PickingUp.X) * (Player.X - PickingUp.X) + (Player.Y - PickingUp.Y) * (Player.Y - PickingUp.Y)) <= 5)
                {
                    Player.AddPossession(PickingUp.Possession);
                    Objects.Remove(PickingUp);
                    PickingUp = null;
                }
            }

            // Check to see if mouse is hovering over creature
            foreach (Creature c in Creatures)
            {
                if (Math.Sqrt((c.X - MouseX) * (c.X - MouseX) + (c.Y - MouseY) * (c.Y - MouseY)) <= c.Size)
                    c.Show = true;
                else
                    c.Show = false;
            }

            // Check to see if mouse is hovering over object
            foreach (Object c in Objects)
            {
                if (Math.Sqrt((c.X - MouseX) * (c.X - MouseX) + (c.Y - MouseY) * (c.Y - MouseY)) <= 3)
                    c.Show = true;
                else
                    c.Show = false;
            }

            // Check to see if mouse is hovering over a portal
            foreach (Portal p in Portals)
            {
                if (Math.Sqrt((p.X - MouseX) * (p.X - MouseX) + (p.Y - MouseY) * (p.Y - MouseY)) <= p.Radius)
                    p.Show = true;
                else
                    p.Show = false;
            }

            // Check to see if some new infrastructure becomes visible to player
            foreach (Obstacle target in Infrastructure)
            {
                if (target.Known == false)
                {
                    bool Visible = true;
                    if (Distance(Player, target.P1) > 150) Visible = false;
                    if (Distance(Player, target.P2) > 150) Visible = false;
                    if (Visible == true)
                    {
                        foreach (Obstacle obstacle in Infrastructure)
                        {
                            if (target != obstacle)
                            {
                                if (Intersection(new PointF(Player.X, Player.Y), target.P1, obstacle.P1, obstacle.P2) == true)
                                {
                                    Visible = false;
                                    break;
                                }
                                if (Intersection(new PointF(Player.X, Player.Y), target.P2, obstacle.P1, obstacle.P2) == true)
                                {
                                    Visible = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (Visible)
                    {
                        target.Known = true;
                    }
                }
            }

            // Check to see if portals are visible to player
            foreach (Portal p in Portals)
            {
                if (p.Known == false)
                {
                    bool Visible = true;
                    if (Distance(Player, new PointF(p.X, p.Y)) > 150)
                        Visible = false;
                    if (Visible == true)
                    {
                        foreach (Obstacle obstacle in Infrastructure)
                        {
                            if (Intersection(new PointF(Player.X, Player.Y), new PointF(p.X, p.Y), obstacle.P1, obstacle.P2) == true)
                            {
                                Visible = false;
                                break;
                            }
                        }
                    }
                    if (Visible == true)
                        p.Known = true;
                }
            }

            // Check to see if other creatures are visible to player
            foreach (Creature c in Creatures)
            {
                if (c == Player)
                    c.Visible = true;
                else
                {
                    c.Visible=true;
                    if (Distance(Player, c) > 150) c.Visible = false;
                    foreach (Obstacle obstacle in Infrastructure)
                    {
                        if (Intersection(new PointF(Player.X, Player.Y), new PointF(c.X, c.Y), obstacle.P1, obstacle.P2) == true)
                        {
                            c.Visible = false;
                            break;
                        }
                    }
                }
            }

            // Check to see if objects are visible to player
            foreach (Object obj in Objects)
            {
                obj.Visible = true;
                if (Distance(Player, new PointF(obj.X,obj.Y)) > 150) obj.Visible = false;
                foreach (Obstacle obstacle in Infrastructure)
                {
                    if (Intersection(new PointF(Player.X, Player.Y), new PointF(obj.X, obj.Y), obstacle.P1, obstacle.P2) == true)
                    {
                        obj.Visible = false;
                        break;
                    }
                }
            }

            // Monster AI
            foreach (Creature c in Creatures)
            {
                if (c != Player)
                {
                    foreach (Creature c1 in Creatures)
                    {
                        if ((c.Target == null) && (c.Alliance != c1.Alliance))
                        {
                            bool Visible = true;
                            foreach (Obstacle obstacle in Infrastructure)
                            {
                                if (Intersection(new PointF(c.X, c.Y), new PointF(c1.X, c1.Y), obstacle.P1, obstacle.P2) == true)
                                {
                                    Visible = false;
                                    break;
                                }
                            }
                            if (Visible)
                            {
                                c.Target = c1;
                            }
                        }
                        else
                        {
                            if (c.Target!=null)
                            c.SetDestination(c.Target.X,c.Target.Y);
                        }
                    }
                }
            }

            // Check for current events
            ProcessEvents();

            // Clear events and effects
            foreach (Creature c in Creatures)
                c.ClearEffects(Clock);

            DrawFrame();
        }



        private void DrawFrame()
        {
            // Draw view background
            //Bitmap b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //Graphics g = Graphics.FromImage(b);
            g.FillRectangle(new SolidBrush(Color.Black), 0, 0, b.Width, b.Height);

            PrintEvents(g); // DEBUG

            // Draw Infrastructure
            foreach (Obstacle Wall in Infrastructure)
            {
                if (Wall.Known)
                    g.DrawLine(new Pen(Color.LightGray, 1), Wall.P1, Wall.P2);
            }
            //g.DrawLine(new Pen(Color.Brown), 650, 360, 650, 390); // door

            // Draw Objects
            foreach (Object item in Objects)
            {
                if (item.Visible == true)
                    item.Draw(g, Clock);
            }

            // Draw Portals
            foreach (Portal p in Portals)
            {
                if (p.Known == true)
                {
                    p.Draw(g);
                }
            }

            // Draw Creatures
            foreach (Creature C in Creatures)
            {
                if (C.Visible == true)
                    C.Draw(g, Clock, HeadsUpX, HeadsUpY);
            }

            double course = Math.Atan2(Player.Y - Player.DestY, Player.DestX - Player.X);
            // Note: course is in radians, 0 is due east
            course *= (180.0 / Math.PI);
            System.Drawing.Font font = new System.Drawing.Font("New Times Roman", 10.0F);
            g.DrawString("Course = " + course.ToString("f0"), font, new SolidBrush(Color.White), new PointF(50, 500));

            // Display view
            pictureBox1.Image = b;
        }



        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // Assign target?
                foreach (Creature c in Creatures)
                {
                    if (Math.Sqrt((c.X - e.X) * (c.X - e.X) + (c.Y - e.Y) * (c.Y - e.Y)) < c.Size)
                    {
                        if (c.Alliance != Player.Alliance)
                        {
                            Player.Target = c;
                            break;
                        }
                    }
                }

                // Set destination
                Player.SetDestination(e.X, e.Y);

                // Elect to pick up object?
                PickingUp = null;
                foreach (Object item in Objects)
                {
                    if (Math.Sqrt((e.X - item.X) * (e.X - item.X) + (e.Y - item.Y) * (e.Y - item.Y)) <= 3)
                    {
                        PickingUp = item;
                        break;
                    }
                }
            }
        }



        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            MouseX = e.X;
            MouseY = e.Y;
            foreach (Creature c in Creatures)
            {
                if (Math.Sqrt((c.X - e.X) * (c.X - e.X) + (c.Y - e.Y) * (c.Y - e.Y)) <= c.Size)
                    c.Show = true;
                else
                    c.Show = false;
            }
        }



        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = false; // http://stackoverflow.com/questions/1304629/contextmenustrip-opened-event-doesnt-fire-after-opeing-event/1304659
            contextMenuStrip1.Items.Clear();
            foreach (Possession p in Player.Possessions)
            {
                contextMenuStrip1.Items.Add(p.Name);
            }
            contextMenuStrip1.Items.Add("Cancel");
        }



        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Possession Consumed = null;
            foreach (Possession p in Player.Possessions)
            {
                if (e.ClickedItem.ToString() == p.Name)
                {
                    if (p.Category == "Shield")
                    {
                        Player.Shield = (Shield)p;
                        break;
                    }
                    if (p.Category == "Weapon")
                    {
                        Player.Weapon = (Weapon)p;
                        break;
                    }
                    if (p.Category == "Armor")
                    {
                        Player.Armor = (Armor)p;
                        break;
                    }
                    if (p.Name == "Healing Potion")
                    {
                        int h = R.Next(4) + 3; // 3 to 6 points of damage healed
                        for (int i = 1; i <= h; i++)
                        {
                            Events.Add(new Event("Healing", Clock + i * 100, Player, 1));
                        }
                        Consumed = p;
                        break;
                    }
                }
            }
            if (Consumed!=null)
                Player.Possessions.Remove(Consumed);
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SavePlayer(Player);
            SaveLevel(Level);
        }
        

    }
}
