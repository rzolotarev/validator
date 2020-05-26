using System.Collections.Generic;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules
{
    public class PointShouldHaveAtLeastOneSelectionRule : RuleNode
    {
        public PointShouldHaveAtLeastOneSelectionRule(BaseYesNoAbsPoint point)
        {
            Point = point;
        }

        public BaseYesNoAbsPoint Point { get; private set; }

        public override bool IsFulfiled
        {
            get
            {
                if (Point.NumberOfCheckedFields < 1) return false;
                else return true;
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
                               Point.AbstainedCheckBoxField
                           };
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