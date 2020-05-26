using System;
using System.Collections.Generic;
using NUnit.Framework;
using OSA.Core.Entities.Registration;
using OSA.Validator.GraphBuilding;

using OSA.Validator.Rules.TablesRules.Enums;
using OSA.Validator.Rules.TablesRules.QCumulativeTableRules;

namespace OSA.Validator.Tests.GraphProviderAndRulesTests.TableRules.QCumulativeTableRuleTests
{
    [TestFixture]
    public class QCumulativeTableRuleIsFulfilledCalculatorTest
    {
        /////////////////////////////////////
        //////////  Простой пакет   /////////
        /////////////////////////////////////

        //Нет доп отметок
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, true)]
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, true)]

        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]


        //При наличии указаний
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        //При наличии указаний и если переданы не все акции
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        //При наличии доверенности
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        //Если переданы не все акции
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, true)]
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        //При наличии доверенности и если переданы не все акции
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        //При наличии указаний по голосованию и при наличии доверенности
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        //При наличии указаний по голосованию, при наличии доверенности и если переданы не все акции
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, true)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Simple, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ///////////////////////////////////////
        //////////////  Продавец  /////////////
        ///////////////////////////////////////

        //Нет доп отметок
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ////При наличии указаний
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ////При наличии указаний и если переданы не все акции
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, true)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ////При наличии доверенности
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ////Если переданы не все акции
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, true)]
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ////При наличии доверенности и если переданы не все акции
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ////При наличии указаний по голосованию и при наличии доверенности
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        //При наличии указаний по голосованию, при наличии доверенности и если переданы не все акции
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Seller, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ///////////////////////////////////////
        /////////////  Покупатель  ////////////
        ///////////////////////////////////////

        ////Нет доп отметок
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.None, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ////При наличии указаний
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ////При наличии указаний и если переданы не все акции
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ////При наличии доверенности
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, true)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ////Если переданы не все акции
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ////При наличии доверенности и если переданы не все акции
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]


        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ////При наличии указаний по голосованию и при наличии доверенности
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, true)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, true)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, true)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, true)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        ////При наличии указаний по голосованию, при наличии доверенности и если переданы не все акции
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Single, CumYesIs.NotChecked, false)]

        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.VotesArentSubmited, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.LessOrEqualThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.Checked, false)]
        [TestCase(Status.Buyer, AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust | AdditionalChecks.NotWholeStockWasPassed, AmountOfStockSubmited.MoreThanThereIsOnPack, NumberOfChecks.Multiple, CumYesIs.NotChecked, false)]

        [Test]
        public void MatrixTest(Status status,
                               AdditionalChecks additionalChecks,
                               AmountOfStockSubmited amountOfStockSubmited,
                               NumberOfChecks numberOfChecks,
                               CumYesIs cumChecks,
                               bool expected)
        {
            var packStatus = GetPackStatusFor(status);

            AddNewVariantAndCheckThatItIsUnique(additionalChecks, packStatus, numberOfChecks, cumChecks, amountOfStockSubmited);


            if (packStatus == PackStatus.Simple &&
                additionalChecks == AdditionalChecks.HasTrust &&
                amountOfStockSubmited == AmountOfStockSubmited.LessOrEqualThanThereIsOnPack &&
                numberOfChecks == NumberOfChecks.Single &&
                cumChecks == CumYesIs.NotChecked)
            {
                HandleTrustExistanceSpecialCase(packStatus, additionalChecks, amountOfStockSubmited, numberOfChecks, cumChecks);
                return;
            }

            var chain1 = GraphProvider.GetQCumulativeTableRule(null, null,
                                                           () => packStatus,
                                                           () => true,
                                                           () => amountOfStockSubmited,
                                                           () => numberOfChecks,
                                                           () => additionalChecks,
                                                           () => cumChecks); 
            var chain2 = GraphProvider.GetQCumulativeTableRule(null, null,
                                                           () => packStatus,
                                                           () => false,
                                                           () => amountOfStockSubmited,
                                                           () => numberOfChecks,
                                                           () => additionalChecks,
                                                           () => cumChecks);


            Assert.That(RuleChainFulfilledResoveHelper.RuleChainIsFulfilled(chain1), Is.EqualTo(expected));
            Assert.That(RuleChainFulfilledResoveHelper.RuleChainIsFulfilled(chain2), Is.EqualTo(expected));
        }

        private static void HandleTrustExistanceSpecialCase(PackStatus packStatus, AdditionalChecks additionalChecks, AmountOfStockSubmited amountOfStockSubmited, NumberOfChecks numberOfChecks, CumYesIs cumChecks)
        {
            var chain1 = GraphProvider.GetQCumulativeTableRule(null, null,
                                                           () => packStatus,
                                                           () => true,
                                                           () => amountOfStockSubmited,
                                                           () => numberOfChecks,
                                                           () => additionalChecks,
                                                           () => cumChecks);
            var chain2 = GraphProvider.GetQCumulativeTableRule(null, null,
                                                           () => packStatus,
                                                           () => false,
                                                           () => amountOfStockSubmited,
                                                           () => numberOfChecks,
                                                           () => additionalChecks,
                                                           () => cumChecks);


            Assert.That(RuleChainFulfilledResoveHelper.RuleChainIsFulfilled(chain1), Is.EqualTo(true));
            Assert.That(RuleChainFulfilledResoveHelper.RuleChainIsFulfilled(chain2), Is.EqualTo(false));
        }

        private void AddNewVariantAndCheckThatItIsUnique(AdditionalChecks additionalChecks, PackStatus packStatus, NumberOfChecks numberOfChecks, CumYesIs cumChecks, AmountOfStockSubmited amountOfStockSubmited)
        {
            //Проверяем, что комбинация не встречалась раньше
            var currentVariant = new WorkedOutVariant
                                     {
                                         AdditionalChecks = additionalChecks,
                                         PackStatus = packStatus,
                                         NumberOfChecks = numberOfChecks,
                                         CumChecks = cumChecks,
                                         AmountOfStockSubmited = amountOfStockSubmited
                                     };
            if (_workedOutVariants.Contains(currentVariant)) Assert.Fail("Повторяющаяся комбинация!");
            else _workedOutVariants.Add(currentVariant);
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _workedOutVariants = new List<WorkedOutVariant>();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            //Проверяем, что были проверены все возможные комбинации входных данных
            if (ExpectedNumberOfVariantsToCheck() != _workedOutVariants.Count)
            {
                throw new ArgumentOutOfRangeException("Были проверены не все варианты! Ожидалось " + ExpectedNumberOfVariantsToCheck() + " вариантов, а были проверены " + _workedOutVariants.Count);
            }
        }

        private int ExpectedNumberOfVariantsToCheck()
        {
            return
                3 * //PackStatus
                8 * //AdditionalChecks
                3 * //AmountOfStockSubmited
                2 * //NumberOfChecks
                2;  //CumYesIs
        }

        private List<WorkedOutVariant> _workedOutVariants;

        private struct WorkedOutVariant
        {
            public PackStatus PackStatus;
            public AdditionalChecks AdditionalChecks;
            public AmountOfStockSubmited AmountOfStockSubmited;
            public NumberOfChecks NumberOfChecks;
            public CumYesIs CumChecks;
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