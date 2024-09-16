using System;
using System.Collections.Generic;
using System.Buffers;
using System.Numerics;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;
using Raylib_CsLo;
using System.Runtime.InteropServices;

namespace GAVisualisationApp
{
    public class Visual
    {
        enum Menu
        {
            MENU_DEMO,
            MENU_SELECTION,
            MENU_CROSSOVER,
            MENU_MUTATION
        }
        enum SubmenuDemo
        {
            SUBMENU_DEMO_WORD_BUILDER,
            SUBMENU_DEMO_CARBOX
        }
        enum SubmenuSelection
        {
            SUBMENU_SELECTION_ROULETTE,
            SUBMENU_SELECTION_ELITISM,
            SUBMENU_SELECTION_TOURNAMENT,
        }
        enum SubmenuCrossover
        {
            SUBMENU_CROSSOVER_SINGLEPOINT,
            SUBMENU_CROSSOVER_TWOPOINT,
            SUBMENU_CROSSOVER_,
        }
        enum SubmenuMutation
        {
            SUBMENU_MUTATION_FLIPBIT,
            SUBMENU_MUTATION_ROTATION,
            SUBMENU_MUTATION_iNVERSION
        }
        enum State
        {
            STATE_NORMAL,
            STATE_SHOW_MENU,
            STATE_SHOW_SUBMENU
        }


        // Context menu
        static readonly string[] menuItems = { "Demo", "Selection", "Crossover", "Mutation" };
        static readonly string[] submenuDemo = { "Word builder", "Carbox" };
        static readonly string[] submenuSelection = { "Roulette", "Elitism", "Tournament"};
        static readonly string[] submenuCrossover = { "Single-point", "Two-point", ""};
        static readonly string[] submenuMutation = { "Flip-Bit", "Rotation", "Inversion"};

        static State state = State.STATE_NORMAL;
        static int mainActive = -1;
        static int mainFocused = -1;
        static int subActive = -1;
        static Rectangle menuRec = new Rectangle(0, 0, 200, 200);

        public static int demoNum = 0;
        public static int selectionNum = 0;
        public static int crossoverNum = 0;
        public static int mutationNum = 0;



        // Sidebar
        public static int gPopSize = 100;
        public static int gChromLen = 100;
        public static int gMaxGen = 30;
        public static int gCrossProb = 90;
        public static int gMutationProb = 20;

        static bool popSizeMode = false;
        static bool chromLenMode = false;
        static bool maxGenMode = false;
        static bool crossProbMode = false;
        static bool mutationProbMode = false;



        public static void ContextMenu()
        {
            string[] submenuText = new string[0];
            int sz = 0;
            int itemHeight = (RayGui.GuiGetStyle((int)GuiControl.LISTVIEW, (int)GuiListViewProperty.LIST_ITEMS_HEIGHT) +
                RayGui.GuiGetStyle((int)GuiControl.LISTVIEW, (int)GuiListViewProperty.LIST_ITEMS_SPACING));

            if (state == State.STATE_SHOW_MENU || state == State.STATE_SHOW_SUBMENU)
            {
                menuRec.height = itemHeight * menuItems.Length + 10;
                int focused = mainFocused;
                unsafe { mainActive = RayGui.GuiListViewEx(menuRec, menuItems, menuItems.Length, &focused, null, mainActive); }


                if (focused != -1)
                    mainFocused = mainActive = focused;

                //close menu when left clicked outside of menu
                if (state == State.STATE_SHOW_MENU)
                {
                    if (Raylib.IsMouseButtonPressed(Raylib.MOUSE_LEFT_BUTTON))
                    {
                        state = State.STATE_NORMAL;
                        mainFocused = mainActive = subActive = -1;
                    }
                }


                if (mainFocused == (int)Menu.MENU_DEMO)
                {
                    submenuText = submenuDemo;
                    state = State.STATE_SHOW_SUBMENU;
                    sz = submenuDemo.Length;
                }
                else if (mainFocused == (int)Menu.MENU_SELECTION)
                {
                    submenuText = submenuSelection;
                    state = State.STATE_SHOW_SUBMENU;
                    sz = submenuSelection.Length;
                }
                else if (mainFocused == (int)Menu.MENU_CROSSOVER)
                {
                    submenuText = submenuCrossover;
                    state = State.STATE_SHOW_SUBMENU;
                    sz = submenuCrossover.Length;
                }
                else if (mainFocused == (int)Menu.MENU_MUTATION)
                {
                    submenuText = submenuMutation;
                    state = State.STATE_SHOW_SUBMENU;
                    sz = submenuMutation.Length;
                }
            }

            if (state == State.STATE_SHOW_SUBMENU && submenuText.Length != 0)
            {
                //create submenu
                Rectangle bounds = new Rectangle(menuRec.X + menuRec.width + 2, menuRec.Y + mainFocused * itemHeight, 200, sz * itemHeight + 10);
                int focused = -1;
                unsafe { subActive = RayGui.GuiListViewEx(bounds, submenuText, sz, &focused, null, subActive); }
                if (Raylib.IsMouseButtonPressed(Raylib.MOUSE_LEFT_BUTTON))
                {
                    //retrive result when clicked on valid menu items
                    if (focused != -1)
                    {
                        Raylib.TraceLog(TraceLogLevel.LOG_INFO, Raylib.TextFormat("CLICKED >>> %s > %s", menuItems[mainActive], submenuText[focused]));

                        if(mainActive == 0)
                            demoNum = focused;
                        else if (mainActive == 1)
                            selectionNum = focused;
                        else if(mainActive == 2)
                            crossoverNum = focused;
                        else if(mainActive == 3)
                            mutationNum = focused;
                    }

                    state = State.STATE_NORMAL;
                    mainFocused = mainActive = subActive = -1;
                }
            }
        }
        unsafe static void Sidebar()
        {
            //Workaround for Raylib wrapper class (passing int* arguments)
            int popSize = gPopSize;
            int chromLen = gChromLen;
            int maxGen = gMaxGen;
            int crossProb = gCrossProb;
            int mutationProb = gMutationProb;


            Raylib.DrawText("Population size", 10, 10, 30, Raylib.BLACK);
            if (RayGui.GuiValueBox(new Rectangle(10, 40, 100, 50), "", &popSize, 0, 1000, popSizeMode))
                popSizeMode = !popSizeMode;
            Raylib.DrawText("Chromosome length", 10, 90, 30, Raylib.BLACK);
            if (RayGui.GuiValueBox(new Rectangle(10, 120, 100, 50), "", &chromLen, 0, 500, chromLenMode))
                chromLenMode = !chromLenMode;
            Raylib.DrawText("Generations", 10, 170, 30, Raylib.BLACK);
            if (RayGui.GuiValueBox(new Rectangle(10, 200, 100, 50), "", &maxGen, 0, 500, maxGenMode))
                maxGenMode = !maxGenMode;
            Raylib.DrawText("Crossover probability", 10, 250, 30, Raylib.BLACK);
            if (RayGui.GuiValueBox(new Rectangle(10, 280, 100, 50), "", &crossProb, 0, 100, crossProbMode))
                crossProbMode = !crossProbMode;
            Raylib.DrawText("Mutation probability", 10, 330, 30, Raylib.BLACK);
            if (RayGui.GuiValueBox(new Rectangle(10, 360, 100, 50), "", &mutationProb, 0, 100, mutationProbMode))
                mutationProbMode = !mutationProbMode;
            //Raylib.DrawText("Seed", 10, 410, 30, Raylib.BLACK);
            //RayGui.GuiTextBox(new Rectangle(10, 440, 100, 50), "", 30, true);


            //Workaround2
            gPopSize = popSize;
            gChromLen = chromLen;
            gMaxGen = maxGen;
            gCrossProb = crossProb;
            gMutationProb = mutationProb;



            Raylib.DrawText("Demonstration", 1190 - Raylib.MeasureText("Demonstration",30), 10, 30, Raylib.BLACK);
            Raylib.DrawText(submenuDemo[demoNum], 1190 - Raylib.MeasureText(submenuDemo[demoNum], 30), 40, 30, Raylib.BLACK);
            Raylib.DrawText("Selection", 1190 - Raylib.MeasureText("Selection", 30), 90, 30, Raylib.BLACK);
            Raylib.DrawText(submenuSelection[selectionNum], 1190 - Raylib.MeasureText(submenuSelection[selectionNum], 30), 120, 30, Raylib.BLACK);
            Raylib.DrawText("Crossover", 1190 - Raylib.MeasureText("Crossover", 30), 170, 30, Raylib.BLACK);
            Raylib.DrawText(submenuCrossover[crossoverNum], 1190 - Raylib.MeasureText(submenuCrossover[crossoverNum], 30), 200, 30, Raylib.BLACK);
            Raylib.DrawText("Mutation", 1190 - Raylib.MeasureText("Mutation", 30), 250, 30, Raylib.BLACK);
            Raylib.DrawText(submenuMutation[mutationNum], 1190 - Raylib.MeasureText(submenuMutation[mutationNum], 30), 280, 30, Raylib.BLACK);


            if (RayGui.GuiButton(new Rectangle(10, 510, 200, 40), "Execute"))
            {
                MainProg.Execute(gPopSize, gChromLen, gMaxGen, gCrossProb, gMutationProb, demoNum, selectionNum, crossoverNum, mutationNum);
            }
        }
        public static void CreateVisual()
        {
            const int screenWidth = 1200;
            const int screenHeight = 675;
            Raylib.InitWindow(screenWidth, screenHeight, "Genetic Algorithm Visual");
            Raylib.SetTargetFPS(60);
            RayGui.GuiLoadStyleDefault();
            RayGui.GuiSetStyle(default, (int)GuiDefaultProperty.TEXT_SIZE, 30);

            
            while (!Raylib.WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------

                //Open context menu
                Vector2 mouse = Raylib.GetMousePosition();
                if(state == State.STATE_NORMAL)
                {
                    if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT))
                    {
                        state = State.STATE_SHOW_MENU;
                        menuRec.X = mouse.X;
                        menuRec.Y = mouse.Y;
                    }
                }


                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib.RAYWHITE);

                ContextMenu();
                Sidebar();
                


                Raylib.EndDrawing();
                //----------------------------------------------------------------------------------
            }

            Raylib.CloseWindow();
            return;
        }
        
    }
}