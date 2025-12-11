using System.Collections.Generic;
namespace BehaviorTreeTest
{
    public class GeneratePossibleItemChoicesAction : BTNode
    {
        IHasDisplayChoices _context;
        public GeneratePossibleItemChoicesAction(IHasDisplayChoices context)
        {
            _context = context;
        }


        protected override NodeState OnUpdate()
        {
            _context.PossibleDisplayChoices = new List<DisplayContext>(DisplaysWithItemsListHandler.Instance.GetDisplaySlotsWithItems());
            return NodeState.Success;
        }
    }
}
