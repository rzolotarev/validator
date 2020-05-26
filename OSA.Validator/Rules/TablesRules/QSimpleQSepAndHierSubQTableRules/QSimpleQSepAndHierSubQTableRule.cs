using System;
using System.Collections.Generic;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules.TablesRules.Enums;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules.TablesRules
{
    public class QSimpleQSepAndHierSubQTableRule : RuleNode
    {

        public QSimpleQSepAndHierSubQTableRule(BaseYesNoAbsPoint point,
            BulletinAdditionalMarks bulletinAdditionalMarks,
            Func<PackStatus> packStatus,
            Func<bool> trustExists,
            Func<AmountOfStockSubmited> amountOfStockSubmited,
            Func<NumberOfChecks> numberOfChecks,
            Func<AdditionalChecks> additionalChecks)
        {
            Point = point;
            AdditionalMarks = bulletinAdditionalMarks;
            PackStatus = packStatus;
            TrustExists = trustExists;
            AmountOfStockSubmited = amountOfStockSubmited;
            NumberOfChecks = numberOfChecks;
            AddtitionalChecks = additionalChecks;
        }

        public BaseYesNoAbsPoint Point { get; private set; }
        public BulletinAdditionalMarks AdditionalMarks { get; private set; }
        private Func<PackStatus> PackStatus { get; set; }
        private Func<AmountOfStockSubmited> AmountOfStockSubmited { get; set; }
        private Func<AdditionalChecks> AddtitionalChecks { get; set; }
        private Func<NumberOfChecks> NumberOfChecks { get; set; }
        private Func<bool> TrustExists { get; set; }

        private readonly QSimpleQSepAndHierSubQTableRuleIsFulfilledCalculator _calculator = new QSimpleQSepAndHierSubQTableRuleIsFulfilledCalculator();

        public override bool IsFulfiled
        {
            get
            {
                return _calculator.GetIsFulfilledFor(PackStatus(), AddtitionalChecks(), AmountOfStockSubmited(), NumberOfChecks(), TrustExists());
            }
        }

        public override bool ShouldGoFurtherDownTheGraph
        {
            get { return IsFulfiled; }
        }

        public override IList<DocField> DependsOn
        {
            get
            {
                return new List<DocField>
                           {
                               Point.YesCheckBoxField,
                               Point.NoCheckBoxField,
                               Point.AbstainedCheckBoxField,
                               Point.YesVotesField,
                               Point.NoVotesField,
                               Point.AbstainedVotesField,
                               
                               AdditionalMarks.HasInstructions,
                               AdditionalMarks.HasTrust,
                               AdditionalMarks.NotWholeStockWasPassed
                           };
            }
        }

        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            return _calculator.GetErrorTextFor(PackStatus(), AddtitionalChecks(), AmountOfStockSubmited(), NumberOfChecks(), TrustExists());
        }
    }
}