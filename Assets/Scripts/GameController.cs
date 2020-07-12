using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Snake
{
    public class GameController : MonoBehaviour
    {
        public GameObject mainMenuWindow;
        public GameObject scoreSubWindow;
        public GameObject pauseSubWindow;
        public GameObject pauseFirstSelected;
        public GameObject gameOverSubWindow;
        public GameObject gameOverFirstSelected;
        public BoardController boardController;
        public Slider gameSpeedSlider;

        private bool isPaused;
        private WaitForSeconds waitForSeconds;

        private void OnEnable()
        {
            StartGame();
        }

        private void Update()
        {
            if (!isPaused)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Pause();
                }

                float vAxis = Input.GetAxisRaw("Vertical");
                float hAxis = Input.GetAxisRaw("Horizontal");

                if (vAxis != 0 || hAxis != 0)
                {
                    boardController.rotateSnake(vAxis, hAxis);
                }
            }
        }

        private IEnumerator GameTick()
        {
            while (true)
            {
                yield return waitForSeconds;
                if (isPaused) yield break;

                bool lost = boardController.UpdateBoard();

                if (lost)
                {
                    GameOver();
                }
            }
        }

        private void StartGame()
        {
            isPaused = false;
            waitForSeconds = new WaitForSeconds(1 / gameSpeedSlider.value);
            boardController.InitBoard();
            scoreSubWindow.SetActive(true);
            StartCoroutine(GameTick());
        }

        public void OnResume()
        {
            isPaused = false;
            pauseSubWindow.SetActive(false);
            scoreSubWindow.SetActive(true);
            StartCoroutine(GameTick());
        }

        public void OnRestart()
        {
            boardController.ClearBoard();
            gameOverSubWindow.SetActive(false);
            scoreSubWindow.SetActive(true);
            StartGame();
        }

        public void OnExit()
        {
            isPaused = true;
            boardController.ClearBoard();
            mainMenuWindow.SetActive(true);
            gameObject.SetActive(false);
        }

        private void Pause()
        {
            isPaused = true;
            scoreSubWindow.SetActive(false);
            pauseSubWindow.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstSelected);
        }

        private void GameOver()
        {
            isPaused = true;
            scoreSubWindow.SetActive(false);
            gameOverSubWindow.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(gameOverFirstSelected);
        }
    }
}
