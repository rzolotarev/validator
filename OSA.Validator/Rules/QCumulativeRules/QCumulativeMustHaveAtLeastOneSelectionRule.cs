using System.Collections.Generic;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules.QCumulativeRules
{
    public class QCumulativeMustHaveAtLeastOneSelectionRule : RuleNode
    {
        public QCumulativeMustHaveAtLeastOneSelectionRule(QCumulative question)
        {
            Question = question;
        }

        public QCumulative Question { get; private set; }

        public override bool IsFulfiled
        {
            get
            {
                return Question.AdditionalMarks.YesCheckBoxField.Value ||
                       Question.AdditionalMarks.NoCheckBoxField.Value ||
                       Question.AdditionalMarks.AbstainedCheckBoxField.Value;
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
                            Question.AdditionalMarks.YesCheckBoxField,
                            Question.AdditionalMarks.NoCheckBoxField,
                            Question.AdditionalMarks.AbstainedCheckBoxField
                       }
                    ;
            }
        }

        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            return ErrorTexts.AT_LEAST_ONE_SELECTION_MUST_BE_MADE;
        }
    }
}
