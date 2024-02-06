using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserGolf.Components.UI
{
    internal class DirectionStrengthIndicator : DrawableGameComponent
    {
        private Ball attachedTo;

        public DirectionStrengthIndicator(Game game) : base(game)
        {
        }
    }
}
