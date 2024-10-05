using System;
using UnityEngine;

namespace BalanceGame
{
    public class MaterialColorManager : MonoBehaviour
    {
        [SerializeField] private ColorPresetSO colorPresetSO;
        [SerializeField] private Material backgroundMat;

        private void Start()
        {
            ChangeBackgroundColor();
            GameEvents.RestartEvent += ChangeBackgroundColor;
        }

        private void ChangeBackgroundColor()
        {
            backgroundMat.SetColor("_Color2", colorPresetSO.BackgroundColors[GameInfo.CurrentStage].ForwardColor);
            backgroundMat.SetColor("_Color1", colorPresetSO.BackgroundColors[GameInfo.CurrentStage].BackgroundColor);
        }
    }
}