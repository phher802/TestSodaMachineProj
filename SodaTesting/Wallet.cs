using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachineProject
{
    public class Wallet
    {
        public List<Coin> coins;

        public Wallet()
        {
            coins = new List<Coin>();
            PickUpChange();
        }

        private void PickUpChange()
        {
            AddCoinsToWallet(5, new Quarter());
            AddCoinsToWallet(5, new Dime());
            AddCoinsToWallet(5, new Nickel());
            AddCoinsToWallet(5, new Penny());
        }

        private void AddCoinsToWallet(int numOfCoins, Coin coin)
        {
            for (int i = 0; i < numOfCoins; i++)
            {
                coins.Add(coin);
            }
        }


        //Takes in an int and checks to see if wallet contains appropriate coin
        public bool ContainsCoin(int coinChoice)
        {
            bool found = false;
            string coinName = UserInterface.DecodeCoinSelection(coinChoice);
            foreach (Coin coin in coins)
            {
                if (coin.name == coinName)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        //Takes in an int and removes a coin of the appropriate type
        //Does NOT check to see if wallet has the coin, that occurs in Customer's ChooseCoinstoDeposit method
        public void RemoveCoin(int coinChoice)
        {
            string coinName = UserInterface.DecodeCoinSelection(coinChoice);
            for (int i = 0; i < coins.Count; i++)
            {
                if (coins[i].name == coinName)
                {
                    coins.RemoveAt(i);
                    break;
                }
            }
        }

        public void AcceptCoins(List<Coin> returnedAmount)
        {
            foreach (Coin coin in returnedAmount)
            {
                coins.Add(coin);
            }
        }
    }
}
