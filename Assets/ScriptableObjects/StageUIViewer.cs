using UnityEngine;
using UnityEngine.UI;

namespace BalanceGame
{
    public class StageUIViewer : MonoBehaviour
    {
        [SerializeField] private GameObject leftTimecheckObject;
        [SerializeField] private GameObject itemsLeftObject;
        [SerializeField] private GameObject failLessThenObject;

        [SerializeField] private Image leftTimeCheckUI;
        [SerializeField] private Text itemsLeftUI;
        [SerializeField] private Text failLessThenUI;

        public void ChangeMode(StageClearType stageClearType)
        {
            leftTimecheckObject.SetActive(stageClearType == StageClearType.Survive);
            itemsLeftObject.SetActive(stageClearType == StageClearType.Drop);
            failLessThenObject.SetActive(stageClearType == StageClearType.FailLessThan);
        }

        public void UpdateLeftTime(float ratio)
        {
            leftTimeCheckUI.fillAmount = ratio;
        }

        public void UpdateLeftItem(int number)
        {
            itemsLeftUI.text = number.ToString();
        }

        public void UpdateFailLessThen(int number)
        {
            failLessThenUI.text = number.ToString();
        }
    }
}