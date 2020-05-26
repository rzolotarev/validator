using System.Collections.Generic;
using OSA.Core.Util;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules.PageRules
{
    public class OwnersCountSignatureWarningRule : RuleNode
    {
        public OwnersCountSignatureWarningRule(Signature signature, int ownersCount)
        {
            #region Preconditions
            Check.That(ownersCount>0);
            #endregion
            Signature = signature;
            OwnersCount = ownersCount;
        }

        public Signature Signature { get; private set; }
        public int OwnersCount { get; private set; }

        public override bool IsFulfiled
        {
            get { return OwnersCount == 1; }
        }

        public override bool ShouldGoFurtherDownTheGraph
        {
            get { return IsFulfiled; }
        }

        public override IList<DocField> DependsOn
        {
            get { return new List<DocField> { Signature.SignatureCheckBox }; }
        }

        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            var placeholder = ErrorTexts.CHECK_COONWERS_SIGNATURES_PLACEHOLDER;
            var value = OwnersCount.ToString();
            var template = ErrorTexts.CHECK_COOWNERS_SIGNATURES;
            return template.Replace(placeholder, value);
        }
    }
}