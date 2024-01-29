using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;


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
public class Map
{
    /// <summary>
    /// The type of map this is. The differnet types of maps are held in <see cref="MapTypes"/>.
    /// </summary>
    private MapTypes type;


	/// <summary>
	/// List of the Components that are on this map. 
	/// </summary>
	private List<GameComponent> components;

	/// <summary>
	/// Returns the value of the Map's components property 
	/// </summary>
	/// <returns>A List containing the GameComponents on this map. </returns>
	public List<GameComponent> GetComponents() { return components; }

	/// <summary>
	/// Create a procedurally generated map
	/// </summary>
	public Map(int screenLength, int screenWidth)
	{
		
	}

	/// <summary>
	/// Create a Map with a premade layout
	/// </summary>
	/// <param name="preset">Number corresponding to the layout you would like.
	/// Layouts:
	/// 0 = Standard tunnel map
	/// </param>
	public Map(int preset, int screenLength, int screenWidth)
	{
		if(preset == 0)
		{
			createStandardTunnel(screenLength, screenWidth);
		}
	}

	/// <summary>
	/// Function to create the standard tunnel preset (Preset 0)
	/// </summary>
	private void createStandardTunnel(int screenLength, int screenWidth)
	{	

		// Set the far left wall for the tunnel
		MapElement leftWall = new MapElement();
		// Calculate the locations for the wall by percentage of screen and round them
		float wallPosX = (float) System.Math.Round(screenLength * 0.3, 0);
        float wallPosY = (float) System.Math.Round(screenWidth * 0.3, 0);
		float wallSize = (float) System.Math.Round(screenWidth * 0.5, 0);

        // Set the wall attributes
        leftWall.position = new Vector2(wallPosX, wallPosY);
		leftWall.size = new Vector2(4, wallSize);
        leftWall.type = ElementTypes.WALL;

        // Set the lower right wall of the tunnel start
        MapElement tunnelLowerRightWall = new MapElement();

		// Shift the starting position and size for the wall
		wallPosX += 40;
		wallPosY -= 44;
		wallSize -= 44;

        // Set the wall attributes
        tunnelLowerRightWall.position = new Vector2(wallPosX, wallPosY);
        tunnelLowerRightWall.size = new Vector2(4, wallSize);
        tunnelLowerRightWall.type = ElementTypes.WALL;


        // Create A wall to enclose the top of the tunnel
        MapElement tunnelTop = new MapElement ();

        //Calculate the location and size for the wall
        wallPosX = (float) (System.Math.Round(screenLength * 0.3, 0) + 4);
        wallPosY = (float) System.Math.Round(screenWidth * 0.3, 0);
		wallSize = (float) System.Math.Round(screenWidth * 0.2, 0);

        // Set the wall attributes
        tunnelTop.position = new Vector2(wallPosX, wallPosY);
        tunnelTop.size = new Vector2(wallSize, 4);
        tunnelTop.type = ElementTypes.WALL;


        // Create A wall to enclose the bottok of the tunnel
        MapElement tunnelBottom = new MapElement();

        //Calculate the location and size for the wall
        wallPosX = (float)  (System.Math.Round(screenLength * 0.3, 0) + 40);
        wallPosY = (float)  (System.Math.Round(screenWidth * 0.3, 0) + 40);
        wallSize = (float) System.Math.Round(screenWidth * 0.2, 0);

        // Set the wall attributes
        tunnelBottom.position = new Vector2(wallPosX, wallPosY);
        tunnelBottom.size = new Vector2(wallSize, 4);
		tunnelTop.type = ElementTypes.WALL;

		// Add the items to the map's element list
		elements.Add(leftWall);
        elements.Add(tunnelBottom);
        elements.Add(tunnelBottom);
        elements.Add(tunnelLowerRightWall);
    }
}
