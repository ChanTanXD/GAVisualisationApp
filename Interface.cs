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
        //Contains Decode, ObjFunc, and misc methods
        public static char[] DecodeWordBuilder(bool[] chrom, int chromLen)
        {
            //Decode binary string into char via ascii code
            //Desired range: 32 to 126
            //7 digits allocated for every character, so every characters will have maximum 128 bits
            char[] build = new char[chromLen / 7];
            int buildIndex = 0;
            int accum = 0;
            int powerOf2 = 1;

            for (int i = 0; i < chromLen; i++)
            {
                //accumulate bits
                if (chrom[i])
                    accum += powerOf2;
                powerOf2 *= 2;


                //Convert binary into char once every 7 bits
                if((i+1) % 7 == 0)
                {
                    build[buildIndex] = (char)accum;

                    buildIndex++;
                    accum = 0;
                    powerOf2 = 1;
                }
            }
            return build;
        }
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
        public static double WordBuildFitness(char[] build, int targetLen, char[] target)
        {
            // higher fitness is better
            double fitness = 0; // start at 0 - the best fitness


            for (int i = 0; i < targetLen; i++)
            {
                // subtract the ascii difference between the target char and the chromosome char
                // Thus 'c' is fitter than 'd' when compared to 'a'.
                fitness -= Math.Abs(target[i] - build[i]);
            }
            return fitness;
        }
        public static double CarboxFitness(double x, int chromLen, int demoMethod)
        {
            return 0;
        }

        public static double ObjFunc(double x, int chromLen)
        {
            //Fitness function, f(x) = x^n
            //Maximum value, f(x) = 1, or x == coef
            //Coefficient to normalize domain

            double coef = Math.Pow(2, chromLen) - 1d;
            const double n = 10d;
            return Math.Pow(x / coef, n);
        }

        public static GeneticAlgorithm Stats(GeneticAlgorithm ga, GeneticAlgorithm.Individual[] pop)
        {
            double fitness = pop[0].fitness;
            float sumFitness = 0f;
            float min = (float)fitness;
            float max = (float)fitness;
            int minIndex = 0;
            int maxIndex = 0;


            for (int i = 0; i < ga.popSize; i++)
            {
                fitness = pop[i].fitness;
                sumFitness += (float)fitness;

                if (fitness > max)
                {
                    max = (float)fitness;
                    maxIndex = i;
                }
                else if (fitness < min)
                {
                    min = (float)fitness;
                    minIndex = i;
                }
            }
            ga.sumFitness = sumFitness;
            ga.min = minIndex;
            ga.max = maxIndex;
            ga.avg = sumFitness / ga.popSize;

            return ga;
        }

        public static GeneticAlgorithm.Individual[] PopulationSort(GeneticAlgorithm.Individual[] pop)
        {
            MergeSort(0, (pop.Length / 2), pop.Length - 1, ref pop);
            return pop;
        }
        static void MergeSort(int l, int m, int r, ref GeneticAlgorithm.Individual[] mainArr)
        {
            if (l == m && m == r)
            {
                return;
            }

            MergeSort(l, (m + l) / 2, m, ref mainArr); //left split
            MergeSort(m + 1, (m + 1 + r) / 2, r, ref mainArr); //right split
            Merge(l, m, r, ref mainArr);
        }
        static void Merge(int l, int m, int r, ref GeneticAlgorithm.Individual[] mainArr)
        {
            int mpOne = m + 1;
            GeneticAlgorithm.Individual[] leftArr = new GeneticAlgorithm.Individual[(m - l) + 1];
            GeneticAlgorithm.Individual[] rightArr = new GeneticAlgorithm.Individual[(r - mpOne) + 1];


            for (int i = 0, iCorres = l; iCorres <= m; i++, iCorres++)   //iCorres: corresponding index on the main array
            {
                leftArr[i] = mainArr[iCorres];
            }
            for (int j = 0, jCorres = mpOne; jCorres <= r; j++, jCorres++)
            {
                rightArr[j] = mainArr[jCorres];
            }


            for (int li = 0, ri = 0; l <= r; l++)
            {
                if (li >= leftArr.Length)
                {
                    mainArr[l] = rightArr[ri];
                    ri++;
                }
                else if (ri >= rightArr.Length)
                {
                    mainArr[l] = leftArr[li];
                    li++;
                }


                else if (leftArr[li].fitness < rightArr[ri].fitness)    //toggle sorting order in this line
                {
                    mainArr[l] = leftArr[li];
                    li++;
                }
                else
                {
                    mainArr[l] = rightArr[ri];
                    ri++;
                }
            }
        }
    }
}
