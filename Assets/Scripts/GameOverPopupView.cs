using UnityEngine;
using UnityEngine.UI;

namespace BalanceGame
{
    public class GameOverPopupView : MonoBehaviour
    {
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Text title;

        private void Start()
        {
            GameEvents.StageEndEvent += SetPopupActive;
            GameEvents.GameOverEvent += SetGameOverText;
            GameEvents.StageClearEvent += SetGameClearText;

            gameObject.SetActive(false);
        }

        private void SetGameClearText()
        {
            // TODO: move it somewhere
            title.text = "Game Clear";
        }

        private void SetGameOverText()
        {
            title.text = "Game Over";
        }

        private void SetPopupActive()
        {
            gameObject.SetActive(true);
        }
    }
}