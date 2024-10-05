namespace BalanceGame
{
    public static class GameEvents
    {
        public delegate void SimpleEvent();
        public static SimpleEvent GameOverEvent;
        public static SimpleEvent StageClearEvent;
        public static SimpleEvent StageEndEvent;
        public static SimpleEvent RestartEvent;
    }
}
