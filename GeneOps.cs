using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static GAVisualisationApp.GeneticAlgorithm;

namespace GAVisualisationApp
{
    public class GeneOps
    {
        //Contains Selection, Crossover, and Mutation methods
        public static int RouletteSelect(int popSize, float sumFitness, Individual[] pop)
        {
            //select a single individual via roulette wheel selection
            int i = 0;
            float partialSum = 0;
            float rand = RandomMethods.RandInt(0, (int)sumFitness);

            while (partialSum < rand && i < popSize-1)
            {
                i++;
                partialSum += (float)pop[i].fitness;
            }
            return i;
        }
        public static int EliteSelect(int popSize, float sumFitness, Individual[] pop)
        {
            //randomly select a single individual, weighted by their fitness ranking
            pop = Interface.PopulationSort(pop);

            int i = 0;
            float partWeight = 3f;
            float rand = RandomMethods.RandFloat();

            for(float weight = 0f; weight < rand && i < popSize - 1; i++)
            {
                weight = popSize * (1/ partWeight);
                partWeight++;
            }
            return i;
        }
        public static int TournamentSelect(int popSize, Individual[] pop)
        {
            //Randomly select two different chromosomes and return the one with best fitness
            int chromA = RandomMethods.RandInt(0, popSize);
            int chromB = RandomMethods.RandInt(0, popSize);

            //Reselect chromeB if both chromes are equal
            while (pop[chromA].chrom.SequenceEqual(pop[chromB].chrom))
                chromB = RandomMethods.RandInt(0, popSize);


            if (pop[chromA].fitness > pop[chromB].fitness)
                return chromA;
            else
                return chromB;
        }
        static bool[] BitFlipMutation(bool[] chrome, ref int mutationNum)
        {
            //Mutate an allele w/ mutationProb, count number of mutations
            int i = RandomMethods.RandInt(0, chrome.Length);
            chrome[i] = !chrome[i];

            mutationNum++;


            return chrome;
        }
        static bool[] InvertMutation(bool[] chrom, float mutationProb, ref int mutationNum, int mutationMethod, int demoMethod)
        {
            //Mutate an allele w/ mutationProb, count number of mutations
            bool mutate = RandomMethods.Flip(mutationProb);

            if (mutate)
            {
                mutationNum++;

                //invert bit position
                int startPos = RandomMethods.RandInt(0, chrom.Length);
                int endPos = RandomMethods.RandInt(0, chrom.Length);
                if(demoMethod == 0)
                {
                    startPos -= (startPos + 1) % 7;
                    endPos -= (endPos + 1) % 7;

                    if (startPos < 0)
                        startPos = 0;
                }
                int limit = endPos - startPos;

                for(int i = 0; i < limit; i++)
                {
                    //Out of bounds, loop back
                    if (startPos == chrom.Length)
                        startPos = 0;
                    if (endPos == -1)
                        endPos = chrom.Length - 1;

                    bool temp = chrom[startPos];
                    chrom[startPos] = chrom[endPos];
                    chrom[endPos] = temp;

                    startPos++;
                    endPos--;
                }
                return chrom;
            }
            else
            {
                //mutation doesn't occur
                return chrom;
            }
        }
        static bool[] RotateMutation(bool[] chrom, float mutationProb, ref int mutationNum, int mutationMethod)
        {
            //Mutate an allele w/ mutationProb, count number of mutations
            bool mutate = RandomMethods.Flip(mutationProb);

            if (mutate)
            {
                mutationNum++;

                //shift right by one bit
                int startPos = RandomMethods.RandInt(0, chrom.Length);
                int endPos = RandomMethods.RandInt(0, chrom.Length);

                for (int i = startPos, j = startPos+1; i != endPos; i++, j++)
                {
                    //Out of bounds, loop back
                    if (i == chrom.Length)
                        i = 0;
                    else if (j == chrom.Length)
                        j = 0;

                    bool temp = chrom[i];
                    chrom[i] = chrom[j];
                    chrom[j] = temp;
                }

                return chrom;
            }
            else
            {
                //mutation doesn't occur
                return chrom;
            }
        }

        static void Mutation(GeneticAlgorithm ga, ref bool[] child1, ref bool[] child2, int mutationMethod, int demoMethod)
        {
            bool mutate = RandomMethods.Flip(ga.mutationProb);

            if (!mutate)
                return;


            if (mutationMethod == 0)
            {
                child1 = BitFlipMutation(child1, ref ga.mutationNum);
                child2 = BitFlipMutation(child2, ref ga.mutationNum);
            }
            else if (mutationMethod == 1)
            {
                child1 = RotateMutation(child1, ga.mutationProb, ref ga.mutationNum, mutationMethod);
                child2 = RotateMutation(child2, ga.mutationProb, ref ga.mutationNum, mutationMethod);
            }
            else if (mutationMethod == 2)
            {
                child1 = InvertMutation(child1, ga.mutationProb, ref ga.mutationNum, mutationMethod, demoMethod);
                child2 = InvertMutation(child2, ga.mutationProb, ref ga.mutationNum, mutationMethod, demoMethod);
            }
        }
        
        public static GeneticAlgorithm SinglePtCross(GeneticAlgorithm ga, int mate1, int mate2,
                                                    ref int iCross, int iPop, int mutationMethod, int demoMethod)
        {
            //Cross 2 parent strings, place in 2 child strings
            bool[] parent1 = ga.oldPop[mate1].chrom;
            bool[] parent2 = ga.oldPop[mate2].chrom;
            bool[] child1 = new bool[ga.chromLen];
            bool[] child2 = new bool[ga.chromLen];

            
            if (RandomMethods.Flip(ga.crossProb))
            {
                iCross = RandomMethods.RandInt(iCross, ga.chromLen - 1);


                //perform crossover between characters
                if (demoMethod == 0)
                    iCross -= iCross % 7;

                ga.crossNum++;
            }
            else
            {
                iCross = ga.chromLen;
            }
            

            //First exchange, 1 to 1 and 2 to 2
            for (int i = 0; i < iCross; i++)
            {
                child1[i] = parent1[i];
                child2[i] = parent2[i];
            }
            //Second exchange, 1 to 2 and 2 to 1
            if (iCross != ga.chromLen)
            {
                //Skip if cross site is chromLen, AKA no crossover
                for (int i = iCross + 1; i < ga.chromLen; i++)
                {
                    child1[i] = parent2[i];
                    child2[i] = parent1[i];
                }
            }

            Mutation(ga, ref child1, ref child2, mutationMethod, demoMethod);
            ga.oldPop[mate1].chrom = parent1;
            ga.oldPop[mate2].chrom = parent2;
            ga.newPop[iPop].chrom = child1;
            ga.newPop[iPop + 1].chrom = child2;
            
            return ga;
        }
    }
}
