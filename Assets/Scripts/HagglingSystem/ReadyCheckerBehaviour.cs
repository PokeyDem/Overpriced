using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ReadyCheckerBehaviour : MonoBehaviour{
    [SerializeField] private string _tag;
    [SerializeField] private HagglingManager _hagglingManager;
    [SerializeField] private GameObject _playerTrigger;
    public UnityEvent readyToHaggle;

    private void OnTriggerEnter(Collider other){

      if (_tag.Equals("NPC") && other.CompareTag("NPC")){
            _playerTrigger.SetActive(true);
            readyToHaggle?.Invoke();
      }
   }

   private void OnTriggerExit(Collider other){
      if (_tag.Equals("NPC") && other.CompareTag("NPC")){
            _hagglingManager.SetNpcReadiness(false);
            _playerTrigger.SetActive(false);
      }
   }
}
