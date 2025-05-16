
using UnityEngine.SceneManagement;

public class GameManager : SingletonDontDestroyOnLoad<GameManager>
{
    public TutorialManager tutorialManager;
    private new void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        DayManager.DayManagerInstance.dayChange.AddListener(End);
    }
    private void End(int day)
    {
        if (day > 7)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}
