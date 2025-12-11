using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class DespawnStrategy : IActionStrategy
    {
        public bool CanPerform => true;

        public bool Complete { get; private set; }

        private IDespawnable _despawnable;
        public DespawnStrategy(IDespawnable despawnable) 
        {
            _despawnable = despawnable;
        }
        public void Start()
        {
            _despawnable.Despawn();
            Complete = true;
        }
    }
}
