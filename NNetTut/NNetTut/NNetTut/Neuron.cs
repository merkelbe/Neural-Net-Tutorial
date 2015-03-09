using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace NNetTut
{
    internal class Neuron
    {
        #region Fields

        internal List<double> weights;

        #endregion

        #region Properties

        //internal List<double> Weights { get { return weights; } }

        #endregion

        internal Neuron(int _numInputs)
        {
            weights = new List<double>();
            for (int i = 0; i < _numInputs + 1; i++)
            {
                weights.Add(m.GetRandomNeuralNetWeight());
            }
        }
    }
}
