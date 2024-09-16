using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GAVisualisationApp.GeneticAlgorithm;

namespace GAVisualisationApp
{
    public class Generation
    {
        public static GeneticAlgorithm Generate(GeneticAlgorithm ga, int demoMethod, int selectionMethod, int crossoverMethod, int mutationMethod)
        {
            //Create a new generation via Select, Crossover, and Mutation
            //***This method assumes an even-numbered popsize***

            //repeat Select, Crossover, and Mutation until newPop is filled
            for (int i = 0; i < ga.popSize; i += 2)
            {
                int mate1 = 0;
                int mate2 = 0;
                int iCross = 1;


                //pick a pair of mates
                if (selectionMethod == 0)
                {
                    mate1 = GeneOps.RouletteSelect(ga.popSize, ga.sumFitness, ga.oldPop);
                    mate2 = GeneOps.RouletteSelect(ga.popSize, ga.sumFitness, ga.oldPop);
                }
                else if(selectionMethod == 1)
                {
                    mate1 = GeneOps.EliteSelect(ga.popSize, ga.sumFitness, ga.oldPop);
                    mate2 = GeneOps.EliteSelect(ga.popSize, ga.sumFitness, ga.oldPop);
                }
                else if (selectionMethod == 2)
                {
                    mate1 = GeneOps.TournamentSelect(ga.popSize, ga.oldPop);
                    mate2 = GeneOps.TournamentSelect(ga.popSize, ga.oldPop);
                }


                //Crossover and Mutation
                if(crossoverMethod == 0)
                    ga = GeneOps.SinglePtCross(ga, mate1, mate2, ref iCross, i, mutationMethod, demoMethod);
                else if (crossoverMethod == 1)      //two-point crossover
                {
                    ga = GeneOps.SinglePtCross(ga, mate1, mate2, ref iCross, i, mutationMethod, demoMethod);
                    if(iCross != ga.chromLen)
                    {
                        iCross++;
                        ga = GeneOps.SinglePtCross(ga, mate1, mate2, ref iCross, i, mutationMethod, demoMethod);
                    }
                }
                else if (crossoverMethod == 2)
                {
                    //ga = GeneOps.SinglePtCross(ga, mate1, mate2, ref iCross, i, mutationMethod);
                }


                //Decode string, evaluate fitness, record parentage data on both children
                if(demoMethod == 0)
                {
                    char[] wordBuild;

                    wordBuild = Interface.DecodeWordBuilder(ga.newPop[i].chrom, ga.chromLen);
                    ga.newPop[i].word = new string(wordBuild);
                    ga.newPop[i].fitness = Interface.WordBuildFitness(wordBuild, ga.target.Length, (ga.target));

                    wordBuild = Interface.DecodeWordBuilder(ga.newPop[i+1].chrom, ga.chromLen);
                    ga.newPop[i+1].word = new string(wordBuild);
                    ga.newPop[i+1].fitness = Interface.WordBuildFitness(wordBuild, ga.target.Length, ga.target);
                }
                /*
                double x;
                x = Interface.Decode(ga.newPop[i].chrom, ga.chromLen);
                ga.newPop[i].fitness = Interface.ObjFunc(x, ga.chromLen);
                
                x = Interface.Decode(ga.newPop[i + 1].chrom, ga.chromLen);
                ga.newPop[i + 1].fitness = Interface.ObjFunc(x, ga.chromLen);
                */
                ga.newPop[i].parent1 = mate1;
                ga.newPop[i].parent2 = mate2;
                ga.newPop[i].xSite = iCross;

                ga.newPop[i + 1].parent1 = mate1;
                ga.newPop[i + 1].parent2 = mate2;
                ga.newPop[i + 1].xSite = iCross;

            }

            ga = Interface.Stats(ga, ga.newPop);
            ga.oldPop = ga.newPop;
            return ga;
        }
    }
}
