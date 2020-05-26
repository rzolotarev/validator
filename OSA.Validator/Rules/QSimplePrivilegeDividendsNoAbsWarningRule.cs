using System.Collections.Generic;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Simple;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules
{
    public class QSimplePrivilegeDividendsNoAbsWarningRule : RuleNode
    {
        private readonly bool _packHasPrivilegeVotes;
        public QSimple Question { get; set; }

        public QSimplePrivilegeDividendsNoAbsWarningRule(QSimple qSimple, bool packHasPrivilegeVotes)
        {
            _packHasPrivilegeVotes = packHasPrivilegeVotes;
            Question = qSimple;
        }

        public override bool IsFulfiled
        {
            get
            {
                if (Question.IsQuestionAboutPrivilegeDividends == false || _packHasPrivilegeVotes == false) return true;
                return !Question.Point.NoCheckBoxField.IsFilled && !Question.Point.AbstainedCheckBoxField.IsFilled;
            }
        }

        public override bool ShouldGoFurtherDownTheGraph
        {
            get { return IsFulfiled; }
        }

        public override IList<DocField> DependsOn
        {
            get { return new List<DocField>
                {
                    Question.Point.NoCheckBoxField,
                    Question.Point.AbstainedCheckBoxField
                };}
        }

        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            return ErrorTexts.PRIVILEGE_NO_ABS_VOTES_WILL_BE_IGNORED;
        }
    }
}
