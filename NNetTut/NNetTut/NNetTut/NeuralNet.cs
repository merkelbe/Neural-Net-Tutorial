using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace NNetTut
{
    class NeuralNet
    {
        List<NeuronLayer> Layers;


        internal NeuralNet(int _numInputs, int _numOutputs, List<int> _hiddenLayerDimensions)
        {
            Layers = new List<NeuronLayer>();
            foreach (int dimension in _hiddenLayerDimensions)
            {
                Layers.Add(new NeuronLayer(dimension, Layers.Count > 0 ? Layers[Layers.Count - 1].NumberOfNeurons : _numInputs));
            }
            Layers.Add(new NeuronLayer(_numOutputs, Layers[Layers.Count - 1].NumberOfNeurons));
        }

        #region Methods

        internal List<double> GetOutputs(List<double> _inputs)
        {
            List<double> outputs = new List<double>(_inputs);
            List<double> inputs;
            foreach (NeuronLayer layer in Layers)
            {
                inputs = new List<double>(outputs);
                outputs = new List<double>();
                if (inputs.Count != layer.NumberOfInputsPerNeuron)
                {
                    throw new ArgumentException("Error in inputs vs number of inputs a neuron can take.");
                }
                else
                {
                    foreach (Neuron neuron in layer.Neurons)
                    {
                        double neuronOutput = 0;
                        for(int i = 0; i < neuron.weights.Count-1; i++)
                        {
                            neuronOutput += inputs[i] * neuron.weights[i];
                        }

                        //Adjusts for the threashold weight
                        neuronOutput -= neuron.weights[neuron.weights.Count - 1];

                        //Normalizes neuron output via Sigmoid Function
                        neuronOutput = Sigmoid(neuronOutput);

                        outputs.Add(neuronOutput);
                    }
                }
            }
            return outputs;
        }

        internal void SetWeights(List<double> _newWeights)
        {
            int index = 0;
            foreach (NeuronLayer layer in Layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    for (int weightIndex = 0; weightIndex < neuron.weights.Count; weightIndex++)
                    {
                        neuron.weights[weightIndex] = _newWeights[index];
                        index++;
                    }
                }
            }
            if (index != _newWeights.Count)
            {
                throw new ArgumentException("New weights count does not match number of weights in neural net.");
            }
        }

        internal List<double> GetWeights()
        {
            List<double> weights = new List<double>();
            foreach (NeuronLayer layer in Layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    foreach (double weight in neuron.weights)
                    {
                        weights.Add(weight);
                    }
                }
            }
            return weights;
        }

        private void SaveWeightsAsTextFile(string _fileNameWithoutExtension)
        {
                StreamWriter streamWriter = new StreamWriter(Directory.GetCurrentDirectory() + "\\" + _fileNameWithoutExtension + ".txt");
                foreach (double weight in this.GetWeights())
                {
                    streamWriter.WriteLine(weight);
                }
                streamWriter.Close();
        }

        private void SetWeightsFromTextFile(string _fileNameWithoutExtension)
        {
            List<double> weights = new List<double>();
            StreamReader streamReader = new StreamReader(Directory.GetCurrentDirectory() + "\\" + _fileNameWithoutExtension + ".txt");
            string line = streamReader.ReadLine();
            while (line != null)
            {
                weights.Add(Convert.ToDouble(line));
                line = streamReader.ReadLine();
            }
            this.SetWeights(weights);
        }

        private double Sigmoid(double _activation, double p = 1)
        {
            return (double)(1/(1+Math.Pow(Math.E,-_activation/p)));
        }

        #endregion
    }
}
