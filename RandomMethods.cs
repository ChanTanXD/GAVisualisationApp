using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GAVisualisationApp
{
    public class RandomMethods
    {
        public static Random rand = new Random();
        static float[] oldRand = new float[55];
        static int curRand;
        static void AdvRand()
        {
            //Retrieves a new batch of pseudorandom numbers
            float newRand;

            for (int i = 0; i < 23; i++)
            {
                newRand = oldRand[i] - oldRand[i + 29];
                if (newRand < 0f)
                    newRand++;
                oldRand[i] = newRand;
            }

            for (int i = 24; i < 54; i++)
            {
                newRand = oldRand[i] - oldRand[i - 22];
                if (newRand < 0f)
                    newRand++;
                oldRand[i] = newRand;
            }
        }
        public static void InitRand(float seed)
        {
            float newRand, prevRand;
            oldRand[54] = seed;
            newRand = 1.0e-7f;
            prevRand = seed;

            for (int i = 1; i < 54; i++)
            {
                int j = (21 * i) % 54;
                oldRand[j] = newRand;
                newRand = prevRand - newRand;
                if (newRand < 0f)
                    newRand++;
                prevRand = oldRand[j];
            }

            for (int i = 0; i < 3; i++)
                AdvRand();
            curRand = 0;
        }
        public static float RandTest()
        {
            //return a random real number between one and zero via subtractive method
            //WIP
            curRand++;

            if (curRand >= 55)
            {
                curRand = 1;
                AdvRand();
            }

            return oldRand[curRand];
        }
        public static float RandFloat()
        {
            float randNum = (float)rand.NextDouble();
            return randNum;
        }
        public static bool Flip(float i)
        {
            //return a random boolean between true and false
            bool flip = i > rand.NextDouble();
            return flip;
        }
        public static int RandInt(int min, int max)
        {
            //return a random integer between the specified bounds
            return rand.Next(min, max);
        }
    }
}
