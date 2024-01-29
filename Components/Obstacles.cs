using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserGolf.Components.Obstacles
{   
    /// <summary>
    /// Interface implemented by Components which act as Obstacles for the ball. -- An Obstacle is defined as any Compoentn which changes the movement of 
    /// the ball on collison.
    /// </summary>
    interface ILaserGolfObstacle
    {
        /// <summary>
        /// Returns true if the Ball obejct given in checkColliding is colliding with this ILaserGolfObstacle
        /// </summary>
        /// <param name="checkColliding"> Ball object to check the collison for</param>
        /// <returns></returns>
        bool checkCollides(Ball checkColliding);

        /// <summary>
        /// Returns a new Vector2 representing the Movement Vector of colliding after the collision takes place.
        /// This method will not check that a collision occurs and the behavior is undefined for Balls not currently colliding 
        /// with this ILaserGolfObstacle.
        /// </summary>
        /// <param name="colliding">The Ball that is currently collidind with this ILaserGolfObstacle</param>
        /// <returns></returns>
        Vector2 collideWith(Ball colliding);

    }

    public class Wall : GameComponent, ILaserGolfObstacle
    {   
        /// <summary>
        /// Backing Rectangle which holds this Walls location in the world
        /// </summary>
        private Rectangle worldRect;

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
         
        }

        public bool checkCollides(Ball checkColliding)
        {
            if(checkColliding.)
        }

        public Vector2 collideWith(Ball colliding)
        {
            throw new NotImplementedException();
        }
    }
}
