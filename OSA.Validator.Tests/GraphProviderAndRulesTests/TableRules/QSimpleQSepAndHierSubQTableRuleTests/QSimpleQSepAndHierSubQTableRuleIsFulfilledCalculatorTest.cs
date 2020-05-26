using System;
using NUnit.Framework;
using OSA.Core.Entities.Registration;
using OSA.Validator.GraphBuilding;
using OSA.Validator.Rules.TablesRules.Enums;

namespace OSA.Validator.Tests.GraphProviderAndRulesTests.TableRules.QSimpleQSepAndHierSubQTableRuleTests
{
    [TestFixture]
    public class QSimpleQSepAndHierSubQTableRuleIsFulfilledCalculatorTest
    {
        /////////////////////////////////////
        //////////  Простой пакет   /////////
        /////////////////////////////////////

        //Нет доп отметок
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, true)]
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, true)]
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, true)]

        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии указаний
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии указаний и если переданы не все акции
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии доверенности
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //Если переданы не все акции
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, true)]
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, true)]
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии доверенности и если переданы не все акции
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии указаний по голосованию и при наличии доверенности
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии указаний по голосованию, при наличии доверенности и если переданы не все акции
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]


        /////////////////////////////////////
        ////////////  Продавец  /////////////
        /////////////////////////////////////

        //Нет доп отметок
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, false)]
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, false)]
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии указаний
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии указаний и если переданы не все акции
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии доверенности
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //Если переданы не все акции
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, false)]
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, true)]
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии доверенности и если переданы не все акции
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии указаний по голосованию и при наличии доверенности
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии указаний по голосованию, при наличии доверенности и если переданы не все акции
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]


        /////////////////////////////////////
        ///////////  Покупатель  ////////////
        /////////////////////////////////////

        //Нет доп отметок
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, false)]
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, false)]
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии указаний
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии указаний и если переданы не все акции
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии доверенности
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, true)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //Если переданы не все акции
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, false)]
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, false)]
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии доверенности и если переданы не все акции
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии указаний по голосованию и при наличии доверенности
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, true)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, true)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]

        //При наличии указаний по голосованию, при наличии доверенности и если переданы не все акции
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, false)]

        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, false)]
        [Test]
        public void MatrixTest(Status status,
                               AdditionalChecks additionalChecks,
                               AmountOfStockSubmited amountOfStockSubmited,
                               NumberOfChecks numberOfChecks,
                               bool expected)
        {
            var packStatus = GetPackStatusFor(status);
            // Специальный случай для голосования по доверенности
            if (packStatus == PackStatus.Simple &&
                additionalChecks == AdditionalChecks.HasTrust &&
                amountOfStockSubmited == AmountOfStockSubmited.LessOrEqualThanThereIsOnPack &&
                numberOfChecks == NumberOfChecks.Single)
            {
                HandleTrustExistanceSpecialCase(packStatus, additionalChecks, amountOfStockSubmited, numberOfChecks);
                return;
            }
            var chain1 = GraphProvider.GetQSimpleQSepAndHierSubQTableRuleChain(null, null,
                                                           () => packStatus,
                                                           () => true,
                                                           () => amountOfStockSubmited,
                                                           () => numberOfChecks,
                                                           () => additionalChecks,
                                                           () => false);
            var chain2 = GraphProvider.GetQSimpleQSepAndHierSubQTableRuleChain(null, null,
                                                           () => packStatus,
                                                           () => false,
                                                           () => amountOfStockSubmited,
                                                           () => numberOfChecks,
                                                           () => additionalChecks,
                                                           () => false);



            Assert.That(RuleChainFulfilledResoveHelper.RuleChainIsFulfilled(chain1), Is.EqualTo(expected));
            Assert.That(RuleChainFulfilledResoveHelper.RuleChainIsFulfilled(chain2), Is.EqualTo(expected));
        }
        private static void HandleTrustExistanceSpecialCase(PackStatus packStatus, AdditionalChecks additionalChecks, AmountOfStockSubmited amountOfStockSubmited, NumberOfChecks numberOfChecks)
        {
            var chain1 = GraphProvider.GetQSimpleQSepAndHierSubQTableRuleChain(null, null,
                                                                                () => packStatus,
                                                                                () => true,
                                                                                () => amountOfStockSubmited,
                                                                                () => numberOfChecks,
                                                                                () => additionalChecks,
                                                                                () => false
                );
            Assert.That(RuleChainFulfilledResoveHelper.RuleChainIsFulfilled(chain1), Is.EqualTo(true));


            var chain2 = GraphProvider.GetQSimpleQSepAndHierSubQTableRuleChain(null, null,
                                                                                () => packStatus,
                                                                                () => false,
                                                                                () => amountOfStockSubmited,
                                                                                () => numberOfChecks,
                                                                                () => additionalChecks,
                                                                                () => false
                );
            Assert.That(RuleChainFulfilledResoveHelper.RuleChainIsFulfilled(chain2), Is.EqualTo(false));
        }


        public enum Status
        {
            Simple,
            Buyer,
            Seller
        }

        private PackStatus GetPackStatusFor(Status status)
        {
            switch (status)
            {
                case Status.Simple:
                    return PackStatus.Simple;
                case Status.Buyer:
                    return PackStatus.Buyer;
                case Status.Seller:
                    return PackStatus.Seller;
                default:
                    throw new NotSupportedException(status.ToString() + " не поддерживается.");
            }
        }
    }


}