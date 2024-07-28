using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GAVisualisationApp.GeneticAlgorithm;

namespace GAVisualisationApp
{
    public class GeneOps
    {
        //Contains Selection, Crossover, and Mutation
        public static int RouletteSelect(int popSize, float sumFitness, Individual[] pop)
        {
            //select a single individual via roulette wheel selection
            int i = 0;
            float partSum = 0f;
            float rand = RandomMethods.RandFloat() * sumFitness;

            while (partSum < rand && i < popSize-1)
            {
                i++;
                partSum += (float)pop[i].fitness;
            }
            return i;
        }
        public static int RankSelect(int popSize, float sumFitness, Individual[] pop)
        {
            //select a single individual via roulette wheel selection
            int i = 0;
            float partSum = 0f;
            float rand = RandomMethods.RandFloat() * sumFitness;

            while (partSum < rand && i < popSize - 1)
            {
                i++;
                partSum += (float)pop[i].fitness;
            }
            return i;
        }
        public static int SteadySelect(int popSize, float sumFitness, Individual[] pop)
        {
            //select a single individual via roulette wheel selection
            int i = 0;
            float partSum = 0f;
            float rand = RandomMethods.RandFloat() * sumFitness;

            while (partSum < rand && i < popSize - 1)
            {
                i++;
                partSum += (float)pop[i].fitness;
            }
            return i;
        }
        public static bool Mutation(bool alleleVal, float mutationProb, ref int mutationNum)
        {
            //Mutate an allele w/ mutationProb, count number of mutations
            bool mutate = RandomMethods.Flip(mutationProb);

            if (mutate)
            {
                mutationNum++;
                //change bit value
                return !alleleVal;
            }
            else
            {
                //no change
                return alleleVal;
            }
        }
        public static GeneticAlgorithm Crossover(GeneticAlgorithm ga, int mate1, int mate2,
                                                    ref int iCross, int iPop)
        {
            //Cross 2 parent strings, place in 2 child strings
            bool[] parent1 = ga.oldPop[mate1].chrom;
            bool[] parent2 = ga.oldPop[mate2].chrom;
            bool[] child1 = new bool[ga.chromLen];
            bool[] child2 = new bool[ga.chromLen];


            if (RandomMethods.Flip(ga.crossProb))
            {
                iCross = RandomMethods.RandInt(1, ga.chromLen - 1);
                ga.crossNum++;
            }
            else
            {
                iCross = ga.chromLen;
            }


            //First exchange, 1 to 1 and 2 to 2
            for (int i = 0; i < iCross; i++)
            {
                child1[i] = Mutation(parent1[i], ga.mutationProb, ref ga.mutationNum);
                child2[i] = Mutation(parent2[i], ga.mutationProb, ref ga.mutationNum);
            }
            //Second exchange, 1 to 2 and 2 to 1
            if (iCross != ga.chromLen)
            {
                //Skip if cross site is chromLen, AKA no crossover
                for (int i = iCross + 1; i < ga.chromLen; i++)
                {
                    child1[i] = Mutation(parent2[i], ga.mutationProb, ref ga.mutationNum);
                    child2[i] = Mutation(parent1[i], ga.mutationProb, ref ga.mutationNum);
                }
            }


            ga.oldPop[mate1].chrom = parent1;
            ga.oldPop[mate2].chrom = parent2;
            ga.newPop[iPop].chrom = child1;
            ga.newPop[iPop + 1].chrom = child2;
            return ga;
        }
    }
}
