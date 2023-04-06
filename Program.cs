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

namespace ConsoleApp7
{
    internal static class Menu
    {

        static public string login_rejestracja ="";
        static public string password_rejestracja ="";
        static public string login = "";
        static public string password = "";
        static public bool czy_dobry_login;
        static public bool czy_dobry_password;
        static public bool dobry_login;
        static public bool dobry_password;
        static public int czy_zalogowano_pomyslnie;




        //static List<string> PobierzPassword(MySqlConnection connection)
        //{
        //    List<string> password_lista = new List<string>();

        //    string selectQuery = "SELECT password FROM user";
        //    MySqlCommand command = new MySqlCommand(selectQuery, connection);

        //    connection.Open();
        //    MySqlDataReader reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        string login = reader.GetString(1);


        //        string user = string.Format("password: {0}", password);
        //        password_lista.Add(user);
        //    }

        //    reader.Close();
        //    connection.Close();

        //    return password_lista;
        //}
        //static List<string> Pobierzlogin(MySqlConnection connection)
        //{
        //    List<string> login_lista = new List<string>();

        //    string selectQuery = "SELECT login FROM user";
        //    MySqlCommand command = new MySqlCommand(selectQuery, connection);

        //    connection.Open();
        //    MySqlDataReader reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        string login = reader.GetString(1);


        //        string user = string.Format("login: {0}", login);
        //        login_lista.Add(user);
        //    }

        //    reader.Close();
        //    connection.Close();

        //    return login_lista;
        //}
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
            //string selectQuery = "SELECT login, password FROM user";
            //MySqlCommand command = new MySqlCommand(selectQuery, conn);

            //List<string> loginy_lista = new List<string>();
            //List<string> password_lista = new List<string>();


            //conn.Open();
            //MySqlDataReader reader = command.ExecuteReader();
            //while (reader.Read())
            //{
            //    string login_logowanie = reader.GetString(0);
            //    loginy_lista.Add(login);
            //    string password_logowanie = reader.GetString(1);
            //    password_lista.Add(password);

            //}
            //conn.Close();

            ////trzeba zrobic sprawdzanie

            //Console.Write("Witam, prosze podac login i haslo, aby uzyskać dostęp do rezerwowania lotów\n");
            //Console.WriteLine("login:  ");
            //login = Console.ReadLine();
            //for(int i = 0; i < loginy_lista.Count; i++)
            //{
            //    if(login == loginy_lista[i])
            //    {
            //        Menu.czy_dobry_login = true;
            //        dobry_login = loginy_lista[i];
            //        czy_zalogowano_pomyslnie = 3;
            //    }
            //    else
            //    {
            //        czy_zalogowano_pomyslnie = 2;
            //    }

            //}
            //if(Menu.czy_dobry_login == true)
            //{
            //    for(int i = 0; i < password_lista.Count; i++)
            //    {
            //        if(password == password_lista[i])
            //        {
            //            Menu.czy_zalogowano_pomyslnie = 1;
            //        }
            //    }
            //}








            //List<string> login_lista = Pobierzlogin(conn);
            //List<string> password_lista = PobierzPassword(conn);

            //for(int i =0; i < login_lista.Count; i++)
            //{
            //    if(login == login_lista[i])
            //    {
            //        dobry_login = true;
            //        czy_zalogowano_pomyslnie = 1;
            //        for (int j = 0; j < password_lista.Count; j++)
            //        {
                        
            //        }
            //    }
            //    else
            //    {
            //        czy_zalogowano_pomyslnie = 0;
            //    }
            //}
            

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
                        Console.ReadKey(true);
                    }else if(czy_zalogowano_pomyslnie == 2)
                    {
                        Console.WriteLine("\n\nnieprawidlowe haslo");
                        Console.ReadKey(true);
                    }
                    else if(czy_zalogowano_pomyslnie == 3)
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
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("\n\nBledne haslo lub login");
                Console.ReadKey(true);
            }
        }
        static public void przegladanie_lotow()
        {

            Console.Write("Witam, prosze podac login i haslo");
            Console.ReadKey();
        }
        //}
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
                                Menu.przegladanie_lotow();
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





















    //string[] pozycjeMenu_wejsciowe = { "Logowanie", "Zaloz konto", "Przeglądaj loty", "\nWyjdz z aplikacji" };
    //int aktywnaPozycjaMenu = 0;
    //Console.Title = "Odlot - loty i przeloty";
    //Console.CursorVisible = false;
    //Console.BackgroundColor = ConsoleColor.White;
    //Console.Clear();
    //Console.ForegroundColor = ConsoleColor.Black;
    //Console.WriteLine("Witamy na stronie odlotu, oto początek twojej niezapomnianej przygody!");
    //Console.WriteLine();
    //for (int i = 0; i < pozycjeMenu_wejsciowe.Length; i++)
    //{
    //    do
    //    {
    //        ConsoleKeyInfo klawisz = Console.ReadKey();
    //        if (klawisz.Key == ConsoleKey.UpArrow)
    //        {
    //            aktywnaPozycjaMenu = (aktywnaPozycjaMenu > 0) ? aktywnaPozycjaMenu - 1 : pozycjeMenu_wejsciowe.Length - 1;
    //            //PokazMenu();
    //        }
    //        else if (klawisz.Key == ConsoleKey.DownArrow)
    //        {
    //            aktywnaPozycjaMenu = (aktywnaPozycjaMenu + 1) % pozycjeMenu_wejsciowe.Length;
    //            //PokazMenu();
    //        }
    //        else if (klawisz.Key == ConsoleKey.Escape)
    //        {
    //            aktywnaPozycjaMenu = pozycjeMenu_wejsciowe.Length - 1;
    //            break;
    //        }
    //        else if (klawisz.Key == ConsoleKey.Enter)
    //            break;

    //    } while (true);
    //    if (i == aktywnaPozycjaMenu)
    //    {
    //        Console.BackgroundColor = ConsoleColor.Black;
    //        Console.ForegroundColor = ConsoleColor.White;
    //        Console.WriteLine(pozycjeMenu_wejsciowe[i]);
    //        Console.BackgroundColor = ConsoleColor.White;
    //        Console.ForegroundColor = ConsoleColor.Black;
    //    }
    //    else
    //    {
    //        Console.WriteLine(pozycjeMenu_wejsciowe[i]);
    //    }














    //while (true)
    //{
    //    Menu.PokazMenu();
    //    Menu.WybieranieOpcji();
    //    Menu.UruchomOpcje_menu_wejsciowe();
    //}



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


