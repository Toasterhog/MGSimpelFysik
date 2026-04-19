using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace MGSimpelFysik
{
    public class Portal
    {
        private PortalHandler handler;
        public Point tile { get; }
        public float orientation { get; }
        public bool flipped { get; }

        public Portal(Point tile, float orientation, bool flipped)
        {
            this.tile = tile;
            this.orientation = orientation;
            this.flipped = flipped;
        }
    }
}
