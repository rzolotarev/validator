using System.Linq;
using OSA.Core.Entities.Registration;
using OSA.Core.Util.Extensions.IEnumerables;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules;
using OSA.Validator.Rules.TablesRules.QCumulativeTableRules;

namespace OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers.Rules
{
    public class QCumulativeTableRuleMarkuper : MarkAllDependantRuleMarkuper
    {
        public override void MarkupNotFulfilled(RuleNode ruleNode)
        {
            base.MarkupNotFulfilled(ruleNode);
            var rule = (QCumulativeTableRule)ruleNode;
            InCaseOfYesCheckAndNoVotesMarkAllVotesAsErrors(rule);
        }

        private void InCaseOfYesCheckAndNoVotesMarkAllVotesAsErrors(QCumulativeTableRule rule)
        {
            var question = rule.Question;
            if (question.AdditionalMarks.YesCheckBoxField.Value == true && question.CandidatePoints.Sum(x=> x.VotesDocField.Value) == FractionLong.Zero)
            {
                question.CandidatePoints.ForEach(x => x.VotesDocField.ErrorLevel = ErrorLevel.Error);
            }
        }

        public override bool CanMarkupRuleNodeOfType(RuleNode ruleNode)
        {
            return ruleNode is QCumulativeTableRule;
        }
    }
}
