using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserGolf.ConfigClasses
{
    /// <summary>
    /// Holds the Width and Height of the playale portion of the screen in Pixels. Same as GraphicsDevice.Viewport but this discounts 
    /// parts of the screen reserved for UI
    /// </summary>
    internal class PlayScreen
    {
        // Width of the screen 
        private int _width;

        // Height of the screen
        private int _height;

        /// <summary>
        /// Width of the screen
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Height of the screen
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Create a new play screen with a specified height and width
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public PlayScreen(int width, int height)
        {
            _width = width;
            _height = height;
        }
    }
}
