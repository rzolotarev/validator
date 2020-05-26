using OSA.Core.Entities.Registration;
using OSA.Core.Util.Extensions.FractionLongs;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.Fields;

namespace OSA.Validator.Tests
{
    public static class Fields
    {
        public static NumberDocField NumberDocField(int value)
        {
            return new NumberDocField(null, Page, value);
        }

        public static CheckBoxDocField CheckBoxDocField(bool value, Page page = null)
        {
            return new CheckBoxDocField(null, page ?? Page, value);
        }

        public static CheckBoxDocField CheckBoxDocField(bool value, ErrorLevel errorLevel)
        {
            return new CheckBoxDocField(null, Page, value){ErrorLevel =  errorLevel};
        }

        public static TextDocField TextBoxDocField(string value)
        {
            return new TextDocField(null, Page, value);
        }

        public static VotesDocField VotesDocField(FractionLong value, Page page = null)
        {
            return new VotesDocField(null, page ?? Page, value);
        }

        public static VotesDocField VotesDocField(FractionLong value, ErrorLevel errorLevel)
        {
            return new VotesDocField(null, Page, value) { ErrorLevel = errorLevel };
        }

        public static VotesDocField VotesDocField(int value)
        {
            return VotesDocField(value.FL());
        }

        private static Page Page
        {
            get
            {
                return new Page(1);
            }
        }
    }
}