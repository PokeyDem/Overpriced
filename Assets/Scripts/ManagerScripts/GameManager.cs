
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonDontDestroyOnLoad<GameManager>
{
    public TutorialManager tutorialManager;
    [SerializeField] private float Goal;
    [SerializeField] private string EndMessageComponentName;
    [SerializeField] private string WinMessageText;
    [SerializeField] private string LoseMessageText;
    private new void Awake()
    {
        base.Awake();
    }
    // private void Start()
    // {
    //     DayManager.DayManagerInstance.dayChange.AddListener(End);
    // }
    public void End(){
        Debug.Log("End check invoke");
        if (DayManager.DayManagerInstance.GetDay() >= 7)
        {
            SceneManager.LoadScene("EndScene");
            
            TextMeshProUGUI message = GameObject.FindGameObjectWithTag("EndMessage").GetComponent<TextMeshProUGUI>();

            if (MoneyManager.MoneyManagerInstance.GetCurrentMoney() >= Goal){
                message.text = LoseMessageText;
            }
            else{
                message.text = WinMessageText;
            }
        }
    }
}
