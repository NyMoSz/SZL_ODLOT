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
        static public string login = "";
        static public string password = "";
        static public bool czy_dobry_login;
        static public bool czy_dobry_password;
        static public bool dobry_login;
        static public bool dobry_password;
        static public int czy_zalogowano_pomyslnie;
        static public bool czy_uzytkownik_zalogowany;
        static public string[][] trasa_tablica;











        static bool CzyPoprawneLogowanie(string login, string password, MySqlConnection conn)
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
            login = Console.ReadLine();


            Console.WriteLine("haslo:  ");


            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    if (Menu.czy_zalogowano_pomyslnie == 1)
                    {
                        Console.WriteLine("\n\nzalogowano pomyslnie");
                        czy_uzytkownik_zalogowany = true;
                        Console.ReadKey(true);
                    }
                    else if (czy_zalogowano_pomyslnie == 2)
                    {
                        Console.WriteLine("\n\nnieprawidlowe haslo");
                        Console.ReadKey(true);
                    }
                    else if (czy_zalogowano_pomyslnie == 3)
                    {
                        Console.WriteLine("\n\nnieprawidlowy login");
                        Console.ReadKey(true);
                    }
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
                Console.WriteLine("\n\nBledne haslo lub login");
                Console.ReadKey(true);
            }
        }






        //static public void przegladanie_lotow(MySqlConnection conn)
        //{

        //    Console.Write("Witam, prosze podac login i haslo");
        //    Console.ReadKey();
        //    List<string[]> rows = PobierzDaneZBazy(conn);

        //    // ustalanie liczby wierszy i kolumn w tablicy
        //    int numRows = rows.Count;
        //    int numCols = rows[0].Length;

        //    // tworzenie tablicy o odpowiednim rozmiarze
        //    string[,] table = new string[numRows, numCols];

        //    // przepisywanie danych z listy do tablicy
        //    for (int i = 0; i < numRows; i++)
        //    {
        //        for (int j = 0; j < numCols; j++)
        //        {
        //            table[i, j] = rows[i][j];
        //        }
        //    }

        //    // wyświetlanie danych z tablicy
        //    for (int i = 0; i < numRows; i++)
        //    {
        //        for (int j = 0; j < numCols; j++)
        //        {
        //            Console.Write("{0}\t", table[i, j]);
        //            Console.ReadKey(true);
        //        }
        //        Console.WriteLine();
        //    }
        //}












        //static public List<string[]> PobierzDaneZBazy(MySqlConnection conn)
        //{
        //    List<string[]> rows = new List<string[]>();

        //    string selectQuery = "SELECT lotnisko.nazwa, samolot.nazwa, samolot.model, samolot.ilosc_max_miejsc FROM trasa, lotnisko, model, samolot WHERE id_lotniska_odlot = lotnisko.id AND id_lotniska_przylot = lotnisko.id AND id_samolotu = samolot.id AND samolot.model = model.id;";
        //    MySqlCommand command = new MySqlCommand(selectQuery, conn);

        //    conn.Open();
        //    MySqlDataReader reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        string[] row = new string[reader.FieldCount];
        //        for (int i = 0; i < reader.FieldCount; i++)
        //        {
        //            row[i] = reader[i].ToString();
        //        }
        //        rows.Add(row);
        //    }

        //    reader.Close();
        //    conn.Close();

        //    return rows;
        //}

        static public void przegladanie_lotow(MySqlConnection conn)
        {
            Console.WriteLine("dzien dobry");
            string selectQuery = "SELECT lotniska_odlotowe.nazwa, lotniska_przylotowe.nazwa, samolot.nazwa, samolot.model, samolot.ilosc_max_miejsc, trasa.cena FROM lotniska_odlotowe, lotniska_przylotowe, samolot, trasa WHERE lotniska_odlotowe.id = trasa.id_lotniska_odlot AND lotniska_przylotowe.id = trasa.id_lotniska_przylot AND samolot.id = trasa.id_samolotu;";
            MySqlCommand command = new MySqlCommand(selectQuery, conn);

            int wiersz = 0;
            int kolumna = 0;

            conn.Open();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string nazwa_lotniska_wylot = reader.GetString(0);
                string nazwa_lotniska_przylot = reader.GetString(1);
                string samolot_nazwa = reader.GetString(2);
                string samolot_model = reader.GetString(3);
                int ilosc_miejsc = reader.GetInt32(4);
                int cena = reader.GetInt32(5);

                Console.WriteLine("{0} --> {1} \njuz od {5} PLN\nlot najlepszymi samolotami takimi jak  {3}   {2}\nspiesz sie bo zostalo {4} wolnych miejsc\n\n\n", nazwa_lotniska_wylot, nazwa_lotniska_przylot, samolot_nazwa, samolot_model, ilosc_miejsc, cena);

            }
            Console.ReadKey(true);
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
                Console.WriteLine("Witamy na stronie odlotu, oto początek twojej niezapomnianej przygody!");
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
                            if(Menu.czy_uzytkownik_zalogowany == true)
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
                            Console.Clear();
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black; 
                            Menu.przegladanie_lotow(conn);
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























