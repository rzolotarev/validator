using System.Collections.Generic;
using System.Linq;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.AmountOfVotesCalculators;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;
using OSA.Validator.Rules.TablesRules.Enums;
using OSA.Core.Util.Extensions.IEnumerables;

namespace OSA.Validator
{
    public class AmountOfStockSubmitedCalculator
    {
        public AmountOfStockSubmited GetAmountOfStockSubmited(BaseYesNoAbsPoint point, FractionLong allowedVotesAmount)
        {
            var pairs = new List<KeyValuePair<bool, FractionLong>>
                            {
                                new KeyValuePair<bool, FractionLong>(point.YesCheckBoxField.Value, point.YesVotesField.Value),
                                new KeyValuePair<bool, FractionLong>(point.NoCheckBoxField.Value, point.NoVotesField.Value),
                                new KeyValuePair<bool, FractionLong>(point.AbstainedCheckBoxField.Value, point.AbstainedVotesField.Value),
                            };

            return GetAmountOfStockSubmittedFor(pairs, allowedVotesAmount);
        }

        public AmountOfStockSubmited GetAmountOfStockSubmited(QCumulative qCum, FractionLong allowedVotesAmount)
        {
            if (qCum.AdditionalMarks.YesCheckBoxField.Value == true && qCum.CandidatePoints.Sum(c => c.VotesDocField.Value) == FractionLong.Zero)
            {
                return AmountOfStockSubmited.VotesArentSubmited;
            }

            var pairs = new List<KeyValuePair<bool, FractionLong>>
                            {
                                new KeyValuePair<bool, FractionLong>(qCum.AdditionalMarks.YesCheckBoxField.Value, qCum.CandidatePoints.Sum(c=>c.VotesDocField.Value)),
                                new KeyValuePair<bool, FractionLong>(qCum.AdditionalMarks.NoCheckBoxField.Value, qCum.AdditionalMarks.NoVotesField.Value),
                                new KeyValuePair<bool, FractionLong>(qCum.AdditionalMarks.AbstainedCheckBoxField.Value, qCum.AdditionalMarks.AbstainedVotesField.Value),
                            };

            return GetAmountOfStockSubmittedFor(pairs, allowedVotesAmount);
        }

        private static AmountOfStockSubmited GetAmountOfStockSubmittedFor(List<KeyValuePair<bool, FractionLong>> checkAndVotesPairs, FractionLong allowedVotesAmount)
        {
            if (checkAndVotesPairs.Where(p => p.Key == true).All(p => p.Value == FractionLong.Zero))
                return AmountOfStockSubmited.VotesArentSubmited;

            FractionLong value = FractionLong.Zero;

            checkAndVotesPairs.ForEach(pair =>
                                           {
                                               if (pair.Key == true) value += AmountOfStockCalculatorsCommon.GetVotesFor(pair.Value, allowedVotesAmount);
                                           });


            if (value <= allowedVotesAmount) return AmountOfStockSubmited.LessOrEqualThanThereIsOnPack;
            else return AmountOfStockSubmited.MoreThanThereIsOnPack;
        }
    }
}