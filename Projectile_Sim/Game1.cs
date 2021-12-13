using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Perlman_ISU;
using System;
using System.Collections.Generic;

namespace Projectile_Sim
{
    
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        private int windowWidth;
        private int windowHeight;

        public const float FPS = 60;

        public const float g = 9.8f / FPS;
        public static float C = 0.5f;

        private Texture2D bgImg;
        private Rectangle bgRec;

        private Texture2D varImg;
        private Rectangle varRec;

        public static Texture2D dotImg;

        private Texture2D objImg;
        private List<Object> obj = new List<Object>();
        public static Point objPos = new Point(40, 600);
        
        private double objSpeed = 11;
        private int angle = 40;
        private int mass = 60;

        private float addCounter = 0;
        private float massCounter = 0;

        public static SpriteFont font;

        private Button massInc;
        private Button massDec;

        private Button angleInc;
        private Button angleDec;

        private Button aInc;
        private Button aDec;

        private Button vInc;
        private Button vDec;

        private Texture2D cannonHeadImg;
        private Rectangle cannonHeadRec;

        private Texture2D cannonPlatImg;
        private Rectangle cannonPlatRec;

        private KeyboardState kb;

        private Button clearBut;

        private Texture2D page1;
        private Texture2D page2;
        private Rectangle pageRec;

        private int instructions = 0;
        private Button next;
        private Button instBut;

        public static Texture2D vectorX;
        public static Texture2D vectorY;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 700;
            
            TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 1000 / (int)FPS);

            graphics.ApplyChanges();

            windowWidth = graphics.GraphicsDevice.Viewport.Width;
            windowHeight = graphics.GraphicsDevice.Viewport.Height;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Fonts/font");

            dotImg = Content.Load<Texture2D>("Images/Sprites/dot");

            bgImg = Content.Load<Texture2D>("Images/Sprites/background");
            bgRec = new Rectangle(0, 0, windowWidth, windowHeight);

            varImg = Content.Load<Texture2D>("Images/Sprites/variables");
            varRec = new Rectangle(930, 50, (int)(varImg.Width / 1.3), (int)(varImg.Height / 1.3));

            objImg = Content.Load<Texture2D>("Images/Sprites/basketball");

            massInc = new Button(Content.Load<Texture2D>("Images/Sprites/plus"), new Vector2(1025, 147));
            massDec = new Button(Content.Load<Texture2D>("Images/Sprites/minus"), new Vector2(950, 157));

            angleInc = new Button(Content.Load<Texture2D>("Images/Sprites/plus"), new Vector2(1025, 221));
            angleDec = new Button(Content.Load<Texture2D>("Images/Sprites/minus"), new Vector2(950, 230));

            aInc = new Button(Content.Load<Texture2D>("Images/Sprites/plus"), new Vector2(1025, 298));
            aDec = new Button(Content.Load<Texture2D>("Images/Sprites/minus"), new Vector2(950, 308));

            vInc = new Button(Content.Load<Texture2D>("Images/Sprites/plus"), new Vector2(1026, 370)); 
            vDec = new Button(Content.Load<Texture2D>("Images/Sprites/minus"), new Vector2(951, 380));

            instBut = new Button(Content.Load<Texture2D>("Images/Sprites/instructions"), new Vector2(465, 30));
            next = new Button(Content.Load<Texture2D>("Images/Sprites/next"), new Vector2(600, 500));

            clearBut = new Button(Content.Load<Texture2D>("Images/Sprites/clear"), new Vector2(915, 440));

            page1 = Content.Load<Texture2D>("Images/Sprites/first screen");
            page2 = Content.Load<Texture2D>("Images/Sprites/second screen");
            pageRec = new Rectangle(0, 0, windowWidth, windowHeight);

            cannonHeadImg = Content.Load<Texture2D>("Images/Sprites/cannon head");
            cannonHeadRec = new Rectangle(70, 590, cannonHeadImg.Width, cannonHeadImg.Height);

            cannonPlatImg = Content.Load<Texture2D>("Images/Sprites/cannon platform");
            cannonPlatRec = new Rectangle(20, 575, cannonPlatImg.Width, cannonPlatImg.Height);

            vectorX = Content.Load<Texture2D>("Images/Sprites/arrow");
            vectorY = Content.Load<Texture2D>("Images/Sprites/vector");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (instructions == 2)
            {
                kb = Keyboard.GetState();

                if (massInc.ButtonPressed() && massCounter == 0)
                {
                    mass += 1;
                    massCounter += 0.05f;
                }

                if (massDec.ButtonPressed() && massCounter == 0 && mass > 10)
                {
                    mass -= 1;
                    massCounter += 0.05f;
                }

                if (angleInc.ButtonPressed() && massCounter == 0 && angle < 90)
                {
                    angle += 5;
                    massCounter += 0.05f;
                }

                if (angleDec.ButtonPressed() && massCounter == 0 && angle > 15)
                {
                    angle -= 5;
                    massCounter += 0.05f;
                }

                if (aInc.ButtonPressed() && massCounter == 0 && C < 1f)
                {
                    C += 0.01f;
                    C = (float)Math.Round(C, 2);
                    massCounter += 0.05f;
                }

                if (aDec.ButtonPressed() && massCounter == 0 && C > 0f)
                {
                    C -= .01f;
                    C = (float)Math.Round(C, 2);
                    massCounter += 0.05f;
                }

                if (vInc.ButtonPressed() && massCounter == 0 && objSpeed < 13f)
                {
                    objSpeed += 0.1f;
                    objSpeed = Math.Round(objSpeed, 2);
                    massCounter += 0.05f;
                }

                if (vDec.ButtonPressed() && massCounter == 0 && objSpeed > 6f)
                {
                    objSpeed -= .1f;
                    objSpeed = Math.Round(objSpeed, 4);
                    massCounter += 0.05f;
                }

                if (kb.IsKeyDown(Keys.Space) && addCounter == 0 && obj.Count < 50)
                {
                    obj.Add(new Object(objImg, mass, new Point(cannonHeadRec.X, cannonHeadRec.Y - 20)));
                    
                    if (angle == 45)
                    {
                        obj[obj.Count - 1].vectorOg = new Vector2((float)(objSpeed * Math.Cos(MathHelper.ToRadians(angle)) * 1.03f), (float)(objSpeed * Math.Sin(MathHelper.ToRadians(angle))));
                        obj[obj.Count - 1].velocity = new Vector2((float)(objSpeed * Math.Cos(MathHelper.ToRadians(angle)) * 1.03f), (float)(objSpeed * Math.Sin(MathHelper.ToRadians(angle))));
                    }
                    else
                    {
                        obj[obj.Count - 1].vectorOg = new Vector2((float)(objSpeed * Math.Cos(MathHelper.ToRadians(angle)) * 1.03f), (float)(objSpeed * Math.Sin(MathHelper.ToRadians(angle))));
                        obj[obj.Count - 1].velocity = new Vector2((float)(objSpeed * Math.Cos(MathHelper.ToRadians(angle))), (float)(objSpeed * Math.Sin(MathHelper.ToRadians(angle))));
                    }

                    addCounter += 0.05f;
                }

                if (clearBut.ButtonPressed())
                {
                    obj.Clear();
                }
                
                if (instBut.ButtonPressed())
                {
                    instructions = 0;
                }

                addCounter = Timer(addCounter, 1.8f);
                massCounter = Timer(massCounter, 0.37f);

                foreach (Object o in obj)
                {
                    o.Launch(ToRadians());
                }
            }
            else
            {
                if (next.ButtonPressed() && massCounter == 0f)
                {
                    instructions++;
                    massCounter += 0.05f;
                }

                if (massCounter != 0)
                {
                    massCounter += 0.05f;

                    if (massCounter >= 1f)
                    {
                        massCounter = 0f;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();

            if (instructions == 2)
            {
                spriteBatch.Draw(bgImg, bgRec, Color.White);

                foreach (Object o in obj)
                {
                    o.Draw(spriteBatch);
                }

                spriteBatch.Draw(varImg, varRec, Color.White);

                massInc.Draw(spriteBatch);
                massDec.Draw(spriteBatch);

                spriteBatch.Draw(cannonHeadImg, cannonHeadRec, null, Color.White, MathHelper.ToRadians(-angle), new Vector2(20, cannonHeadRec.Height / 2), SpriteEffects.None, 0);
                spriteBatch.Draw(cannonPlatImg, cannonPlatRec, Color.White);

                angleInc.Draw(spriteBatch);
                angleDec.Draw(spriteBatch);

                aInc.Draw(spriteBatch);
                aDec.Draw(spriteBatch);

                vInc.Draw(spriteBatch);
                vDec.Draw(spriteBatch);

                instBut.Draw(spriteBatch);

                clearBut.Draw(spriteBatch);

                spriteBatch.DrawString(font, mass.ToString() + "kg", new Vector2(1014, 112), Color.Black);
                spriteBatch.DrawString(font, Convert.ToString(angle), new Vector2(1016, 187), Color.Black);
                spriteBatch.DrawString(font, C.ToString("F"), new Vector2(1117, 264), Color.Black);
                spriteBatch.DrawString(font, objSpeed.ToString("F1") + "m/s", new Vector2(1036, 338), Color.Black);
            }
            else 
            {
                if (instructions == 0)
                {
                    spriteBatch.Draw(page1, pageRec, Color.White);
                }
                else
                {
                    spriteBatch.Draw(page2, pageRec, Color.White);
                }

                next.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private double ToRadians()
        {
            return angle * Math.PI / 180;
        }

        private float Timer(float timer, float max)
        {
            if (timer != 0)
            {
                timer += 0.05f;

                if (timer >= max)
                {
                    timer = 0;
                }
            }

            return timer;
        }
    }
}
