using UnityEngine;
using UnityEngine.EventSystems;

namespace Snake
{
    public class MainMenuController : MonoBehaviour
    {
        public GameObject gameWindow;
        public GameObject settingsWindow;
        public GameObject firstSelected;

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        public void OnPlay()
        {
            gameWindow.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnSettings()
        {
            settingsWindow.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnExit()
        {
            Application.Quit();
        }
    }
}
