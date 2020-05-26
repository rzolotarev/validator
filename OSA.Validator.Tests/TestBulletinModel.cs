using System.Collections.Generic;
using System.Collections.ObjectModel;
using OSA.Core.Entities.Registration;
using OSA.Core.Util.Extensions.FractionLongs;
using OSA.Core.Util.Extensions.Int;
using OSA.Editor.ViewModels;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Separate;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Simple;

namespace OSA.Validator.Tests
{
    public static class TestBulletinModel
    {
        public static ManualBulletinScreenModel Create(List<Question> questions, List<Signature> signatures = null, FractionLong? allowedVotesAmount = null, PackStatus packStatus = null, bool trustExists = false, bool packetIsRegistered = true)
        {
            if (packStatus == null) packStatus = PackStatus.Simple;

            var additionalMarks = GetAdditionalMarks();

            var packetIsRegisteredCheckBoxField = Fields.CheckBoxDocField(packetIsRegistered);

            const int doesntMatter = 1;
            const bool doesntMatter2 = false;
            var doesntMatter3 = new List<int>();
            var doesntMatter4 = string.Empty;
            var doesntMatter5 = EditorMode.BulletinsOperator;
            int doesntMatter6 = 1;

            if (signatures == null) signatures = new List<Signature>();

            var result = new ManualBulletinScreenModel(new ObservableCollection<Question>(questions), signatures, additionalMarks, doesntMatter, doesntMatter3,
                                                      doesntMatter4,
                                                     packetIsRegisteredCheckBoxField, doesntMatter2,
                                                     doesntMatter5, doesntMatter2, doesntMatter6, packStatus,
                                                     trustExists, false);
            result.Questions.ForEach(q => q.AllowedVotesAmountFractions = new List<FractionLong> { allowedVotesAmount ?? 0.FL() });
            return result;
        }

        public static BulletinAdditionalMarks GetAdditionalMarks(bool? hasInstructions = null, bool? hasTrust = null, bool? notWholeStockWasPassed = null)
        {
            return new BulletinAdditionalMarks(Fields.CheckBoxDocField(hasInstructions ?? false), Fields.CheckBoxDocField(hasTrust ?? false), Fields.CheckBoxDocField(notWholeStockWasPassed ?? false));
        }


        public static QSimple GetFreshQSimple(Page page)
        {
            var qSimple = new QSimple(1, GetFreshBaseYesNoAbsPoint(page));
            qSimple.IsQuestionAboutPrivilegeDividends = false;
            return qSimple;
        }

        public static QCumulative GetFreshQCumulative(Page page)
        {
            var points = new List<QCumCandidatePoint>();
            10.Times(i => points.Add(new QCumCandidatePoint(i + 1, Fields.VotesDocField(0.FL(), page))));
            return new QCumulative(1,
                                   points,
                                   new QCumulativeAdditionalMarks(Fields.CheckBoxDocField(false, page),
                                                                  Fields.CheckBoxDocField(false, page),
                                                                  Fields.VotesDocField(0.FL(), page),
                                                                  Fields.CheckBoxDocField(false, page),
                                                                  Fields.VotesDocField(0.FL(), page)))
            {
                PositionsCount = 5
            };
        }

        public static QSeparate GetFreshQSeparate(Page page)
        {
            var points = new List<QSepCandidatePoint>();
            10.Times(() => points.Add(GetFreshQSepCandidatePoint(page)));
            return new QSeparate(1, points) { PositionsCount = 5 };
        }

        public static QSepCandidatePoint GetFreshQSepCandidatePoint(Page page)
        {
            return new QSepCandidatePoint(1, GetFreshBaseYesNoAbsPoint(page));
        }

        public static BaseYesNoAbsPoint GetFreshBaseYesNoAbsPoint(Page page = null, FractionLong? yesVotes = null, bool? yesCheck = null)
        {
            return new BaseYesNoAbsPoint(Fields.CheckBoxDocField(yesCheck ?? false, page),
                                         Fields.CheckBoxDocField(false, page),
                                         Fields.CheckBoxDocField(false, page),
                                         Fields.VotesDocField(yesVotes ?? 0.FL(), page),
                                         Fields.VotesDocField(0.FL(), page),
                                         Fields.VotesDocField(0.FL(), page));
        }

    }
}
