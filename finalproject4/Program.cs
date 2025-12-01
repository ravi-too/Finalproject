UserService userService = new UserService();

while (true)
{
    try
    {
        Console.WriteLine("\n==== ATM sistema ====");
        Console.WriteLine("1. Shesvla");
        Console.WriteLine("2. registracia");
        Console.WriteLine("3. gasvla");
        Console.Write("airchie: ");

        string choice = Console.ReadLine();

        if (choice == "1")
        {
            Console.Write("piradi nomeri: ");
            string pid = Console.ReadLine();

            Console.Write("password (4 cifri): ");
            string pass = Console.ReadLine();

            User user = userService.Login(pid, pass);

            if (user == null)
            {
                Console.WriteLine("user ver moidzebna!");
                continue;
            }

            ATM atm = new ATM(user);
            RunATM(atm, userService);
        }
        else if (choice == "2")
        {
            Console.Write("saxeli: ");
            string name = Console.ReadLine();

            Console.Write("gvari: ");
            string surname = Console.ReadLine();

            Console.Write("piradi nomeri: ");
            string pid = Console.ReadLine();

            User newUser = userService.Register(name, surname, pid);

            Console.WriteLine($"registracia warmatebit dasrulda! Tqveni parolia: {newUser.Password}");
        }
        else if (choice == "3")
        {
            break;
        }
        else
        {
            Console.WriteLine("araswori archevani!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("shecdoma: " + ex.Message);
    }
}

static void RunATM(ATM atm, UserService userService)
{
    while (true)
    {
        Console.WriteLine("\n---- ATM menu ----");
        Console.WriteLine("1. balansis naxva ");
        Console.WriteLine("2. balansis shevseba");
        Console.WriteLine("3. tanxins gamotana");
        Console.WriteLine("4. exit");
        Console.Write("airchie: ");

        string option = Console.ReadLine();

        try
        {
            if (option == "1")
            {
                atm.CheckBalance();
            }
            else if (option == "2")
            {
                Console.Write("Tanxa: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                atm.Deposit(amount);
                userService.SaveUsers();
            }
            else if (option == "3")
            {
                Console.Write("Tanxa: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                atm.Withdraw(amount);
                userService.SaveUsers();
            }
            else if (option == "4")
            {
                break;
            }
            else
            {
                Console.WriteLine("araswori archevani!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("shecdoma: " + ex.Message);
        }
    }
}
