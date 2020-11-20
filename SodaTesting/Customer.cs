using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachineProject
{
    public class Customer
    {
        public Wallet wallet;
        public Backpack backpack;

        public Customer()
        {
            wallet = new Wallet();
            backpack = new Backpack();
        }

        //This large method relies on user input through Console.ReadLines, so it is not easily testable.
        //However, many submethods that are called within it only rely on parameters being passed in
        //So they can be individually tested by calling them with appropriate parameters
        public List<Coin> ChooseCoinsToDeposit()
        {
            List<Coin> deposit = null;
            bool input = true;
            while (input)
            {
                bool success = Int32.TryParse(UserInterface.DisplayCoinOptions(), out int coinChoice);
                Console.Clear();
                if (success && coinChoice > 0 && coinChoice < 5)
                {
                    if (wallet.ContainsCoin(coinChoice))
                    {
                        wallet.RemoveCoin(coinChoice);
                        deposit = DepositSingleCoin(coinChoice, deposit);
                    }
                    else
                    {
                        UserInterface.NoCoinMessage(coinChoice);
                    }
                }
                else if (coinChoice == 5)
                {
                    if (deposit != null)
                    {
                        input = false;
                    }
                }
                else if (coinChoice == 6)
                {
                    if (deposit != null)
                    {
                        wallet.AcceptCoins(deposit);
                        deposit.Clear();
                    }
                }
                UserInterface.WelcomeMessage();
                UserInterface.DisplayValue("deposited", deposit);
            }
            return deposit;
        }

        //Takes in an int and a list of coins, and adds appropriate coin to the list and returns it.
        //If list passed in is null, it will create a new list and add the first coin to it.
        public List<Coin> DepositSingleCoin(int coinChoice, List<Coin> deposit)
        {
            if (deposit == null)
            {
                deposit = new List<Coin>();
            }
            switch (coinChoice)
            {
                case 1:
                    deposit.Add(new Quarter());
                    break;
                case 2:
                    deposit.Add(new Dime());
                    break;
                case 3:
                    deposit.Add(new Nickel());
                    break;
                case 4:
                    deposit.Add(new Penny());
                    break;
                default:
                    break;
            }
            return deposit;
        }
    }
}
