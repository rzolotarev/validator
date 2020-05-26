using System.Collections.Generic;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules
{
    public class PacketMustBeRegisteredRule : RuleNode
    {
        public PacketMustBeRegisteredRule(CheckBoxDocField packetIsRegistered)
        {
            _packetIsRegistered = packetIsRegistered;
        }

        public CheckBoxDocField _packetIsRegistered { get; private set; }

        public override bool IsFulfiled
        {
            get { return _packetIsRegistered.IsFilled; }
        }

        public override bool ShouldGoFurtherDownTheGraph
        {
            get { return IsFulfiled; }
        }

        public override IList<DocField> DependsOn
        {
            get { return new List<DocField> { _packetIsRegistered }; }
        }

        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            return "Пакет не зарегистрирован.";
        }
    }
}