using UnityEngine;

namespace BalanceGame
{
    public class GameOverAreaDetector : MonoBehaviour
    {
        private void Start()
        {
            GameEvents.RestartEvent += Reset;
        }

        private void Reset()
        {
            GameInfo.FailedItemCount = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(GeneralVariables.Bird)) return;

            GameInfo.ReportFailedItem();
        }
    }
}