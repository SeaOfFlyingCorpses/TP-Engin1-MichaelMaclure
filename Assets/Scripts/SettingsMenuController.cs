using UnityEngine;
using UnityEngine.UI;
namespace Checker
{
    public class SettingsMenuController : MonoBehaviour
    {
        [Header("Références")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private Slider musicSlider;

        private void Start()
        {
            musicSlider.value = AudioManager.Instance.musicVolume;
        }
        public void ToggleSettings()
        {
            bool isActive = settingsPanel.activeSelf;
            settingsPanel.SetActive(!isActive);
        }
        public void CloseSettingsPanel()
        {
            settingsPanel.SetActive(false);
        }
    }
}
