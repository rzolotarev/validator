using System;
using OSA.Configuration;
using OSA.Core.Entities.Registration;
using OSA.Core.Util;
using OSA.Core.Util.Extensions.IEnumerables;
using OSA.Editor.ViewModels;
using OSA.Editor.ViewModels.Fields;
using OSA.Editor.ViewModels.TrustViewModel;

namespace OSA.Validator.TrustValidation
{
    public class TrustValidator
    {
        public void ValidateMainTrustFields(VipAttorneyRecognizedTrustScreenModel trust)
        {
            ResetFields(trust);
            ValidateDateAndSignatureFields(trust);
        }

        public void ValidateEditorModel<T>(T model) where T : TrustScreenModelBase
        {
            var m = (TrustScreenModelBase)model;
            if (model is VipAttorneyRecognizedTrustScreenModel) ValidateEditorModel1((VipAttorneyRecognizedTrustScreenModel)m);
            else if (model is VipAttorneyNonTypicalTrustScreenModel) ValidateEditorModel1((VipAttorneyNonTypicalTrustScreenModel)m);
            else if (model is ManualTrustScreenModel) ValidateEditorModel1((ManualTrustScreenModel)m);
            else if (model is RegistrationTrustScreenModel) ValidateEditorModel1((RegistrationTrustScreenModel)m);
            else throw new NotSupportedException("Тип " + model.GetType() + " не поддерживается.");
        }

        private void ValidateEditorModel1(VipAttorneyRecognizedTrustScreenModel model)
        {
            ResetFields(model);
            ValidateDateAndSignatureFields(model);
            ValidateAttorney(model);
        }

        private void ValidateEditorModel1(VipAttorneyNonTypicalTrustScreenModel model)
        {
            ResetFields(model);
            ValidateDateFields(model);
        }

        private void ValidateEditorModel1(ManualTrustScreenModel model)
        {
            ResetFields(model);
            ValidateDateFields(model);
        }

        private void ValidateEditorModel1(RegistrationTrustScreenModel model)
        {
            ResetFields(model);
            ValidateDateFields(model);
        }


        private void ValidateAttorney(VipAttorneyRecognizedTrustScreenModel model)
        {
            if (model.VipAttorney.Value == null ||
                (!model.VipAttorneyWasManuallySet &&
                    (model.RecognizedAttorney.ResultName != model.VipAttorney.Value.Attorney.PersonInfo.Name ||
                    DocumentNumbersDontMatch(model))
                ))
            {
                model.RecognizedAttorney.Fields.ForEach(f =>
                    {
                        f.ErrorLevel = ErrorLevel.Error;
                        f.ErrorText = "Распознанный представитель не совпадает с ожидаемым"
                            + (model.VipAttorney != null ? " (" + model.VipAttorney.Value.Attorney.PersonInfo.Name + ")" : "");
                    });
            }
        }

        private bool DocumentNumbersDontMatch(VipAttorneyRecognizedTrustScreenModel model)
        {
            var physAttorney = model.RecognizedAttorney as PhysicalPerson;
            var physBoxAttorney = model.VipAttorney.Value.Attorney.PersonInfo as PhysicalPersonInfo;
            if (physAttorney != null && physBoxAttorney != null)
            {
                return physAttorney.DocumentNumber.Value != physBoxAttorney.DocumentNumber;
            }
            return false;
        }

        private static void ValidateDateAndSignatureFields(VipAttorneyRecognizedTrustScreenModel trust)
        {
            ValidateDateFields(trust);

            SignatureFieldsMustBePresent(trust);
        }

        private static void ValidateDateFields(TrustScreenModelBase trust)
        {
            DateMustBeCorrect(trust.TrustStartDate);
            TrustStartDateMustBeNoGreaterThanToday(trust);

            switch (trust.TrustFinishDate.Type.Value)
            {
                case TrustFinishDate.TrustFinishDateType.Perpetual:
                    {
                        trust.TrustFinishDate.CustomTrustDate.Day.ErrorLevel = ErrorLevel.WasntChecked;
                        trust.TrustFinishDate.CustomTrustDate.Month.ErrorLevel = ErrorLevel.WasntChecked;
                        trust.TrustFinishDate.CustomTrustDate.Year.ErrorLevel = ErrorLevel.WasntChecked;

                        trust.TrustFinishDate.CustomNYears.ErrorLevel = ErrorLevel.WasntChecked;
                        break;
                    }
                case TrustFinishDate.TrustFinishDateType.NYears:
                    {
                        trust.TrustFinishDate.CustomTrustDate.Day.ErrorLevel = ErrorLevel.WasntChecked;
                        trust.TrustFinishDate.CustomTrustDate.Month.ErrorLevel = ErrorLevel.WasntChecked;
                        trust.TrustFinishDate.CustomTrustDate.Year.ErrorLevel = ErrorLevel.WasntChecked;

                        TrustStartDateMustBeLessOrEqualToTrustFinishDate(trust);
                        break;
                    }
                case TrustFinishDate.TrustFinishDateType.Custom:
                    {
                        DateMustBeCorrect(trust.TrustFinishDate.CustomTrustDate);
                        TrustStartDateMustBeLessOrEqualToTrustFinishDate(trust);

                        trust.TrustFinishDate.CustomNYears.ErrorLevel = ErrorLevel.WasntChecked;
                        break;
                    }
                default:
                    throw new NotSupportedException("Тип " + trust.TrustFinishDate.Type.Value + " не поддерживается.");
            }
        }

        private static void SignatureFieldsMustBePresent(VipAttorneyRecognizedTrustScreenModel trust)
        {
            ValidateMandatoryBoolField(trust.TrusterSignature, "Отсутвствует подпись доверителя.");
            ValidateMandatoryBoolField(trust.CertifierSignature, "Отсутвствует подпись удостоверителя.");
            ValidateMandatoryBoolField(trust.CertifierStamp, "Отсутвствует печать удостоверителя.");
        }

        private static void ValidateMandatoryBoolField(CheckBoxDocField checkBoxDocField, string отсутвствуетПодписьДоверителя)
        {
            if (checkBoxDocField.Value == false)
            {
                checkBoxDocField.ErrorLevel = ErrorLevel.Error;
                checkBoxDocField.ErrorText = отсутвствуетПодписьДоверителя;
            }
        }


        private static void ResetFields(ITaskViewModel trust)
        {
            trust.Fields.ForEach(f => f.ResetErrorsAndWarnings());
        }

        private static void TrustStartDateMustBeNoGreaterThanToday(TrustScreenModelBase trust)
        {
            if (trust.TrustStartDate.HasErrors()) return;

            DateTime trustStartDate;
            var parsed = TryToParseDate(trust.TrustStartDate, out trustStartDate);
            #region Assertions
            Check.That(parsed);
            #endregion
            if (trustStartDate.Date > TimeServer.Now.Date)
            {
                const string errorText = "Дата свершения доверенности должна быть не позднее сегодняшнего дня";
                trust.TrustStartDate.RaiseError(errorText);
            }
        }

        private static void TrustStartDateMustBeLessOrEqualToTrustFinishDate(TrustScreenModelBase trust)
        {
            if (trust.TrustStartDate.HasErrors()) return;

            var trustFinishDateType = trust.TrustFinishDate.Type.Value;

            if (trustFinishDateType == TrustFinishDate.TrustFinishDateType.Custom && trust.TrustFinishDate.CustomTrustDate.HasErrors()) return;

            DateTime trustStartDate;
            DateTime trustFinishDate;
            GetValidDatesFrom(trust, out trustStartDate, out trustFinishDate);


            if (trustStartDate.Date > trustFinishDate.Date)
            {
                const string errorText = "Дата свершения доверенности не может быть позднее даты окончания ее действия";
                trust.TrustStartDate.RaiseError(errorText);

                if (trustFinishDateType == TrustFinishDate.TrustFinishDateType.Custom)
                {
                    trust.TrustFinishDate.CustomTrustDate.RaiseError(errorText);
                }
                else if (trustFinishDateType == TrustFinishDate.TrustFinishDateType.NYears)
                {
                    var customNYears = trust.TrustFinishDate.CustomNYears;
                    customNYears.ErrorLevel = ErrorLevel.Error;
                    customNYears.ErrorText = errorText;
                }
            }
        }


        private static void GetValidDatesFrom(TrustScreenModelBase trust, out DateTime trustStartDate, out DateTime trustFinishDate)
        {
            var parsed1 = TryToParseDate(trust.TrustStartDate, out trustStartDate);
            trustFinishDate = trust.TrustFinishDate.ResultDateTime.Value.Value;

            #region Assertions
            Check.That(parsed1);
            #endregion
        }


        private static void DateMustBeCorrect(TrustDate trustDate)
        {
            const string errorText = "Некорректный формат даты";

            DateTime result;
            var parsed = TryToParseDate(trustDate, out result);
            if (!parsed) trustDate.RaiseError(errorText);

        }

        private static bool TryToParseDate(TrustDate trustDate, out DateTime result)
        {
            var parsedTrustDate = trustDate.ToDateTime();
            result = parsedTrustDate != null ? parsedTrustDate.Value : default(DateTime);
            return parsedTrustDate != null;
        }
    }

    public static class TrustValidatorExtensionMethods
    {
        public static void RaiseError(this TrustDate trustDate, string errorText)
        {
            Action<NumberDocField> setError = field =>
            {
                field.ErrorLevel = ErrorLevel.Error;
                field.ErrorText = errorText;
            };

            setError(trustDate.Day);
            setError(trustDate.Month);
            setError(trustDate.Year);
        }

        public static void ResetErrorsAndWarnings(this TrustDate trustDate)
        {
            trustDate.Day.ResetErrorsAndWarnings();
            trustDate.Month.ResetErrorsAndWarnings();
            trustDate.Year.ResetErrorsAndWarnings();
        }

        public static void ResetErrorsAndWarnings(this Person person)
        {
            person.Fields.ForEach(ResetErrorsAndWarnings);
        }

        public static void ResetErrorsAndWarnings(this DocField field)
        {
            field.ErrorLevel = ErrorLevel.Valid;
            field.ErrorText = "";
        }

        public static bool HasErrors(this TrustDate trustDate)
        {
            return trustDate.Day.HasErrors() ||
                   trustDate.Month.HasErrors() ||
                   trustDate.Year.HasErrors();
        }

        public static bool HasErrors(this DocField field)
        {
            return field.ErrorLevel == ErrorLevel.Error;
        }
    }
}
