using LaserGolf.ConfigClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaserGolf.Components.Obstacles
{
    /// <summary>
    /// Abstract class extended by Components which act as Obstacles for the ball. -- An Obstacle is defined as any Component which changes the movement of 
    /// the ball on collison.
    /// </summary>
    internal abstract class LaserGolfObstacle : DrawableGameComponent
    {

        /// <summary>
        /// Texture sampled when drawing Obstacles
        /// </summary>
        protected static Texture2D colorStrip = null;

        /// <summary>
        /// This is the assumed width of the screen at the time of creation. If not zero this value will be used to scale the width of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the width
        /// <br/>
        /// What scaling by width means is determined by the indiviudal subclass and subclasses are allowed to disallow (not implement) Load time scaling
        /// </summary>
        protected double _scaleWidth = 0.0;

        /// <summary>
        /// This is the assumed height of the screen at the time of creation. If not zero this value will be used to scale the height of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the height
        /// <br/>
        /// What scaling by height means is determined by the indiviudal subclass and subclasses are allowed to disallow (not implement) Load time scaling
        /// </summary>
        protected double _scaleHeight = 0.0;

        /// <summary>
        /// This is the assumed width of the screen at the time of creation. If not zero this value will be used to scale the x position of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the x position
        /// <br/>
        /// What scaling the x position means is determined by the indiviudal subclass and subclasses are allowed to disallow (not implement) Load time scaling
        /// </summary>
        protected double _scaleX = 0.0;

        /// <summary>
        /// This is the assumed height of the screen at the time of creation. If not zero this value will be used to scale the  y position of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the  y position
        /// <br/>
        /// What scaling the y position means is determined by the indiviudal subclass and subclasses are allowed to disallow (not implement) Load time scaling
        /// </summary>
        protected double _scaleY = 0.0;

        /// <summary>
        /// This is used to allow the user to finetune placement of an obstacle and apply shifts after load time scaling has occured. <br/>
        /// This can be used to overcome issues caused by placing scaled and fully or partially unscaled objects near each others.
        /// The shifts are held in a Vector 4 where: <br/>
        /// x = shift the x position after load time scaling <br/>
        /// y = shift the y position after load time scaling <br/>
        /// z = shift to the width after load time scaling <br/>
        /// w = shift to the height after load time scaling <br/>
        /// Like all load time scaling values how each of these values are used is up to the sublass and subclasses are allowed to disallow (not implement)
        /// these shifts
        /// </summary>
        protected Vector4 _constantShifts = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>
        /// This is used to allow the user to finetune placement of an obstacle and apply shifts after load time scaling has occured. <br/>
        /// This can be used to overcome issues caused by placing scaled and fully or partially unscaled objects near each others.
        /// The shifts are held in a Vector 4 where: <br/>
        /// x = shift the x position after load time scaling <br/>
        /// y = shift the y position after load time scaling <br/>
        /// z = shift to the width after load time scaling <br/>
        /// w = shift to the height after load time scaling <br/>
        /// Like all load time scaling values how each of these values are used is up to the sublass and subclasses are allowed to disallow (not implement)
        /// these shifts
        /// </summary>
        public Vector4 ConstantShifts
        {
            get { return _constantShifts; } 
            set { _constantShifts = value; }
        }

        /// <summary>
        /// This is the assumed width of the screen at the time of creation. If not zero this value will be used to scale the width of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the width
        /// <br/>
        /// What scaling by width means is determined by the indiviudal subclass and subclasses are allowed to disallow (not implement) Load time scaling
        /// </summary>
        public double ScaleWidth
        {
            get { return _scaleWidth; }
            set { _scaleWidth = value; }
        }

        /// <summary>
        /// This is the assumed height of the screen at the time of creation. If not zero this value will be used to scale the height of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the height
        /// <br/>
        /// What scaling by height means is determined by the indiviudal subclass and subclasses are allowed to disallow (not implement) Load time scaling
        /// </summary>
        public double ScaleHeight
        {
            get { return _scaleHeight; }
            set { _scaleHeight = value; }
        }

        /// <summary>
        /// This is the assumed width of the screen at the time of creation. If not zero this value will be used to scale the x position of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the x position
        /// <br/>
        /// What scaling the x position means is determined by the indiviudal subclass and subclasses are allowed to disallow (not implement) Load time scaling
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
        /// <br/>
        /// What scaling the y position means is determined by the indiviudal subclass and subclasses are allowed to disallow (not implement) Load time scaling
        /// </summary>
        public double ScaleY
        {
            get { return _scaleY; }
            set { _scaleY = value; }
        }


        /// <summary>
        /// Simple constructor which calls the super class constuctor
        /// </summary>
        /// <param name="game">The Game object this Component is a part of</param>
        protected LaserGolfObstacle(Game game) : base(game)
        {}

        /// <summary>
        /// Returns true if the Ball obejct given in checkColliding is colliding with this LaserGolfObstacle
        /// </summary>
        /// <param name="checkColliding"> Ball object to check the collison for</param>
        /// <returns></returns>
        public abstract bool checkCollides(Ball checkColliding);

        /// <summary>
        /// Returns a new Vector2 representing the Movement Vector of colliding after the collision takes place.
        /// This method will not check that a collision occurs and the behavior is undefined for Balls not currently colliding 
        /// with this ILaserGolfObstacle.
        /// </summary>
        /// <param name="colliding">The Ball that is currently collidind with this LaserGolfObstacle</param>
        /// <param name="gameTime">Gametime object at the time this collision happens</param>
        /// <returns></returns>
        public abstract Vector2 collideWith(Ball colliding, GameTime gameTime);


        /// <summary>
        /// Ovveride of LoadContents which loads the Texture used for Coloring Obstacles
        /// </summary>
        /// 
        protected override void LoadContent()
        {
            if (colorStrip == null)
            {
                colorStrip = ((TextureContainer)Game.Services.GetService(typeof(TextureContainer))).ColorStrip;
            }

            base.LoadContent();
        }

    }

    internal class Wall : LaserGolfObstacle
    {


        // Instance variables
        /// <summary>
        /// Backing Rectangle which holds this Walls location in the world
        /// </summary>
        protected Rectangle worldRect;

        /// <summary>
        /// Holds the color of this wall as a one by one rectangle matching a location on a color strip texture
        /// </summary>
        protected Rectangle color;

        /// <summary>
        /// Tracks when the ball is colliding with this wall to prevent double collision
        /// </summary>
        private bool collding = false;

        /// <summary>
        /// The amount that this Wall is rotated in degrees
        /// </summary>
        private float _rotation = 0.0f;

        protected SoundEffect _collisionSound;

        /// <summary>
        /// Create a new Wall and intialize its place in the world
        /// </summary>
        /// <param name="game">The Game object this Wall is a part of</param>
        /// <param name="worldPos">A Point holding this Wall's position in the world</param>
        /// <param name="width">The width (Size along the x axis) of this Wall in pixels</param>
        /// <param name="height">The height (Size along the y axis) of this Wall in pixels</param>
        public Wall(Game game, Point worldPos, int width, int height) : base(game)
        {
            // Create the backing rectangle for this Wall
            worldRect = new Rectangle(worldPos.X, worldPos.Y, width, height);

            //  Set the color as a location on the Color Strip
            color = new Rectangle(StripColors.WHITE, 0, 1, 1);

        }


        /// <summary>
        /// Create a new Wall and intialize its place in the world
        /// </summary>
        /// <param name="game">The Game object this Wall is a part of</param>
        /// <param name="worldPos">A Point holding this Wall's position in the world</param>
        /// <param name="width">The width (Size along the x axis) of this Wall in pixels</param>
        /// <param name="height">The height (Size along the y axis) of this Wall in pixels</param>
        /// <param name="rotation"> The number of degrees this rectangle is rotated </param>
        public Wall(Game game, Point worldPos, int width, int height, float rotation) : base(game)
        {
            // Create the backing rectangle for this Wall
            worldRect = new Rectangle(worldPos.X, worldPos.Y, width, height);

            //  Set the color as a location on the Color Strip
            color = new Rectangle(StripColors.WHITE, 0, 1, 1);


            _rotation = rotation;
        }

        public override bool checkCollides(Ball checkColliding)
        {
            int ballXPos = (int) Math.Round(checkColliding.Position.X);
            int ballYPos = (int)Math.Round(checkColliding.Position.Y);

            // Create a bounding box (rectangle) for the ball
            Rectangle boundingBox = new Rectangle(ballXPos, ballYPos, checkColliding.Scale, checkColliding.Scale);

            // Collision for unrotated wall
            // Check if this Wall intersects the bounding box for the Ball
            if(_rotation == 0 && worldRect.Intersects(boundingBox))
            {

                // If they do intersect do a pixel by pixel check for collision
                Rectangle intersect = Rectangle.Intersect(worldRect, boundingBox);

                for(int i =intersect.X; i < intersect.X + intersect.Width; i++)
                {
                    for(int j= intersect.Y; j < intersect.Y + intersect.Height; j++)
                    {
                        int ballPoint = checkColliding.PixelColor[i - ballXPos, j - ballYPos].A;
                        //int wallPoint = imColor[x - (int)imPos.X, y - (int)imPos.Y].A;

                        if(ballPoint != 0)
                        {
                            //Debug.WriteLine("X: " + worldRect.X + "\nY: " + worldRect.Y);
                            //Debug.WriteLine("W: " + worldRect.Width + "\nH: " + worldRect.Height);

                            if (!collding)
                            {
                                collding = true;
                                //TODO - Implement rotation
                                return true;
                            }


                        }
                    }
                }
            }
            else if(_rotation != 0)
            {
                // Calculate lower left corner for rect
                double leftX = worldRect.Height * Math.Cos(Ball.toRadians(90 + _rotation));
                double leftY = worldRect.Height * Math.Sin(Ball.toRadians(90 + _rotation));

                Vector2 lowerLeft = new Vector2((float) leftX + worldRect.X, (float) leftY + worldRect.Y);


                // Calculate x of rectangle at the point of the ball
                float slope = (lowerLeft.Y - worldRect.Y) / (lowerLeft.X - worldRect.X);
                float intercept = (worldRect.Y - (slope * worldRect.X));
                float xcheck = (checkColliding.Position.Y - intercept) / slope;

                // Get a point on the rect to compare to
                Vector2 closestPoint;
                // If the ball x point isn't within the rect set it to the corners 
                if(checkColliding.Position.Y <= worldRect.Y)
                {
                    closestPoint = new Vector2(worldRect.X, worldRect.Y);
                }
                else if (checkColliding.Position.Y >= lowerLeft.Y)
                {
                    closestPoint = new Vector2(lowerLeft.X, lowerLeft.Y);
                }
                else
                {
                    closestPoint = new Vector2(xcheck, checkColliding.Position.Y);
                }

                //Get the dist from the cloest point to the ball
                double dist = Math.Pow((closestPoint.X - checkColliding.Position.X), 2) + Math.Pow((closestPoint.Y - checkColliding.Position.Y), 2);
                dist = Math.Sqrt(dist);

                if (Math.Abs(dist) <= checkColliding.Scale / 2.0f){
                    return true;
                }

            }

            // If this point is reached the bounding box and the wall do not intersect so false is returned
            collding = false;
            return false; 
        }

        public override Vector2 collideWith(Ball colliding, GameTime gameTime)
        {
            /*
            Vector2 c1Normalized = Vector2.Normalize(imVel);
            Vector3 c1Normal3D = Vector3.Cross(new Vector3(c1Normalized, 0), new Vector3(0, 0, -1));
            Vector2 c1Norm = new Vector2(c1Normal3D.X, c1Normal3D.Y);
            */


            Vector2 normal;

            // Collision for unrotated
            if(_rotation == 0)
            {
                // Get normal vector for the side facing the ball
                Vector2 diffVector = new Vector2(colliding.Position.X - worldRect.X, colliding.Position.Y - worldRect.Y);
                if (diffVector.Y < 0 || diffVector.Y > worldRect.Height)
                {
                    normal = Vector2.UnitY;
                }
                else
                {
                    normal = Vector2.UnitX;
                }
            }
            else
            {
                // Calculate the slope
                double leftX = worldRect.Height * Math.Cos(Ball.toRadians(90 + _rotation));
                double leftY = worldRect.Height * Math.Sin(Ball.toRadians(90 + _rotation));

                Vector2 lowerLeft = new Vector2((float)leftX + worldRect.X, (float)leftY + worldRect.Y);

                // Calculate x of rectangle at the point of the ball
                Vector2 slope = new Vector2(worldRect.X, worldRect.Y) - lowerLeft;
                slope.Normalize();

                Vector3 normal3D = Vector3.Cross(new Vector3(slope, 0), new Vector3(0, 0, -1));
                normal = new Vector2(normal3D.X, normal3D.Y);
            }

            // play collision audio
            _collisionSound.Play();

            return Vector2.Reflect(colliding.Velocity, normal);
        }

        protected override void LoadContent()
        {
            PlayScreen screen = (PlayScreen)Game.Services.GetService(typeof(PlayScreen));

            _collisionSound = Game.Content.Load<SoundEffect>("wallCollisionSound");

            // Scale the sides and position if indicated
            if (ScaleWidth != 0.0)
            {
                worldRect.Width = (int) Math.Round((worldRect.Width / ScaleWidth) * screen.Width);
            }

            if (ScaleHeight != 0.0)
            {
                worldRect.Height = (int) Math.Round((worldRect.Height / ScaleHeight) * screen.Height);
            }

            if (ScaleX != 0.0)
            {
                worldRect.X = (int)Math.Round((worldRect.X / ScaleX) * screen.Width);
            }

            if (ScaleY != 0.0)
            {
                worldRect.Y = (int)Math.Round((worldRect.Y / ScaleY) * screen.Height);
            }

            worldRect = new Rectangle((int)Math.Round(worldRect.X + ConstantShifts.X), (int)Math.Round(worldRect.Y + ConstantShifts.Y), (int)Math.Round(worldRect.Width + ConstantShifts.Z), (int)Math.Round(worldRect.Height + ConstantShifts.W));

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {

            ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(colorStrip, worldRect, color, Color.White, (float)Ball.toRadians(_rotation),  new Vector2(0,0), SpriteEffects.None, 0.1f);

            // Create a lighting effects
            float lightScalar = 0.2f;
            // Effects for vertical axis locked
            if(worldRect.Height > worldRect.Width)
            {

                Rectangle leftLightEffect = new Rectangle(worldRect.X + worldRect.Width, worldRect.Y, worldRect.Width, worldRect.Height);
                Rectangle rightLightEffect = new Rectangle(worldRect.X - worldRect.Width, worldRect.Y, worldRect.Width, worldRect.Height);

                ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(colorStrip, leftLightEffect, color, Color.White * lightScalar, (float)Ball.toRadians(_rotation), new Vector2(0, 0), SpriteEffects.None, 0.9f);
                ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(colorStrip, rightLightEffect, color, Color.White * lightScalar, (float)Ball.toRadians(_rotation), new Vector2(0, 0), SpriteEffects.None, 0.9f);

            }
            // Effects for horizontal axis locked
            else if (worldRect.Width > worldRect.Height)
            {

                Rectangle leftLightEffect = new Rectangle(worldRect.X, worldRect.Y + worldRect.Height, worldRect.Width, worldRect.Height);
                Rectangle rightLightEffect = new Rectangle(worldRect.X, worldRect.Y - worldRect.Height, worldRect.Width, worldRect.Height);

                ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(colorStrip, leftLightEffect, color, Color.White * lightScalar, (float)Ball.toRadians(_rotation), new Vector2(0, 0), SpriteEffects.None, 0.9f);
                ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(colorStrip, rightLightEffect, color, Color.White * lightScalar, (float)Ball.toRadians(_rotation), new Vector2(0, 0), SpriteEffects.None, 0.9f);

            }



            base.Draw(gameTime);
        }
    }

    internal class Slope : LaserGolfObstacle
    {


        //Statics
        /// <summary>
        /// Texture used for drawing red Slopes
        /// </summary>
        private static Texture2D _redTexture;

        /// <summary>
        /// Texture used for drawing blue Slopes
        /// </summary>
        private static Texture2D _blueTexture;

        // Instance variables
        /// <summary>
        /// Backing Rectangle which holds this Slope's location in the world
        /// </summary>
        private Rectangle worldRect;

        /// <summary>
        /// Boolean used for determine to use a red or blue texture <br/>
        /// true == red <br/>
        /// false == blue
        /// </summary>
        private bool _isRedTex;


        /// <summary>
        /// Vector2 representing the change in velocity (in x and y direction) a Ball on this slope experiences. 
        /// <br/>
        /// A negative x component means this slopo moves upwards to the right.  <br/>
        /// A negative y component means this slopo moves upwards towards the bottom of the screen. <br/>
        /// A positive x component means this slopo moves upwards to the left.  <br/>
        /// A positive y component means this slopo moves upwards towards the top of the screen. 
        /// </summary>
        private Vector2 _velocityChange;

        /// <summary>
        /// Vector2 which holds a precise location which is scaled at load time to get the location of the world rec
        /// </summary>
        private Vector2 _locationPreLoad;

        /// <summary>
        /// Vector2 which holds precise dimensions that are scaled at load time to get the width and height of the worldRect
        /// </summary>
        private Vector2 _dimensionsPreLoad;


        /// <summary>
        /// Tracks when the ball is colliding with this Slope to prevent double collision
        /// </summary>
        //private bool collding = false;

        /// <summary>
        /// Create a new Slope and intialize its place in the world
        /// </summary>
        /// <param name="game">The Game object this Wall is a part of</param>
        /// <param name="worldPos">A Vector2 holding this Wall's position in the world</param>
        /// <param name="width">The width (Size along the x axis) of this Wall in pixels</param>
        /// <param name="height">The height (Size along the y axis) of this Wall in pixels</param>
        /// <param name="velocityChange"> Vector2 holding the x and y components of the change in velopcity experienced when on this slope </param>
        public Slope(Game game, Vector2 worldPos, float width, float height, Vector2 velocityChange) : base(game)
        {
            // Create the backing rectangle for this Wall
           // worldRect = new Rectangle(worldPos.X, worldPos.Y, width, height);

            // Set up the postion and dimensions for this slope
            _locationPreLoad = worldPos;
            _dimensionsPreLoad = new Vector2(width, height);

            this._velocityChange = velocityChange;

            //  Set the color as a location on the Color Strip

            if (Math.Abs(velocityChange.X) > Math.Abs(velocityChange.Y))
            {
                if(velocityChange.X > 0)
                {
                    _isRedTex = false;

                }
                else
                {
                    _isRedTex = true;

                }
            }
            else if (Math.Abs(velocityChange.Y) > Math.Abs(velocityChange.X))
            {
                if (velocityChange.Y < 0)
                {
                    _isRedTex = false;

                }
                else
                {
                    _isRedTex = true;

                }
            }
            else
            {
                if (velocityChange.X + velocityChange.Y > 0)
                {
                    _isRedTex = true;
                }
                else
                {
                    _isRedTex = false;
                }
            }

        }

        public override bool checkCollides(Ball checkColliding)
        {
            int ballXPos = (int)Math.Round(checkColliding.Position.X);
            int ballYPos = (int)Math.Round(checkColliding.Position.Y);

            // Create a bounding box (rectangle) for the ball
            Rectangle boundingBox = new Rectangle(ballXPos, ballYPos, checkColliding.Scale, checkColliding.Scale);

            // Check if this Wall intersects the bounding box for the Ball
            if (worldRect.Intersects(boundingBox))
            {

                // If they do intersect do a pixel by pixel check for collision
                Rectangle intersect = Rectangle.Intersect(worldRect, boundingBox);

                for (int i = intersect.X; i < intersect.X + intersect.Width; i++)
                {
                    for (int j = intersect.Y; j < intersect.Y + intersect.Height; j++)
                    {
                        int ballPoint = checkColliding.PixelColor[i - ballXPos, j - ballYPos].A;
                        //int wallPoint = imColor[x - (int)imPos.X, y - (int)imPos.Y].A;

                        if (ballPoint != 0)
                        {

                            //TODO - Implement rotation
                            return true;



                        }
                    }
                }
            }

            // If this point is reached the bounding box and the wall do not intersect so false is returned
            return false;
        }

        public override Vector2 collideWith(Ball colliding, GameTime gameTime)
        {

            return colliding.Velocity + (_velocityChange * ((float) gameTime.ElapsedGameTime.TotalSeconds));
        }

        protected override void LoadContent()
        {
            PlayScreen screen = (PlayScreen)Game.Services.GetService(typeof(PlayScreen));

            if (_redTexture == null)
            {
                _redTexture = Game.Content.Load<Texture2D>("LGGSlopeTextureRed");
            }

            if (_blueTexture == null)
            {
                _blueTexture = Game.Content.Load<Texture2D>("LGGSlopeTextureBlue");
            }

            // Scale the sides and position if indicated
            if (ScaleWidth != 0.0)
            {
                //worldRect.Width = (int)Math.Round((worldRect.Width / ScaleWidth) * screen.Width);
                _dimensionsPreLoad.X = (float) ((_dimensionsPreLoad.X / ScaleWidth) * screen.Width);

            }

            if (ScaleHeight != 0.0)
            {
                _dimensionsPreLoad.Y = (float)((_dimensionsPreLoad.Y / ScaleWidth) * screen.Width);
               // worldRect.Height = (int)Math.Round((worldRect.Height / ScaleHeight) * screen.Height);
            }

            if (ScaleX != 0.0)
            {
                _locationPreLoad.X = (float) ((_locationPreLoad.X / ScaleX) * screen.Width);
                //worldRect.X = (int)Math.Round((worldRect.X / ScaleX) * screen.Width);
            }

            if (ScaleY != 0.0)
            {
                _locationPreLoad.Y = (float) ((_locationPreLoad.Y / ScaleY) * screen.Height);
                //worldRect.Y = (int)Math.Round((worldRect.Y / ScaleY) * screen.Height);
            }

            //  Create the backing rectangle for this Slope
            worldRect = new Rectangle((int)Math.Round(_locationPreLoad.X + _constantShifts.X) , (int)Math.Round(_locationPreLoad.Y + _constantShifts.Y), (int)Math.Round(_dimensionsPreLoad.X + _constantShifts.Z) , (int)Math.Round(_dimensionsPreLoad.Y + _constantShifts.W));

            

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            //  Find angle to rotate the slope by based on the direction of its slope
            float theta;

            if (_velocityChange.X == 0)
            {
                if (_velocityChange.Y > 0)
                {
                    theta = (float)Ball.toRadians(180);
                }
                else
                {
                    theta = 0;
                }
            }
            else if (_velocityChange.Y == 0)
            {
                if (_velocityChange.X > 0)
                {
                    theta = (float)Ball.toRadians(90);
                }
                else
                {
                    theta = (float)Ball.toRadians( -90);
                }
            }
            else
            {
                theta = (float) Math.Atan(_velocityChange.Y / _velocityChange.X);
            }


            // Draw texture depending on direction
            if (_isRedTex)
            {
                Vector2 origin = new(_redTexture.Width / 2f, _redTexture.Height / 2f);
                Rectangle dest = new Rectangle(worldRect.X, worldRect.Y, worldRect.Width, worldRect.Height);
                dest.X = (int)(dest.X + dest.Width / 2f);
                dest.Y = (int)(dest.Y + dest.Height / 2f);
                ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(_redTexture, dest, null, Color.White, theta, origin, SpriteEffects.None, 0.5f);

            }
            else
            {
                Vector2 origin = new(_blueTexture.Width / 2f, _blueTexture.Height / 2f);
                Rectangle dest = new Rectangle(worldRect.X, worldRect.Y, worldRect.Width, worldRect.Height);
                dest.X = (int)(dest.X + dest.Width / 2f);
                dest.Y = (int)(dest.Y + dest.Height / 2f);
                ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(_blueTexture, dest, null, Color.White, theta, origin, SpriteEffects.None, 0.5f);

            }

            base.Draw(gameTime);
        }
    }

    internal class BorderField : Wall
    {
        /// <summary>
        /// Amount to scale the color by to control opacity
        /// </summary>
        private float _colorScale = 0.5f;

        public BorderField(Game game, Point worldPos, int width, int height) : base(game, worldPos, width, height)
        {
            //  Set the color as a location on the Color Strip
            color = new Rectangle(StripColors.BLUE, 0, 1, 1);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _collisionSound = Game.Content.Load<SoundEffect>("borderCollisionSound");
        }

        public override bool checkCollides(Ball checkColliding)
        {
            bool ret = base.checkCollides(checkColliding);

            if(ret)
            {
                _colorScale = 1.0f;
            }
            else
            {
                _colorScale = 0.5f;
            }

            return ret;
        }


        public override void Draw(GameTime gameTime)
        {

            ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(colorStrip, worldRect, color, Color.Blue * _colorScale);

        }


    }
}
