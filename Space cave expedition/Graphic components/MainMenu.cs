﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Space_cave_expedition.Enums;
using Space_cave_expedition.Helpers;

namespace Space_cave_expedition.Graphic_Components
{
    public class MainMenu
    {
        #region Section displaying and manipulation

        public void DisplayMenu()
        {
            Console.SetCursorPosition(0, 0);
            switch (CurrentSection)
            {
                case MainMenuSection.MainMenu:
                    DisplayMainMenu();
                    break;
                case MainMenuSection.MapSelection:
                    DisplayPlay();
                    break;
                default:
                    throw new ArgumentException("Unexpected MainMenuSection.");
            }
        }

        private void DisplayMainMenu()
        {
            MainMenuHelper.MakeFrame();
            //Title text
            DisplayMainTitle();
            MainMenuHelper.FillALine('=', 1, 13);

            if (currentCursorIndex == 0)
                MainMenuHelper.WriteInCenter("Play", 15, ConsoleColor.Gray, ConsoleColor.Blue);
            else
                MainMenuHelper.WriteInCenter("Play", 15, ConsoleColor.Gray, ConsoleColor.Black);

            if (currentCursorIndex == 1)
                MainMenuHelper.WriteInCenter("Editor", 18, ConsoleColor.Gray, ConsoleColor.Blue);
            else
                MainMenuHelper.WriteInCenter("Editor", 18, ConsoleColor.Gray, ConsoleColor.Black);

            if (currentCursorIndex == 2)
                MainMenuHelper.WriteInCenter("Settings", 21, ConsoleColor.Gray, ConsoleColor.Blue);
            else
                MainMenuHelper.WriteInCenter("Settings", 21, ConsoleColor.Gray, ConsoleColor.Black);

            if (currentCursorIndex == 3)
                MainMenuHelper.WriteInCenter("Exit", 24, ConsoleColor.Gray, ConsoleColor.Blue);
            else
                MainMenuHelper.WriteInCenter("Exit", 24, ConsoleColor.Gray, ConsoleColor.Black);
        }
        private void DisplayPlay()
        {
            //Load maps
            mainMaps = Helper.GetAndVerifyMaps(Environment.CurrentDirectory + "\\Map layouts\\Main");
            customMaps = Helper.GetAndVerifyMaps(Environment.CurrentDirectory + "\\Map layouts\\Custom");

            //New game top part
            MainMenuHelper.MakeFrame();
            MainMenuHelper.WriteInCenter("New game", 2);
            MainMenuHelper.FillALine('=', 1, 4);

            for(int i = 5; i < Console.WindowHeight - 2; i++)
            {
                MainMenuHelper.WriteInCenter("|", Console.CursorTop);
            }

            //Display main maps
            MainMenuHelper.WriteText("Main maps", 3, 6);
            for(int i = 0; i < mainMaps.Count; i++)
            {
                if (currentTabIndex == 0 && currentCursorIndex == i)
                    MainMenuHelper.WriteText(mainMaps[i].Split('\\')[^1], 6, ConsoleColor.Gray, ConsoleColor.Blue);
                else
                    MainMenuHelper.WriteText(mainMaps[i].Split('\\')[^1], 6);
            }

            //Display custom maps
            MainMenuHelper.WriteText("Custom maps", Console.WindowWidth / 2 + 4, 6);

            for (int i = 0; i < customMaps.Count; i++)
            {
                if (currentTabIndex == 1 && currentCursorIndex == i)
                    MainMenuHelper.WriteText(customMaps[i].Split('\\')[^1], Console.WindowWidth / 2 + 7, ConsoleColor.Gray, ConsoleColor.Blue);
                else
                    MainMenuHelper.WriteText(customMaps[i].Split('\\')[^1], Console.WindowWidth / 2 + 7);
            }
        }

        #endregion
        /// <summary>
        /// On which tab the user is currently on, used in the map selection for whether the player is on the right or left side.
        /// </summary>
        int currentTabIndex;
        /// <summary>
        /// Index for which position the in-game cursor is in the main menu.
        /// </summary>
        int currentCursorIndex;
        /// <summary>
        /// The highest index the cursor can have.
        /// </summary>
        int CursorIndexLimit { get; set; }
        private MainMenuSection _CurrentSection;
        /// <summary>
        /// Which main menu section is currently being displayed.
        /// </summary>
        public MainMenuSection CurrentSection
        {
            get { return _CurrentSection; }
            set { _CurrentSection = value;}
        }
        private List<string> mainMaps;
        private List<string> customMaps;

        /// <summary>
        /// Creates an instance of a main menu and displays it.
        /// </summary>
        public MainMenu()
        {            
            Console.SetWindowSize(60, 28);
            Console.BufferHeight = 29;
            Console.BufferWidth = 61;
            Console.CursorVisible = false;
            CurrentSection = MainMenuSection.MainMenu;
            ConsoleHook.DisableAllResizingControl();

            DisplayMenu();
            WaitForInput();
        }
        private void WaitForInput()
        {
            while (true)
            {
                ConsoleKey pressedKey = Console.ReadKey().Key;

                switch (pressedKey)
                {
                    //Up, down and enter navigation
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        if (currentCursorIndex > 0)
                            currentCursorIndex--;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        if (currentCursorIndex < CursorIndexLimit)
                            currentCursorIndex++;
                        break;
                    case ConsoleKey.Enter:
                        if(CurrentSection == MainMenuSection.MainMenu)
                        {
                            switch (currentCursorIndex)
                            {
                                case 0:
                                    CurrentSection = MainMenuSection.MapSelection;
                                    Console.Clear();
                                    break;
                                case 1:
                                    CurrentSection = MainMenuSection.Editor;
                                    Console.Clear();
                                    break;
                                case 2:
                                    CurrentSection = MainMenuSection.Settings;
                                    Console.Clear();
                                    break;
                                case 3:
                                    Console.Clear();
                                    return;
                            }
                        }
                        else if (CurrentSection == MainMenuSection.MapSelection)
                        {

                        }
                        break;
                    //Navigation with tabIndex
                    case ConsoleKey.Tab:
                        if(CurrentSection == MainMenuSection.MapSelection)
                        {
                            if (currentTabIndex == 0)
                            {
                                goto case ConsoleKey.RightArrow;
                            }
                            else
                            {
                                goto case ConsoleKey.LeftArrow;
                            }
                        }
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        if(CurrentSection == MainMenuSection.MapSelection)
                        {
                            currentTabIndex = 0;
                            CursorIndexLimit = mainMaps.Count;

                            if (currentCursorIndex > CursorIndexLimit)
                                currentCursorIndex = CursorIndexLimit;
                        }
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        if (CurrentSection == MainMenuSection.MapSelection)
                        {
                            currentTabIndex = 1;
                            CursorIndexLimit = customMaps.Count;

                            if (currentCursorIndex > CursorIndexLimit)
                                currentCursorIndex = CursorIndexLimit;
                        }
                        break;
                }
                DisplayMenu();
            }
        }


        //Displaying helper methods
        private void DisplayMainTitle(int startingTop = 1)
        {
            Console.CursorTop = startingTop;
            string titleText = @"             __   __            __           ;" + "\n" + 
                               @"            /    /  \  |    |  /             " + "\n" +
                               @"            |    |__|  \    /  |__           " + "\n" +
                               @"            |    |  |   \  /   |             " + "\n" +
                               @"            \__  |  |    \/    \__           " + "\n" +
                               @" ___         __          __    __    __   __ " + "\n" +
                               @"/     \  /  /  \  |     /  \  /  \  /    /  \" + "\n" +
                               @"|___   \/   |__/  |     |  |  |__/  |__  |__/" + "\n" +
                               @"|      /\   |     |     |  |  | \   |    | \ " + "\n" +
                               @"\___  /  \  |     |___  \__/  |  \  \__  |  \" + "\n";

            foreach(string s in titleText.Split('\n'))
            {
                MainMenuHelper.WriteInCenter(s, Console.CursorTop);
            }
        }
}
}
