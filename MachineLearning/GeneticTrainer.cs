using System;
using System.Collections.Generic;
using System.Text;

namespace MachineLearning
{
    public class GeneticTrainer
    {
        public GeneticTrainer()
        {

        }

        public void Train((Network net, double fitness)[] population, Random random, double mutationRate)
        {
            Array.Sort(population, (a, b) => b.fitness.CompareTo(a.fitness));

            int start = (int)(population.Length * 0.1);
            int end = (int)(population.Length * 0.9);

            for (int i = start; i < end; i++)
            {
                Crossover(population[random.Next(start)].net, population[i].net, random);
                Mutate(population[i].net, random, mutationRate);
            }
            for (int i = end; i < population.Length; i++)
            {
                population[i].net.Randomize(random);
            }
        }

        public void Mutate(Network net, Random random, double mutationRate)
        {
            foreach (Layer layer in net.Layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    for (int i = 0; i < neuron.Weights.Length; i++)
                    {
                        if (random.NextDouble() < mutationRate)
                        {
                            if (random.Next(2) == 0)
                            {
                                neuron.Weights[i] *= random.NextDouble(0.5, 1.5);
                            }
                            else
                            {
                                neuron.Weights[i] *= -1;
                            }
                        }
                    }

                    if (random.NextDouble() < mutationRate)
                    {
                        if (random.Next(2) == 0)
                        {
                            neuron.Bias *= random.NextDouble(0.5, 1.5);
                        }
                        else
                        {
                            neuron.Bias *= -1;
                        }
                    }
                }
            }
        }

        public void Crossover(Network winner, Network loser, Random random)
        {
            for (int i = 0; i < winner.Layers.Length; i++)
            {
                Layer winLayer = winner.Layers[i];
                Layer childLayer = loser.Layers[i];

                int cutPoint = random.Next(winLayer.Neurons.Length);
                bool flip = random.Next(2) == 0;

                for (int j = (flip ? 0 : cutPoint); j < (flip ? cutPoint : winLayer.Neurons.Length); j++)
                {
                    Neuron winNeuron = winLayer.Neurons[j];
                    Neuron childNeuron = childLayer.Neurons[j];

                    winNeuron.Weights.CopyTo(childNeuron.Weights, 0);
                    childNeuron.Bias = winNeuron.Bias;
                }
            }
        }
    }
}
