namespace OSA.Validator.Rules.TablesRules.HelperClasses
{
    public class ValidationResult
    {
        private ValidationResult(bool isFullfilled, string errorText)
        {
            IsFullfilled = isFullfilled;
            ErrorText = errorText;
        }

        public bool IsFullfilled { get; private set; }
        public string ErrorText { get; private set; }

        public static ValidationResult Fullfilled
        {
            get { return new ValidationResult(true, null); }
        }

        public static ValidationResult NotFullfilled(string errorText)
        {
            return new ValidationResult(false, errorText);
        }
    }
}
