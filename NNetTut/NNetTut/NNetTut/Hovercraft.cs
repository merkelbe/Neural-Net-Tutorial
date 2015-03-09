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
    class Hovercraft : GameObject
    {
        float speed;
        private double angle;
        private double steer = 0;
        private int rotatedWidth { get { return (int)(Math.Abs(Math.Cos(angle))*sprite.Width + Math.Abs(Math.Sin(angle)) * sprite.Height); } }
        private int rotatedHeight { get { return (int)(Math.Abs(Math.Sin(angle)) * sprite.Width + Math.Abs(Math.Cos(angle)) * sprite.Height); } }

        //private int width;
        

        internal Hovercraft(int _xCoord, int _yCoord, float _speed, double _angle, Sprite _sprite, int _windowWidth, int _windowHeight)
        {
            active = true;

            destinationRectangle = new Rectangle(_xCoord, _yCoord, _sprite.Width, _sprite.Height);
            
            speed = _speed;
            angle = _angle;

            //width = _sprite.Width;

            sprite = _sprite;

            this.WINDOW_WIDTH = _windowWidth;
            this.WINDOW_HEIGHT = _windowHeight;
        }

        internal void Update(GameTime _gameTime)
        {
            this.ELAPSED_TIME += _gameTime.ElapsedGameTime.Milliseconds;
            if (this.ELAPSED_TIME > this.REFRESH_RATE)
            {
                this.ELAPSED_TIME -= this.REFRESH_RATE;
                
                //One way to move without using inputs
                this.X += (int)(Math.Cos(angle) * speed);
                this.Y += (int)(Math.Sin(angle) * speed);
                steer += 0.001 * (m.random.Next(11) - 5);
                angle += steer;

                // Clamping Logic
                angle = angle % (Math.PI * 2);
                if (this.X - rotatedWidth / 2 < 0)
                {
                    this.X = rotatedWidth / 2;
                    angle = (angle > Math.PI ? 3 : 1) * Math.PI - angle;
                }
                else if (this.X + rotatedWidth / 2 > WINDOW_WIDTH)
                {
                    this.X = WINDOW_WIDTH - rotatedWidth / 2;
                    angle = (angle > Math.PI ? 3 : 1) * Math.PI - angle;
                }
                if (this.Y - rotatedHeight / 2 < 0)
                {
                    this.Y = rotatedHeight / 2;
                    angle = Math.PI * 2 - angle;
                }
                else if (this.Y + rotatedHeight / 2 > WINDOW_HEIGHT)
                {
                    this.Y = WINDOW_HEIGHT - rotatedHeight / 2;
                    angle = Math.PI * 2 - angle;
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
