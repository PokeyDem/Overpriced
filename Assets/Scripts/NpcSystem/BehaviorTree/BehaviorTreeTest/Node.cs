
//using System.Collections.Generic;
//namespace BehaviorTreeTest
//{
//    public enum NodeState
//    {
//        RUNNING,
//        SUCCESS,
//        FAILURE,
//        RESTART
//    }
//    public class Node
//    {
//        protected NodeState state;

//        public Node parent;
//        protected List<Node> children = new List<Node>();

//        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

//        public Node()
//        {
//            parent = null;
//        }
//        public Node(List<Node> children)
//        {
//            foreach (Node child in children)
//            {
//                _Attach(child);
//            }
//        }

//        private void _Attach(Node node)
//        {
//            node.parent = this;
//            children.Add(node);
//        }
//        public virtual NodeState Evaluate() => NodeState.FAILURE;

//        public void SetData(string key, object value)
//        {
//            _dataContext[key] = value;
//        }

//        public object GetData(string key)
//        {
//            Node node = this;
//            while (node != null)
//            {
//                if (node._dataContext.TryGetValue(key, out var value))
//                    return value;
//                node = node.parent;
//            }
//            return null;
//        }
//        //public void SetData(string key, object value)
//        //{
//        //    if (parent != null)
//        //    {
//        //        parent.SetData(key, value);
//        //    }
//        //    else
//        //        _dataContext[key] = value;
//        //}

//        //public object GetData(string key)
//        //{
//        //    if (parent != null)
//        //    {
//        //        return parent.GetData(key);
//        //    }
//        //    if (_dataContext.TryGetValue(key, out var value))
//        //        return value;
//        //    return null;
//        //}
//        public bool ClearData(string key)
//        {
//            if (_dataContext.Remove(key))
//                return true;

//            if (parent != null)
//                return parent.ClearData(key);

//            return false;
//        }
//    }

//}
