namespace BalanceGame
{
    public static class GameInfo
    {
        public static int CurrentStage = 0;
        public static int MaxStageUnlocked = 0;
        public static int Coin = 0;

        public static TotalStageData TotalStageData;
        public static LevelData CurrentStageData
        {
            get
            {
                return TotalStageData.levelDatas[CurrentStage];
            }
        }

        public static float TimePassed;
        public static int FellItemFromPipeCount;
        public static int FailedItemCount;

        public static void ReportFailedItem()
        {
            FailedItemCount++;

            // TODO: FUTURE MONOH WILL FIND BETTER SOLUTION I TRUST YOU
            var stageType = GameInfo.CurrentStageData.StageType;
            if (stageType == StageClearType.FailLessThan
                && GameInfo.FailedItemCount < GameInfo.CurrentStageData.FailLessThen)
            {
                return;
            }

            GameEvents.StageEndEvent?.Invoke();
            GameEvents.GameOverEvent?.Invoke();
        }
    }
}