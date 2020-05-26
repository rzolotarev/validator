using OSA.Core.Util.Extensions.IEnumerables;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules;
using OSA.Validator.Rules.QCumulativeRules;

namespace OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers.Rules
{
    public class QCumulativeWarningRuleMarkuper:RuleNodeMarkuperBase
    {
        public override void MarkupFulfilled(RuleNode ruleNode)
        {
            var rule = (QCumulativeWarningRule)ruleNode;
            rule.Question.AdditionalMarks.YesCheckBoxField.ErrorLevel = ErrorLevel.Valid;
            rule.Question.AdditionalMarks.NoCheckBoxField.ErrorLevel = ErrorLevel.Valid;
            rule.Question.AdditionalMarks.AbstainedCheckBoxField.ErrorLevel = ErrorLevel.Valid;

            rule.Question.CandidatePoints.ForEach(c => c.VotesDocField.ErrorLevel = ErrorLevel.Valid);
            rule.Question.AdditionalMarks.NoVotesField.ErrorLevel = ErrorLevel.Valid;
            rule.Question.AdditionalMarks.AbstainedVotesField.ErrorLevel = ErrorLevel.Valid;
        }

        public override void MarkupNotFulfilled(RuleNode ruleNode)
        {
            var rule = (QCumulativeWarningRule)ruleNode;
            rule.Question.AdditionalMarks.YesCheckBoxField.ErrorLevel = ErrorLevel.Warning;
            rule.Question.AdditionalMarks.NoCheckBoxField.ErrorLevel = ErrorLevel.Warning;
            rule.Question.AdditionalMarks.AbstainedCheckBoxField.ErrorLevel = ErrorLevel.Warning;
            
            rule.Question.CandidatePoints.ForEach(c => c.VotesDocField.ErrorLevel = ErrorLevel.Warning);
            rule.Question.AdditionalMarks.NoVotesField.ErrorLevel = ErrorLevel.Warning;
            rule.Question.AdditionalMarks.AbstainedVotesField.ErrorLevel = ErrorLevel.Warning;
        }

        public override bool CanMarkupRuleNodeOfType(RuleNode ruleNode)
        {
            return ruleNode is QCumulativeWarningRule;
        }
    }
}