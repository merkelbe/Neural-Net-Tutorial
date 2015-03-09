using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNetTut
{
    class Sprite
    {

        #region Fields

        private Texture2D entireImage;
        private string imageName;
        private int imagesPerColumn;
        private int imagesPerRow;
        private Rectangle sourceRectangle;
        private int frameIndex;
        private int imageCount;
        private Dictionary<int, int> frameIndexToXCoord;
        private Dictionary<int, int> frameIndexToYCoord;

        #endregion

        #region Properties

        internal Texture2D EntireImage { get { return entireImage; } }
        internal string SpriteName { get { return imageName; } }
        internal int Height { get { return entireImage.Height / imagesPerColumn; } }
        internal int Width { get { return entireImage.Width / imagesPerRow; } }
        internal int ImagesPerColumn { get { return imagesPerColumn; } }
        internal int ImagesPerRow { get { return imagesPerRow; } }
        internal Rectangle SourceRectangle { get { return sourceRectangle; } }
        internal Vector2 SourceRectangleCenter { get { return new Vector2((float)Width / 2, (float)Height / 2); } }

        #endregion

        #region Constructors

        internal Sprite(ContentManager _content, string _spriteName, int _imagesPerColumn = 1, int _imagesPerRow = 1, int _imageCount = -1)
        {
            imageName = _spriteName;
            entireImage = _content.Load<Texture2D>(_spriteName);
            
            imagesPerColumn = _imagesPerColumn;
            imagesPerRow = _imagesPerRow;
            imageCount = _imageCount == -1 ? _imagesPerColumn * _imagesPerRow : _imageCount;
            sourceRectangle = new Rectangle(0, 0, this.Width, this.Height);
            frameIndex = 0;

            // Sets up dictionarys to get x and y coordinates based on frame index.  Used to set frames in class' methods
            frameIndexToXCoord = new Dictionary<int, int>();
            frameIndexToYCoord = new Dictionary<int, int>();
            int currentXCoord = 0;
            int currentYCoord = 0;
            for (int i = 0; i < imageCount; i++)
            {
                frameIndexToXCoord.Add(i, currentXCoord);
                frameIndexToYCoord.Add(i, currentYCoord);
                if ((i+1) % imagesPerRow == 0)
                {
                    currentXCoord = 0;
                    currentYCoord += this.Height;
                }
                else
                {
                    currentXCoord += this.Width;
                }
            }
        }

        #endregion

        #region Methods

        internal void NextFrame()
        {
            frameIndex = frameIndex + 1 >= imageCount ? imageCount - 1 : frameIndex + 1;
            SetFrameByIndex(frameIndex);
        }

        internal void PreviousFrame()
        {
            frameIndex = frameIndex - 1 < 0 ? 0 : frameIndex - 1;
            SetFrameByIndex(frameIndex);
        }

        internal void SetFrame(int _newFrameIndex)
        {
            SetFrameByIndex(_newFrameIndex);
        }

        private void SetFrameByIndex(int _index)
        {
            if (_index < 0 || _index >= imageCount)
                throw new ArgumentException("Argument for the frame index in Set Frame is out of range.");
            else
            {
                frameIndex = _index;
                sourceRectangle.X = frameIndexToXCoord[frameIndex];
                sourceRectangle.Y = frameIndexToXCoord[frameIndex];
            }
        }
        #endregion



    }
}
