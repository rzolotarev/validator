using System;
using System.Collections.Generic;
using System.Linq;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Separate;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules.TablesRules.Enums;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules.QSeparateRules
{
    public class QSeparateYesChecksShouldNotExceedPlacesCountRule : RuleNode
    {
        public QSeparateYesChecksShouldNotExceedPlacesCountRule(QSeparate question, BulletinAdditionalMarks additionalMarks, Func<FractionLong> allowedVotesAmount)
        {
            Question = question;
            AdditionalMarks = additionalMarks;
            AllowedVotesAmount = allowedVotesAmount;
        }

        public QSeparate Question { get; private set; }
        public BulletinAdditionalMarks AdditionalMarks { get; private set; }
        public Func<FractionLong> AllowedVotesAmount { get; private set; }

        public override bool IsFulfiled
        {
            get
            {
                var additionalChecks = BulletinAdditionalMarksToAdditionalChecksEnumConverter.ConvertToAdditionalChecksEnum(AdditionalMarks);
                if (MultiVariantVotingAllowanceInterpreter.IsMultiVariantVotingAllowed(additionalChecks) && SomeVotesAreSpecified())
                {
                    return true;
                }
                else
                {
                    var numberOfYesChecks = GetYesCheckBoxes().Where(x => x.Value == true).Count();
                    return numberOfYesChecks <= Question.PositionsCount;
                }
            }
        }

        private bool SomeVotesAreSpecified()
        {
            var amountOfStockSubmitedCalculator = new AmountOfStockSubmitedCalculator();
            return Question.CandidatePoints.Any(cp => amountOfStockSubmitedCalculator.GetAmountOfStockSubmited(cp, AllowedVotesAmount()) != AmountOfStockSubmited.VotesArentSubmited);
        }

        public override bool ShouldGoFurtherDownTheGraph
        {
            get { return IsFulfiled; }
        }

        public override IList<DocField> DependsOn
        {
            get
            {
                return GetYesCheckBoxes()
                        .Union(GetNoCheckBoxes())       // Добавляем зависимости от полей Против и Воздеражлся, чтобы при некорректно 
                        .Union(GetAbsCheckBoxes())      // отсканированном бюллетене у оператора не возникало трудностей с заполнением
                        .Union(
                            new List<DocField>
                                {
                                    AdditionalMarks.HasInstructions,
                                    AdditionalMarks.HasTrust,
                                    AdditionalMarks.NotWholeStockWasPassed
                                })
                         .ToList();
            }
        }

        private IEnumerable<CheckBoxDocField> GetYesCheckBoxes()
        {
            return GetNonInvalidCandidatePoints()
                .Select(x => x.YesCheckBoxField);
        }

        private IEnumerable<CheckBoxDocField> GetNoCheckBoxes()
        {
            return GetNonInvalidCandidatePoints()
                .Select(x => x.NoCheckBoxField);
        }

        private IEnumerable<CheckBoxDocField> GetAbsCheckBoxes()
        {
            return GetNonInvalidCandidatePoints()
                .Select(x => x.AbstainedCheckBoxField);
        }

        private IList<QSepCandidatePoint> GetNonInvalidCandidatePoints()
        {
            var result =
                Question
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
                                     x.ErrorText == ErrorTexts.NUMBER_OF_YES_CHECKES_SHOULD_NOT_EXCEED_PLACES_COUNT || 
                                     x.ErrorText == null)) // Текст равен Null до установки, а установка текста использует данный метод, так что не фильтруем такие поля
                .ToList();
            return result;
        }

        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            return ErrorTexts.NUMBER_OF_YES_CHECKES_SHOULD_NOT_EXCEED_PLACES_COUNT; 
        }
    }
}
