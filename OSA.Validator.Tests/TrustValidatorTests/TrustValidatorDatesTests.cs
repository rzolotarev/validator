using System;
using NUnit.Framework;
using OSA.Configuration;
using OSA.Editor.ViewModels.Fields;
using OSA.Editor.ViewModels.TrustViewModel;

namespace OSA.Validator.Tests.TrustValidatorTests
{
    [TestFixture]
    public class TrustValidatorDatesTests : TrustValidatorTestsBase
    {
        [Test]
        [TestCase(11, 11, 11, ErrorLevel.Valid, "")]
        [TestCase(33, 11, 11, ErrorLevel.Error, "Некорректный формат даты")]
        [TestCase(11, 13, 11, ErrorLevel.Error, "Некорректный формат даты")]
        [TestCase(1, 3, 11, ErrorLevel.Valid, "")]
        public void DateTests(int day, int month, int year, ErrorLevel expectedErrorLevel, string expectedErrorText)
        {
            var trustStartDate = GetTrustDate(day, month, year);
            var trust = GetTrust(trustStartDate: trustStartDate);
            Validate(trust);

            AssertThatTrustDateHasSpecificErrorLevel(trustStartDate, expectedErrorLevel, expectedErrorText);

            var trustFinishDate = GetTrustFinishDate(day, month, year,trustStartDate);
            var trust2 = GetTrust(trustFinishDate: trustFinishDate);

            Validate(trust2);

            AssertThatTrustDateHasSpecificErrorLevel(trustFinishDate.CustomTrustDate, expectedErrorLevel, expectedErrorText);
        }

        [Test]
        [TestCase(1, ErrorLevel.Error, "Дата свершения доверенности должна быть не позднее сегодняшнего дня")]
        [TestCase(0, ErrorLevel.Valid, "")]
        [TestCase(-1, ErrorLevel.Valid, "")]
        public void TrustStartDateMustBeLessOrEqualToToday(int dayOffset, ErrorLevel expectedErrorLevel, string expectedErrorText)
        {
            var now = TimeServer.Now.Date;
            var trustStartDatetime = now.AddDays(dayOffset);
            var trustStartDate = GetTrustDate(trustStartDatetime);
            var trust = GetTrust(trustStartDate: trustStartDate);

            Validate(trust);

            AssertThatTrustDateHasSpecificErrorLevel(trustStartDate, expectedErrorLevel, expectedErrorText);
        }

        [Test]
        public void TrustStartDateMustBeLessOrEqualToTrustFinishDate()
        {
            const string expectedErrorText = "Дата свершения доверенности не может быть позднее даты окончания ее действия";

            var now = TimeServer.Now.Date;
            var yeasterday = now.AddDays(-1);
            var trustStartDate = GetTrustDate(now);
            var trustFinishDate = GetTrustFinishDate(yeasterday, trustStartDate);

            var trust = GetTrust(trustStartDate: trustStartDate, trustFinishDate: trustFinishDate);

            Validate(trust);

            AssertThatTrustDateHasSpecificErrorLevel(trustStartDate, ErrorLevel.Error, expectedErrorText);

            AssertThatTrustDateHasSpecificErrorLevel(trustFinishDate.CustomTrustDate, ErrorLevel.Error, expectedErrorText);
        }

        [Test]
        public void DateFormatIsCheckedBeforeCheckingConstraintsOnDates()
        {
            var now = TimeServer.Now.Date;
            var tomorrow = now.AddDays(1);
            var yesterday = now.AddDays(-1);
            var trustStartDate = GetTrustDate(tomorrow.Day, 13, tomorrow.Year);
            var trustFinishDate = GetTrustFinishDate(yesterday.Day, 13, yesterday.Year,trustStartDate);
            var trust = GetTrust(trustStartDate: trustStartDate, trustFinishDate: trustFinishDate);

            Validate(trust);

            AssertThatTrustDateHasSpecificErrorLevel(trustStartDate, ErrorLevel.Error, "Некорректный формат даты");
            AssertThatTrustDateHasSpecificErrorLevel(trustFinishDate.CustomTrustDate, ErrorLevel.Error, "Некорректный формат даты");
        }

        private static TrustDate GetTrustDate(DateTime trustStartDatetime)
        {
            var year = trustStartDatetime.Year;
            return GetTrustDate(trustStartDatetime.Day, trustStartDatetime.Month, year % 100);
        }

        private static TrustFinishDate GetTrustFinishDate(DateTime trustFinishDatetime, TrustDate trustStartTime)
        {
            var year = trustFinishDatetime.Year;
            return GetTrustFinishDate(trustFinishDatetime.Day, trustFinishDatetime.Month, year%100, trustStartTime);
        }

        private static void AssertThatTrustDateHasSpecificErrorLevel(TrustDate trustDate, ErrorLevel expectedErrorLevel, string expectedErrorText)
        {
            AssertThatFieldHasError(trustDate.Day, expectedErrorLevel, expectedErrorText);
            AssertThatFieldHasError(trustDate.Month, expectedErrorLevel, expectedErrorText);
            AssertThatFieldHasError(trustDate.Year, expectedErrorLevel, expectedErrorText);
        }

        private static void AssertThatFieldHasError(DocField field, ErrorLevel expectedErrorLevel, string expectedErrorText)
        {
            Assert.That(field.ErrorLevel, Is.EqualTo(expectedErrorLevel));
            Assert.That(field.ErrorText, Is.EqualTo(expectedErrorText));
        }
    }

    public static class ExtensionMethods
    {
        public static NumberDocField ToNumberField(this int i)
        {
            return Fields.NumberDocField(i);
        }
    }
}