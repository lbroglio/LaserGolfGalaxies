using LaserGolf.Components;
using LaserGolf.Components.Obstacles;
using LaserGolf.ConfigClasses;
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
        private List<LaserGolfObstacle> _obstacles;
        private Ball _playBall;
        private Point _startLoc;
        private PlayScreen _screen;

        /// <summary>
        /// Percentage to shrink velocity by every second 
        /// </summary>
        public readonly double FRICTION_COEFFICENT = 0.75;

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
            Map map = new Map(this, 0, 3.0 / 4.0);

            // TODO: Add your initialization logic here
            _playBall = new Ball(this, map.BallPos);
            _playBall.ScaleX = 100;
            _playBall.ScaleY = 75;

            Components.Add(_playBall);

            _obstacles = new List<LaserGolfObstacle>();


            // Convert the MapElements to drawable MonoGame Objects
            for (int i = 0; i < map.Obstacles.Count; i++)
            {
                LaserGolfObstacle currObs = map.Obstacles[i];
                Components.Add(currObs);
                _obstacles.Add(currObs);
            }

            Components.Add(map.Hole);


            _screen = new PlayScreen(1080, 810);
            Services.AddService(typeof(PlayScreen), _screen);


            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 1010;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            Services.AddService(typeof(SpriteBatch), _spriteBatch);
            base.LoadContent();

            //Set the start loc 
            _startLoc = new Point((int)System.Math.Round(_playBall.Position.X), (int)System.Math.Round(_playBall.Position.Y));



        }

        protected override void Update(GameTime gameTime)
        {

  

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
           

            base.Update(gameTime);

            //Check collision for the ball and the obstacles
            for(int i = 0;i < _obstacles.Count; i++)
            {
                LaserGolfObstacle currOb = _obstacles[i];
                if(currOb.checkCollides(_playBall))
                {
                    
                    _playBall.Velocity = currOb.collideWith(_playBall, gameTime);
                }
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.BackToFront);
            // Draw the starting rectangle for the golf ball
            int rectWidth = (int) System.Math.Round((_screen.Width / 40) * 1.5);
            int rectHeight = 4;
            int rectX = (int)System.Math.Round(_startLoc.X - ((_screen.Width / 40) * 0.25));
            int rectY = (int)System.Math.Round(_startLoc.Y + ((_screen.Width / 40) * 1.1));
            Rectangle destRect = new Rectangle(rectX, rectY, rectWidth, rectHeight);


            _spriteBatch.Draw(((TextureContainer)Services.GetService(typeof(TextureContainer))).ColorStrip, destRect, new Rectangle(StripColors.YELLOW, 0, 1, 1), Color.Yellow);

            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
