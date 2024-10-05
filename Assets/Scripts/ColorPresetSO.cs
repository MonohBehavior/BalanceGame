using System;
using UnityEngine;

namespace BalanceGame
{
    [CreateAssetMenu(fileName = "ColorPreset", menuName = "BalanceGame/ColorPreset")]
    public class ColorPresetSO : ScriptableObject
    {
        public BackgroundColorPreset[] BackgroundColors;
        public Color[] WhateverColors;
    }

    [Serializable]
    public class BackgroundColorPreset
    {
        public Color ForwardColor;
        public Color BackgroundColor;
    }
}
