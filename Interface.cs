using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GAVisualisationApp
{
    public class Interface
    {
        //Contains Decode and ObjFunc
        public static double Decode(bool[] chrom, int bitsLen)
        {
            //Decode string as unsigned binary integer, 1=true and 0=false
            double accum = 0d;
            double powerOf2 = 1d;

            for (int i = 0; i < bitsLen; i++)
            {
                if (chrom[i])
                    accum += powerOf2;

                powerOf2 *= 2d;
            }

            return accum;
        }
        public static double ObjFunc(double x, int chromLen)
        {
            //Fitness function, f(x) = x^n
            //Maximum value, f(x) = 1, or x == coef
            //Coefficient to normalize domain

            //*******fix array index, then delete this line********
            //chromLen--;

            double coef = Math.Pow(2, chromLen) - 1d;
            const double n = 10d;

            return Math.Pow(x / coef, n);
        }
        public static GeneticAlgorithm Stats(GeneticAlgorithm ga, GeneticAlgorithm.Individual[] pop)
        {
            float sumFitness = 0f;
            float min = 1f;
            float max = 0f;
            double fitness;

            for (int i = 0; i < ga.popSize; i++)
            {
                fitness = pop[i].fitness;
                sumFitness += (float)fitness;

                if (fitness > max)
                    max = (float)fitness;
                else if (fitness < min)
                    min = (float)fitness;
            }

            ga.sumFitness = sumFitness;
            ga.min = min;
            ga.max = max;
            ga.avg = sumFitness / ga.popSize;

            return ga;
        }
    }
}
