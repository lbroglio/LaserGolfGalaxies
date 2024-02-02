using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserGolf.Components
{
    internal class Ball : DrawableGameComponent
    {
        // Statics
        /// <summary>
        /// The Texture object which holds the loaded Texture for this image
        /// </summary>
        private static Texture2D _texture = null;
        /// <summary>
        /// Retrieve the Texture Object holding the loaded texture for this ball
        /// </summary>
        public static Texture2D Texture {
            get { return _texture; }
        }

        // Instance variables and properties
        /// <summary>
        /// Stores the location of this Ball on the screen
        /// </summary>
        private Point _position;
        /// <summary>
        /// Stores the current velocity (Change in position every second of game time) of this Ball
        /// </summary>
        private Vector2 _velocity;
        /// <summary>
        /// 2D Array which stores the location of the individual pixels of this ball. Used for advanced collision detection.
        /// </summary>
        private Color[,] _pixelColor;
        /// <summary>
        /// Vector used for scaling this Ball on the screen and for hit detection
        /// </summary>
        private Vector2 _scale;

        /// <summary>
        /// Retrieve or Set the Point object which holds the location of this Ball on the screen
        /// </summary>
        public Point Position
        {
            get { return _position; }
            set { _position = value; }
        }
        /// <summary>
        /// Retrieve or set the Vector2 object which holds the amount the Position of this ball changes every second of game time
        /// </summary>
        public Vector2 Velocity {
            get { return _velocity; }
            set { _velocity = value; }
        }
        /// <summary>
        /// Retrieve the array which stores the location of the individual pixels of this ball. Used for advanced collision detection.
        /// </summary>
        public Color[,] PixelColor {
            get { return _pixelColor; }
        }
        /// <summary>
        /// Vector used for scaling this Ball on the screen and for hit detection
        /// </summary>
        public Vector2 Scale { get{ return _scale; } }

        /// <summary>
        /// Create a new Ball Object
        /// </summary>
        /// <param name="game">The game object this Ball will be a part of</param>
        /// <param name="startPosition"> The location on the screen that this ball starts at</param>
        /// <param name="texture">The loaded Texture2D Object that hodls the Sprite for this Ball</param>
        public Ball(Game game, Point startPosition) : base(game)
        {   
            // Intialize starting values
            _velocity = Vector2.Zero;
            _position = startPosition;
        }

        protected override void LoadContent()
        {
            // Load  the texture for the ball but only once
            if(_texture == null){
                _texture = Game.Content.Load<Texture2D>("LGGBallTexture");
            }

            // Build the PixelColor arrays from the Texture
            _pixelColor = new Color[_texture.Width, _texture.Height];

            // Store Data from Texture in an array
            Color[] pixelData = new Color[_texture.Width * _texture.Height];
            _texture.GetData<Color>(pixelData);

            // Build a 2D Array to store the array holding the data
            for (int i = 0; i < _texture.Width; i++)
            {
                for (int j = 0; j < _texture.Height; j++)
                {
                    _pixelColor[i, j] = pixelData[i + j * _texture.Width];
                }

            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Update position based on  Velocity
           _position.X += (int)Math.Round(_velocity.X * gameTime.ElapsedGameTime.TotalSeconds);
           _position.Y += (int)Math.Round(_velocity.Y * gameTime.ElapsedGameTime.TotalSeconds);
           base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(_texture, new Rectangle(_position.X, _position.Y, 20, 20), new Rectangle(0, 0, _texture.Width, _texture.Height), Color.White);
            base.Draw(gameTime);

        }
    }
}
