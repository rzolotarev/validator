using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Validator.Rules.TablesRules.Enums;

namespace OSA.Validator.Rules
{
    public static class BulletinAdditionalMarksToAdditionalChecksEnumConverter
    {
        public static AdditionalChecks ConvertToAdditionalChecksEnum(BulletinAdditionalMarks marks)
        {
            var result = AdditionalChecks.None;

            if (marks.NotWholeStockWasPassed.Value == true)
            {
                result |= AdditionalChecks.NotWholeStockWasPassed;
            }
            if (marks.HasInstructions.Value == true)
            {
                result |= AdditionalChecks.HasInstructions;
            }
            if (marks.HasTrust.Value == true)
            {
                result |= AdditionalChecks.HasTrust;
            }
            return result;
        }
    }
}