using System.Collections.Generic;
using System.Linq;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules;
using OSA.Validator.Visitors.ErrorMarkupers;

namespace OSA.Validator
{
    public  class GraphWalker
    {
        private readonly VotesFieldsWasntCheckedSetter _votesFieldsWasntCheckedSetter = new VotesFieldsWasntCheckedSetter();

        public void WalkThrough(RuleNode rootNode, BulletinScreenModelBase bulletin)
        { 
            var visitor = new GeneralErrorMarkerVisitor();
            rootNode.Accept(visitor);

            SetWasntChecked(visitor);

            _votesFieldsWasntCheckedSetter.SetVoteFieldsThatDontHaveCorrespondingChecksSetAsWasntChecked(bulletin);
        }

        private  void SetWasntChecked(ErrorMarkuperVisitorBase visitor)
        {
            var allVisitedFields = GetAllVisitedFields(visitor);
            var fieldsBelowVisited = GetFieldsBelowVisited(visitor);

            var wasntCheckedFields = fieldsBelowVisited.Except(allVisitedFields);

            wasntCheckedFields.ToList()
                .ForEach(f => f.ErrorLevel = ErrorLevel.WasntChecked);
        }

        private  List<DocField> GetFieldsBelowVisited(ErrorMarkuperVisitorBase visitor)
        {
            var result = new List<DocField>();
            foreach (var rule in visitor.VisitedRules.Where(r=> !r.ShouldGoFurtherDownTheGraph))
            {
                result.AddRange(rule.DependantRulesDependOn);
            }
            result = result.Distinct().ToList();
            return result;
        }

        private  List<DocField> GetAllVisitedFields(ErrorMarkuperVisitorBase visitor)
        {
            var result = new List<DocField>();
            foreach (var rule in visitor.VisitedRules.Where(x=>! (x is ParallelRule)))
            {
                result.AddRange(rule.DependsOn);
            }
            result = result.Distinct().ToList();
            return result;
        }
    }
}