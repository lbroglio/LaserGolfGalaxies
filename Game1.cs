using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace LaserGolf
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<Rectangle> gameWalls;

        // Textures
        private Texture2D colorStrip;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            colorStrip = Content.Load<Texture2D>("colorStrip");

            // TODO: use this.Content to load your game content here

            Map map = new Map(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            // Convert the MapElements to drawable MonoGame Objects
            for(int i=0; i < map.GetElements().Count; i++)
            {
                MapElement curr = map.GetElements()[i];
                gameWalls = new List<Rectangle>();


                if (curr.type == ElementTypes.WALL)
                {
                    Rectangle wall = new Rectangle((int)(curr.position.X), (int) (curr.position.Y), (int)(curr.size.X), (int)(curr.size.Y));
                    gameWalls.Add(wall);
                
                }   
            }
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(colorStrip, northWall, wallColor, Color.White);
            _spriteBatch.Draw(colorStrip, southWall, wallColor, Color.White)
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
