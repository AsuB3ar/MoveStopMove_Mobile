using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MoveStopMove.Manager
{
    using System;
    using Utilitys;
    using MoveStopMove.Core.Data;
    [DefaultExecutionOrder(-10)]
    public class GameManager : SingletonPersistent<GameManager>
    {
        //[SerializeField] UserData userData;
        //[SerializeField] CSVData csv;
        //private static GameState gameState = GameState.MainMenu;

        // Start is called before the first frame update
        public event Action OnStartGame;
        public event Action OnStopGame;
        bool gameIsRun = false;
        public bool GameIsRun => gameIsRun;

        private List<IPersistentData> persistentDataObjects;
        [SerializeField]
        public GameData GameData;

        int maxScreenHeight = 1080;
        float screenRatio;
        protected override void Awake()
        {
            base.Awake();
            Input.multiTouchEnabled = false;
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            screenRatio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
            if (Screen.currentResolution.height > maxScreenHeight)
            {
                Screen.SetResolution(Mathf.RoundToInt(screenRatio * (float)maxScreenHeight), maxScreenHeight, true);
            }
            Screen.fullScreen = false;
        }

        public void SetResolution(int width, int height)
        {
            Screen.SetResolution(width, height, true);
        }

        public void SetResolution(float ratio)
        {
            int height = Screen.currentResolution.height;
            int width = Mathf.RoundToInt(ratio * height);
            Screen.SetResolution(width, height, true);
            Screen.fullScreen = false;
        }

        public void SetFullScreen()
        {
            Screen.SetResolution(Mathf.RoundToInt(screenRatio * (float)maxScreenHeight), maxScreenHeight, true);
        }

        public void StartGame()
        {
            gameIsRun = true;
            Time.timeScale = 1;
            OnStartGame?.Invoke();
        }

        public void StopGame()
        {
            gameIsRun = false;
            OnStopGame?.Invoke();
        }

        public void LoadGame()
        {
            GameData.OnInitData();
        }      
    }
}