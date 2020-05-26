using System;
using System.Collections.Generic;
using System.Linq;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Separate;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Simple;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules;
using OSA.Validator.Rules.PageRules;
using OSA.Validator.Rules.QCumulativeRules;
using OSA.Validator.Rules.QSeparateRules;
using OSA.Validator.Rules.TablesRules;
using OSA.Validator.Rules.TablesRules.Enums;
using OSA.Validator.Rules.TablesRules.QCumulativeTableRules;
using OSA.Validator.Rules.TablesRules.QSimpleQSepAndHierSubQTableRules;

namespace OSA.Validator.GraphBuilding
{
    public static class GraphProvider
    {
 
        public static RuleNode BuildGraph(BulletinScreenModelBase bulletin)
        {
            return RuleBuilder
                    .RootRule(new PacketMustBeRegisteredRule(bulletin.PacketIsRegistered))
                                .AddParallelRule(bulletin.Signatures, 
                                                 signature => 
                                                  RuleBuilder.RootRule(new SignatureMustBePresentRule(signature))
                                                             .AddParallelRule(new OwnersCountSignatureWarningRule(signature, bulletin.OwnersCount), 
                                                                              GetQuestionsParallelRule(signature.AssociatedQuestions, bulletin))
                                                             .GetRoot())
                    .GetRoot();
        }

        private static RuleNode GetQuestionsParallelRule(List<Question> questions, BulletinScreenModelBase bulletin)
        {
            var questionRules = new List<RuleNode>();
            questionRules.AddRange(GetQSimpleParallelRules(questions, bulletin));
            questionRules.AddRange(GetQSeparateParallelRules(questions, bulletin));
            questionRules.AddRange(GetQCumulativeParallelRules(questions, bulletin));
            return RuleBuilder
                .ParallelRootRule(questionRules.ToArray())
                .GetRoot();
        }

        private static IEnumerable<RuleNode> GetQCumulativeParallelRules(List<Question> questions, BulletinScreenModelBase bulletin)
        {
            return 
                questions
                .OfType<QCumulative>()
                .Select(qCum =>
                            {
                                var allowedVotesAmount = qCum.GetTotalPossibleVotesSum();
                                var result =
                                    RuleBuilder
                                        .RootRule(new QCumulativeMustHaveAtLeastOneSelectionRule(qCum))
                                            .AddRuleChain(GetQCumulativeTableRule(bulletin, qCum, allowedVotesAmount))
                                                .AddRule(new QCumulativeFractionDistributionRule(qCum, () => qCum.AllowedVotesAmountFractions.Select(x => x * (int)qCum.PositionsCount).ToList(), () => BulletinAdditionalMarksToAdditionalChecksEnumConverter.ConvertToAdditionalChecksEnum(bulletin.AdditionalMarks), bulletin.IsADRBulletin, () => bulletin.AdditionalMarks.HasInstructions.Value)) //todo: move calculation somewhere
                                                    .AddRule(new AdrMustHaveExactVotesSpreadedWithMinimalSpredForQCum(qCum, () => allowedVotesAmount, bulletin.IsADRBulletin))
                                                        .AddRule(new QCumulativeWarningRule(qCum, bulletin.AdditionalMarks, () => allowedVotesAmount))
                                        .GetRoot();
                                return result;
                            });
        }

        private static RuleNode GetQCumulativeTableRule(BulletinScreenModelBase bulletin, QCumulative qCum, FractionLong allowedVotesAmount)
        {
            return GetQCumulativeTableRule(qCum, bulletin.AdditionalMarks, () => bulletin.PackStatus, () => bulletin.TrustExists, () => AmountOfStockSubmitedCalculator.GetAmountOfStockSubmited(qCum, allowedVotesAmount), () => GetNumberOfChecks(qCum), () => BulletinAdditionalMarksToAdditionalChecksEnumConverter.ConvertToAdditionalChecksEnum(bulletin.AdditionalMarks), () => qCum.AdditionalMarks.YesCheckBoxField.Value ? CumYesIs.Checked : CumYesIs.NotChecked);
        }

        private static IEnumerable<RuleNode> GetQSeparateParallelRules(List<Question> questions, BulletinScreenModelBase bulletin)
        {
            return questions
                    .OfType<QSeparate>().ToList()
                    .Select(qsep =>
                            RuleBuilder
                                .RootRule(new ParallelRule(qsep.CandidatePoints.Select(cp =>
                                                           RuleBuilder
                                                               .RootRuleChain(GetRuleForBaseYesNoAbstainedPoint(cp, bulletin, qsep.AmountOfStock))
                                                                   .AddRule(new QSimpleQSepAndHierSubQFractionDistributionRule(cp, () => qsep.AllowedVotesAmountFractions, bulletin.IsADRBulletin, () => bulletin.AdditionalMarks.HasInstructions.Value))
                                                                       .AddRule(new QSimpleQSepAndHierSubQWarningRule(cp, bulletin.AdditionalMarks, () => qsep.AmountOfStock))

                                                           .GetRoot()).ToList(),
                                                           shouldGoFurtherDownTheGraphAnyway: true))
                                         .AddRule(new QSeparateYesChecksShouldNotExceedPlacesCountRule(qsep, bulletin.AdditionalMarks, () => qsep.AmountOfStock))
                                            .AddRule(new AdrMustHaveExactOrLessVotesSpreadedForQSep(qsep, () => qsep.AmountOfStock, bulletin.IsADRBulletin))
                                .GetRoot()
                    );
        }

        private static List<RuleNode> GetQSimpleParallelRules(List<Question> questions, BulletinScreenModelBase bulletin)
        {
            return questions
                .OfType<QSimple>()
                .ToList()
                .Select(q =>
                        RuleBuilder
                            .RootRuleChain(GetRuleForBaseYesNoAbstainedPoint(q.Point, bulletin, q.AmountOfStock, isQSimpWithPrivDividends: q.IsQuestionAboutPrivilegeDividends.Value && bulletin.HasPrivilegeVotes))
                                .AddRule(new QSimpleQSepAndHierSubQFractionDistributionRule(q.Point, () => q.AllowedVotesAmountFractions, bulletin.IsADRBulletin, () => bulletin.AdditionalMarks.HasInstructions.Value))
                                    .AddRule(new AdrMustHaveExactVotesSpreadedForQSimple(q.Point, () => q.AmountOfStock, bulletin.IsADRBulletin))
                                        .AddRule(new QSimpleQSepAndHierSubQWarningRule(q.Point, bulletin.AdditionalMarks, () => q.AmountOfStock))
                                            .AddRule(new QSimplePrivilegeDividendsNoAbsWarningRule(q, bulletin.HasPrivilegeVotes))
                            .GetRoot()
                )
                .ToList();
        }

        private static RuleNode GetRuleForBaseYesNoAbstainedPoint(BaseYesNoAbsPoint qsp, BulletinScreenModelBase bulletin, FractionLong amountOfStock, bool isQSimpWithPrivDividends = false)
        {
            return RuleBuilder.
                RootRule(new PointShouldHaveAtLeastOneSelectionRule(qsp))
                    .AddRule(GetQSimpleQSepAndHierSubQTableRuleChain(qsp, bulletin, amountOfStock, isQSimpWithPrivDividends))
                .GetRoot();
        }

        private static RuleNode GetQSimpleQSepAndHierSubQTableRuleChain(BaseYesNoAbsPoint point, BulletinScreenModelBase bulletin, FractionLong amountOfStock, bool isQSimpWithPrivDividends = false)
        {
            var additionalMarks = bulletin.AdditionalMarks;
            Func<PackStatus> packStatus = () => bulletin.PackStatus;
            Func<NumberOfChecks> numberOfChecks = () => GetNumberOfChecks(point);
            Func<AdditionalChecks> additionalChecks = () => BulletinAdditionalMarksToAdditionalChecksEnumConverter.ConvertToAdditionalChecksEnum(bulletin.AdditionalMarks);
            Func<bool> trustExists = () => bulletin.TrustExists;
            Func<AmountOfStockSubmited> amountOfStockSubmited = () => AmountOfStockSubmitedCalculator.GetAmountOfStockSubmited(point, amountOfStock);
            Func<bool> isQSimpWithPrivDividendsFunc = () => isQSimpWithPrivDividends;
            return GetQSimpleQSepAndHierSubQTableRuleChain(point, additionalMarks, packStatus, trustExists, amountOfStockSubmited, numberOfChecks, additionalChecks, isQSimpWithPrivDividendsFunc);
        }

        public static RuleNode GetQSimpleQSepAndHierSubQTableRuleChain(BaseYesNoAbsPoint point, BulletinAdditionalMarks additionalMarks, Func<PackStatus> packStatus, Func<bool> trustExists, Func<AmountOfStockSubmited> amountOfStockSubmited, Func<NumberOfChecks> numberOfChecks, Func<AdditionalChecks> additionalChecks, Func<bool> isQSimpWithPrivDividends)
        {
            return RuleBuilder
                .RootRule(new InCaseOfSimplePackPointWithNoAdditionalMarksAlwaysPassesRule(point, additionalMarks, packStatus, numberOfChecks, additionalChecks, isQSimpWithPrivDividends))
                    .AddRule(new MultivariantVotingPointRule(point, additionalMarks, numberOfChecks, additionalChecks))
                        .AddRule(new QSimpleQSepAndHierSubQTableRule(point, additionalMarks, packStatus, trustExists, amountOfStockSubmited, numberOfChecks, additionalChecks))
                .GetRoot();
        }

        public static RuleNode GetQCumulativeTableRule(QCumulative qCumulative,
                                                       BulletinAdditionalMarks additionalMarks,
                                                       Func<PackStatus> packStatus,
                                                       Func<bool> trustExists,
                                                       Func<AmountOfStockSubmited> amountOfStockSubmited,
                                                       Func<NumberOfChecks> numberOfChecks,
                                                       Func<AdditionalChecks> additionalChecks,
                                                       Func<CumYesIs> cumChecks)
        {
            return RuleBuilder
                .RootRule(new InCaseOfSimplePackQCumulativeWithNoAdditionalMarksAlwaysPassesRule(qCumulative, additionalMarks, packStatus, amountOfStockSubmited, numberOfChecks, additionalChecks, cumChecks))
                    .AddRule(new MultivariantVotingQCumulativeRule(qCumulative, additionalMarks, numberOfChecks, additionalChecks))
                        .AddRule(new QCumulativeTableRule(qCumulative, additionalMarks, packStatus, trustExists, amountOfStockSubmited, numberOfChecks, additionalChecks, cumChecks))
            .GetRoot();
        }

        private static readonly AmountOfStockSubmitedCalculator AmountOfStockSubmitedCalculator = new AmountOfStockSubmitedCalculator();

        private static NumberOfChecks GetNumberOfChecks(BaseYesNoAbsPoint point)
        {
            var checks = new List<CheckBoxDocField> { point.YesCheckBoxField, point.NoCheckBoxField, point.AbstainedCheckBoxField };
            if (checks.All(c => c.Value == false)) throw new ArgumentOutOfRangeException(string.Empty, "�� ����� �� ����� �����");

            if (checks.Where(c => c.Value == true).Count() == 1) return NumberOfChecks.Single;
            else return NumberOfChecks.Multiple;
        }

        private static NumberOfChecks GetNumberOfChecks(QCumulative qCum)
        {
            var checks = new List<CheckBoxDocField>
                             {
                                 qCum.AdditionalMarks.YesCheckBoxField,
                                 qCum.AdditionalMarks.NoCheckBoxField,
                                 qCum.AdditionalMarks.AbstainedCheckBoxField
                             };

            if (checks.All(c => c.Value == false)) throw new ArgumentOutOfRangeException(string.Empty, "�� ����� �� ����� �����");

            if (checks.Where(c => c.Value == true).Count() == 1) return NumberOfChecks.Single;
            else return NumberOfChecks.Multiple;
        }
    }
}