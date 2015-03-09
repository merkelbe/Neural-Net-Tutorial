using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NNetTut
{
    class SmartTank: GameObject
    {
        private Tank tank;
        private NeuralNet neuralNet;
        private Genome genome;

        private double leftSpeed;
        private double rightSpeed;
        private double xDistToClosestMine;
        private double yDistToClosestMine;

        internal Genome Genome 
        { 
            get 
            { 
                return genome; 
            } 
            set 
            { 
                genome = value;
                neuralNet.SetWeights(value.Weights);
            } 
        }

        internal SmartTank(Tank _tank, NeuralNet _neuralNet, double _initLeftSpeed, double _initRightSpeed)
        {
            tank = _tank;
            neuralNet = _neuralNet;
            leftSpeed = _initLeftSpeed;
            rightSpeed = _initRightSpeed;

            genome = new Genome(neuralNet.GetWeights());
        }

        internal void Update(GameTime _gameTime, List<Mine> _mines )
        {
            this.ELAPSED_TIME += _gameTime.ElapsedGameTime.Milliseconds;
            if (this.ELAPSED_TIME > this.REFRESH_RATE)
            {
                FindNearestMineAndSetDistances(_mines, out xDistToClosestMine, out yDistToClosestMine);
                List<double> newSpeeds = neuralNet.GetOutputs(new List<double>() { leftSpeed, rightSpeed, xDistToClosestMine, yDistToClosestMine });
                leftSpeed = newSpeeds[0]*Game1.gameSpeed.Speed*2-newSpeeds[0]*Game1.gameSpeed.Speed;
                rightSpeed = newSpeeds[1]*Game1.gameSpeed.Speed*2-newSpeeds[1]*Game1.gameSpeed.Speed;
                tank.Update(_gameTime, leftSpeed, rightSpeed);
                this.ELAPSED_TIME -= this.REFRESH_RATE;
                destinationRectangle = tank.CollisionRectangle;
            }

        }

        internal void Draw(SpriteBatch _spriteBatch)
        {
            tank.Draw(_spriteBatch);
        }

        private void FindNearestMineAndSetDistances(List<Mine> _mines, out double _xDistToClosestMine, out double _yDistToClosestMine)
        {
            _xDistToClosestMine = -1;
            _yDistToClosestMine = -1;
            double closestDistance = -1;
            double currentDistance;
            foreach (Mine mine in _mines)
            {
                currentDistance = Math.Sqrt(Math.Pow(mine.X - tank.X, 2) + Math.Pow(mine.Y - tank.Y, 2));
                if (closestDistance == -1 || currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    _xDistToClosestMine = mine.X - tank.X;
                    _yDistToClosestMine = mine.Y - tank.Y;
                }
                
            }
        }

        internal void IncFitness()
        {
            genome.IncFitness();
        }
    }
}
