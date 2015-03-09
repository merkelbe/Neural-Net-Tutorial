using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNetTut
{
    class NeuronLayer
    {
        #region Fields

        int m_NumNeurons;
        int m_NumInputsPerNeuron;
        List<Neuron> m_Neurons;

        #endregion

        #region Properties

        internal List<Neuron> Neurons { get { return m_Neurons; } }
        internal int NumberOfNeurons { get { return m_NumNeurons; } }
        internal int NumberOfInputsPerNeuron { get { return m_NumInputsPerNeuron; } }

        #endregion

        #region Constructors

        internal NeuronLayer(int _numNeurons, int _numInputsPerNeuron)
        {
            m_NumNeurons = _numNeurons;
            m_NumInputsPerNeuron = _numInputsPerNeuron;
            m_Neurons = new List<Neuron>();
            for (int i = 0; i < _numNeurons; i++)
            {
                m_Neurons.Add(new Neuron(_numInputsPerNeuron));
            }
        }

        #endregion
    }
}
