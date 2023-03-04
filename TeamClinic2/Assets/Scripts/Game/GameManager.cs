using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager instance;

        [SerializeField] private GameState gameState = GameState.Ready;

        [Header("Modules")]
        public GameObject pauseUI;


        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {

            switch (gameState)
            {
                case GameState.Ready:
                {
                    gameState = GameState.Starting;
                    break;
                }

                case GameState.Starting:
                {
                    
                    Debug.Log("Game starting...");
                    
                    gameState = GameState.Running;

                    
                    break;
                }
                case GameState.Running:
                {
                    if(Input_PauseDown())
                    {
                        Pause();
                    }

                    break;
                }
                case GameState.Paused:
                {
                    if(Input_PauseDown())
                    {
                        UnPause();
                    }
                    break;
                }
                case GameState.Win:
                {
                    
                    break;
                }
            }
        }

        

        public void PlayerDied()
        {
            gameState = GameState.Lost;
        }

        private void SwitchGameState(GameState newState)
        {
            gameState = newState;
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void Pause()
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
            StartUIMode();

            SwitchGameState(GameState.Paused);
        }

        public void UnPause()
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
            StopUIMode();
            
            SwitchGameState(GameState.Running);
        }

        public void StartUIMode()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            PlayerManager.instance.DeactivatePlayer();
        }

        public void StopUIMode()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerManager.instance.ActivatePlayer();
        }

        private bool Input_PauseDown()
        {
           // return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P);
           return Input.GetKeyDown(KeyCode.P);
        }
    }

    public enum LevelState
    {
        Unloaded, Ready, Loaded, Loading, Running, Paused, Done
    }

    public enum GameState
    {
        Ready, Paused, Running, Starting, Win, Lost
    }
    
}