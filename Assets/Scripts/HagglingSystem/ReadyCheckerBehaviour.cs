using UnityEngine;

public class ReadyCheckerBehaviour : MonoBehaviour{
   [SerializeField] private string _tag;
   [SerializeField] private HagglingManager _hagglingManager;

   private void OnTriggerEnter(Collider other){
      if (_tag.Equals("Player") && other.CompareTag("Player")){
         _hagglingManager.SetPlayerReadiness(true);
      }

      if (_tag.Equals("NPC") && other.CompareTag("NPC")){
         _hagglingManager.SetNpcReadiness(true);
         _hagglingManager.SetNpc(other.GetComponent<NpcBehaviour>());
      }
      
   }

   private void OnTriggerExit(Collider other){
      if (_tag.Equals("Player") && other.CompareTag("Player"))
         _hagglingManager.SetPlayerReadiness(false);
      if (_tag.Equals("NPC") && other.CompareTag("NPC")){
         _hagglingManager.SetNpcReadiness(false);
      }
   }
}
