
using UnityEngine;

namespace BehaviorTreeTest
{
    public class BTree : MonoBehaviour
    {
        public BTNode Root;
        public Blackboard Blackboard = new();


        private void Update()
        {
            Root?.Tick();
        }
    }
}
