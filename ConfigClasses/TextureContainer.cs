using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserGolf.ConfigClasses
{
    internal class TextureContainer : DrawableGameComponent
    {



        /// <summary>
        /// Color Strip texture used for drawing rectangles
        /// </summary>
        private Texture2D _colorStrip = null;

        /// <summary>
        /// Color Strip texture used for drawing rectangles
        /// </summary>
        public Texture2D ColorStrip
        {
            get { return _colorStrip; }

        }
        public TextureContainer(Game game) : base(game)
        { }

        protected override void LoadContent()
        {
            if (_colorStrip == null)
            {
                _colorStrip = Game.Content.Load<Texture2D>("LGGColorStrip");

            }
        }
    }
}
