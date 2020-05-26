using System;
using System.Collections.Generic;
using System.Linq;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels;
using OSA.Editor.ViewModels.Fields;
using OSA.Editor.ViewModels.TrustViewModel;
using OSA.Validator.TrustValidation;

namespace OSA.Validator.Tests.TrustValidatorTests
{
    public class TrustValidatorTestsBase
    {
        private readonly TrustValidator _trustValidator = new TrustValidator();

        protected static TrustFinishDate GetTrustFinishDate(int day, int month, int year, TrustDate trustStartDate)
        {
            return new TrustFinishDate(GetTrustDate(day, month, year), trustStartDate, DefaultNumberDocField(), TrustFinishDate.TrustFinishDateType.Custom);
        }

        protected static TrustDate GetTrustDate(int day, int month, int year)
        {
            return new TrustDate(day.ToNumberField(), month.ToNumberField(), year.ToNumberField());
        }

        protected PhysicalPerson GetPhysicalPerson(string lastName, string firstName, string middleName, string documentNumber)
        {
            return new PhysicalPerson(GetTextBox(lastName), GetTextBox(firstName), GetTextBox(middleName), GetTextBox(documentNumber));
        }

        protected JuridicalPerson GetJuridicalPerson(params string[] titleFields)
        {
            return new JuridicalPerson(titleFields.Select(GetTextBox).ToList());
        }

        protected VipAttorneyRecognizedTrustScreenModel GetTrust(PhysicalPerson truster = null, Person attorney = null, VipAttorney trustBoxAttorney = null,  TrustFinishDate trustFinishDate = null, TrustDate trustStartDate = null, CheckBoxDocField trusterSignature = null, CheckBoxDocField certifierSignature = null, CheckBoxDocField certifierStamp = null)
        {
            var doesntMatter = EditorMode.TrustsOperator;
            var startDate = trustStartDate ?? DefaultDate();
            return new VipAttorneyRecognizedTrustScreenModel(truster ?? DefaultPerson(),
                             attorney ?? DefaultPerson(), 
                             trustBoxAttorney ?? GetDefaultVipAttorney(),
                             startDate,
                             trustFinishDate ?? (new TrustFinishDate(trustStartDate != null ? GetTrustDate(trustStartDate.Day.Value, trustStartDate.Month.Value, trustStartDate.Year.Value + 1) : DefaultDate(), startDate, DefaultNumberDocField(), TrustFinishDate.TrustFinishDateType.Custom)),
                                                                         
                             trusterSignature ?? DefaultCheckbox(),
                             certifierSignature ?? DefaultCheckbox(),
                             certifierStamp ?? DefaultCheckbox(),
                             doesntMatter,
                             new List<VipAttorney>(),
                             true);
        }

        private static VipAttorney GetDefaultVipAttorney()
        {
            return GetPhysicalVipAttorney("", "");
        }

        protected static VipAttorney GetPhysicalVipAttorney(string fio, string documentNumber)
        {
            var personInfo = new PhysicalPersonInfo(DateTime.Now, fio, documentNumber);
            return GetlVipAttorneyWith(personInfo);
        }

        protected static VipAttorney GetJuridicalVipAttorney(string title)
        {
            var personInfo = new JuridicalPersonInfo(DateTime.Now, title);
            return GetlVipAttorneyWith(personInfo);
        }

        private static VipAttorney GetlVipAttorneyWith(PersonInfo personInfo)
        {
            var attorney = new Attorney(personInfo);
            var vipAttorney = attorney.CreateVip(1, new Organization("1", 1, 1));
            return vipAttorney;
        }

        protected void Validate(VipAttorneyRecognizedTrustScreenModel trust)
        {
            _trustValidator.ValidateEditorModel(trust);
        }

        private static CheckBoxDocField DefaultCheckbox()
        {
            return Fields.CheckBoxDocField(true);
        }

        private static TrustDate DefaultDate()
        {
            return new TrustDate(11.ToNumberField(), 11.ToNumberField(), 19.ToNumberField());
        }

        private static PhysicalPerson DefaultPerson()
        {
            return new PhysicalPerson(GetTextBox("Vasin"), GetTextBox("Vasya"), GetTextBox("Vasil'evich"), GetTextBox("123412"));
        }


        private static NumberDocField DefaultNumberDocField()
        {
            return 0.ToNumberField();
        }

        private static TextDocField GetTextBox(string str)
        {
            return Fields.TextBoxDocField(str);
        }
    }
}