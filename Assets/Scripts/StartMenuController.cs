using UnityEngine;

namespace Checker
{
    public class StartMenuController : MonoBehaviour
    {
        [Header("Références")]
        [SerializeField] private GameObject menuCanvas;
        [SerializeField] private SettingsMenuController settingsMenu;
        private void Start()
        {
            PauseGame();
        }
        
        public void StartGame()
        {
            if (settingsMenu != null)
                settingsMenu.CloseSettingsPanel();
            ResumeGame();
            HideMenu();
        }
        
        private void PauseGame()
        {
            Time.timeScale = 0f;
        }

      
        private void ResumeGame()
        {
            Time.timeScale = 1f;
        }
        
        private void HideMenu()
        {
            menuCanvas.SetActive(false);
        }
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
