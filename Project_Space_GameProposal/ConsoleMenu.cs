using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Space_GameProposal {
    internal class ConsoleMenu {

        public void ConsoleDevMenu() {
            string ascii = @"
██████╗ ██████╗  ██████╗      ██╗███████╗ ██████╗████████╗    ███████╗██████╗  █████╗  ██████╗███████╗
██╔══██╗██╔══██╗██╔═══██╗     ██║██╔════╝██╔════╝╚══██╔══╝    ██╔════╝██╔══██╗██╔══██╗██╔════╝██╔════╝
██████╔╝██████╔╝██║   ██║     ██║█████╗  ██║        ██║       ███████╗██████╔╝███████║██║     █████╗  
██╔═══╝ ██╔══██╗██║   ██║██   ██║██╔══╝  ██║        ██║       ╚════██║██╔═══╝ ██╔══██║██║     ██╔══╝  
██║     ██║  ██║╚██████╔╝╚█████╔╝███████╗╚██████╗   ██║       ███████║██║     ██║  ██║╚██████╗███████╗
╚═╝     ╚═╝  ╚═╝ ╚═════╝  ╚════╝ ╚══════╝ ╚═════╝   ╚═╝       ╚══════╝╚═╝     ╚═╝  ╚═╝ ╚═════╝╚══════╝
                                                                                                      ";

            
            ConsoleKeyInfo input = new ConsoleKeyInfo();

            Console.CursorVisible = false;

            int option = 1;

            string color = "\u001b[0m";
            string def = "\u001b[32m";

            bool select = true;

            Console.WriteLine(ascii);
            int left = Console.CursorLeft;
            int top = Console.CursorTop;

            while (option != 1 || select) {

                while (select) {
                    
                    Console.SetCursorPosition(left, top);
                    Console.WriteLine("Console menu: Navigate with Key arrows or [W] [S]: Enter to Select");
                    Console.WriteLine();
                    Console.WriteLine($"{(option == 1 ? def : color)}<< Launch Project Space.exe >>{color}");
                    Console.WriteLine($"{(option == 2 ? def : color)}<< Project Description >>{color}");
                    Console.WriteLine($"{(option == 3 ? def : color)}<< Exit >>{color}");

                    input = Console.ReadKey(true);

                    switch (input.Key) {
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            option = (option == 3 ? 1 : option + 1);
                            break;
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            option = (option == 1 ? 3 : option - 1);
                            break;
                        case ConsoleKey.Enter:
                            select = false;
                            break;
                    }
                }

                switch (option) {
                    case 1:
                        Console.Clear();
                        Application.Run(new FormMenu());
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine(ascii);
                        Console.WriteLine("(11/08/2023)\n\nGame Developed by: ");
                        Console.WriteLine("John Aldrine F. Lim");
                        Console.WriteLine("John Wilbert J. Laiño");
                        Console.WriteLine("David Joseph N. Cortez");

                        Console.WriteLine("\nAn ITLAB Requirement for 2023 - 2024\n");
                        Console.WriteLine("This is a looped arcade style game where you control your ship to avoid asteroids.");
                        Console.WriteLine("The farther your spacecraft go, the more score and coins you will get. (Coins refers to score)");
                        Console.WriteLine("These coins can be used to purchase different components of a spacecraft to upgrade your own ship");
                        Console.WriteLine("The components have different attributes such as weight, thrust power, fuel capacity and durability. \nYour goal is to reach the end of the limitless universe and set a new high score.");
                        Console.WriteLine("\nPress Enter to return...");
                        input = Console.ReadKey(true);
                        Console.Clear();
                        Console.Write(ascii); 
                        select = true;
                        break;

                    case 3:
                        Environment.Exit(0);
                        break;
                }
            }
           
        }
    }
}
