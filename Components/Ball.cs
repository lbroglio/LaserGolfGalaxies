using LaserGolf.ConfigClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserGolf.Components
{
    internal class Ball : DrawableGameComponent
    {
        // Statics

        /// <summary>
        /// Variable which holds the base speed used to easily change the balls speed without adjusting values as they are scaled 
        /// in code.
        /// </summary>
        private readonly float BASE_SPEED = 150f;
        /// <summary>
        /// The Texture object which holds the loaded Texture for this image
        /// </summary>
        private static Texture2D _texture = null;
        /// <summary>
        /// The Color Strip object used for drawing the force indicator
        /// </summary>
        private static Texture2D _shotIndicatorTex = null;
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
        /// List of bools used for buffering inputs which change shot force
        /// </summary>
        private bool[] bufferInput;
        /// <summary>
        /// The player number that owns this ball
        /// </summary>
        private int _owner;
        /// <summary>
        /// Track whether to pass turn when moving
        /// </summary>
        private bool _turnCheck = false;




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
        public Ball(Game game, Vector2 startPosition, int ownerNum) : base(game)
        {   
            // Intialize starting values
            _velocity = Vector2.Zero;
            _position = startPosition;
            bufferInput = new bool[2];
            _owner = ownerNum;
        }

        public static double toRadians(double degrees)
        {
            return ( Math.PI / 180) * degrees;
        }

        protected override void LoadContent()
        {
            // Load  the texture for the ball but only once
            if (_texture == null)
            {
                _texture = Game.Content.Load<Texture2D>("LGGBallTexture");
            }



            // Add the color strip texture for use drawing the shot indicator
            if (_shotIndicatorTex == null)
            {
                _shotIndicatorTex = Game.Content.Load<Texture2D>("LGGShotIndicator");
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
            PlayScreen screen = (PlayScreen) Game.Services.GetService(typeof(PlayScreen));

            _scale = screen.Width / 40;

            if (ScaleX != 0.0)
            {
                _position.X = (int)Math.Round((_position.X / ScaleX) * screen.Width);
            }

            if (ScaleY != 0.0)
            {
                _position.Y = (int)Math.Round((_position.Y / ScaleY) * screen.Height);
            }


            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            // Only update if it is this player's turn
            int currPlayer = ((StateTracker)Game.Services.GetService(typeof(StateTracker))).CurrentPlayer;
            if (currPlayer == _owner)
            {
                PlayScreen screen = (PlayScreen)Game.Services.GetService(typeof(PlayScreen));


                // If the Ball is currently moving
                if (_velocity.X != 0 || _velocity.Y != 0)
                {

                    // Update position based on  Velocity
                    _position.X += (float)(_velocity.X * gameTime.ElapsedGameTime.TotalSeconds);
                    _position.Y += (float)(_velocity.Y * gameTime.ElapsedGameTime.TotalSeconds);

                    //  Degrade velocity by the Game's set friction value
                    _velocity.X -= (float)(_velocity.X * (((LaserGolfGalaxies)Game).FRICTION_COEFFICENT * gameTime.ElapsedGameTime.TotalSeconds));
                    _velocity.Y -= (float)(_velocity.Y * (((LaserGolfGalaxies)Game).FRICTION_COEFFICENT * gameTime.ElapsedGameTime.TotalSeconds));

                    //  Set velocity to zero as it gets close
                    if (Math.Abs(_velocity.X) < 1)
                    {
                        _velocity.X = 0;
                    }

                    if (Math.Abs(_velocity.Y) < 1)
                    {
                        _velocity.Y = 0;
                    }

                }
                // If the ball isn't moving handle input if sufficent time has passed since the last input was read
                else
                {
                    // Pass the turn if this player has already gone
                    if(_turnCheck == true)
                    {
                        ((StateTracker)Game.Services.GetService(typeof(StateTracker))).nextTurn();
                        _turnCheck = false;
                        return;
                    }

                    KeyboardState state = Keyboard.GetState();

                    // Handle angle change
                    if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
                    {
                        shotAngle += 5;

                        // Wrap around if over 
                        if (shotAngle >= 360)
                        {
                            shotAngle = shotAngle - 360;
                        }
                    }
                    else if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
                    {
                        shotAngle -= 5;

                        //Wrap around if below zero
                        if (shotAngle < 0)
                        {
                            shotAngle = 360 + shotAngle;
                        }
                    }

                    // Handle shot force change
                    if ((state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up)) && !bufferInput[0])
                    {
                        bufferInput[0] = true;
                        // 16 is maximum force
                        if (shotForce < 16)
                        {
                            shotForce += 1;
                        }
                    }

                    if ((state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down)) && !bufferInput[1])
                    {
                        bufferInput[1] = true;
                        // 1 is minimum force
                        if (shotForce > 1)
                        {
                            shotForce -= 1;
                        }
                    }

                    //Handle ball fired 
                    if (state.IsKeyDown(Keys.Space))
                    {
                        // Set to pass turn when movement ends
                        _turnCheck = true;

                        // Scale the speed to screen size
                        int screenHeight = screen.Height;
                        float scaledSpeed = BASE_SPEED * (screenHeight / 100);

                        // Scale the speed by the shot force
                        scaledSpeed /= (16f - (shotForce - 1));

                        // Build a rotation Matrix
                        float asRadians = (float)toRadians(shotAngle);
                        Matrix rot = Matrix.CreateRotationZ(asRadians);

                        //Create a velocity vector from the scaled speec and rotate it by a rotation matrix
                        //and use it to set the velocity of the ball
                        _velocity = Vector2.Transform(new Vector2(0, -1 * scaledSpeed), rot);

                        // Increment score for this ball's player
                        ((StateTracker)Game.Services.GetService(typeof(StateTracker))).Score[_owner] += 1;

                    }

                    // Debuffer input when keys are released
                    if (state.IsKeyUp(Keys.W) && state.IsKeyUp(Keys.Up) && bufferInput[0])
                    {
                        bufferInput[0] = false;
                    }

                    if (state.IsKeyUp(Keys.S) && state.IsKeyUp(Keys.Down) && bufferInput[1])
                    {
                        bufferInput[1] = false;
                    }

                }

                base.Update(gameTime);

            }


        }

        public override void Draw(GameTime gameTime)
        {

            // Only draw if it is this player's turn
            int currPlayer = ((StateTracker)Game.Services.GetService(typeof(StateTracker))).CurrentPlayer;
            if (currPlayer == _owner)
            {
                // Draw the ball itself
                ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(_texture, new Rectangle((int)Math.Round(_position.X), (int)Math.Round(_position.Y), _scale, _scale), new Rectangle(0, 0, _texture.Width, _texture.Height), Color.White);

                // If the ball is standing still and its your turn draw the shot indicator
                if (_velocity.X == 0 && _velocity.Y == 0 && !_turnCheck)
                {
                    // Sample a section of the texture
                    Rectangle srcRect = new Rectangle(0, 8 - (shotForce / 2), 1, 1);


                    int IND_WIDTH = 6;
                    // Calculate where the location of the destination rectangle

                    // Convert the x and y coordiantes to world coordiantes not relavtive to the ball
                    float newX = _position.X + (_scale / 2);
                    float newY = _position.Y + (_scale / 2);

                    // Move away in the correct direction
                    Vector2 drawLoc = new Vector2(-1 * IND_WIDTH / 2, -1f * ((_scale / 2) + 10 + (shotForce * 2)));
                    drawLoc = Vector2.Transform(drawLoc, Matrix.CreateRotationZ((float)toRadians(shotAngle)));
                    newX += drawLoc.X;
                    newY += drawLoc.Y;

                    // Create the destination rectangle
                    Rectangle destRec = new Rectangle((int)Math.Round(newX), (int)Math.Round(newY), IND_WIDTH, shotForce * 2);


                    ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(_shotIndicatorTex, destRec, srcRect, Color.White, (float)toRadians(shotAngle), new Vector2(0, 0), SpriteEffects.None, 0.0f);

                }
            }

            base.Draw(gameTime);

        }
    }
}
