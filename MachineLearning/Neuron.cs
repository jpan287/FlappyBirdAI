﻿using System;

namespace MachineLearning
{
    public class Neuron
    {
        public double Bias;
        public double[] Weights;
        public double Output;
        public Func<double, double> Activation;

        public Neuron(Func<double, double> activation, int inputCount)
        {
            Activation = activation;
            Weights = new double[inputCount];
        }

        public void Randomize(Random rand)
        {
            Bias = rand.NextDouble(-0.5, 0.5);
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = rand.NextDouble(-0.5, 0.5);
            }
        }

        public double Compute(double[] input)
        {
            Output = 0;
            for (int i = 0; i < Weights.Length; i++)
            {
                Output += input[i] * Weights[i];
            }

            Output = Activation(Output + Bias);
            return Output;
        }
    }
}
