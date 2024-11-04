using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ButtonActions : MonoBehaviour
    {
        [SerializeField] private CanvasGroup mainCanvas;
        [SerializeField] private CanvasGroup settingsCanvas;
        
        public void OnPlayClicked()
        {
            // could be async, then we show a loading "screen" until the loaded event fires.
            // Rather than straight loading the scene, we might also trigger an animation of the
            // PC heading into the door and then do the load.
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