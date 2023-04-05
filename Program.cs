using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ConsoleApp7
{
    internal static class Menu
    {
        static public string[] pozycjeMenu_wejsciowe = { "Logowanie", "Zaloz konto", "Przeglądaj loty",  "\nWyjdz z aplikacji" };
        static public int aktywnaPozycjaMenu = 0;
        static public string login_rejestracja;
        static public string password_rejestracja;
        static public string login;
        static public string password;
        public static void StartMenu()
        {
            Console.Title = "Odlot - loty i przeloty";
            Console.CursorVisible = false;
            while (true)
            {
                PokazMenu();
                WybieranieOpcji();
                UruchomOpcje_menu_wejsciowe();
            }
            static void PokazMenu()
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Witamy na stronie odlotu, oto początek twojej niezapomnianej przygody!");
                Console.WriteLine();
                for (int i = 0; i < pozycjeMenu_wejsciowe.Length; i++)
                {
                    if (i == aktywnaPozycjaMenu)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White; 
                        Console.WriteLine(pozycjeMenu_wejsciowe[i]);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.WriteLine(pozycjeMenu_wejsciowe[i]);
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
                        aktywnaPozycjaMenu = (aktywnaPozycjaMenu > 0) ? aktywnaPozycjaMenu - 1 : pozycjeMenu_wejsciowe.Length - 1;
                        PokazMenu();
                    }
                    else if (klawisz.Key == ConsoleKey.DownArrow)
                    {
                        aktywnaPozycjaMenu = (aktywnaPozycjaMenu + 1) % pozycjeMenu_wejsciowe.Length;
                        PokazMenu();
                    }
                    else if (klawisz.Key == ConsoleKey.Escape)
                    {
                        aktywnaPozycjaMenu = pozycjeMenu_wejsciowe.Length - 1;
                        break;
                    }
                    else if (klawisz.Key == ConsoleKey.Enter)
                        break;

                } while (true);
            }
            static void UruchomOpcje_menu_wejsciowe()
            {
                switch (aktywnaPozycjaMenu)
                {
                    case 0: 
                        Console.Clear(); 
                        Logowanie();
                        break;
                    case 1: 
                        Console.Clear(); 
                        rejestracja(); 
                        break;
                    case 2: 
                        Console.Clear(); 
                        przegladanie_lotow(); 
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                    default:
                        Environment.Exit(0);
                        break;
                }
            }
            static void Logowanie()
            {

                //trzeba zrobic sprawdzanie

                Console.Write("Witam, prosze podac login i haslo, aby uzyskać dostęp do rezerwowania lotów\n");
                Console.WriteLine("login:  ");
                login = Console.ReadLine();
                Console.WriteLine("haslo:  ");

                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        Console.Write("Zalogowano pomyślnie");
                        break;

                    }

                    // Jeśli użytkownik nacisnął klawisz Backspace, usuń ostatni znak z hasła
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        if (password.Length > 0)
                        {
                            password = password.Substring(0, password.Length - 1);
                            Console.Write("\b \b"); // Usuń ostatni znak z ekranu
                        }
                    }

                    // Jeśli użytkownik nacisnął inny klawisz, dodaj go do hasła i wyświetl gwiazdkę
                    if (key.KeyChar != '\u0000' && key.Key != ConsoleKey.Backspace)
                    {
                        password += key.KeyChar;
                        Console.Write("*");
                    }
                }
            }
            static void rejestracja()
            {

                Console.Write("Witam, prosze podac login i haslo, aby zarejestrowac sie w naszej bazie danych\n");
                Console.WriteLine("login:  ");
                login_rejestracja = Console.ReadLine();
                Console.WriteLine("haslo:  ");

                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    // Jeśli użytkownik nacisnął klawisz Enter, zakończ pętlę
                    if (key.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine("\n\nZalogowano pomyślnie - nacisnij jakikolwiek przycisk aby wyjsc ");
                        //dodawanie do bazy danych
                        
                                Console.ReadKey(true);
                                break;
                            


                    }

                    // Jeśli użytkownik nacisnął klawisz Backspace, usuń ostatni znak z hasła
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        if (password_rejestracja.Length > 0)
                        {
                            password_rejestracja = password_rejestracja.Substring(0, password_rejestracja.Length - 1);
                            Console.Write("\b \b"); // Usuń ostatni znak z ekranu
                        }
                    }

                    // Jeśli użytkownik nacisnął inny klawisz, dodaj go do hasła i wyświetl gwiazdkę
                    if (key.KeyChar != '\u0000' && key.Key != ConsoleKey.Backspace)
                    {
                        password_rejestracja += key.KeyChar;
                        Console.Write("*");
                    }
                }
            }
            static void przegladanie_lotow()
            {
                Console.Write("Witam, prosze podac login i haslo");
                Console.ReadKey();
            }
        }
    }
    internal class Program
    {
        public static void Main()
        {
            Menu.StartMenu();




            //string connectionString = "server=localhost;user id=root;password=;database=szl_odlot";
            //MySqlConnection conn = new MySqlConnection(connectionString);

            //try
            //{
            //    conn.Open();
            //    MySqlCommand Dodawanie_uzytkownika = new MySqlCommand("INSERT INTO user (login, password, level) VALUES ('a', 'b', 2" , conn);
            //    //MySqlCommand Dodawanie_uzytkownika = new MySqlCommand("INSERT INTO user (login, password, level) VALUES (" + Menu.login_rejestracja + ", " + Menu.password_rejestracja + ", " + 2, conn);
            //    MySqlDataReader reader = Dodawanie_uzytkownika.ExecuteReader();
            //    reader.Close();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //finally
            //{
            //    conn.Close();
            //}

        }
    }
}
