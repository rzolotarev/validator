using System.Collections.Generic;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules.PageRules
{
    public class SignatureMustBePresentRule : RuleNode
    {
        public SignatureMustBePresentRule(Signature signature)
        {
            Signature = signature;
        }

        public Signature Signature { get; private set; }

        public override bool IsFulfiled
        {
            get { return Signature.SignatureCheckBox.Value; }
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
            return ErrorTexts.PAGE_MUST_HAVE_SIGNATURE;
        }
    }
}