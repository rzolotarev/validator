using System;
using System.Collections.Generic;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.AmountOfVotesCalculators;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules
{
    public class QSimpleQSepAndHierSubQWarningRule : RuleNode
    {
        private readonly BaseYesNoAbsPoint _point;


        private static readonly ExactAmountOfStockSubmitedCalulator ExactAmountOfStockSubmitedCalulator = new ExactAmountOfStockSubmitedCalulator();
        private BulletinAdditionalMarks _additionalMarks;
        private readonly Func<FractionLong> _allowedVotesAmount;


        public QSimpleQSepAndHierSubQWarningRule(BaseYesNoAbsPoint point, BulletinAdditionalMarks additionalMarks, Func<FractionLong> allowedVotesAmount)
        {
            _point = point;
            AdditionalMarks = additionalMarks;
            _allowedVotesAmount = allowedVotesAmount;
        }

        public override bool IsFulfiled
        {
            get
            {
                return ExactAmountOfStockSubmitedCalulator.GetVotesForYesField(_point, _allowedVotesAmount(), AdditionalMarks)
                       + ExactAmountOfStockSubmitedCalulator.GetVotesForNoField(_point, _allowedVotesAmount(), AdditionalMarks)
                       + ExactAmountOfStockSubmitedCalulator.GetVotesForAbstainedField(_point, _allowedVotesAmount(), AdditionalMarks) == _allowedVotesAmount();
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
                var emptyList = new List<DocField>();
                return emptyList;
            }
        }

        public BaseYesNoAbsPoint Point
        {
            get { return _point; }
        }

        public BulletinAdditionalMarks AdditionalMarks
        {
            get { return _additionalMarks; }
            set { _additionalMarks = value; }
        }

        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            return ErrorTexts.NOT_ALL_VOTES_WERE_DIVIDED_AMONG_FIELDS;
        }
    }
}