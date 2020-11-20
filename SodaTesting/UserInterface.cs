using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachineProject
{
    static class UserInterface
    {
        public static void WelcomeMessage()
        {
            Console.WriteLine("Thanks for choosing dCC Soda!"
                + "\nSodas offered: Cola (.35), Orange (.06), Root Beer (.60)\n");
        }

        public static string DisplayCoinOptions()
        {
            Console.WriteLine("Please input desired coins"
                + "\nPress 1 for Quarter"
                + "\nPress 2 for Dime"
                + "\nPress 3 for Nickel"
                + "\nPress 4 for Penny"
                + "\nPress 5 when done to choose a soda"
                + "\nPress 6 to cancel and refund"
                );
            string coinChoice = Console.ReadLine();
            return coinChoice;
        }

        public static string ChooseSoda()
        {
            Console.WriteLine("Sodas offered: Cola, Orange, Root Beer"
                + "\nPress 1 for Cola, .35"
                + "\nPress 2 for Orange, .06"
                + "\nPress 3 for Root Beer, .60"
                );
            string sodaChoice = "";
            while (sodaChoice == "")
            {
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        sodaChoice = "Cola";
                        break;
                    case "2":
                        sodaChoice = "Orange Soda";
                        break;
                    case "3":
                        sodaChoice = "Root Beer";
                        break;
                    default:
                        Console.WriteLine("Error, try again.");
                        break;
                }
            }
            return sodaChoice;
        }

        public static string AskBuyAnotherSoda()
        {
            Console.WriteLine("Do you want to purchase another soda?"
                + "\n Press 1 for yes"
                + "\n Press 2 for no");
            string buyAnotherChoice = Console.ReadLine();
            return buyAnotherChoice;
        }

        //Returns money value of a list of coins
        public static double CheckValue(List<Coin> coins)
        {
            if(coins == null)
            {
                return 0;
            }
            double totalValue = 0;
            foreach (Coin coin in coins)
            {
                totalValue += coin.Value;
            }
            return Math.Round(totalValue, 2);
        }

        public static string DecodeCoinSelection(int consoleChoice)
        {
            string coinName = "";
            switch (consoleChoice)
            {
                case 1:
                    coinName = "quarter";
                    break;
                case 2:
                    coinName = "dime";
                    break;
                case 3:
                    coinName = "nickel";
                    break;
                case 4:
                    coinName = "penny";
                    break;
            }
            return coinName;
        }

        //Final display message from Execute method
        public static void DecodeStatusCode(int statusCode, string sodaChoice, double change, double payment)
        {
            switch (statusCode)
            {
                case 1:
                    Console.WriteLine($"Out of {sodaChoice}. Collect {payment} below and try again.");
                    break;
                case 2:
                    Console.WriteLine($"Insufficient payment for {sodaChoice}. Collect {payment} below and try again.");
                    break;
                case 3:
                    Console.WriteLine($"Enjoy your {sodaChoice}! Thanks for using exact change!");
                    break;
                case 4:
                    Console.WriteLine($"Enjoy your {sodaChoice}! Collect {change} below.");
                    break;
                case 5:
                    Console.WriteLine($"Insufficient change to complete refund, we apologize for the inconvenience. Collect {payment} below.");
                    break;
                default:
                    Console.WriteLine($"ERROR: Status Unknown. Returning {payment}");
                    break;
            }
        }

        public static void DisplayValue(string message, List<Coin> coins)
        {
            Console.WriteLine($"Total Amount {message}: {UserInterface.CheckValue(coins)}");
        }

        public static void NoCoinMessage(int coinChoice)
        {
            string coinName = UserInterface.DecodeCoinSelection(coinChoice);
            Console.WriteLine($"You don't have any coins of type: {coinName}!");
        }
    }
}
