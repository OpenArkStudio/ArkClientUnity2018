namespace ARKGame
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class ARKGameEntry
    {
        public static ConfigComponent Config
        {
            get;
            private set;
        }
        public static AFNetComponent AFNet;
        //public static HPBarComponent HPBar
        //{
        //    get;
        //    private set;
        //}

        private static void InitCustomComponents()
        {
            Config = UnityGameFramework.Runtime.GameEntry.GetComponent<ConfigComponent>();
            AFNet = UnityGameFramework.Runtime.GameEntry.GetComponent<AFNetComponent>();
            //HPBar = UnityGameFramework.Runtime.GameEntry.GetComponent<HPBarComponent>();
        }
    }
}
