using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    internal class Menu
    {
        static string[] pozycjeMenu = { "Opcja 1", "Opcja 2", "Opcja 3", "Opcja 4", "Opcja 5", "Koniec" };
        static int aktywnaPozycjaMenu = 0;

        public static void StartMenu()
        {
            Console.Title = "elo zelo";
            Console.CursorVisible = false;
            while (true)
            {
                PokazMenu();
                WybieranieOpcji();
                UruchomOpcje();
            }
            static void PokazMenu()
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("hejka naklejka");
                Console.WriteLine();
                for (int i = 0; i < pozycjeMenu.Length; i++)
                {
                    if (i == aktywnaPozycjaMenu)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.ForegroundColor = ConsoleColor.White; 
                        Console.WriteLine("{0,-35}", pozycjeMenu[i]);
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                    }
                    else
                    {
                        Console.WriteLine(pozycjeMenu[i]);
                    }
                }
            }
            static void WybieranieOpcji()
            {
                do
                {
                    ConsoleKeyInfo klawisz = Console.ReadKey();
                    if (klawisz.Key == ConsoleKey.UpArrow)
                    {
                        aktywnaPozycjaMenu = (aktywnaPozycjaMenu > 0) ? aktywnaPozycjaMenu - 1 : pozycjeMenu.Length - 1;
                        PokazMenu();
                    }
                    else if (klawisz.Key == ConsoleKey.DownArrow)
                    {
                        aktywnaPozycjaMenu = (aktywnaPozycjaMenu + 1) % pozycjeMenu.Length;
                        PokazMenu();
                    }
                    else if (klawisz.Key == ConsoleKey.Escape)
                    {
                        aktywnaPozycjaMenu = pozycjeMenu.Length - 1;
                        break;
                    }
                    else if (klawisz.Key == ConsoleKey.Enter)
                        break;

                } while (true);
            }
            static void UruchomOpcje()
            {
                switch (aktywnaPozycjaMenu)
                {
                    case 0: 
                        Console.Clear(); 
                        opcjawBudowie();
                        break;
                    case 1: 
                        Console.Clear(); 
                        opcjawBudowie(); 
                        break;
                    case 2: 
                        Console.Clear(); 
                        opcjawBudowie(); 
                        break;
                    case 3: 
                        Console.Clear(); 
                        opcjawBudowie(); 
                        break;
                    case 4: 
                        Console.Clear(); 
                        opcjawBudowie(); 
                        break;
                    case 5: 
                        Environment.Exit(0); 
                        break;
                    default:
                        Environment.Exit(0);
                        break;
                }
            }
            static void opcjawBudowie()
            {
                Console.SetCursorPosition(12, 4);
                Console.Write("Witam, dzien dobry");
                Console.ReadKey();
            }
        }
    }
    internal class Program
    {
        public static void Main()
        {
            Menu.StartMenu();
        }
    }
}
