
using UnityEditor;
using UnityEngine;

public class MainMenuManager : SingletonDontDestroyOnLoad<MainMenuManager>
{
    private bool _isInSubMenu = false;
    private bool _isInMainMenu = true;
    private bool _firstGameStarted = false;
    
    public void SwitchToMainMenu()
    {
        if (!_firstGameStarted)
            PauseMenuManager.Instance.DisablePausing();
        
        //Disable trigger switching
        CameraController.Instance.ChangeTriggerSwitchState(false);
        
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
        CameraController.Instance.ChangeTriggerSwitchState(true);
        
        PlayerControl.Instance.EnableControl();
        
        UIManager.Instance.EnableHUD();
        
        TutorialManager.Instance.EnableTutorial();
        
        MainMenuAnimationManager.Instance.CloseDrawer();
        
        CameraController.Instance.SwitchToShop();
        
        UIManager.Instance.HideMainMenuButtons();
        _isInSubMenu = false;
    }

    private void OpenDrawer()
    {
        MainMenuAnimationManager.Instance.OpenDrawer();
        CameraController.Instance.SwitchToCashRegisterDrawer();
        _isInSubMenu = true;
    }

    private void PressButton(GameObject button)
    {
        MainMenuAnimationManager.Instance.PressButton(button.transform);
    }

    public void OnNewGameButtonPress(GameObject button)
    {
        if (_isInSubMenu) return;

        if (!_firstGameStarted)
        {
            PressButton(button);
            SwitchToGame();
            _firstGameStarted = true;
            PauseMenuManager.Instance.EnablePausing();
        }
        else
        {
            PressButton(button);
            GameManager.Instance.ReloadGame();
            SwitchToGame();
            _isInMainMenu = false; 
        }
    }

    public void BlankShot(GameObject button) // For test purposes
    {
        PressButton(button);
    }

    public void OnSettingButtonPress(GameObject button)
    {
        if (_isInSubMenu) return;
        PressButton(button);
        OpenDrawer();
        MainMenuUIManager.Instance.ShowSettingsUI();
    }

    public void OnSaveGameButtonPress(GameObject button)
    {
        if (_isInSubMenu) return;
        PressButton(button);
        SaveManager.Instance.SaveGameInTemporarySlot();
        MainMenuAnimationManager.Instance.RotateHandleWithoutRelatedAction(true);
    }

    public void OnLoadGameButtonPress(GameObject button)
    {
        if (_isInSubMenu) return;
        PressButton(button);
        SaveManager.Instance.LoadGameFromTemporarySlot();
        MainMenuAnimationManager.Instance.RotateHandleWithoutRelatedAction(false);
    }

    public void OnCreditsButtonPress(GameObject button)
    {
        if (_isInSubMenu) return;
        PressButton(button);
        OpenDrawer();
        MainMenuUIManager.Instance.ShowCreditsUI();
    }

    public void OnDrawerHandleClick()
    {
        MainMenuAnimationManager.Instance.CloseDrawer();
        CameraController.Instance.SwitchToCashRegister();
        _isInSubMenu = false;
    }

    public void OnExitButtonPress(GameObject button)
    {
        if (_isInSubMenu) return;
        PressButton(button);
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public bool IsInSubMenu()
    {
        return _isInSubMenu;
    }
}
