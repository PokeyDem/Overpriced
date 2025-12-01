
using UnityEngine;

public class MainMenuManager : SingletonDontDestroyOnLoad<MainMenuManager>
{
    private bool _isInSubMenu = false;
    
    public void SwitchToMainMenu()
    {
        //Disable player control
        PlayerControl.Instance.DisableControl();
        
        //Disable game UI
        UIManager.Instance.DisableHUD();
        
        //Disable tutorial UI
        TutorialManager.Instance.DisableTutorial();
        
        //Disable transparent displays
        
        //Switch camera
        CameraController.Instance.SwitchToCashRegister();
        
        UIManager.Instance.ShowMainMenuButtons();
    }

    public void SwitchToGame()
    {
        PlayerControl.Instance.EnableControl();
        
        UIManager.Instance.EnableHUD();
        
        TutorialManager.Instance.EnableTutorial();
        
        MainMenuAnimationManager.Instance.CloseDrawer();
        
        CameraController.Instance.SwitchToShop();
        
        UIManager.Instance.HideMainMenuButtons();
    }

    public void OnTestButtonPress(GameObject button)
    {
        MainMenuAnimationManager.Instance.PressButton(button.transform);
        MainMenuAnimationManager.Instance.OpenDrawer();
        CameraController.Instance.SwitchToCashRegisterDrawer();
        _isInSubMenu = true;
    }

    public void OnNewGameButtonPress()
    {
        if (_isInSubMenu) return;
        SwitchToGame();
    }

    public void OnSettingButtonPress(GameObject button)
    {
        if (_isInSubMenu) return;
        OnTestButtonPress(button);
    }

    public void OnDrawerHandleClick()
    {
        MainMenuAnimationManager.Instance.CloseDrawer();
        CameraController.Instance.SwitchToCashRegister();
        _isInSubMenu = false;
    }

    public bool IsInSubMenu()
    {
        return _isInSubMenu;
    }
}
