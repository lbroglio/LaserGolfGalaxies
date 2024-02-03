using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using LaserGolf.Components.Obstacles;
using LaserGolf.Components;
using System;


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
    /// Holds the type a <see cref="MapElement"/> is.Used to tell the Game how to draw this element
    /// </summary>
    public enum ElementTypes
    {
        HOLE,
        START_POINT,
        WALL,
        ELEVATION_DOWN,
        ELEVATION_UP,
        GREEN
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
        /// List of the Obstacles that are on this map. 
        /// </summary>
        public List<LaserGolfObstacle> Obstacles { get { return _obstacles; } }



        /// <summary>
        /// List of the Obstacles that are on this map. 
        /// </summary>
        public GolfHole Hole { get { return _hole; } }

        /// <summary>
        /// Create a procedurally generated map
        /// </summary>
        public Map(int screenWidth, int screenHeight)
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
        /// <param name="screenLength"> The assumed width of the Game's screen Map will be made to the scale of this width</param>
        /// <param name="screenHeight"> The assumed height of the Game's screen Map will be made to the scale of this height</param>

        public Map(Game game, int preset, int screenWidth, int screenHeight)
        {
            _obstacles = new List<LaserGolfObstacle>();

            if (preset == 0)
            {
                createStandardTunnel(game, screenWidth, screenHeight);
            }
        }

        /// <summary>
        /// Function to create the standard tunnel preset (Preset 0)
        /// </summary>
        private void createStandardTunnel(Game game, int screenWidth, int screenHeight)
        {
            int lineSize = 8;

            // Set the far left wall for the tunnel

            // Calculate the locations for the wall by percentage of screen and round them
            int wallPosX = (int)System.Math.Round(screenWidth * 0.1, 0);
            int wallPosY = (int)System.Math.Round(screenHeight * 0.1, 0);
            int wallSize = (int)System.Math.Round(screenHeight * 0.5, 0);

            // Create the Wall Object
            Wall leftWall = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            leftWall.ScaleHeight = 75;
            leftWall.ScaleX = 100;
            leftWall.ScaleY = 75;

            // Set the lower right wall of the tunnel start

            // Shift the starting position and size for the wall
            wallPosX += ((screenWidth / 40) * 3);
            wallPosY += ((screenWidth / 40) * 3);// + (lineSize/ screenWidth);
            wallSize -= ((screenWidth / 40) * 3) + (lineSize/ screenWidth);

            // Create the wall Object
            Wall rightWall = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            rightWall.ScaleHeight = 75;
            rightWall.ScaleX = 100;
            rightWall.ScaleY = 75;


            // Create A wall to enclose the top of the tunnel

            //Calculate the location and size for the wall
            wallPosX = (int) (System.Math.Round(screenWidth * 0.1, 0));
            wallPosY = (int) System.Math.Round((screenHeight * 0.1) +  (lineSize/ screenWidth), 0);
            wallSize = (int) System.Math.Round(screenWidth * 0.3, 0);

            // Create the Wall Object
            Wall tunnelTop = new Wall(game, new Point(wallPosX, wallPosY), wallSize, lineSize);
            tunnelTop.ScaleWidth = 100;
            tunnelTop.ScaleX = 100;
            tunnelTop.ScaleY = 75;


            // Create A wall to enclose the bottok of the tunnel

            //Calculate the location and size for the wall
            wallPosX = (int) (System.Math.Round((screenWidth * 0.1) + ((screenWidth / 40) * 3), 0));
            wallPosY = (int) (System.Math.Round((screenHeight * 0.1) + ((screenWidth / 40) * 3), 0));
            wallSize = (int) System.Math.Round((screenHeight * 0.3) - ((screenWidth / 40) * 3), 0); 

            // Create the Wall Object
            Wall tunnelBottom = new Wall(game, new Point(wallPosX, wallPosY), wallSize, lineSize);
            tunnelBottom.ScaleWidth = 100;
            tunnelBottom.ScaleX = 100;
            tunnelBottom.ScaleY = 75;


            //  Build the end box

            // Build the boxes left top wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenWidth * 0.4, 0);
            wallPosY = (int)System.Math.Round(screenHeight * 0.05, 0);
            wallSize = (int)System.Math.Round(screenHeight * 0.05, 0);

            // Create the Wall Object
            Wall boxLeftTop = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            boxLeftTop.ScaleHeight = 75;
            boxLeftTop.ScaleX = 100;
            boxLeftTop.ScaleY = 75;

            // Build the boxes left bottom wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenWidth * 0.4, 0);
            wallPosY = (int)System.Math.Round((screenHeight * 0.1) + ((screenWidth / 40) * 3), 0);
            wallSize = (int)System.Math.Round(screenHeight * 0.5, 0);

            // Create the Wall Object
            Wall boxLeftBottom = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            boxLeftBottom.ScaleHeight = 75;
            boxLeftBottom.ScaleX = 100;
            boxLeftBottom.ScaleY = 75;

            // Build the boxes bottom wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenWidth * 0.4, 0);
            wallPosY = (int)System.Math.Round((screenHeight * 0.6) + ((screenWidth / 40) * 3), 0);
            wallSize = (int)System.Math.Round(screenWidth * 0.5, 0);

            // Create the Wall Object
            Wall boxBottom = new Wall(game, new Point(wallPosX, wallPosY), wallSize, lineSize);
            boxBottom.ScaleWidth = 100;
            boxBottom.ScaleX = 100;
            boxBottom.ScaleY = 75;

            // Build the boxes top wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenWidth * 0.4, 0);
            wallPosY = (int)System.Math.Round((screenHeight * 0.05), 0);
            wallSize = (int)System.Math.Round(screenWidth * 0.5, 0);

            // Create the Wall Object
            Wall boxTop = new Wall(game, new Point(wallPosX, wallPosY), wallSize, lineSize);
            boxTop.ScaleWidth = 100;
            boxTop.ScaleX = 100;
            boxTop.ScaleY = 75;

            // Build the boxes Right wall

            // Calculate the locations for the wall by percentage of screen and round them
            wallPosX = (int)System.Math.Round(screenWidth * 0.9, 0);
            wallPosY = (int)System.Math.Round(screenHeight * 0.05, 0);
            wallSize = (int)System.Math.Round((screenHeight * 0.55) + ((screenWidth / 40) * 3), 0);

            // Create the Wall Object
            Wall boxRight = new Wall(game, new Point(wallPosX, wallPosY), lineSize, wallSize);
            boxRight.ScaleHeight = 75;
            boxRight.ScaleX = 100;
            boxRight.ScaleY = 75;


            // Create hole
            int holePosX = (int) Math.Round(screenWidth * 0.65, 0);
            int holePosY = (int)Math.Round(screenHeight * 0.5, 0);
            GolfHole hole = new GolfHole(game, new Point(holePosX, holePosY));
            hole.ScaleX = 100;
            hole.ScaleY = 75;
            _hole = hole;


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

        }
    }

}
