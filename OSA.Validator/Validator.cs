using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.GraphBuilding;
using OSA.Validator.Rules;

namespace OSA.Validator
{
    public class Validator
    {
        private readonly RuleNode _graphRootNode;
        private readonly GraphWalker _graphWalker = new GraphWalker();

        public BulletinScreenModelBase Bulletin { get; private set; }

        public Validator(BulletinScreenModelBase bulletin)
        {
            Bulletin = bulletin;
            _graphRootNode = GraphProvider.BuildGraph(bulletin);
        }

        public void Validate()
        {
            _graphWalker.WalkThrough(_graphRootNode, Bulletin);
        }
    }
}