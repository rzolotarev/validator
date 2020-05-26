using System;
using System.Collections.Generic;
using System.Linq;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Separate;
using OSA.Editor.ViewModels.Fields;
using OSA.Editor.ViewModels.ValidityChecking;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules
{
    public class AdrMustHaveExactOrLessVotesSpreadedForQSep : RuleNode
    {
        private readonly Func<FractionLong> _allowedVotesAmount;
        private readonly bool _isADRBulletin;
        private readonly AdrQuestionValidityChecker _adrQuestionValidityChecker = new AdrQuestionValidityChecker();

        public AdrMustHaveExactOrLessVotesSpreadedForQSep(QSeparate qsep, Func<FractionLong> allowedVotesAmount, bool isADRBulletin)
        {
            _allowedVotesAmount = allowedVotesAmount;
            _isADRBulletin = isADRBulletin;
            QSeparate = qsep;
        }

        public QSeparate QSeparate { get; private set; }


        public override bool IsFulfiled
        {
            get 
            { 
                if (!_isADRBulletin) return true;
                return _adrQuestionValidityChecker.QSeparateIsValid(QSeparate, _allowedVotesAmount());
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
                return GetNonInvalidCandidatePoints()
                    .SelectMany(x =>
                        {
                            return 
                                new List<DocField>
                                    {
                                        x.YesCheckBoxField,
                                        x.YesVotesField,
                                        x.NoCheckBoxField,
                                        x.NoVotesField,
                                        x.AbstainedCheckBoxField,
                                        x.AbstainedVotesField,
                                    };
                        })
                     .ToList();
            }
        }

        private IList<QSepCandidatePoint> GetNonInvalidCandidatePoints()
        {
            var result =
                QSeparate
                .CandidatePoints
                .Where(cp => new List<DocField>
                                 {
                                     cp.YesCheckBoxField,
                                     cp.YesVotesField,
                                     cp.NoCheckBoxField,
                                     cp.NoVotesField,
                                     cp.AbstainedCheckBoxField,
                                     cp.AbstainedVotesField,
                                 }
                                 .All(x =>
                                     x.ErrorLevel != ErrorLevel.Error ||
                                     x.ErrorText == ErrorTexts.ADR_MUST_HAVE_EXACT_VOTES_AMOUNT_SPREADED ||
                                     x.ErrorText == null)) 
                .ToList();
            return result;
        }


        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            return ErrorTexts.ADR_MUST_HAVE_EXACT_VOTES_AMOUNT_SPREADED_OR_LESS_WITH_AT_LEAST_ONE_CANDIDATE_HAVING_EXACT_VOTES_AMOUNT;
        }
    }
}