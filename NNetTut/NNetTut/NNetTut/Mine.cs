using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNetTut
{
    class Mine : GameObject
    {
        private int WINDOW_WIDTH;
        private int WINDOW_HEIGHT;
        private SmartTank smartTankToImproveFitnessOf;

        internal Mine(int _xCoord, int _yCoord, Sprite _sprite, int _windowWidth, int _windowHeight)
        {
            active = true;
            sprite = _sprite;
            destinationRectangle = new Rectangle(_xCoord, _yCoord, sprite.Width, sprite.Height);
            WINDOW_WIDTH = _windowWidth;
            WINDOW_HEIGHT = _windowHeight;
        }

        internal void Update(GameTime _gameTime, List<SmartTank> _smartTanks, List<Mine> _mines)
        {
            this.ELAPSED_TIME += _gameTime.ElapsedGameTime.Milliseconds;
            if (this.ELAPSED_TIME >= this.REFRESH_RATE)
            {
                if (Touches(_smartTanks,out smartTankToImproveFitnessOf))
                {
                    smartTankToImproveFitnessOf.IncFitness();

                    List<Mine> otherMines = new List<Mine>(_mines);
                    otherMines.Remove(this);
                    
                    //Respawns mine in a different location (not touching another mine or tank
                    while (Touches(_smartTanks) || Touches(otherMines))
                    {
                        destinationRectangle.X = m.random.Next(sprite.Width, WINDOW_WIDTH - 2*sprite.Width);
                        destinationRectangle.Y = m.random.Next(sprite.Width, WINDOW_HEIGHT - 2*sprite.Height);
                    }
                }
                this.ELAPSED_TIME -= this.REFRESH_RATE;
            }
        }

        internal void Draw(SpriteBatch _spriteBatch)
        {
            if (this.active)
            {
                _spriteBatch.Draw(sprite.EntireImage, destinationRectangle, Color.White);
            }
        }

        internal bool Touches<T>(List<T> _objects) where T : GameObject
        {
            foreach (T thing in _objects)
            {
                if(thing.CollisionRectangle.Intersects(this.CollisionRectangle))
                {
                    return true;
                }
            }
            return false;
        }

        internal bool Touches<T>(List<T> _objects, out T _objectTouched) where T:GameObject
        {
            foreach (T thing in _objects)
            {
                if (thing.CollisionRectangle.Intersects(this.CollisionRectangle))
                {
                    _objectTouched = thing;
                    return true;
                }
            }
            _objectTouched = null;
            return false;
        }
    }
}
