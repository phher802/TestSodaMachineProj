﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachineProject
{
    public class Simulation
    {
        public Customer customer;
        public SodaMachine sodaMachine;

        public Simulation()
        {
            customer = new Customer();
            sodaMachine = new SodaMachine();
        }

        public void RunSim()
        {
            string buyChoice = "1";
            while (buyChoice == "1")
            {
                Console.Clear();
                UserInterface.WelcomeMessage();
                List<Coin> deposit = customer.ChooseCoinsToDeposit();
                string choice = UserInterface.ChooseSoda();
                sodaMachine.Execute(customer, choice, deposit);
                buyChoice = UserInterface.AskBuyAnotherSoda();
            }
            Console.ReadLine();

        }
    }
}
