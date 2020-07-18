using System.Collections;
using TMPro;
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
        public TextMeshProUGUI scoreText;

        private bool isPaused;
        private WaitForSeconds waitForSeconds;
        private int score;

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
            Exit();
        }

        private void OnEnable()
        {
            StartGame();
        }

        private void Update()
        {
            if (isPaused)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Exit();
                }
            }
            else
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

        private void StartGame()
        {
            isPaused = false;
            waitForSeconds = new WaitForSeconds(1 / gameSpeedSlider.value);
            boardController.InitBoard();
            scoreSubWindow.SetActive(true);
            score = 0;
            scoreText.SetText(score.ToString());
            StartCoroutine(GameTick());
        }

        private IEnumerator GameTick()
        {
            while (true)
            {
                yield return waitForSeconds;
                if (isPaused) yield break;

                GameState gameState = boardController.GetGameState();
                boardController.UpdateBoard(gameState);

                if (gameState == GameState.AppleEaten)
                {
                    ++score;
                    scoreText.SetText(score.ToString());
                    boardController.CreateApple();
                }
                else if (gameState == GameState.Lost)
                {
                    GameOver();
                }
            }
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

        private void Exit()
        {
            boardController.ClearBoard();
            gameOverSubWindow.SetActive(false);
            pauseSubWindow.SetActive(false);
            mainMenuWindow.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
