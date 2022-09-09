using System;

namespace bankkonto
{

    class Program
    {
        static void Main(string[] args)
        {
            Bankaccounts Gmünd = new Bankaccounts(10000);

            // HERE CAN YOU MAKE A NEW ACCOUNT

            Gmünd[79] = new Bankaccount("Mulham", "mulsth");
            Gmünd[74] = new Bankaccount("cris", "1234", 100);
            Gmünd[354] = new Bankaccount("Morhaf", "mor", 50);
            Gmünd[6969] = new Bankaccount("Marcin", "855632", 10);
            Gmünd[187] = new Bankaccount("Clemens", "123456", 300);
            
            /////////////////////////////////////////////////////

            using (StreamWriter sw = new StreamWriter("accountlist.txt"))
            {
                for(int i = 0; i < Bankaccounts.Anz; i++)
                {
                    if(Gmünd[i] != null)
                        sw.WriteLine(Gmünd[i]);
                }
            }

            Bankaccount.Login(Gmünd);
        }
    }

    public class Bankaccounts
    {
        public static int Anz { get; set; }
        Bankaccount[] accounts;
        public Bankaccounts(int anz)
        {
            accounts = new Bankaccount[anz];
            Anz = anz;
        }
        public Bankaccount this[int index]
        {
            get { return accounts[index]; }
            set
            {
                if (index >= 0 && index < accounts.Length)
                    accounts[index] = value;
                else
                    throw new Exception("...");
            }
        }
        public int Count()
        {
            return accounts.Length;
        }
    }

    public class Bankaccount
    {
        private double Money { get; set; }
        private double credit;
        private string accname;
        private string pass;
        private void Accountbalance()
        {
            Console.WriteLine();
            Console.WriteLine("----------------------");
            Console.WriteLine($"your current Account balance is: {Money}\nyour credits are: {credit}");
            Console.WriteLine("----------------------");
            Console.WriteLine();
        }
        private double Credit(double drawal)
        {
            if (drawal <= Money * 2.5 || Money == 0 && drawal <= 200)
            {
                double hlp = drawal - Money;
                credit += drawal - Money;
                Money = 0;
                Console.WriteLine($"you get {hlp} Euro credit Successfully!");
            }
            else if (drawal > Money * 2.5)
            {
                Console.WriteLine($"you cant get that much Credit :( \n your max possible credit is: {Money * 2.5}");
            }
            return credit;
        }
        private double Drawal(double drawal)
        {

            if (drawal > Money)
            {
                Console.WriteLine($"you dont have enough Money in your Account to drawal {drawal} Euro\nyour Account balance is: {Money} Euro");
                Console.WriteLine("Do you want to get credit? yes or no");
                string answer = Console.ReadLine();
                if (answer == "yes")
                {
                    Credit(drawal);
                }

                else
                {
                    Console.WriteLine("Have a nice day :)");
                    Console.Clear();
                }
            }
            else
            {
                Money -= drawal;
                Console.WriteLine($"you drawal {drawal} successfully");
            }
            return Money;

        }
        private double deposited(double deposited)
        {
            Console.WriteLine($"You have deposited {deposited} successfully");
            if (credit > 0)
            {
                if (deposited > credit)
                {
                    double hlp;
                    hlp = deposited - credit;
                    Money += hlp;
                    credit = 0;
                    return Money;
                }
                else
                {
                    credit -= deposited;
                    return credit;
                }

            }
            else if (credit == 0)
            {
                Money += deposited;
                return Money;

            }
            return Money;
        }
        private void transaction(Bankaccounts acc)
        {
            Console.WriteLine("Please write the number of the account, that you want transfer the money to");
            int num = Convert.ToInt32(Console.ReadLine());
            Bankaccount recive = acc[num];
            Console.WriteLine("how much do you want to trans?");
            double trans = Convert.ToDouble(Console.ReadLine());
            if (this.Money > 0 && this.Money >= trans)
            {
                this.Money -= trans;
                if (recive.credit == 0)
                    recive.Money += trans;
                else if (recive.credit > 0)
                {
                    if (recive.credit > trans)
                        recive.credit -= trans;
                    else
                    {
                        trans -= recive.credit;
                        recive.credit = 0;
                        recive.Money += trans;
                    }
                }
                Console.WriteLine($"you have transed {trans} successfully");
            }
            else if (this.Money < trans)
            {
                Console.WriteLine($"you dont have enough Money in your Account to trans {trans} Euro\nyour Account balance is: {Money} Euro");
                Console.WriteLine("Do you want to get credit? yes or no");
                string answer = Console.ReadLine();
                if (answer == "yes")
                {
                    Credit(trans);
                    trans -= recive.credit;
                    recive.Money = trans;
                    recive.credit = 0;
                    Console.WriteLine($"you have transed {trans} successfully");
                }
                else
                    Console.WriteLine($"you dont have enough Money for this transaction!! Please deposited {trans} in your Account and try again");
            }
        }
        public void Login1(Bankaccounts acc, bool dobreak = true)
        {
            int count = 0;
            while (dobreak)
            {
                Console.WriteLine("Please enter your Account name: ");
                string accname = Console.ReadLine();
                Console.WriteLine("please enter your passwort: ");
                string pass = Console.ReadLine();
                if (this.accname == accname && this.pass == pass)
                {
                    this.StartMenu(acc);
                    break;
                }
                else
                {
                    count++;
                    if (count == 3)
                    {
                        Console.WriteLine($"\nyou tried {count} times\t please try again later");
                        dobreak = false;
                    }
                    else
                        Console.WriteLine("please try again");

                }

            }
        }
        public static void Login(Bankaccounts acc)
        {
            int count = 0;
            bool dobreak = true;
            while (true)
            {
                Console.WriteLine("Please enter your Account number:");
                int accnum = Convert.ToInt32(Console.ReadLine());

                if (accnum <= acc.Count() && accnum >= 0)
                {
                    acc[accnum].Login1(acc);

                }
                else if (accnum > acc.Count())
                    throw new Exception("Account not Found");
                else
                {
                    count++;
                    if (count == 3)
                    {
                        Console.WriteLine($"\nyou tried {count} times\t please try again later");
                        dobreak = false;
                    }
                    else
                        Console.WriteLine("please try again");

                }
            }


        }
        private void StartMenu(Bankaccounts acc)
        {

            bool dobreak = true;
            while (dobreak)
            {
                Console.Clear();
                Console.WriteLine("===================================================");
                Console.WriteLine("Welcome to the Start Menu\n how can we help you?\nplease choose one of the following options or their number");
                Console.WriteLine(" Accountbalance(1)\tDrawal(2)\tdeposited(3)\nTransaction(4)\tExit(5)");
                Console.WriteLine("===================================================");
                string answer = Console.ReadLine();
                answer = answer.ToLower();
                if (answer == "accountbalance" || answer == "1")
                {
                    Accountbalance();
                }
                else if (answer == "drawal" || answer == "2")
                {
                    Console.WriteLine("how much do you want to drawal?");
                    double ans = Convert.ToDouble(Console.ReadLine());
                    Drawal(ans);
                }
                else if (answer == "deposited" || answer == "3")
                {
                    Console.WriteLine("how much do you want to deposited?");
                    double ans = Convert.ToDouble(Console.ReadLine());
                    deposited(ans);
                }
                else if (answer == "transaction" || answer == "4")
                {
                    transaction(acc);

                }
                else if (answer == "exit" || answer == "5")
                {
                    Console.WriteLine("Have a nice Day :)");
                    dobreak = false;
                    Console.WriteLine("-------------------");
                    System.Threading.Thread.Sleep(1000);
                }
                else
                    Console.WriteLine("The value, that you give can not be accepted! please try again");
                System.Threading.Thread.Sleep(2000);
            }
        }
        public Bankaccount(string accname, string pass, double Money = 0)
        {
            this.accname = accname;
            this.pass = pass;
            this.Money = Money;
        }
        public override string ToString()
        {
            return accname + "\t" + pass + "\t" + Money;
        }
    }
}
