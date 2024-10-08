﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAVisualisationApp
{
    public class GeneticAlgorithm
    {
        const int maxPop = 1000;
        const int maxString = 30;
        public struct Individual
        {
            public Individual(bool[] chromosome, double x, double fitness, int parent1, int parent2, int xsite)
            {
                chrom = chromosome;
                this.x = x;
                this.fitness = fitness;
                this.parent1 = parent1;
                this.parent2 = parent2;
                this.xSite = xsite;
            }
            public bool[] chrom { get; set; }
            public double x { get; set; }
            public double fitness { get; set; }
            public int parent1 { get; set; }
            public int parent2 { get; set; }
            public int xSite { get; set; }
            public string word { get; set; }
        }
        //individual[] represents population
        public Individual[] oldPop = new Individual[maxPop];
        public Individual[] newPop = new Individual[maxPop];
        public int popSize, chromLen, gen, maxGen;
        public float crossProb, mutationProb, sumFitness;
        public int mutationNum, crossNum;
        public int max, min;
        public float avg;

        //the target for word builder 
        public char[] target = new char[0];
    }
}
