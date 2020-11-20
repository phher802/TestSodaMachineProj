using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachineProject
{
    public class SodaMachine
    {
        public List<Coin> register;
        public List<Can> inventory;

        public SodaMachine()
        {
            register = new List<Coin>();
            inventory = new List<Can>();
            FillRegister();
            FillStock();
        }

        //Overload of constructor to simulate different conditions for testing
        public SodaMachine(string testCondition)
        {
            register = new List<Coin>();
            inventory = new List<Can>();
            if (testCondition == "noMoney")
            {
                //Only cans, no money. Will have insufficient change
                FillStock();
            }
            else if (testCondition == "noSoda")
            {
                //Only money, no cans. All cans out of stock
                FillRegister();
            }
            else if (testCondition == "nothing")
            {
                //No cans or money
            }
        }

        public void FillRegister()
        {
            AddCoinsToRegister(20, new Quarter());
            AddCoinsToRegister(10, new Dime());
            AddCoinsToRegister(20, new Nickel());
            AddCoinsToRegister(50, new Penny());
        }

        public void FillStock()
        {
            for (int i = 0; i < 12; i++)
            {
                inventory.Add(new Cola());
                inventory.Add(new RootBeer());
                inventory.Add(new OrangeSoda());
            }
        }

        //Takes in a coin name, checks to see if coin of that type exists in register
        public bool ContainsCoin(string coinName)
        {
            bool found = false;
            foreach (Coin coin in register)
            {
                if (coin.name == coinName)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        //Used by constructor to add a certain amount of any coin
        public void AddCoinsToRegister(int numOfCoins, Coin coin)
        {
            for (int i = 0; i < numOfCoins; i++)
            {
                register.Add(coin);
            }
        }

        //Main public method that allows a customer to try to purchase a soda
        //Will result in the 5 possibilities mentioned in user story depending on parameters and state of sodamachine's inventory/register
        public int Execute(Customer customer, string sodaChoice, List<Coin> deposit)
        {
            double payment = AcceptCoins(deposit);

            Can selection = PrepareCan(sodaChoice);

            double change = DetermineAmountOfChange(selection, payment);

            int statusCode = AttemptSale(change);

            switch (statusCode)
            {
                case 1:
                    //Choice was out of stock, return deposit to customer's wallet
                    EjectDeposit(deposit);
                    customer.wallet.AcceptCoins(deposit);
                    break;
                case 2:
                    //Insufficient payment deposited, return deposit to customer's wallet
                    EjectDeposit(deposit);
                    customer.wallet.AcceptCoins(deposit);
                    break;
                case 3:
                    //Exact change, dispense soda to customer's backpack
                    DispenseSodaToCustomer(customer, selection);
                    break;
                case 4:
                    //Overpayed, dispense soda to backpack and change to wallet
                    DispenseSodaToCustomer(customer, selection);
                    List<Coin> refund = CreateChange(change);
                    customer.wallet.AcceptCoins(refund);
                    break;
                case 5:
                    //Machine out of change, return deposit
                    EjectDeposit(deposit);
                    customer.wallet.AcceptCoins(deposit);
                    break;
                default:
                    //We shouldn't get here, but return deposit just in case
                    EjectDeposit(deposit);
                    customer.wallet.AcceptCoins(deposit);
                    break;
            }

            //Displays console message informing user of results
            UserInterface.DecodeStatusCode(statusCode, sodaChoice, change, payment);

            //return value is not captured in program, but can be tested
            return statusCode;
        }

        //Takes in a list of coins and adds them to internal register, returns the money value of the list
        public double AcceptCoins(List<Coin> deposit)
        {
            foreach (Coin coin in deposit)
            {
                register.Add(coin);
            }
            return UserInterface.CheckValue(deposit);
        }

        //Takes in a string and returns a can of that type if available.
        //If stock does not contain that type of can, this will return null
        public Can PrepareCan(string canChoice)
        {
            Can actualCan = null;
            switch (canChoice)
            {
                case "Cola":
                    Can cola = new Cola();
                    if (ContainsCan(cola))
                    {
                        actualCan = cola;
                    }
                    break;
                case "Orange Soda":
                    Can orange = new OrangeSoda();
                    if (ContainsCan(orange))
                    {
                        actualCan = orange;
                    }
                    break;
                case "Root Beer":
                    Can rootbeer = new RootBeer();
                    if (ContainsCan(rootbeer))
                    {
                        actualCan = rootbeer;
                    }
                    break;
                default:
                    break;
            }
            return actualCan;
        }

        //Takes in a can instance, checks to see if that type of can exists in inventory
        public bool ContainsCan(Can selection)
        {
            bool found = false;
            foreach (Can can in inventory)
            {
                if (can.name == selection.name)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        //Compares payment to price of can and returns the difference, positive, negative, or zero
        //If out of stock, change will be -100
        public double DetermineAmountOfChange(Can can, double payment)
        {
            double change = -100;
            if (can != null)
            {
                change = payment - can.Cost;
            }
            return change;
        }

        //Takes in a double which represents whether customer over or underpaid (will be -100 if out of stock)
        //Returns ints 1-5 as status code of attempted sale
        //See Execute method for status code explanations
        public int AttemptSale(double change)
        {
            int statusCode = 0;
            if (change == -100)
            {
                statusCode = 1;
            }
            else if (change < 0)
            {
                statusCode = 2;
            }
            else if (change == 0)
            {
                statusCode = 3;
            }
            else if (change > 0)
            {
                //Attempt to make change to see if register has enough coins,
                //but can't return them yet so must put them back in reg for now
                List<Coin> refund = CreateChange(change);
                if (refund.Count > 0)
                {
                    statusCode = 4;
                    AcceptCoins(refund);
                }
                else
                {
                    statusCode = 5;
                }
            }
            return statusCode;
        }

        //Creates a list of Coins equal to the value of parameter
        //Will return empty list if insufficient change exists
        public List<Coin> CreateChange(double changeAmount)
        {
            List<Coin> refund = new List<Coin>();

            foreach (Coin coin in register.ToList())
            {
                changeAmount = Math.Round(changeAmount, 2);
                if (coin.Value == 0.25 && changeAmount >= 0.25)
                {
                    changeAmount -= 0.25;
                    register.Remove(coin);
                    refund.Add(coin);
                }
                else if (coin.Value == 0.10 && changeAmount >= 0.10)
                {
                    changeAmount -= 0.10;
                    register.Remove(coin);
                    refund.Add(coin);
                }
                else if (coin.Value == 0.05 && changeAmount >= 0.05)
                {
                    changeAmount -= 0.05;
                    register.Remove(coin);
                    refund.Add(coin);
                }
                else if (coin.Value == 0.01 && changeAmount >= 0.01)
                {
                    changeAmount -= 0.01;
                    register.Remove(coin);
                    refund.Add(coin);
                }
            }
            changeAmount = Math.Round(changeAmount, 2);
            if (changeAmount != 0)
            {
                //Insufficient change in reg, so re-add refund to register and return empty list
                AcceptCoins(refund);
                refund.Clear();
            }
            return refund;
        }

        //Takes in a list of coins and removes them from register
        //This only works on a List that was previously added, such as "deposit"
        public void EjectDeposit(List<Coin> coins)
        {
            foreach (Coin coin in coins)
            {
                register.Remove(coin);
            }
        }

        //Removes can from inventory, adds it to Customer backpack
        public void DispenseSodaToCustomer(Customer customer, Can selection)
        {
            RemoveCan(selection);
            customer.backpack.cans.Add(selection);
        }

        //Removes first can from inventory that matches Can passed in
        public void RemoveCan(Can can)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].name == can.name)
                {
                    inventory.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
