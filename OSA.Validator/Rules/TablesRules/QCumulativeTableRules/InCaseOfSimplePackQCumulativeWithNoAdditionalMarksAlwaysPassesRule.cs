using System;
using System.Collections.Generic;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules.TablesRules.Enums;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules.TablesRules.QCumulativeTableRules
{
    public class InCaseOfSimplePackQCumulativeWithNoAdditionalMarksAlwaysPassesRule : RuleNode
    {
        private readonly Func<PackStatus> _packStatus;
        private readonly Func<AmountOfStockSubmited> _amountOfStockSubmited;
        private readonly Func<NumberOfChecks> _numberOfChecks;
        private readonly Func<AdditionalChecks> _additionalChecks;
        private readonly Func<CumYesIs> _cumChecks;
        public QCumulative Question { get; set; }
        public BulletinAdditionalMarks AdditionalMarks { get; set; }

        public InCaseOfSimplePackQCumulativeWithNoAdditionalMarksAlwaysPassesRule(QCumulative qCumulative,
                                                                                  BulletinAdditionalMarks additionalMarks,
                                                                                  Func<PackStatus> packStatus,
                                                                                  Func<AmountOfStockSubmited> amountOfStockSubmited,
                                                                                  Func<NumberOfChecks> numberOfChecks,
                                                                                  Func<AdditionalChecks> additionalChecks,
                                                                                  Func<CumYesIs> cumChecks)
        {
            _packStatus = packStatus;
            _amountOfStockSubmited = amountOfStockSubmited;
            _numberOfChecks = numberOfChecks;
            _additionalChecks = additionalChecks;
            _cumChecks = cumChecks;
            Question = qCumulative;
            AdditionalMarks = additionalMarks;
        }

        public override bool IsFulfiled
        {
            get { return true; }
        }

        public override bool ShouldGoFurtherDownTheGraph
        {
            get
            {
                return
                    _packStatus() != PackStatus.Simple
                    ||
                    !IsExceptionalCaseForSimplePack(_additionalChecks(), _amountOfStockSubmited(), _numberOfChecks(), _cumChecks());
            }
        }

        public override IList<DocField> DependsOn
        {
            get
            {
                return new List<DocField>
                           {
                               Question.AdditionalMarks.NoCheckBoxField,
                               Question.AdditionalMarks.AbstainedCheckBoxField,

                               AdditionalMarks.HasInstructions,
                               AdditionalMarks.HasTrust,
                               AdditionalMarks.NotWholeStockWasPassed,
                           };
            }
        }

        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            throw new NotSupportedException("Shouldnt ever get here.");
        }

        private static bool IsExceptionalCaseForSimplePack(AdditionalChecks additionalChecks, AmountOfStockSubmited amountOfStockSubmited, NumberOfChecks numberOfChecks, CumYesIs cumChecks)
        {
            return (additionalChecks == AdditionalChecks.None /*&&
                    amountOfStockSubmited == AmountOfStockSubmited.MoreThanThereIsOnPack*/
                                                                                           &&
                    numberOfChecks == NumberOfChecks.Single &&
                    cumChecks == CumYesIs.NotChecked);
        }
    }
}