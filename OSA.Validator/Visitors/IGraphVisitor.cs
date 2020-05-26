using OSA.Validator.Rules;

namespace OSA.Validator.Visitors
{
    public interface IGraphVisitor
    {
        void Visit(RuleNode rule);
        void Visit(ParallelRule rule);
    }
}