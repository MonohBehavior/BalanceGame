using System;
using System.Collections.Generic;

namespace BalanceGame
{
    public class PlayData
    {
        public int NumberOfFailedItems;
        public float TimesPassed;
    }

    [Serializable]
    public class TotalStageData
    {
        public List<LevelData> levelDatas = new();
    }

    [Serializable]
    public class LevelData
    {
        public StageClearType StageType;

        public PlateInfo PlateInfo = new();

        public float FallingSpeed;
        // Survive
        public float SurviveAimSeconds;
        public int FailLessThen;

        // Drop X Items
        public List<DropGameObjectType> DropItemList = new();
    }

    public class DropGameObjectType
    {
        public int GameObjectType;
        public int Color;
        public float Size;
        public int GimmickType;
    }

    public enum DropItemType
    {
        Sparrow = 0,
        FatSparrow = 1,
        Fish = 2,
    }

    [Serializable]
    public class DropItem
    {
        public DropItemType ItemType;

        public float weight;
        public float size;

        // TODO: Where to go?
        //public GameObject GetDropItem(DropItemType type)
        //{

        //}
    }

    [Serializable]
    public class PlateInfo
    {
        public int NumberOfPlates = 1;
        public int Mass = 50;
        public int ConstantForce = 200;
        public float Size = 1.3f;
    }
}