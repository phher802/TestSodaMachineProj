﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SodaMachineProject;

namespace SodaMachineTestProject
{
    [TestClass]
    public class SodaMachineClassTests
    {
        [TestMethod]
        public void Execute_OverPayForOrange_TwoQuarters_GetStatus4()
        {
            //Arrange
            //We need a SodaMachine, a Customer, and a list of coins to test this method, so we instantiate them in the Arrange
            SodaMachine soda = new SodaMachine();
            Customer cust = new Customer();
            List<Coin> twoQuarters = new List<Coin>() { new Quarter(), new Quarter() };

            //Since we're overpaying, we expect to get status code of 4 when we run the method, so 'expected' = 4
            int expected = 4;
            //Just declaring 'actual', not setting its value yet
            int actual;

            //Act
            //Here we actually call the method we want to test, and set 'actual' equal to the method's return
            actual = soda.Execute(cust, "Orange Soda", twoQuarters);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Execute_OverPayForOrange_TwoQuarters_RegisterHas95CoinsAfterSale()
        {
            //Arrange
            //Same Arrange as last test!
            //There are lots of things happening in this method, so we can test many different things with same setup!
            //This test checks to make sure register has correct amount of coins after sale
            //What else could you test with a similar setup?
            SodaMachine soda = new SodaMachine();
            Customer cust = new Customer();
            List<Coin> twoQuarters = new List<Coin>() { new Quarter(), new Quarter() };

            //100 coins in starting reg + 2 quarters payment - 7 coins as change = 95 coins, so 'expected' = 95 
            int expected = 95;
            int actual;

            //Act
            //Call Execute to trigger sale
            soda.Execute(cust, "Orange Soda", twoQuarters);
            //We're testing number of coins in the register, so 'actual' = count of coin list.
            actual = soda.register.Count;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ContainsCoin_DoesRegisterContainCoin_RegisterContainsAQuarterAndDime()
        {
            //Arrange - objects,variables, etc.
            //need to instantiate soda machine, need list of coins, and a register list
            SodaMachine soda = new SodaMachine();
            //List<Coin> register = new List<Coin>() { new Quarter(), new Dime() };


            bool expected = true;
            bool actual;


            //Act - call methods to test
            //call ContainsCoins to check if coins are in register
            actual = soda.ContainsCoin("quarter");

            //Assert - expected output
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddCoinsToRegister_AreCoinsAddedToRegister_RegisterWillHave21Quarters()
        {
            //Arrange
            SodaMachine soda = new SodaMachine();
            Coin quarter = new Coin();

            int expected = 101;
            int actual;

            //Act
            soda.AddCoinsToRegister(1, quarter);
            actual = soda.register.Count;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ContainsCan_DoesInventoryHaveSelectedCan_InventoryDoesHaveCan()
        {
            //Arrange - objects/variables/etc
            SodaMachine soda = new SodaMachine();
            Can cola = new Cola();

            bool expected = true;
            bool actual;

            //Act

            actual = soda.ContainsCan(cola);

            //Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void DetermineAmountOfChange_CaculateChangeToReturn_AmountOfChangeIs15Cents()
        {
            //Arrange - objects, variables, etc.
            SodaMachine soda = new SodaMachine();
            Can orangeSoda = new OrangeSoda();
            double payment = 0.25;

            double expected = 0.19;
            double actual;

            //Act
            actual = soda.DetermineAmountOfChange(orangeSoda, payment);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveAndAdd_RemoveSelectedCanFromInventoryToAddToBackpack_InventoryIs35CansBackpackIs1()
        {
            // Arrange

            SodaMachine soda = new SodaMachine();
            Customer customer = new Customer();
            Can cola = new Cola();

            double inventoryExpected = 35;
            double backpackExpected = 1;
            double inventoryActual;
            double backpackActual;

            // Act
            soda.DispenseSodaToCustomer(customer, cola);
            inventoryActual = soda.inventory.Count;
            backpackActual = customer.backpack.cans.Count;
            //Assert

            Assert.AreEqual(inventoryExpected, inventoryActual);
            Assert.AreEqual(backpackExpected, backpackActual);

        }

        [TestMethod]
        public void PrepareCan_CheckToSeeIfSelectedCanIsAvailable_ReturnColaIfAvailable()
        {
            // Arrange
            SodaMachine soda = new SodaMachine();
            Can cola = new Cola();
            Can orangeSoda = new OrangeSoda();
            Can newCan;

            string expected = cola.name;
            string actual;

            // Act
            newCan = soda.PrepareCan("Cola");
            actual = newCan.name;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
