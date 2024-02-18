using LaserGolf.Components;
using LaserGolf.Components.Obstacles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace LaserGolf.Maps
{
    /// <summary>
    /// Enum which holds the different types that map can be. Used for defining what kind of obstacles and structure a procedurally created map should have.
    /// </summary>
    public enum MapTypes
    {
        TUNNEL,
        MAZE
    }

    /// <summary>
    /// Represents an object/element placed on a <see cref="Map"/>. Can be obstacles, the start point, or the hole.
    /// </summary>


    /// <summary>
    /// This class is used to represent a map that can be created by the game.  Holds information on what kind of obstacles are on the map and where they are.
    /// Handles creating a procedurally generated map or can serve a set of pre built maps.
    /// </summary>
    internal class Map
    {
        /// <summary>
        /// List of the Obstacles that are on this map. 
        /// </summary>
        private List<LaserGolfObstacle> _obstacles;

        /// <summary>
        /// The hole (end) component of this Map
        /// </summary>
        private GolfHole _hole;

        /// <summary>
        /// Vector holding where on the map the ball starts
        /// </summary>
        private Vector2 _ballPos;

        /// <summary>
        /// List of the Obstacles that are on this map. 
        /// </summary>
        public List<LaserGolfObstacle> Obstacles { get { return _obstacles; } }



        /// <summary>
        /// List of the Obstacles that are on this map. 
        /// </summary>
        public GolfHole Hole { get { return _hole; } }

        /// <summary>
        /// Vector holding where on the map the ball starts
        /// </summary>
        public Vector2 BallPos
        {
            get { return _ballPos; }
            set { _ballPos = value; }
        }

        /// <summary>
        /// Create a procedurally generated map
        /// </summary>
        public Map(Game game, double aspectRatio)
        {

        }

        /// <summary>
        /// Create a Map with a premade layout
        /// </summary>
	    /// <param name="game"> The Game object the components of this Map will exist on</param>
        /// <param name="preset">Number corresponding to the layout you would like.
        /// Layouts:
        /// 0 = Standard tunnel map
        /// </param>
        /// <param name="screenRatioMult"> A number which you can multiply the width of the play screen by to get the height. 
        /// Used to prevent stretching when scaling
        /// </param>

        public Map(Game game, int preset, double screenRatioMult)
        {
            _obstacles = new List<LaserGolfObstacle>();

            if (preset == 0)
            {
                createStandardTunnel(game, screenRatioMult);
            }
        }

        /// <summary>
        /// Function to create the standard tunnel preset (Preset 0)
        /// </summary>
        private void createStandardTunnel(Game game, double screenRatioMult)
        {
            int lineSize = 8;

            int screenSize = 100;

            // Set the far left wall for the tunnel

            // Calculate the locations for the wall by percentage of screen and round them
            int wallPosX = (int)System.Math.Round(screenSize * 0.1, 0);
            int wallPosY = (int)System.Math.Round(screenSize * 0.1, 0);
            int wallSize = (int)System.Math.Round(screenSize * 0.5, 0);

            // Create the Wall Object
            Wall leftWall = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            leftWall.ScaleHeight = (int) Math.Round(screenSize * screenRatioMult);
            leftWall.ScaleX = screenSize;
            leftWall.ScaleY = (int) Math.Round(screenSize * screenRatioMult);

            // Set the lower right wall of the tunnel start

            // Shift the starting position and size for the wall
            wallPosX += ((screenSize / 40) * 3);
            wallPosY += ((screenSize / 40) * 3);// + (lineSize/ screenSize);
            wallSize -= ((screenSize / 40) * 3) + (lineSize/ screenSize);

            // Create the wall Object
            Wall rightWall = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            rightWall.ScaleHeight = (int) Math.Round(screenSize * screenRatioMult);
            rightWall.ScaleX = screenSize;
            rightWall.ScaleY = (int) Math.Round(screenSize * screenRatioMult);


            // Create A wall to enclose the top of the tunnel

            //Calculate the location and size for the wall
            wallPosX = (int) (System.Math.Round(screenSize * 0.1, 0));
            wallPosY = (int) System.Math.Round((screenSize * 0.1) +  (lineSize/ screenSize), 0);
            wallSize = (int) System.Math.Round(screenSize * 0.3, 0);

            // Create the Wall Object
            Wall tunnelTop = new Wall(game, new Point(wallPosX, wallPosY), wallSize, lineSize);
            tunnelTop.ScaleWidth = screenSize;
            tunnelTop.ScaleX = screenSize;
            tunnelTop.ScaleY = (int) Math.Round(screenSize * screenRatioMult);


            // Create A wall to enclose the bottok of the tunnel

            //Calculate the location and size for the wall
            wallPosX = (int) (System.Math.Round((screenSize * 0.1) + ((screenSize / 40) * 3), 0));
            wallPosY = (int) (System.Math.Round((screenSize * 0.1) + ((screenSize / 40) * 3), 0));
            wallSize = (int) System.Math.Round((screenSize * 0.3) - ((screenSize / 40) * 3), 0); 

            // Create the Wall Object
            Wall tunnelBottom = new Wall(game, new Point(wallPosX, wallPosY), wallSize, lineSize);
            tunnelBottom.ScaleWidth = screenSize;
            tunnelBottom.ScaleX = screenSize;
            tunnelBottom.ScaleY = (int) Math.Round(screenSize * screenRatioMult);


            //  Build the end box

            // Build the boxes left top wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenSize * 0.4, 0);
            wallPosY = (int)System.Math.Round(screenSize * 0.05, 0);
            wallSize = (int)System.Math.Round(screenSize * 0.05, 0);

            // Create the Wall Object
            Wall boxLeftTop = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            boxLeftTop.ScaleHeight = (int) Math.Round(screenSize * screenRatioMult);
            boxLeftTop.ScaleX = screenSize;
            boxLeftTop.ScaleY = (int) Math.Round(screenSize * screenRatioMult);

            // Build the boxes left bottom wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenSize * 0.4, 0);
            wallPosY = (int)System.Math.Round((screenSize * 0.1) + ((screenSize / 40) * 3), 0);
            wallSize = (int)System.Math.Round(screenSize * 0.5, 0);

            // Create the Wall Object
            Wall boxLeftBottom = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            boxLeftBottom.ScaleHeight = (int) Math.Round(screenSize * screenRatioMult);
            boxLeftBottom.ScaleX = screenSize;
            boxLeftBottom.ScaleY = (int) Math.Round(screenSize * screenRatioMult);

            // Build the boxes bottom wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenSize * 0.4, 0);
            wallPosY = (int)System.Math.Round((screenSize * 0.6) + ((screenSize / 40) * 3), 0);
            wallSize = (int)System.Math.Round(screenSize * 0.5, 0);

            // Create the Wall Object
            Wall boxBottom = new Wall(game, new Point(wallPosX, wallPosY), wallSize, lineSize);
            boxBottom.ScaleWidth = screenSize;
            boxBottom.ScaleX = screenSize;
            boxBottom.ScaleY = (int) Math.Round(screenSize * screenRatioMult);

            // Build the boxes top wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenSize * 0.4, 0);
            wallPosY = (int)System.Math.Round((screenSize * 0.05), 0);
            wallSize = (int)System.Math.Round(screenSize * 0.5, 0);

            // Create the Wall Object
            Wall boxTop = new Wall(game, new Point(wallPosX, wallPosY), wallSize, lineSize);
            boxTop.ScaleWidth = screenSize;
            boxTop.ScaleX = screenSize;
            boxTop.ScaleY = (int) Math.Round(screenSize * screenRatioMult);

            // Build the boxes Right wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenSize * 0.9, 0);
            wallPosY = (int)System.Math.Round(screenSize * 0.05, 0);
            wallSize = (int)System.Math.Round((screenSize * 0.55) + ((screenSize / 40) * 3), 0);

            // Create the Wall Object
            Wall boxRight = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            boxRight.ScaleHeight = (int) Math.Round(screenSize * screenRatioMult);
            boxRight.ScaleX = screenSize;
            boxRight.ScaleY = (int) Math.Round(screenSize * screenRatioMult);

            // Add a rectangle in the corner for bouncing off of 
            wallPosX = (int)System.Math.Round((screenSize * 0.1) + 4, 0);
            wallPosY = (int)System.Math.Round(screenSize * 0.1, 0);
            wallSize = (int)System.Math.Round(screenSize * 0.075, 0);

            
            // Create the Wall Object
            Wall bounceWall = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize, 30);
            bounceWall.ScaleHeight = (int) Math.Round(screenSize * screenRatioMult);
            bounceWall.ScaleX = screenSize;
            bounceWall.ScaleY = (int) Math.Round(screenSize * screenRatioMult);
            

            // Create borders

            BorderField leftBorder = new BorderField(game, new Point(0, 0), lineSize, screenSize);
            leftBorder.ScaleX = screenSize;
            leftBorder.ScaleY = (int) Math.Round(screenSize * screenRatioMult);
            leftBorder.ScaleHeight = screenSize;


            BorderField topBorder = new BorderField(game, new Point(lineSize, 0), screenSize, lineSize);
            topBorder.ScaleY = (int) Math.Round(screenSize * screenRatioMult);
            topBorder.ScaleWidth = screenSize;

            BorderField bottomBorder = new BorderField(game, new Point(0, (int)Math.Round(screenSize * 0.99)), screenSize, lineSize);
            bottomBorder.ScaleX = screenSize;
            bottomBorder.ScaleY = screenSize;
            bottomBorder.ScaleWidth = screenSize;

            BorderField rightBorder = new BorderField(game, new Point((int)Math.Round(screenSize * 0.99), 0), lineSize, screenSize);
            rightBorder.ScaleX = screenSize;
            rightBorder.ScaleY = (int) Math.Round(screenSize * screenRatioMult);
            rightBorder.ScaleHeight = screenSize;

            // Add Slopes
            float slopeX = (screenSize * 0.1f);
            float slopeY = screenSize * 0.3f;

            Slope upSlope = new Slope(game, new Vector2(slopeX, slopeY), ((screenSize / 40) * 3), ((screenSize / 40) * 3), new Vector2(0, 150));
            upSlope.ScaleWidth = screenSize;
            upSlope.ScaleHeight = screenSize;
            upSlope.ScaleX = screenSize;
            upSlope.ScaleY = (int) Math.Round(screenSize * screenRatioMult);
            upSlope.ConstantShifts = new Vector4(8, 0, -8, 0);


            slopeX = (screenSize * 0.3f);
            slopeY = (screenSize * 0.1f) + (lineSize / screenSize);

            Slope downSlope = new Slope(game, new Vector2(slopeX, slopeY), ((screenSize / 40) * 3), ((screenSize / 40) * 3), new Vector2(100, 0));
            downSlope.ScaleWidth = screenSize;
            downSlope.ScaleHeight = screenSize;
            downSlope.ScaleX = screenSize;
            downSlope.ScaleY = (int) Math.Round(screenSize * screenRatioMult);
            downSlope.ConstantShifts = new Vector4(0, 8, 0, -8);


            // Create hole
            int holePosX = (int) Math.Round(screenSize * 0.65, 0);
            int holePosY = (int)Math.Round(screenSize * 0.5 , 0);
            GolfHole hole = new GolfHole(game, new Point(holePosX, holePosY));
            hole.ScaleX = screenSize;
            hole.ScaleY = (int) Math.Round(screenSize * screenRatioMult);
            _hole = hole;

            //  Set ball starting position
            float ballPosX = (float)((screenSize * 0.1) + ((screenSize / 40) * 1.25));
            float ballPosY = screenSize * 0.65F;
            _ballPos = new Vector2(ballPosX, ballPosY);
            

            // Add the items to the map's obstacle list
            _obstacles.Add(leftWall);
            _obstacles.Add(tunnelTop);
            _obstacles.Add(tunnelBottom);
            _obstacles.Add(rightWall);
            _obstacles.Add(boxLeftTop);
            _obstacles.Add(boxLeftBottom);
            _obstacles.Add(boxBottom);
            _obstacles.Add(boxTop);
            _obstacles.Add(boxRight);
            _obstacles.Add(leftBorder);
            _obstacles.Add(rightBorder);
            _obstacles.Add(topBorder);
            _obstacles.Add(bottomBorder);
            _obstacles.Add(upSlope);
            _obstacles.Add(downSlope);
            _obstacles.Add(bounceWall);


        }
    }

}
