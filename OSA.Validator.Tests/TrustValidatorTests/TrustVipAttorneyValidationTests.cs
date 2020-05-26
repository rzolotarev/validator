using System.Linq;
using NUnit.Framework;
using OSA.Core.Util;
using OSA.Core.Util.Extensions.IEnumerables;
using OSA.Editor.ViewModels.Fields;
using OSA.Editor.ViewModels.TrustViewModel;

namespace OSA.Validator.Tests.TrustValidatorTests
{
    [TestFixture]
    public class TrustVipAttorneyValidationTests:TrustValidatorTestsBase
    {
        [Test]
        public void TrustBoxAndRecognizedAttorneysMatchTest()
        {
            var trust = GetTrust(attorney: GetPhysicalPerson("Представительный", "Представитель", "Представителич", "123456"),
                                 trustBoxAttorney: GetPhysicalVipAttorney("Представительный Представитель Представителич", "123456"));
            Validate(trust);

            AttorneyFieldsDontHaveErrors(trust);
        }

        [Test]
        public void TrustBoxAndRecognizedAttorneysDocumentNumberMismatchTest()
        {
            var trust = GetTrust(attorney: GetPhysicalPerson("Представительный", "Представитель", "Представителич", "123456"),
                                 trustBoxAttorney: GetPhysicalVipAttorney("Представительный Представитель Представителич", "654987"));
            Validate(trust);

            AttorneyFieldsHaveErrorWithText(trust, "Распознанный представитель не совпадает с ожидаемым (Представительный Представитель Представителич)");
        }

        [Test]
        public void TrustBoxAndRecognizedAttorneysMismatchTest()
        {
            var trust = GetTrust(attorney: GetPhysicalPerson("Представительный1", "Представитель", "Представителич", "123456"),
                                 trustBoxAttorney: GetPhysicalVipAttorney("Представительный Представитель Представителич", "123456"));
            Validate(trust);

            AttorneyFieldsHaveErrorWithText(trust, "Распознанный представитель не совпадает с ожидаемым (Представительный Представитель Представителич)");
        }

        [Test]
        public void TrustBoxAndRecognizedJurAttorneysMatchTest()
        {
            var trust = GetTrust(attorney: GetJuridicalPerson("Газ", "Ваз", "Маз"),
                                 trustBoxAttorney: GetJuridicalVipAttorney("Газ Ваз Маз"));
            Validate(trust);

            AttorneyFieldsDontHaveErrors(trust);
        }
        [Test]
        public void TrustBoxAndRecognizedJurAttorneysMismatchTest()
        {
            var trust = GetTrust(attorney: GetJuridicalPerson("1", "Ваз", "Маз"),
                                 trustBoxAttorney: GetJuridicalVipAttorney("Газ Ваз Маз"));
            Validate(trust);

            AttorneyFieldsHaveErrorWithText(trust, "Распознанный представитель не совпадает с ожидаемым (Газ Ваз Маз)");
        }

        private void AttorneyFieldsHaveErrorWithText(VipAttorneyRecognizedTrustScreenModel trust, string expectedErrorTextForAttorneyFields)
        {
            var fields = trust.Fields.Where(x => x.Parent == trust.RecognizedAttorney);
            #region Assertions
            Check.That(fields.Any());
            #endregion

            fields.ForEach(f => Assert.That(f.ErrorLevel, Is.EqualTo(ErrorLevel.Error)));
            fields.ForEach(f => Assert.That(f.ErrorText, Is.EqualTo(expectedErrorTextForAttorneyFields)));
        }

        private void AttorneyFieldsDontHaveErrors(VipAttorneyRecognizedTrustScreenModel trust)
        {
            var fields = trust.Fields.Where(x => x.Parent == trust.RecognizedAttorney);
            #region Assertions
            Check.That(fields.Any());
            #endregion

            fields.ForEach(f => Assert.That(f.ErrorLevel, Is.EqualTo(ErrorLevel.Valid)));
        }
    }
}
