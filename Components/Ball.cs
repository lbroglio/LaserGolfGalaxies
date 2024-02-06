using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
        /// The Color Strip object used for drawing the force indicator
        /// </summary>
        private static Texture2D _colorStrip = null;
        /// <summary>
        /// Retrieve the Texture Object holding the loaded texture for this ball
        /// </summary>
        public static Texture2D Texture {
            get { return _texture; }
        }


        // Instance variables and properties
        /// <summary>
        /// Current user selected angle of the shot on this ball
        /// </summary>
        private double shotAngle = 0.0;
        /// <summary>
        /// Current user selected force of the shot on this wall
        /// </summary>
        private int shotForce = 0;
        /// <summary>
        /// Stores the location of this Ball on the screen
        /// </summary>
        private Vector2 _position;
        /// <summary>
        /// Stores the current velocity (Change in position every second of game time) of this Ball
        /// </summary>
        private Vector2 _velocity;
        /// <summary>
        /// 2D Array which stores the location of the individual pixels of this ball. Used for advanced collision detection.
        /// </summary>
        private Color[,] _pixelColor;
        /// <summary>
        /// Number of pixels to scale the diameter of this Ball to for hit detection and when it is drawn on the screen 
        /// </summary>
        private int _scale;
        /// <summary>
        /// This is the assumed width of the screen at the time of creation. If not zero this value will be used to scale the x position of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the x position
        /// </summary>
        protected double _scaleX = 0.0;

        /// <summary>
        /// This is the assumed height of the screen at the time of creation. If not zero this value will be used to scale the  y position of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the  y position
        /// </summary>
        protected double _scaleY = 0.0;

        /// <summary>
        /// Retrieve or Set the Point object which holds the location of this Ball on the screen
        /// </summary>
        public Vector2 Position
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
        /// Number of pixels to scale the diameter of this Ball to for hit detection and when it is drawn on the screen 
        /// </summary>
        public int Scale { get{ return _scale; } }
        /// <summary>
        /// This is the assumed width of the screen at the time of creation. If not zero this value will be used to scale the x position of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the x position
        /// </summary>
        public double ScaleX
        {
            get { return _scaleX; }
            set { _scaleX = value; }
        }

        /// <summary>
        /// This is the assumed height of the screen at the time of creation. If not zero this value will be used to scale the  y position of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the  y position
        /// </summary>
        public double ScaleY
        {
            get { return _scaleY; }
            set { _scaleY = value; }
        }


        /// <summary>
        /// Create a new Ball Object
        /// </summary>
        /// <param name="game">The game object this Ball will be a part of</param>
        /// <param name="startPosition"> The location on the screen that this ball starts at</param>
        /// <param name="texture">The loaded Texture2D Object that hodls the Sprite for this Ball</param>
        public Ball(Game game, Vector2 startPosition) : base(game)
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


            // Set scale values
            _scale = Game.GraphicsDevice.Viewport.Width / 40;

            if (ScaleX != 0.0)
            {
                _position.X = (int)Math.Round((_position.X / ScaleX) * Game.GraphicsDevice.Viewport.Width);
            }

            if (ScaleY != 0.0)
            {
                _position.Y = (int)Math.Round((_position.Y / ScaleY) * Game.GraphicsDevice.Viewport.Height);
            }


            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            
            // If the Ball is currently moving
            if(_velocity.X != 0 && _velocity.Y != 0)
            {
                // Update position based on  Velocity
                _position.X += (int)Math.Round(_velocity.X * gameTime.ElapsedGameTime.TotalSeconds);
                _position.Y += (int)Math.Round(_velocity.Y * gameTime.ElapsedGameTime.TotalSeconds);

                //  Degrade velocity by the Games set friction values

                // Degrade the x velocity proportional to its percentage of the total velocity
                int xDegradation = (int) Math.Round(((LaserGolfGalaxies)Game).FRICTION_COEFFICENT * (_velocity.X / (_velocity.X + _velocity.Y)));
                int yDegradation = (int) Math.Round(((LaserGolfGalaxies)Game).FRICTION_COEFFICENT * (_velocity.Y / (_velocity.X + _velocity.Y)));

                if (_velocity.X > 0)
                {
                    _velocity.X -= (float)(xDegradation * gameTime.ElapsedGameTime.TotalSeconds);

                    // If the velocity went below zero set it to zero 
                    if (_velocity.X < 0)
                    {
                        _velocity.X = 0;
                    }
                }
                else                
                {
                    _velocity.X += (float)(xDegradation * gameTime.ElapsedGameTime.TotalSeconds);

                    // If the velocity went above zero set it to zero 
                    if (_velocity.X > 0)
                    {
                        _velocity.X = 0;
                    }
                }

                if (_velocity.Y > 0)
                {
                    _velocity.Y -= (float)(yDegradation * gameTime.ElapsedGameTime.TotalSeconds);

                    // If the velocity went below zero set it to zero 
                    if (_velocity.Y < 0)
                    {
                        _velocity.Y = 0;
                    }
                }

                else
                {
                    _velocity.Y += (float)(yDegradation * gameTime.ElapsedGameTime.TotalSeconds);

                    // If the velocity went above zero set it to zero 
                    if (_velocity.Y > 0)
                    {
                        _velocity.Y = 0;
                    }
                }
            }
            // If the ball isn't moving handle input
            else
            {
                KeyboardState state = Keyboard.GetState();

                // Handle angle change
                if(state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Right))
                {
                    shotAngle += 10;

                    // Wrap around if over 
                    if(shotAngle >= 360)
                    {
                        shotAngle = shotAngle - 360;
                    }
                }
                else if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Left))
                {
                    shotAngle -= 10;

                    //Wrap around if below zero
                    if (shotAngle < 0)
                    {
                        shotAngle = 360 + shotAngle;
                    }
                }

                // Handle shot force change
                if(state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up))
                {
                    // 16 is maximum force
                    if(shotForce < 16)
                    {
                        shotForce += 1;
                    }
                }
                else if(state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))
                {
                    // 1 is minimum force
                    if (shotForce > 1)
                    {
                        shotForce -= 1;
                    }
                }

                //Handle ball fired 
                if (state.IsKeyDown(Keys.Space))
                {

                }

                base.Update(gameTime);
        }

        }

        public override void Draw(GameTime gameTime)
        {
            ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(_texture, new Rectangle((int)Math.Round(_position.X), (int)Math.Round(_position.Y), _scale, _scale), new Rectangle(0, 0, _texture.Width, _texture.Height), Color.White);
            base.Draw(gameTime);

        }
    }
}
