using System;
using System.Collections;
using System.Collections.Generic;

namespace BalanceGame
{
    /// <summary>
    /// State Machine
    /// </summary>
    /// TODO: Hello super mighty Monoh, here I was making a state machine,
    /// which works like Meta VR Telelporter because that is few examples I read...
    /// So, Most of the functions are in the BalanaceGameManager script,
    /// So. FIX.
    public class BalanceGameController : Singleton<BalanceGameController>
    {
        public CurrentGameStageMode CurrentMode;
        public CurrentIntentions CurrentIntentions;

        private void Start()
        {
            CurrentMode = CurrentGameStageMode.Starting;

            SetUp();
        }

        public Action InitAction;

        private void SetUp()
        {
            InitAction();

            StartCoroutine(Playing());
        }

        public List<Action> PlayingAction = new();

        private IEnumerator Playing()
        {
            CurrentMode = CurrentGameStageMode.Playing;
            while (CurrentIntentions == CurrentIntentions.Playing)
            {
                foreach (var act in PlayingAction)
                {
                    act();
                }
                yield return null;
            }

            switch (CurrentIntentions)
            {
                case CurrentIntentions.GameOver:
                    CurrentMode = CurrentGameStageMode.GameOver;
                    StartCoroutine(GameOver());
                    break;
                case CurrentIntentions.GameClear:
                    CurrentMode = CurrentGameStageMode.GameClear;
                    StartCoroutine(GameClear());
                    break;
            }
        }

        public Action GameOverAction;

        private IEnumerator GameOver()
        {
            GameOverAction();

            while (CurrentIntentions == CurrentIntentions.GameOver)
            {
                yield return null;
            }

            if (CurrentIntentions == CurrentIntentions.Reset)
            {
                CurrentMode = CurrentGameStageMode.Reset;
                StartCoroutine(GameReset());
            }
        }

        public Action GameClearAction;

        private IEnumerator GameClear()
        {
            GameClearAction();

            while (CurrentIntentions == CurrentIntentions.GameClear)
            {
                yield return null;
            }

            if (CurrentIntentions == CurrentIntentions.Reset)
            {
                CurrentMode = CurrentGameStageMode.Reset;
                StartCoroutine(GameReset());
            }
        }

        public Action GameResetAction;

        private IEnumerator GameReset()
        {
            GameResetAction();

            while (CurrentIntentions == CurrentIntentions.Reset)
            {
                yield return null;
            }

            if (CurrentIntentions == CurrentIntentions.Playing)
            {
                CurrentMode = CurrentGameStageMode.Playing;
                StartCoroutine(Playing());
            }
        }
    }
}