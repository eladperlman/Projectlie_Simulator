using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projectile_Sim
{
    class Object
    {
        private Texture2D img;
        private Rectangle rec;

        private Rectangle xVector;
        private Rectangle yVector;

        private List<Rectangle> dotRec = new List<Rectangle>();

        public Vector2 velocity = Vector2.Zero;
        public Vector2 vectorOg = Vector2.Zero;

        private int dotThick = 2;

        private bool drawVectors = false;

        public double Mass { get; set; }

        public Object(Texture2D img, double mass, Point loc)
        {
            this.img = img;
            Mass = mass;
            rec = new Rectangle(loc.X, loc.Y, img.Width, img.Height);

            xVector = new Rectangle(rec.Center.X, rec.Center.Y, Game1.vectorX.Width, Game1.vectorX.Height / 2);
            yVector = new Rectangle(rec.X, rec.Center.Y - Game1.vectorY.Height / 2, Game1.vectorY.Width / 2, Game1.vectorY.Height);
        }

        public void Launch(double angle)
        {
            if (rec.Y < Game1.objPos.Y)
            {
                xVector.Width = (int)(Game1.vectorX.Width * Math.Abs(velocity.X) / vectorOg.X);
                yVector.Height = (int)(Game1.vectorY.Height * Math.Abs(velocity.Y) / vectorOg.Y);
                yVector.Y += (int)(Game1.vectorY.Height * Math.Abs(velocity.Y) / vectorOg.Y);

                dotRec.Add(new Rectangle(rec.Center.X, rec.Center.Y, dotThick, dotThick));

                Vector2 air = CalcAir((float)Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2)), angle);
                velocity.Y -= Game1.g + air.Y;
                velocity.X -= air.X;

                Console.WriteLine(velocity.X + ", " + velocity.Y);
                
                rec.X = (int)(rec.X + velocity.X);
                xVector.X = (int)(xVector.X + velocity.X);
                xVector.Y = (int)(xVector.Y - velocity.Y);
                yVector.X = (int)(yVector.X + velocity.X);
                yVector.Y = (int)(yVector.Y - velocity.Y);
                rec.Y = (int)(rec.Y - velocity.Y);
            }

            if (rec.Y >= Game1.objPos.Y)
            {
                rec.Y = Game1.objPos.Y;
                drawVectors = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Rectangle dot in dotRec)
            {
                spriteBatch.Draw(Game1.dotImg, dot, Color.White);
            }

            if (drawVectors)
            {
                spriteBatch.Draw(Game1.vectorX, xVector, Color.White);
                spriteBatch.Draw(Game1.vectorY, yVector, Color.White);
            }

            spriteBatch.Draw(img, rec, Color.White);
        }

        private Vector2 CalcAir(float speed, double angle)
        {
            double airAcc = (Game1.C * Math.Pow(speed, 4.4)) / Math.Pow(Mass, 2);

            return new Vector2((float)(airAcc * Math.Cos(angle) / Game1.FPS), (float)(airAcc * Math.Sin(angle) / Game1.FPS));
        }
    }
}
