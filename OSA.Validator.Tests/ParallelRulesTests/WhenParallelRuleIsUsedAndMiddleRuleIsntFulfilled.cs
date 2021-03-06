using System.Collections.Generic;
using NUnit.Framework;
using OSA.Validator.Rules;
using Rhino.Mocks;

namespace OSA.Validator.Tests.ParallelRulesTests
{
    [TestFixture]
    public class WhenParallelRuleIsUsedAndMiddleRuleIsntFulfilled
    {
        private MockRepository _mocks;
        private bool _result;

        [TestFixtureSetUp]
        public void TextFixtureSetUp()
        {
            _mocks = new MockRepository();
            var rule1 = _mocks.StrictMock<RuleNode>();
            var rule2 = _mocks.StrictMock<RuleNode>();
            var rule3 = _mocks.StrictMock<RuleNode>();

            rule1.Expect(x => x.IsFulfiled).Return(true);
            rule2.Expect(x => x.IsFulfiled).Return(false);

            var parallelRule = new ParallelRule(new List<RuleNode> { rule1, rule2, rule3 });

            _mocks.ReplayAll();

            _result = parallelRule.IsFulfiled;
        }

        [Test]
        public void Result_will_be_negative()
        {
            Assert.That(_result, Is.EqualTo(false));
        }
    }
}