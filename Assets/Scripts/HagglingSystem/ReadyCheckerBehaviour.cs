using TMPro;
using UnityEngine;

public class ReadyCheckerBehaviour : MonoBehaviour{
    [SerializeField] private string _tag;
    [SerializeField] private HagglingManager _hagglingManager;
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private GameObject _playerTrigger;


    private void OnTriggerEnter(Collider other){
      if (_tag.Equals("Player") && other.CompareTag("Player")){
            _hagglingManager.SetPlayerReadiness(true);
      }

      if (_tag.Equals("NPC") && other.CompareTag("NPC")){
            _hagglingManager.SetNpc(other.GetComponent<NpcBehaviour>()); //changed order of methods
            _hagglingManager.SetNpcReadiness(true);
            _playerTrigger.SetActive(true);
            _text.gameObject.SetActive(true);
      }
   }

   private void OnTriggerExit(Collider other){
      if (_tag.Equals("Player") && other.CompareTag("Player"))
             _hagglingManager.SetPlayerReadiness(false);
      if (_tag.Equals("NPC") && other.CompareTag("NPC")){
            _hagglingManager.SetNpcReadiness(false);
            _playerTrigger.SetActive(false);
            _text.gameObject.SetActive(false);
      }
   }
}
