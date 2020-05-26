﻿using OSA.Validator.Rules;

namespace OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers.Rules
{
    public class PacketMustBeRegisteredRuleMarkuper : MarkAllDependantRuleMarkuper
    {
        public override bool CanMarkupRuleNodeOfType(RuleNode ruleNode)
        {
            return ruleNode is PacketMustBeRegisteredRule;
        }
    }
}