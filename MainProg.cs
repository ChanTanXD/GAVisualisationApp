using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

//Reminder: All population indexes start at 1 instead of 0
//Try initiating a loop with i = 1 when population is involved

namespace GAVisualisationApp
{
    public class MainProg
    {
        public static void Execute(int popSize, int chromLen, int maxGen, int crossProb, int mutationProb,
                                    int demoNum, int selectionNum, int crossoverNum, int mutationNum)
        {
            GeneticAlgorithm ga = new GeneticAlgorithm();
            int gen = 0;

            //Initialize GA values
            float crossProbFloat = (float)crossProb / 100f;
            float mutationProbFloat = (float)mutationProb / 100f;
            ga = Initialization.Init(popSize, chromLen, maxGen, crossProbFloat, mutationProbFloat, 0f, demoNum, "Fuck Satay");
            float[] maxFit = new float[ga.maxGen];

            //Generate following generations
            for (gen = 0; gen < ga.maxGen; gen++)
            {
                ga = Generation.Generate(ga, demoNum,selectionNum,crossoverNum,mutationNum);
                maxFit[gen] = ga.max;
                Console.WriteLine("gen{0} fittest individual: {1}",gen+1 , ga.newPop[ga.max].fitness);
                Console.WriteLine("gen{0} fittest individual: {1}", gen+1, ga.newPop[ga.max].word);

                //for (int i = 0; i < ga.popSize; i++)
                    //Console.WriteLine(ga.newPop[i].word);
            }
            Console.WriteLine(Interface.WordBuildFitness(ga.target, ga.target.Length, ga.target));
        }
        public static void Main(string[] args)
        {
            Visual.CreateVisual();
        }

        public static void PrintChrom(bool[] chrom)
        {
            foreach (bool bit in chrom)
            {
                if (bit)
                    Console.Write(1);
                else if (!bit)
                    Console.Write(0);
            }
            Console.WriteLine();
        }
    }
}
