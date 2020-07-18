using UnityEngine;
using UnityEngine.EventSystems;

namespace Snake
{
    public class SettingsController : MonoBehaviour
    {
        public GameObject mainMenuWindow;
        public GameObject firstSelected;

        public void OnBack()
        {
            Back();
        }

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
        }

        private void Back()
        {
            mainMenuWindow.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
