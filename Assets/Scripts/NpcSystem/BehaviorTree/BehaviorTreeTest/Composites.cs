using System.Collections.Generic;

namespace BehaviorTreeTest
{
    public abstract class CompositeNode : BTNode
    {
        protected List<BTNode> _children = new();

        protected CompositeNode(List<BTNode> children)
        {
            foreach (var child in children)
            {
                AddChild(child);
            }
        }

        protected CompositeNode(Blackboard bb,List<BTNode> children)
        {
            Blackboard = bb;
            foreach (var child in children)
            {
                AddChild(child);
            }
        }

        public void AddChild(BTNode child)
        {
            _children.Add(child);
            child.Initialize(this, Blackboard);
        }
    }
    public class Sequence : CompositeNode
    {
        private int _index;

        public Sequence(List<BTNode> children) : base(children)
        {
        }
        public Sequence(Blackboard bb, List<BTNode> children) : base(bb,children)
        {
        }

        protected override void OnStart() => _index = 0;


        protected override NodeState OnUpdate()
        {
            while (_index < _children.Count)
            {
                var result = _children[_index].Tick();
                if (result != NodeState.Success)
                    return result;
                _index++;
            }
            return NodeState.Success;
        }
    }
    public class Selector : CompositeNode
    {
        private int _index;

        public Selector(List<BTNode> children) : base(children)
        {
        }
        public Selector(Blackboard bb, List<BTNode> children) : base(bb,children)
        {
        }

        protected override void OnStart() => _index = 0;


        protected override NodeState OnUpdate()
        {
            while (_index < _children.Count)
            {
                var result = _children[_index].Tick();
                if (result != NodeState.Failure)
                    return result;
                _index++;
            }
            return NodeState.Failure;
        }
    }
}