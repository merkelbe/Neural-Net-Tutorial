using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NNetTut
{
    class GameObject
    {
        #region Fields

        protected int WINDOW_WIDTH;
        protected int WINDOW_HEIGHT;

        protected int REFRESH_RATE = 16;
        protected int ELAPSED_TIME = 0;
        
        protected Sprite sprite;
        protected Rectangle destinationRectangle;

        protected bool active;

        #endregion

        #region Properties

        internal int X { get { return destinationRectangle.X; } set { destinationRectangle.X = value; } }
        internal int Y { get { return destinationRectangle.Y; } set { destinationRectangle.Y = value; } }
        internal Rectangle CollisionRectangle { get { return destinationRectangle; } }

        #endregion

        #region Constructors

        #endregion

        #region Methods

        internal virtual void Update()
        {
        }

        internal virtual void Draw()
        {
        }

        #endregion
    }
}
