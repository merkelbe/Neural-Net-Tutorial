using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NNetTut
{
    static class m
    {
        internal static Random random = new Random();

        internal static int RandomIndex(List<int> _cutoffList)
        {
            int index = 0;
            int rando = random.Next(0, _cutoffList[_cutoffList.Count - 1]);
            while (rando >= _cutoffList[index])
            {
                index++;
            }
            return index;
        }

        internal static double GetRandomNeuralNetWeight()
        {
            int randomPopPop = 2000;
            return random.NextDouble() * randomPopPop - randomPopPop / 2;
        }

        internal static List<Genome> GeneticAlgorithm(List<Genome> _population, double _mutationRate = 0.1, double _crossoverRate = 0.7)
        {
            List<Genome> updatedPopulation = new List<Genome>();
            int populationSize = _population.Count;
            List<int> fitnessCutoffs = new List<int>();
            int chromosomeLength = -1;
            double totalFitness = 0;
            double bestFitness = -1;
            Genome bestGenome= new Genome(new List<double>(),0);
            foreach (Genome genome in _population)
            {
                totalFitness += genome.Fitness;
                fitnessCutoffs.Add((int)totalFitness);
                if (chromosomeLength == -1)
                {
                    chromosomeLength = genome.Weights.Count;
                }
                if (genome.Fitness > bestFitness)
                {
                    bestFitness = genome.Fitness;
                    bestGenome = new Genome(genome.Weights, genome.Fitness);
                }
            }
            m.AverageFitnessByGeneration.Add(totalFitness / populationSize);
            for (int geneNum = 0; geneNum < populationSize-1; geneNum++)
            {
                List<double> momChromo = new List<double>(_population[m.RandomIndex(fitnessCutoffs)].Weights);
                List<double> popChromo = new List<double>(_population[m.RandomIndex(fitnessCutoffs)].Weights);
                List<double> childChromo = new List<double>();
                int crossoverIndex = m.random.NextDouble() < _crossoverRate ? m.random.Next(0, chromosomeLength) : chromosomeLength;
                for (int chromNum = 0; chromNum < chromosomeLength; chromNum++)
                {
                    childChromo.Add(m.random.NextDouble() < _mutationRate/Math.Max(Math.Pow(bestGenome.Fitness,2),1) ? GetRandomNeuralNetWeight() : chromNum < crossoverIndex ? popChromo[chromNum] : momChromo[chromNum]);
                }
                updatedPopulation.Add(new Genome(childChromo, 5));
            }
            bestGenome = new Genome(bestGenome.Weights); ;
            updatedPopulation.Add(bestGenome);
            return updatedPopulation;
        }

        internal static List<double> AverageFitnessByGeneration = new List<double>();
    }
}
