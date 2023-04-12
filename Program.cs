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
using Mysqlx.Crud;
using Org.BouncyCastle.Utilities.Collections;

namespace ConsoleApp7
{
    internal static class Menu
    {

        static public string login_rejestracja = "";
        static public string password_rejestracja = "";
        //static public string login = "";
        //static public string password = "";
        static public string login_uzytkownika_zalogowanego;
        static public bool czy_dobry_login;
        static public bool czy_dobry_password;
        static public bool dobry_login;
        static public bool dobry_password;
        static public int czy_zalogowano_pomyslnie;
        static public bool czy_uzytkownik_zalogowany;
        static public int wiersz;
        static public int wiersz2;
        static public string[,] tablica_lotow = new string[999, 8];
        static public string[,] tablica_twoje_loty = new string[999, 8];
        static public int id_user;











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
            string password = "";


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
                login_uzytkownika_zalogowanego = login;
            }
            else
            {
                czy_uzytkownik_zalogowany = false;
                Console.WriteLine("\n\nBledne haslo lub login\nSproboj ponownie - enter\npowrot - escape");
                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        Logowanie(conn);
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }

            }
        }








        static public void przegladanie_lotow(MySqlConnection conn)
        {
            string selectQuery = "SELECT lotniska_odlotowe.nazwa, lotniska_przylotowe.nazwa, samolot.nazwa, samolot.model, samolot.ilosc_max_miejsc, trasa.ilosc_miejsc, trasa.cena, trasa.id FROM lotniska_odlotowe JOIN lotniska_przylotowe JOIN samolot JOIN trasa ON lotniska_odlotowe.id = trasa.id_lotniska_odlot AND lotniska_przylotowe.id = trasa.id_lotniska_przylot AND samolot.id = trasa.id_samolotu;";
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
                tablica_lotow[i, 2] = reader.GetString(6);
                tablica_lotow[i, 3] = reader.GetString(3);
                tablica_lotow[i, 4] = reader.GetString(2);
                tablica_lotow[i, 5] = reader.GetString(5);
                tablica_lotow[i, 6] = reader.GetString(4);
                tablica_lotow[i, 7] = reader.GetString(7);

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
        static public void rezerwacja_lotow(MySqlConnection conn, string login, int ID_trasa)
        {
            string selectQuery = "SELECT id FROM user WHERE login = @login;";
            MySqlCommand command = new MySqlCommand(selectQuery, conn);
            command.Parameters.AddWithValue("@login", login);
            conn.Open();
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string ID_user = reader.GetString(0);
            reader.Close();
            Console.Write("Witam w okienku rezerwacji lotow \n\nProszę podać ilość biletów jaka państwa interesuje: ");
            int ilosc_biletow = int.Parse(Console.ReadLine());
            
            string selectQuery2 = "INSERT INTO user_trasa (ID_trasa, ID_user, ilosc_biletow) VALUES (@ID_trasa, @ID_user, @ilosc_biletow);";
            MySqlCommand command2 = new MySqlCommand(selectQuery2, conn);
            command2.Parameters.AddWithValue("@ID_trasa", ID_trasa);
            command2.Parameters.AddWithValue("@ID_user", ID_user);
            command2.Parameters.AddWithValue("@ilosc_biletow", ilosc_biletow);
            string selectQuery3 = "UPDATE trasa SET ilosc_miejsc = ilosc_miejsc - @ilosc_biletow WHERE trasa.id = @ID_trasa;";
            MySqlCommand command3 = new MySqlCommand(selectQuery3, conn);
            command3.Parameters.AddWithValue("@ilosc_biletow", ilosc_biletow);
            command3.Parameters.AddWithValue("@ID_trasa", ID_trasa);
            command3.ExecuteNonQuery();
            int rowsAffected = command2.ExecuteNonQuery();
            conn.Close();
            Console.WriteLine("{0} wiersz dodany do tabeli.", rowsAffected);
        }

        static public void pokaz_lot_uzytkownika(MySqlConnection conn, string login)
        {
            
            string selectQuery = "SELECT id FROM user WHERE login = @login;";
            MySqlCommand command = new MySqlCommand(selectQuery, conn);
            command.Parameters.AddWithValue("@login", login);
        
            conn.Open();
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string id_user = reader.GetString(0);
            reader.Close();
            string selectQuery2 = "SELECT DISTINCT ilosc_biletow, lotniska_odlotowe.nazwa, lotniska_przylotowe.nazwa, samolot.nazwa, samolot.model, samolot.ilosc_max_miejsc, trasa.ilosc_miejsc, trasa.cena FROM lotniska_odlotowe JOIN user JOIN user_trasa JOIN lotniska_przylotowe JOIN samolot JOIN trasa ON lotniska_odlotowe.id = trasa.id_lotniska_odlot AND lotniska_przylotowe.id = trasa.id_lotniska_przylot AND samolot.id = trasa.id_samolotu AND ID_trasa = trasa.id WHERE ID_user = @id_user;";
            MySqlCommand command2 = new MySqlCommand( selectQuery2, conn);
            Console.ForegroundColor = ConsoleColor.Black;
            
            command2.Parameters.AddWithValue("@id_user", id_user);
            MySqlDataReader reader2 = command2.ExecuteReader();
            reader2.Read();
            wiersz2 = 0;
            while (reader2.Read())
            {
                

                tablica_twoje_loty[wiersz2, 0] = reader2.GetString(0);
                tablica_twoje_loty[wiersz2, 1] = reader2.GetString(3);
                tablica_twoje_loty[wiersz2, 2] = reader2.GetString(4);
                tablica_twoje_loty[wiersz2, 3] = reader2.GetString(1);
                tablica_twoje_loty[wiersz2, 4] = reader2.GetString(2);
                tablica_twoje_loty[wiersz2, 5] = reader2.GetString(5);
                tablica_twoje_loty[wiersz2, 6] = reader2.GetString(6);
                tablica_twoje_loty[wiersz2, 7] = reader2.GetString(7);

                wiersz2++;

            }
            
            //Console.WriteLine(tablica_twoje_loty[i, 0] + " " + tablica_twoje_loty[i, 2] + " " + tablica_twoje_loty[i, 3] + " " + tablica_twoje_loty[i, 4] + " " + tablica_twoje_loty[i, 5] + " " + tablica_twoje_loty[i, 6] + " " + tablica_twoje_loty[i, 7] + " ");
            reader2.Close();
            conn.Close();
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
                                Menu.przegladanie_lotow(conn);
                                Console.Clear();
                                Console.WriteLine("udalo ci sie " + Menu.login_uzytkownika_zalogowanego);
                                bool przegladanie_rezerwowanie_zalogowany = true;
                                bool przegladanie_wolnych_lotow = true;
                                bool elozelo = true;
                                int opcja_zalogowany = 1;
                                mainMenuChoice = 4;

                                while (przegladanie_rezerwowanie_zalogowany != false)
                                {

                                    Console.Clear();
                                    Console.WriteLine("zalogowany pomyslnie jako   " + Menu.login_uzytkownika_zalogowanego);
                                    Console.BackgroundColor = ConsoleColor.White;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Menu.przegladanie_lotow(conn);
                                    
                                    
                                    Console.WriteLine("Dostepne loty - wybierz interesujacy cie lot za pomoca klawisza enter\n\n");
                                    Console.Write("");
                                    Console.ForegroundColor = mainMenuChoice == 4 ? ConsoleColor.Green : ConsoleColor.Black;
                                    Console.WriteLine("twoje loty");
                                    Console.ForegroundColor = mainMenuChoice == 5 ? ConsoleColor.Green : ConsoleColor.Black;
                                    Console.WriteLine("dostepne loty");
                                    Console.ForegroundColor = mainMenuChoice == 6 ? ConsoleColor.Green : ConsoleColor.Black;
                                    Console.WriteLine("wyloguj sie");
                                    mainMenuKey = Console.ReadKey();
                                    if (mainMenuKey.Key == ConsoleKey.UpArrow)
                                    {
                                        mainMenuChoice = Math.Max(4, mainMenuChoice - 1);
                                    }
                                    else if (mainMenuKey.Key == ConsoleKey.DownArrow)
                                    {
                                        mainMenuChoice = Math.Min(6, mainMenuChoice + 1);
                                    }
                                    else if (mainMenuKey.Key == ConsoleKey.Escape)
                                    {
                                        przegladanie_rezerwowanie_zalogowany = false;
                                    }
                                    else if (mainMenuKey.Key == ConsoleKey.Enter)
                                    {
                                        
                                        switch (mainMenuChoice)
                                        {
                                            case 4:
                                                {
                                                    Menu.pokaz_lot_uzytkownika(conn, Menu.login_uzytkownika_zalogowanego);
                                                    
                                                    while (elozelo != false)
                                                    {
                                                        //Console.Clear();
                                                        Console.BackgroundColor = ConsoleColor.White;
                                                        Console.ForegroundColor = ConsoleColor.Black;
                                                        for (int j = 0; j < Menu.wiersz2; j++)
                                                        {
                                                            for (int k = 0; k < 8; k++)
                                                            {
                                                                if (k == 0)
                                                                {
                                                                    Console.Write("Kupiles " + Menu.tablica_twoje_loty[j, k] + " biletow");

                                                                }
                                                                else if (k == 1)
                                                                {
                                                                    Console.Write("\nNa lot " + Menu.tablica_twoje_loty[j, k] + " samolotem ");
                                                                }
                                                                else if (k == 2)
                                                                {
                                                                    Console.Write(Menu.tablica_twoje_loty[j, k] );
                                                                }
                                                                else if (k == 3)
                                                                {
                                                                    Console.Write("\nZ " + Menu.tablica_twoje_loty[j, k] + " do ");
                                                                }
                                                                else if (k == 4)
                                                                {
                                                                    Console.Write(Menu.tablica_twoje_loty[j, k]);
                                                                }
                                                                else if (k == 5)
                                                                {
                                                                    Console.Write("\nZ " + Menu.tablica_twoje_loty[j, k] +" biletow zostalo jeszcze ");
                                                                }
                                                                else if (k == 6)
                                                                {
                                                                    Console.Write(Menu.tablica_twoje_loty[j, k]);
                                                                }else if (k == 7)
                                                                {
                                                                    Console.Write("Kupiles bilety za " + Menu.tablica_twoje_loty[j, k]);
                                                                }
                                                            }


                                                        }
                                                        ConsoleKeyInfo twoje_loty = Console.ReadKey(true);

                                                        switch (twoje_loty.Key)
                                                        {
                                                            case ConsoleKey.Escape:
                                                                elozelo = false;
                                                                break;
                                                            case ConsoleKey.Enter:
                                                                Console.Clear();
                                                                Console.BackgroundColor = ConsoleColor.White;
                                                                Console.ForegroundColor = ConsoleColor.Black;
                                                                Console.WriteLine("chuj dziala");
                                                                Console.ReadKey(true);
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                }
                                            case 5:
                                                {
                                                    while (przegladanie_wolnych_lotow != false)
                                                    {
                                                        Console.Clear();
                                                        for (int j = 0; j < Menu.wiersz; j++)
                                                        {
                                                            if (j == opcja_zalogowany)
                                                            {
                                                                Console.BackgroundColor = ConsoleColor.White;
                                                                Console.ForegroundColor = ConsoleColor.Green;
                                                            }
                                                            else
                                                            {
                                                                Console.BackgroundColor = ConsoleColor.White;
                                                                Console.ForegroundColor = ConsoleColor.Black;
                                                            }

                                                            for (int k = 0; k < 7; k++)
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
                                                                    Console.Write("\nSpiesz sie bo zostalo jeszcze " + Menu.tablica_lotow[j, k] + " miejsc ");
                                                                }
                                                                else if (k == 6)
                                                                {
                                                                    Console.Write("z " + Menu.tablica_lotow[j, k] + " dostępnych\n\n");
                                                                }
                                                            }


                                                        }



                                                        ConsoleKeyInfo pryegladanie_rezerwowanie_klucz = Console.ReadKey(true);

                                                        switch (pryegladanie_rezerwowanie_klucz.Key)
                                                        {
                                                            case ConsoleKey.Escape:
                                                                przegladanie_wolnych_lotow = false;
                                                                break;
                                                            case ConsoleKey.Enter:
                                                                Console.Clear();
                                                                Console.BackgroundColor = ConsoleColor.White;
                                                                Console.ForegroundColor = ConsoleColor.Black;
                                                               
                                                                Menu.rezerwacja_lotow(conn, Menu.login_uzytkownika_zalogowanego, opcja_zalogowany+1);
                                                                Console.ReadKey(true);
                                                                break;
                                                            case ConsoleKey.UpArrow:
                                                                opcja_zalogowany = (opcja_zalogowany == 0) ? Menu.wiersz - 1 : opcja_zalogowany - 1;
                                                                break;
                                                            case ConsoleKey.DownArrow:
                                                                opcja_zalogowany = (opcja_zalogowany == Menu.wiersz - 1) ? 0 : opcja_zalogowany + 1;
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                }
                                            case 6: 
                                                {
                                                    przegladanie_rezerwowanie_zalogowany = false;
                                                    Console.WriteLine("elo żelo");
                                                    break;
                                                }
                                        }
                                        
                                    }
                                }

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
                                Console.WriteLine("Dostepne loty\n\n");


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

                                    for (int k = 0; k < 7; k++)
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
                                            Console.Write("\nSpiesz sie bo zostalo jeszcze " + Menu.tablica_lotow[j, k] + " miejsc ");
                                        }
                                        else if (k == 6)
                                        {
                                            Console.Write("z " + Menu.tablica_lotow[j, k] + " dostępnych\n\n");
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