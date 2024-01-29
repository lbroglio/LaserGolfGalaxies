using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserGolf.Components.Obstacles
{
    public class Wall : GameComponent 
    {   
        /// <summary>
        /// Backing Rectangle which stores this Walls location in the world
        /// </summary>
        private Rectangle worldPrescense;

        public Wall(Game game, int screenWidth, int screenHeight) : base(game)
        {
        }
    }
}
