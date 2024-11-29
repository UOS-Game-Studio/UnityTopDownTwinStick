using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    /// <summary>
    /// ButtonActions acts as a general purpose UI interaction handler
    /// it offers a set of handlers for button events from the main menu and in the pause menu
    /// </summary>
    public class ButtonActions : MonoBehaviour
    {
        [SerializeField] private CanvasGroup mainCanvas;
        [SerializeField] private CanvasGroup settingsCanvas;
        
        public void OnPlayClicked()
        {
            /*
             * could be LoadSceneAsync, then we show a loading "screen" until the loaded event fires.
             * Rather than straight loading the scene, we could also trigger an animation of the
             * PC heading into the door and use that as a loading thing.
             */
            SceneManager.LoadScene("Scenes/Game");
        }

        public void OnMenuClicked()
        {
            // for now, when we return to the menu scene, we'll destroy any current SettingsControl object
            SettingsControl settings = FindFirstObjectByType<SettingsControl>();
            Destroy(settings.gameObject);
            
            SceneManager.LoadScene("Scenes/Menu");
        }
        
        private void EnableCanvas(CanvasGroup canvasGroup)
        {
            // there's no neater way of toggling the status of a canvas group, sadly!
            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        private void DisableCanvas(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        
        public void OnSettingsClicked()
        {
            EnableCanvas(settingsCanvas);
            DisableCanvas(mainCanvas);
        }

        public void OnBackClicked()
        {
            EnableCanvas(mainCanvas);
            DisableCanvas(settingsCanvas);
        }
        
        public void OnQuitClicked()
        {
            Application.Quit();
        }
    }
}