using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace ConsoleApp7
{
    internal static class Menu
    {

        static public string login_rejestracja = "";
        static public string password_rejestracja = "";
        //static public string login = "";
        //static public string password = "";
        static public bool czy_dobry_login;
        static public bool czy_dobry_password;
        static public bool dobry_login;
        static public bool dobry_password;
        static public int czy_zalogowano_pomyslnie;
        static public bool czy_uzytkownik_zalogowany;
        static public int wiersz;
        static public string[,] tablica_lotow = new string[3, 6];











        static public bool CzyPoprawneLogowanie(string login, string password, MySqlConnection conn)
        {
            string selectQuery = "SELECT Password FROM user WHERE login = @login";
            MySqlCommand command = new MySqlCommand(selectQuery, conn);
            command.Parameters.AddWithValue("@login", login);

            conn.Open();
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                string storedPassword = reader.GetString(0);

                if (password == storedPassword)
                {
                    conn.Close();
                    return true;
                }
            }

            conn.Close();
            return false;
        }







        static public void Logowanie(MySqlConnection conn)
        {


            Console.Write("Witam, prosze podac login i haslo, aby uzyskać dostęp do rezerwowania lotów\n");
            Console.WriteLine("login:  ");
            string login = Console.ReadLine();
            string password ="";


            Console.WriteLine("haslo:  ");


            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    
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
                if (key.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                    break;
                }
            }
            CzyPoprawneLogowanie(login, password, conn);
            if (CzyPoprawneLogowanie(login, password, conn) == true)
            {
                Console.WriteLine("\n\nZalogowano pomyslnie");
                czy_uzytkownik_zalogowany = true;
            }
            else
            {
                czy_uzytkownik_zalogowany = false;
                Console.WriteLine("\n\nBledne haslo lub login\nSproboj ponownie - enter\npowrot - escape");
                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if(key.Key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        Logowanie(conn);
                    }else if (key.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }

            }
        }








        static public void przegladanie_lotow(MySqlConnection conn)
        {
            string selectQuery = "SELECT lotniska_odlotowe.nazwa, lotniska_przylotowe.nazwa, trasa.cena, samolot.nazwa, samolot.model, samolot.ilosc_max_miejsc FROM lotniska_odlotowe, lotniska_przylotowe, samolot, trasa WHERE lotniska_odlotowe.id = trasa.id_lotniska_odlot AND lotniska_przylotowe.id = trasa.id_lotniska_przylot AND samolot.id = trasa.id_samolotu;";
            MySqlCommand command = new MySqlCommand(selectQuery, conn);
            conn.Open();
            MySqlDataReader reader = command.ExecuteReader();



            wiersz = 0;
            while (reader.Read())
            {
                wiersz++;
            }


            reader.Close();
            reader = command.ExecuteReader();

            int i = 0;
            while (reader.Read())
            {
                tablica_lotow[i, 0] = reader.GetString(0);
                tablica_lotow[i, 1] = reader.GetString(1);
                tablica_lotow[i, 2] = reader.GetString(2);
                tablica_lotow[i, 3] = reader.GetString(3);
                tablica_lotow[i, 4] = reader.GetString(4);
                tablica_lotow[i, 5] = reader.GetString(5);

                i++;

            }

            reader.Close();
            conn.Close();

        }







        static public void add_user(MySqlConnection conn, string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Nie można dodać pustego wiersza.");
                Console.ReadKey(true);
            }
            else
            {

                string insertQuery = "INSERT INTO user (login, password, level) VALUES (@login, @password, 1)";
                MySqlCommand command = new MySqlCommand(insertQuery, conn);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);

                conn.Open();
                int rowsAffected = command.ExecuteNonQuery();
                conn.Close();

                Console.WriteLine("{0} wiersz dodany do tabeli.", rowsAffected);
                Console.ReadKey(true);
            }

        }

    }









    internal class Program
    {
        public static void Main()
        {

            string connectionString = "server=localhost;user id=root;password=;database=szl_odlot";
            MySqlConnection conn = new MySqlConnection(connectionString);



            bool exit = false;
            int mainMenuChoice = 1;
            while (!exit)
            {

                Console.Title = "Odlot - loty i przeloty";
                Console.CursorVisible = false;
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();
                Console.WriteLine("Witamy na stronie odlotu, oto początek twojej niezapomnianej przygody!\n\nUzyj strzalek do poruszania sie, zatwierdz opcje enterem, zatrzymaj program escapem");
                Console.WriteLine();
                Console.ForegroundColor = mainMenuChoice == 1 ? ConsoleColor.Green : ConsoleColor.Black;
                Console.WriteLine("Zalouj sie");
                Console.ForegroundColor = mainMenuChoice == 2 ? ConsoleColor.Green : ConsoleColor.Black;
                Console.WriteLine("Zarejestruj sie");
                Console.ForegroundColor = mainMenuChoice == 3 ? ConsoleColor.Green : ConsoleColor.Black;
                Console.WriteLine("Przegladaj loty");
                ConsoleKeyInfo mainMenuKey = Console.ReadKey();
                if (mainMenuKey.Key == ConsoleKey.UpArrow)
                {
                    mainMenuChoice = Math.Max(1, mainMenuChoice - 1);
                }
                else if (mainMenuKey.Key == ConsoleKey.DownArrow)
                {
                    mainMenuChoice = Math.Min(3, mainMenuChoice + 1);
                }
                else if (mainMenuKey.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
                else if (mainMenuKey.Key == ConsoleKey.Enter)
                {
                    switch (mainMenuChoice)
                    {
                        case 1:
                            Console.Clear();
                            Menu.Logowanie(conn);
                            if (Menu.czy_uzytkownik_zalogowany == true)
                            {
                                Console.WriteLine("udalo ci sie");
                                Console.ReadKey(true);
                            }

                            break;







                        case 2:

                            Console.Clear();
                            Console.Write("Witam, prosze podac login i haslo, aby zarejestrowac sie w naszej bazie danych\n");
                            Console.WriteLine("login:  ");
                            Menu.login_rejestracja = Console.ReadLine();
                            Console.WriteLine("haslo:  ");

                            while (true)
                            {
                                ConsoleKeyInfo key = Console.ReadKey(true);

                                // Jeśli użytkownik nacisnął klawisz Enter, zakończ pętlę
                                if (key.Key == ConsoleKey.Enter)
                                {
                                    Console.WriteLine("\n\nZarejestrowano pomyślnie - nacisnij jakikolwiek przycisk aby wyjsc ");
                                    //dodawanie do bazy danych
                                    break;
                                }

                                // Jeśli użytkownik nacisnął klawisz Backspace, usuń ostatni znak z hasła
                                if (key.Key == ConsoleKey.Backspace)
                                {
                                    if (Menu.password_rejestracja.Length > 0)
                                    {
                                        Menu.password_rejestracja = Menu.password_rejestracja.Substring(0, Menu.password_rejestracja.Length - 1);
                                        Console.Write("\b \b"); // Usuń ostatni znak z ekranu
                                    }
                                }

                                // Jeśli użytkownik nacisnął inny klawisz, dodaj go do hasła i wyświetl gwiazdkę
                                if (key.KeyChar != '\u0000' && key.Key != ConsoleKey.Backspace)
                                {
                                    Menu.password_rejestracja += key.KeyChar;
                                    Console.Write("*");
                                }
                            }
                            string nowy_login = "";
                            string nowy_password = "";
                            nowy_login = Menu.login_rejestracja;
                            nowy_password = Menu.password_rejestracja;
                            Menu.add_user(conn, nowy_login, nowy_password);
                            break;






                        case 3:
                            Menu.przegladanie_lotow(conn);
                            bool przegladanie_lotow_nie_zalogowany = true;
                            int opcja = 1;
                            
                            while (przegladanie_lotow_nie_zalogowany)
                            {
                                Console.Clear();
                                
                                Console.BackgroundColor = ConsoleColor.White;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.WriteLine("Dostepne loty");
                                

                                    for (int j = 0; j < Menu.wiersz; j++)
                                    {
                                    if (j == opcja)
                                    {
                                        Console.BackgroundColor = ConsoleColor.White;
                                        Console.ForegroundColor = ConsoleColor.Green;
                                    }
                                    else
                                    {
                                        Console.BackgroundColor = ConsoleColor.White;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                    }

                                    for (int k = 0; k < 6; k++)
                                    {
                                        if (k == 0)
                                        {
                                            Console.Write(Menu.tablica_lotow[j, k] + " --> ");

                                        }
                                        else if (k == 1)
                                        {
                                            Console.Write(Menu.tablica_lotow[j, k]);
                                        }
                                        else if (k == 2)
                                        {
                                            Console.Write("\nCeny juz od " + Menu.tablica_lotow[j, k] + " PLN");
                                        }
                                        else if (k == 3)
                                        {
                                            Console.Write("\nLot najlepszymi samolotami takimi jak " + Menu.tablica_lotow[j, k]);
                                        }
                                        else if (k == 4)
                                        {
                                            Console.Write(" " + Menu.tablica_lotow[j, k]);
                                        }
                                        else if (k == 5)
                                        {
                                            Console.Write("\nSpiesz sie bo zostalo jeszcze " + Menu.tablica_lotow[j, k] + " miejsc\n\n");
                                        }
                                    }


                                    }


                                ConsoleKeyInfo pryegladanieklucz = Console.ReadKey(true);

                                switch (pryegladanieklucz.Key) 
                                {
                                    case ConsoleKey.Escape:
                                        przegladanie_lotow_nie_zalogowany = false;
                                        break;
                                    case ConsoleKey.Enter:
                                        Console.Clear();
                                        Console.BackgroundColor = ConsoleColor.White;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.WriteLine("Musisz sie najpierw zalogowac lub zarejestrowac, aby zarezerwowac lot");
                                        Console.ReadKey(true);
                                        break;
                                    case ConsoleKey.UpArrow:
                                        opcja = (opcja == 0) ? Menu.wiersz - 1 : opcja - 1;
                                        break;
                                    case ConsoleKey.DownArrow:
                                        opcja = (opcja == Menu.wiersz - 1) ? 0 : opcja + 1;
                                        break;
                                }



                            }
                            break;






                        case 4:
                            exit = true;
                            break;
                    }
                }
            }
        }
    }
}























