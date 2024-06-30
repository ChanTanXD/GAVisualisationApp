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
        public static GeneticAlgorithm Generate(GeneticAlgorithm ga)
        {
            //Create a new generation via Select, Crossover, and Mutation
            //***This method assumes an even-numbered popsize***
            int i, mate1, mate2, iCross;
            i = 1;
            iCross = 0;


            //repeat Select, Crossover, and Mutation until newPop is filled
            while (i <= ga.popSize)
            {
                //pick a pair of mates
                mate1 = GeneOps.Select(ga.popSize, ga.sumFitness, ga.oldPop);
                mate2 = GeneOps.Select(ga.popSize, ga.sumFitness, ga.oldPop);

                //Crossover and Mutation
                ga = GeneOps.Crossover(ga, mate1, mate2, ref iCross, i);


                //Decode string, evaluate fitness, record parentage data on both children
                double x;
                x = Interface.Decode(ga.newPop[i].chrom, ga.chromLen);
                ga.newPop[i].fitness = Interface.ObjFunc(x, ga.chromLen);
                ga.newPop[i].parent1 = mate1;
                ga.newPop[i].parent2 = mate2;
                ga.newPop[i].xSite = iCross;
                x = Interface.Decode(ga.newPop[i + 1].chrom, ga.chromLen);
                ga.newPop[i + 1].fitness = Interface.ObjFunc(x, ga.chromLen);
                ga.newPop[i + 1].parent1 = mate1;
                ga.newPop[i + 1].parent2 = mate2;
                ga.newPop[i + 1].xSite = iCross;

                //Increase population index
                i += 2;
            }

            ga = Interface.Stats(ga, ga.newPop);
            ga.oldPop = ga.newPop;
            return ga;
        }
    }
}
