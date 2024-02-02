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

        //  Obstacle Textures

        public LaserGolfGalaxies()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Ball ball = new Ball(this, new Point(0,0));

            Components.Add(ball);

            // Create the Map to play the Game on
            Map map = new Map(this, 0, 100, 100);

            // Convert the MapElements to drawable MonoGame Objects
            for (int i = 0; i < map.Obstacles.Count; i++)
            {
                LaserGolfObstacle currObs = map.Obstacles[i];
                currObs.ScaleWidth = 100;
                currObs.ScaleHeight = 100;
                Components.Add(currObs);
            }

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
            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
