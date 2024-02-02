using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
        /// Simple constructor which calls the super class constuctor
        /// </summary>
        /// <param name="game">The Game object this Component is a part of</param>
        protected LaserGolfObstacle(Game game) : base(game)
        {}

        /// <summary>
        /// Returns true if the Ball obejct given in checkColliding is colliding with this ILaserGolfObstacle
        /// </summary>
        /// <param name="checkColliding"> Ball object to check the collison for</param>
        /// <returns></returns>
        public abstract bool checkCollides(Ball checkColliding);

        /// <summary>
        /// Returns a new Vector2 representing the Movement Vector of colliding after the collision takes place.
        /// This method will not check that a collision occurs and the behavior is undefined for Balls not currently colliding 
        /// with this ILaserGolfObstacle.
        /// </summary>
        /// <param name="colliding">The Ball that is currently collidind with this ILaserGolfObstacle</param>
        /// <returns></returns>
        public abstract Vector2 collideWith(Ball colliding);

        /// <summary>
        /// Ovveride of LoadContents which loads the Texture used for Coloring Obstacles
        /// </summary>
        /// 
        protected override void LoadContent()
        {
            if (colorStrip == null)
            {
                colorStrip = Game.Content.Load<Texture2D>("LGGColorStrip");
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
        private Rectangle worldRect;

        /// <summary>
        /// Holds the color of this wall as a one by one rectangle matching a location on a color strip texture
        /// </summary>
        private Rectangle color;

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
            color = new Rectangle(9, 0, 1, 1);

        }

        /// <summary>
        /// Create a new Wall and intialize its place in the world. Also set its ScaleX and ScaleY properties
        /// </summary>
        /// <param name="game">The Game object this Wall is a part of</param>
        /// <param name="worldPos">A Point holding this Wall's position in the world</param>
        /// <param name="width">The width (Size along the x axis) of this Wall in pixels</param>
        /// <param name="height">The height (Size along the y axis) of this Wall in pixels</param>
        /// <param name="scaleWidth">The assumed screen width when this function is called. 
        /// Will be later used for scaling the width of the wall. Set to 0 to if width should not be scaled </param>
        /// <param name="scaleHeight">The assumed screen height when this function is called. 
        /// Will be later used for scaling the height of the wall. Set to 0 to if height should not be scaled </param>
        public Wall(Game game, Point worldPos, int width, int height, double scaleWidth, double scaleHeight) : base(game)
        { 
            // Create the backing rectangle for this Wall
            worldRect = new Rectangle(worldPos.X, worldPos.Y, width, height);

            //  Set the color as a location on the Color Strip
            color = new Rectangle(0, 0, 1, 1);

            //  Set the backing variables for the ScaleX and ScaleY properties
            _scaleWidth = scaleWidth;
            _scaleHeight = scaleHeight;

        }

        public override bool checkCollides(Ball checkColliding)
        {
            // Create a bounding box (rectangle) for the ball
            Rectangle boundingBox = new Rectangle(checkColliding.Position.X, checkColliding.Position.Y, Ball.Texture.Width, Ball.Texture.Height);

            // Check if this Wall intersects the bounding box for the Ball
            if(worldRect.Intersects(boundingBox))
            {
                // If they do intersect do a pixel by pixel check for collision
                Rectangle intersect = Rectangle.Intersect(worldRect, boundingBox);

                for(int i =intersect.X; i < intersect.X + intersect.Width; i++)
                {
                    for(int j= intersect.Y; j < intersect.Y + intersect.Height; j++)
                    {
                        int ballPoint = checkColliding.PixelColor[i - checkColliding.Position.X, j - checkColliding.Position.Y].A;
                        //int wallPoint = imColor[x - (int)imPos.X, y - (int)imPos.Y].A;

                        if(ballPoint != 0)
                        {
                            // TODO - Implement a check to prevent double collision
                            return true;

                        }
                    }
                }
            }

            // If this point is reached the bounding box and the wall do not intersect so false is returned
            return false; 
        }

        public override Vector2 collideWith(Ball colliding)
        {
            throw new NotImplementedException();
        }

        protected override void LoadContent()
        { 
            // Scale the sides if indicated
            if (ScaleWidth != 0.0)
            {
                worldRect.Width = (int) Math.Round((worldRect.Width / ScaleWidth) * Game.GraphicsDevice.Viewport.Width);
            }

            if (ScaleHeight != 0.0)
            {
                worldRect.Height = (int) Math.Round((worldRect.Height / ScaleHeight) * Game.GraphicsDevice.Viewport.Height);
            }


            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {

            ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(colorStrip, worldRect, color, Color.White);

            base.Draw(gameTime);
        }
    }
}
