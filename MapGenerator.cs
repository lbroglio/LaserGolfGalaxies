using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using LaserGolf.Components.Obstacles;


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
        /// The type of map this is. The different types of maps are held in <see cref="MapTypes"/>.
        /// </summary>
        private MapTypes type;


        /// <summary>
        /// List of the Obstacles that are on this map. 
        /// </summary>
        private List<LaserGolfObstacle> _obstacles;

        /// <summary>
        /// List of the Obstacles that are on this map. 
        /// </summary>
        public List<LaserGolfObstacle> Obstacles { get { return _obstacles; } }

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

            // Set the far left wall for the tunnel

            // Calculate the locations for the wall by percentage of screen and round them
            int wallPosX = (int)System.Math.Round(screenWidth * 0.3, 0);
            int wallPosY = (int)System.Math.Round(screenHeight * 0.3, 0);
            int wallSize = (int)System.Math.Round(screenHeight * 0.5, 0);

            // Create the Wall Object
            Wall leftWall = new Wall(game, new Point(wallPosX, wallPosY),4, wallSize);

            // Set the lower right wall of the tunnel start

            // Shift the starting position and size for the wall
            wallPosX += 40;
            wallPosY -= 44;
            wallSize -= 44;

            // Create the wall Object
            Wall rightWall = new Wall(game, new Point(wallPosX, wallPosY), 4, wallSize);


            // Create A wall to enclose the top of the tunnel

            //Calculate the location and size for the wall
            wallPosX = (int) (System.Math.Round(screenWidth * 0.3, 0) + 4);
            wallPosY = (int) System.Math.Round(screenHeight * 0.3, 0);
            wallSize = (int) System.Math.Round(screenHeight * 0.2, 0);

            // Create the Wall Object
            Wall tunnelTop = new Wall(game, new Point(wallPosX, wallPosY), wallSize, 4);



            // Create A wall to enclose the bottok of the tunnel

            //Calculate the location and size for the wall
            wallPosX = (int) (System.Math.Round(screenWidth * 0.3, 0) + 40);
            wallPosY = (int) (System.Math.Round(screenHeight * 0.3, 0) + 40);
            wallSize = (int) System.Math.Round(screenHeight * 0.2, 0);

            // Create the Wall Object
            Wall tunnelBottom = new Wall(game, new Point(wallPosX, wallPosY), wallSize, 4);

            // Add the items to the map's element list
            _obstacles.Add(leftWall);
            _obstacles.Add(tunnelTop);
            _obstacles.Add(tunnelBottom);
            _obstacles.Add(rightWall);
        }
    }

}
