using UnityEngine;
using System;
using System.Collections;

namespace SgLib
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;

        public int Score { get; private set; }
        public int Progress { get; private set; }

        public int HighScore { get; private set; }
        public int Level { get; private set; }

        public bool HasNewHighScore { get; private set; }

        public static event Action<int> ScoreUpdated = delegate {};
        public static event Action<int> ProgressUpdated = delegate {};
        public static event Action<int> HighscoreUpdated = delegate {};
        public static event Action<int> LevelUpdated = delegate {};

        private const string HIGHSCORE = "HIGHSCORE";
        // key name to store high score in PlayerPrefs

        void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void Start()
        {
            Reset();
        }

        public void Reset()
        {
            // Initialize score
            Score = 0;
            Progress = 0;
            Level = 1;

            // Initialize highscore
            HighScore = PlayerPrefs.GetInt(HIGHSCORE, 0);
            HasNewHighScore = false;
        }

        public void AddScore(int amount)
        {
            Score += amount;

            // Fire event
            ScoreUpdated(Score);

            if (Score > HighScore)
            {
                UpdateHighScore(Score);
                HasNewHighScore = true;
            }
            else
            {
                HasNewHighScore = false;
            }
        }

        public void AddProgress(int amount)
        {
            Progress += amount;
            

            if (Progress >= 30)
            {
                Progress = 0;
                AddLevel(1);
                //update level
                //reset
            }

            //update Level
        }

        public void AddLevel(int amount)
        {
            Level += amount;
            LevelUpdated(Level);

            //przyspieszenie levelu
        }
        public void UpdateHighScore(int newHighScore)
        {
            // Update highscore if player has made a new one
            if (newHighScore > HighScore)
            {
                HighScore = newHighScore;
                PlayerPrefs.SetInt(HIGHSCORE, HighScore);
                HighscoreUpdated(HighScore);
            }
        }
    }
}
