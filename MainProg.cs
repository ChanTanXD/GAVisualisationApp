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
        public static void Main(string[] args)
        {
            GeneticAlgorithm ga = new GeneticAlgorithm();
            int gen = 0;


            //Initialize GA values
            ga = Initialization.Init();
            float[] maxFit = new float[ga.maxGen];

            //Generate following generations
            for (gen = 0; gen < ga.maxGen; gen++)
            {
                ga = Generation.Generate(ga);
                maxFit[gen] = ga.max;
                Console.WriteLine("gen{0} fittest individual: {1}",gen , ga.max);
            }

            Visual.CreateVisual(maxFit);
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
