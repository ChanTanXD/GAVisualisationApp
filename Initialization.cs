using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GAVisualisationApp
{
    public class Initialization
    {
        //Contains Init and PopInit
        public static GeneticAlgorithm Init(int popSize, int chromLen, int maxGen, float crossProb, float mutationProb,
                                            float seed,int demoMethod, string targetWord)
        {
            GeneticAlgorithm ga = new GeneticAlgorithm();

            //100 max
            ga.popSize = popSize;
            //ga.chromLen = chromLen;
            ga.chromLen = (targetWord.Length * 7);
            ga.maxGen = maxGen;
            //0.0 to 1.0
            ga.crossProb = crossProb;
            //0.0 to 1.0
            ga.mutationProb = mutationProb;
            //0.0 to 1.0
            RandomMethods.InitRand(seed);
            ga.mutationNum = 0;
            ga.crossNum = 0;


            if(demoMethod == 0)
            {
                ga.target = targetWord.ToCharArray();
            }


            ga = PopInit(ga,demoMethod);
            ga = Interface.Stats(ga, ga.oldPop);
            return ga;
        }

        static GeneticAlgorithm PopInit(GeneticAlgorithm ga, int demoMethod)
        {
            for (int i = 0; i < ga.popSize; i++)
            {
                GeneticAlgorithm.Individual indiv = new GeneticAlgorithm.Individual();
                indiv.chrom = new bool[ga.chromLen];
                ga.newPop[i].chrom = new bool[ga.chromLen];


                for (int j = 0; j < ga.chromLen; j++)
                {
                    indiv.chrom[j] = RandomMethods.Flip(0.5f);
                }


                if (demoMethod == 0)
                {
                    char[] wordBuild;
                    wordBuild = Interface.DecodeWordBuilder(indiv.chrom, ga.chromLen);
                    indiv.word = new string(wordBuild);
                    indiv.fitness = Interface.WordBuildFitness(wordBuild, ga.target.Length, ga.target);
                }

                /*
                for (int j = 0; j < ga.chromLen; j++)
                    indiv.chrom[j] = RandomMethods.Flip(0.5f);

                indiv.x = Interface.Decode(indiv.chrom, ga.chromLen);
                indiv.fitness = Interface.ObjFunc(indiv.x, ga.chromLen, demoMethod);
                */
                indiv.parent1 = 0;
                indiv.parent2 = 0;
                indiv.xSite = 0;

                ga.oldPop[i] = indiv;
            }

            return ga;
        }
    }
}
