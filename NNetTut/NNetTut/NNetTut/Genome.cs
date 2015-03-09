using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNetTut
{
    class Genome
    {
        private double fitness;
        private double initialFitness;

        internal List<double> Weights;
        internal double Fitness
        {
            get { return fitness - (initialFitness-1); }
        }

        internal Genome(List<double> _weights, double _initialFitness= 5)
        {
            Weights = _weights;
            initialFitness = _initialFitness;
            fitness = _initialFitness;
        }

        internal void IncFitness()
        {
            fitness *= 1.2;
        }
    }
}
