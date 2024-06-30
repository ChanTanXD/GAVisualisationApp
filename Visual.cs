using System;
using System.Collections.Generic;
using System.Buffers;
using System.Numerics;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;
using Raylib_cs;
using System.Runtime.InteropServices;

namespace GAVisualisationApp
{
    public class Visual
    {
        public static void CreateVisual(float[] maxFit)
        {
            const int screenWidth = 800;
            const int screenHeight = 450;
            Raylib.InitWindow(screenWidth, screenHeight, "Genetic Algorithm Visual");
            Raylib.SetTargetFPS(60);
            int maxGen = maxFit.Length;
            double norm = 0;

            while (!Raylib.WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------

                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                Raylib.BeginDrawing();

                Raylib.ClearBackground(Color.RayWhite);

                Raylib.DrawText("Darker = fitter", 20, 20, 20, Color.DarkGray);

                for(int i = 1; i < maxGen; i++)
                {
                    norm = Math.Pow(maxFit[i], 5);
                    Color fitScore = new Color { A = (byte)(255d * norm), B = 0, G = 0, R = 0 };
                    Raylib.DrawRectangle((screenWidth / maxGen) * i, 320, screenWidth / (maxGen * 2), 200, fitScore);
                }
                

                Raylib.EndDrawing();
                //----------------------------------------------------------------------------------
            }

            Raylib.CloseWindow();
            return;
        }
    }
}