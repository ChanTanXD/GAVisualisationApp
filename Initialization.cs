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
        public static GeneticAlgorithm ga = new GeneticAlgorithm();

        //Contains DataInit, PopInit, and Init
        static void DataInit()
        {
            Console.WriteLine("Enter population size (100 max)");
            ga.popSize = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter chromosome length");
            ga.chromLen = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter maximum generations");
            ga.maxGen = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter crossover probability (0.0 to 1.0)");
            ga.crossProb = float.Parse(Console.ReadLine());
            Console.WriteLine("Enter mutation probability (0.0 to 1.0)");
            ga.mutationProb = float.Parse(Console.ReadLine());
            Console.WriteLine("Enter seed number for RNG (0.0 to 1.0)");
            RandomMethods.InitRand(float.Parse(Console.ReadLine()));
            ga.mutationNum = 0;
            ga.crossNum = 0;
        }

        static void PopInit()
        {
            for (int i = 0; i < ga.popSize; i++)
            {
                GeneticAlgorithm.Individual indiv = new GeneticAlgorithm.Individual();
                indiv.chrom = new bool[ga.chromLen];
                ga.newPop[i].chrom = new bool[ga.chromLen];


                for (int j = 0; j < ga.chromLen; j++)
                    indiv.chrom[j] = RandomMethods.Flip(0.5f);

                indiv.x = Interface.Decode(indiv.chrom, ga.chromLen);
                indiv.fitness = Interface.ObjFunc(indiv.x, ga.chromLen);
                indiv.parent1 = 0;
                indiv.parent2 = 0;
                indiv.xSite = 0;

                ga.oldPop[i] = indiv;
            }
        }

        public static GeneticAlgorithm Init()
        {

            DataInit();
            PopInit();
            ga = Interface.Stats(ga, ga.oldPop);
            return ga;
        }
    }
}
