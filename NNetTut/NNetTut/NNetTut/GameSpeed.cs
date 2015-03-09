using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NNetTut
{
    class GameSpeed
    {
        private int speed;
        private bool keyUpDown;
        private bool keyDownDown;

        internal int Speed { get { return speed; } }

        internal GameSpeed(int _speed = 20)
        {
            speed = _speed;
        }

        internal void Update(KeyboardState _keyboardState)
        {

            if (_keyboardState.IsKeyDown(Keys.Up))
            {
                keyUpDown = true;
            }
            else
            {
                if(keyUpDown)
                {
                    keyUpDown = false;
                    speed = Math.Min(speed+1,20);
                }
            }
            if(_keyboardState.IsKeyDown(Keys.Down))
            {
                keyDownDown = true;
            }
            else
            {
                if (keyDownDown)
                {
                    keyDownDown = false;
                    speed = Math.Max(speed - 1, 1);
                }
            }
        }
    }
}
