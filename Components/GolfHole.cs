﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserGolf.ConfigClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LaserGolf.Components
{



    internal class GolfHole : DrawableGameComponent
    {
        // Statics
        /// <summary>
        /// The Texture object which holds the loaded Texture for this image
        /// </summary>
        private static Texture2D _texture = null;
        /// <summary>
        /// Retrieve the Texture Object holding the loaded texture for this ball
        /// </summary>
        public static Texture2D Texture
        {
            get { return _texture; }
        }


        /// <summary>
        /// The position of the hole in the world
        /// </summary>
        private Point _position;

        /// <summary>
        /// This is the assumed width of the screen at the time of creation. If not zero this value will be used to scale the width of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the width
        /// </summary>
        private int _scale = 0;

        /// <summary>
        /// This is the assumed width of the screen at the time of creation. If not zero this value will be used to scale the x position of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the x position
        /// </summary>
        private double _scaleX = 0.0;

        /// <summary>
        /// This is the assumed height of the screen at the time of creation. If not zero this value will be used to scale the  y position of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the  y position
        /// </summary>
        private double _scaleY = 0.0;

        /// <summary>
        /// Retrieve the array which stores the location of the individual pixels of this ball. Used for advanced collision detection.
        /// </summary>
        private Color[,] _pixelColor;

        /// <summary>
        /// Retrieve the array which stores the location of the individual pixels of this ball. Used for advanced collision detection.
        /// </summary>
        public Color[,] PixelColor
        {
            get { return _pixelColor; }
        }

        /// <summary>
        /// The radius of the hole in pixels
        /// </summary>
        public Point Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// This is the assumed width of the screen at the time of creation. If not zero this value will be used to scale the width of this obstacle.
        /// 
        /// If this is set to zero it will be viewed as logically false and no change will be made to the width
        /// </summary>
        public int Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }


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

        public GolfHole(Game game, Point worldPos) : base(game)
        {
            // Set the position
            _position = worldPos;

        }

        public  bool checkWin(Ball checkColliding)
        {

            int ballXPos = (int)Math.Round(checkColliding.Position.X);
            int ballYPos = (int)Math.Round(checkColliding.Position.Y);

            // Create a bounding box (rectangle) for the ball and the hole
            Rectangle ballBoundingBox = new Rectangle(ballXPos, ballYPos, checkColliding.Scale, checkColliding.Scale);
            Rectangle holeBoundingBox = new Rectangle(_position.X, _position.Y, _scale, _scale);

            // Check if this Wall intersects the bounding box for the Ball
            if (holeBoundingBox.Intersects(ballBoundingBox))
            {

                // If they do intersect do a pixel by pixel check for collision
                Rectangle intersect = Rectangle.Intersect(holeBoundingBox, ballBoundingBox);

                for (int i = intersect.X; i < intersect.X + intersect.Width; i++)
                {
                    for (int j = intersect.Y; j < intersect.Y + intersect.Height; j++)
                    {
                        int ballPoint = checkColliding.PixelColor[i - ballXPos, j - ballYPos].A;
                        //int wallPoint = imColor[x - (int)imPos.X, y - (int)imPos.Y].A;

                        if (ballPoint != 0)
                        {

                            return true;



                        }
                    }
                }
            }

            // If this point is reached the two bounding boxes do not intersect or the pixel by pixel check fails
            return false;
        }


        protected override void LoadContent()
        {
            // Load  the texture for the ball but only once
            if (_texture == null)
            {
                _texture = Game.Content.Load<Texture2D>("LGGHoleTexture");
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

            PlayScreen screen = (PlayScreen)Game.Services.GetService(typeof(PlayScreen));


            // Set scale values
            _scale = (int) 
                Math.Round((screen.Width / 40) * 1.5);

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


        public override void Draw(GameTime gameTime)
        {
            ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(_texture, new Rectangle(_position.X, _position.Y, _scale, _scale), new Rectangle(0, 0, _texture.Width, _texture.Height), Color.Green);


            // Create a lighting effects
            float lightScalar = 0.2f;
            // Effects for vertical axis locked
            ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(_texture, new Rectangle(_position.X - (_scale / 2), _position.Y - (_scale /  2), (_scale * 2), (_scale * 2)), new Rectangle(0, 0, _texture.Width, _texture.Height), Color.Green * lightScalar,  0.0f, new Vector2(0,0), SpriteEffects.None, 0.9f);

            base.Draw(gameTime);


        }


    }
}
