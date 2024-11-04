using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    // This class is a bit of a catch-all thing, if our main menu
    // had more elements, we should probably break this up into smaller logical chunks.
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

        private void EnableCanvas(CanvasGroup canvasGroup)
        {
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