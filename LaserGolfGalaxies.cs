using LaserGolf.Components;
using LaserGolf.Components.Obstacles;
using LaserGolf.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace LaserGolf
{
    public class LaserGolfGalaxies : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<Rectangle> gameWalls;
        private Ball playBall;

        public readonly int FRICTION_COEFFICENT = 1;

        public LaserGolfGalaxies()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            TextureContainer textureCon = new TextureContainer(this);
            Services.AddService(typeof(TextureContainer), textureCon);
            Components.Add(textureCon);

            // Create the Map to play the Game on
            Map map = new Map(this, 0, 100, 100);

            // TODO: Add your initialization logic here
            playBall = new Ball(this, map.BallPos);
            playBall.ScaleX = 100;
            playBall.ScaleY = 75;

            Components.Add(playBall);


            // Convert the MapElements to drawable MonoGame Objects
            for (int i = 0; i < map.Obstacles.Count; i++)
            {
                LaserGolfObstacle currObs = map.Obstacles[i];
                Components.Add(currObs);
            }

            Components.Add(map.Hole);



            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 810;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            Services.AddService(typeof(SpriteBatch), _spriteBatch);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
           
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            // Draw the starting rectangle for the golf ball
            int rectWidth = (int) System.Math.Round((GraphicsDevice.Viewport.Width / 40) * 1.5);
            int rectHeight = 4;
            int rectX = (int)System.Math.Round(playBall.Position.X - ((GraphicsDevice.Viewport.Width / 40) * 0.25));
            int rectY = (int)System.Math.Round(playBall.Position.Y + ( (GraphicsDevice.Viewport.Width / 40) * 1.1));
            Rectangle destRect = new Rectangle(rectX, rectY, rectWidth, rectHeight);


            _spriteBatch.Draw(((TextureContainer)Services.GetService(typeof(TextureContainer))).ColorStrip, destRect, new Rectangle(StripColors.YELLOW, 0, 1, 1), Color.Yellow);

            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
