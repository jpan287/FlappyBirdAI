using System;
using System.Collections.Generic;
using System.Text;

namespace MachineLearning
{
    public class Network
    {
        public Layer[] Layers;
        public double[] Outputs => Layers[Layers.Length - 1].Output;

        public Network(Func<double, double> activation, int inputCount, params int[] NeuronsPerLayer)
        {
            Layers = new Layer[NeuronsPerLayer.Length];

            Layers[0] = new Layer(activation, inputCount, NeuronsPerLayer[0]);
            for (int i = 1; i < Layers.Length; i++)
            {
                Layers[i] = new Layer(activation, NeuronsPerLayer[i - 1], NeuronsPerLayer[i]);
            }
        }

        public void Randomize(Random rand)
        {
            for (int i = 0; i < Layers.Length; i++)
            {
                Layers[i].Randomize(rand);
            }
        }

        public double[] Compute(double[] input)
        {
            double[] output = input;
            for (int i = 0; i < Layers.Length; i++)
            {
                output = Layers[i].Compute(output);
            }

            return output;
        }

        public double MAE(double[][] input, double[][] desiredOutput)
        {
            double mae = 0;
            for (int r = 0; r < input.Length; r++)
            {
                Compute(input[r]);
                double rowError = 0;
                for (int i = 0; i < desiredOutput[r].Length; i++)
                {
                    rowError += Math.Abs(Outputs[i] - desiredOutput[r][i]);
                }
                rowError /= desiredOutput[r].Length;
                mae += rowError;
            }
            return mae / input.Length;
        }
    }
}
