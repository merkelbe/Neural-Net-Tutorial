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
    class Tank : GameObject
    {
        private double angle;
        private int rotatedWidth { get { return (int)(Math.Abs(Math.Cos(angle))*sprite.Width + Math.Abs(Math.Sin(angle)) * sprite.Height); } }
        private int rotatedHeight { get { return (int)(Math.Abs(Math.Sin(angle)) * sprite.Width + Math.Abs(Math.Cos(angle)) * sprite.Height); } }

        internal Tank(int _xCoord, int _yCoord, double _angle, Sprite _sprite, int _windowWidth, int _windowHeight)
        {
            this.active = true;
            this.destinationRectangle = new Rectangle(_xCoord, _yCoord, _sprite.Width, _sprite.Height);
            this.angle = _angle;
            this.sprite = _sprite;
            this.WINDOW_WIDTH = _windowWidth;
            this.WINDOW_HEIGHT = _windowHeight;
        }

        internal void Update(GameTime _gameTime, double _leftSpeed, double _rightSpeed)
        {
            this.ELAPSED_TIME += _gameTime.ElapsedGameTime.Milliseconds;
            if (this.ELAPSED_TIME > this.REFRESH_RATE)
            {
                this.ELAPSED_TIME -= this.REFRESH_RATE;
                
                double angleInc;

                if (_leftSpeed == _rightSpeed)
                {
                    this.X += (int)(Math.Cos(angle) * _leftSpeed);
                    this.Y += (int)(Math.Sin(angle) * _leftSpeed);
                }
                else
                {
                    angleInc = (_leftSpeed - _rightSpeed) / (double)sprite.Width;
                    this.X += (int)((_leftSpeed + _rightSpeed) / (_leftSpeed - _rightSpeed) * (double)sprite.Width / 2.0 * (Math.Cos(angle - Math.PI / 2.0 + angleInc) - Math.Cos(angle - Math.PI / 2.0)));
                    this.Y += (int)((_leftSpeed + _rightSpeed) / (_leftSpeed - _rightSpeed) * (double)sprite.Width / 2.0 * (Math.Sin(angle - Math.PI / 2.0 + angleInc) - Math.Sin(angle - Math.PI / 2.0)));
                    angle += angleInc;
                }

                // Clamping Logic
                angle = angle % (Math.PI * 2);
                if (this.X - rotatedWidth / 2 < 0)
                {
                    this.X = rotatedWidth / 2;
                    //angle = (angle > Math.PI ? 3 : 1) * Math.PI - angle;
                }
                else if (this.X + rotatedWidth / 2 > WINDOW_WIDTH)
                {
                    this.X = WINDOW_WIDTH - rotatedWidth / 2;
                    //angle = (angle > Math.PI ? 3 : 1) * Math.PI - angle;
                }
                if (this.Y - rotatedHeight / 2 < 0)
                {
                    this.Y = rotatedHeight / 2;
                    //angle = Math.PI * 2 - angle;
                }
                else if (this.Y + rotatedHeight / 2 > WINDOW_HEIGHT)
                {
                    this.Y = WINDOW_HEIGHT - rotatedHeight / 2;
                    //angle = Math.PI * 2 - angle;
                }
            }
        }

        internal void Draw(SpriteBatch _spriteBatch)
        {
            if (this.active)
            {
                _spriteBatch.Draw(sprite.EntireImage, destinationRectangle, sprite.SourceRectangle, Color.White, (float)angle, sprite.SourceRectangleCenter, SpriteEffects.None, 0f);
            }
        }
    }
}
