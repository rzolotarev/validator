using System;
using System.Collections.Generic;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules.TablesRules.Enums;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules.TablesRules.QSimpleQSepAndHierSubQTableRules
{
    public class InCaseOfSimplePackPointWithNoAdditionalMarksAlwaysPassesRule : RuleNode
    {
        public BaseYesNoAbsPoint Point { get; private set; }
        public BulletinAdditionalMarks AdditionalMarks { get; private set; }
        private readonly Func<PackStatus> _status;
        private readonly Func<NumberOfChecks> _numberOfChecks;
        private readonly Func<AdditionalChecks> _additionalChecks;
        private readonly Func<bool> _isQSimpWithPrivDividends;

        public InCaseOfSimplePackPointWithNoAdditionalMarksAlwaysPassesRule(BaseYesNoAbsPoint point,
                                                                            BulletinAdditionalMarks additionalMarks,
                                                                            Func<PackStatus> packStatus,
                                                                            Func<NumberOfChecks> numberOfChecks,
                                                                            Func<AdditionalChecks> additionalChecks,
                                                                            Func<bool> isQSimpWithPrivDividends)
        {
            Point = point;
            AdditionalMarks = additionalMarks;
            _status = packStatus;
            _numberOfChecks = numberOfChecks;
            _additionalChecks = additionalChecks;
            _isQSimpWithPrivDividends = isQSimpWithPrivDividends;
        }

        public override bool IsFulfiled
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Является ли особенным случаем для простого пакета
        /// </summary>
        private static bool IsExceptionalCaseForSimple(AdditionalChecks additionalChecks, NumberOfChecks checks)
        {
            return additionalChecks == AdditionalChecks.None && checks == NumberOfChecks.Single;
        }

        public override bool ShouldGoFurtherDownTheGraph
        {
            get { return _status() != PackStatus.Simple || !IsExceptionalCaseForSimple(_additionalChecks(), _numberOfChecks()) || _isQSimpWithPrivDividends(); }
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
            throw new NotSupportedException("Shouldnt ever get here.");
        }
    }
}