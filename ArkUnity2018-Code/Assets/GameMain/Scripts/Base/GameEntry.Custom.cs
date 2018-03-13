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
        public static AFNetComponent AFNet { get; private set; }
        public static AFDataComponent AFData { get; private set; }
        //public static HPBarComponent HPBar
        //{
        //    get;
        //    private set;
        //}

        private static void InitCustomComponents()
        {
            Config = UnityGameFramework.Runtime.GameEntry.GetComponent<ConfigComponent>();
            AFNet = UnityGameFramework.Runtime.GameEntry.GetComponent<AFNetComponent>();
            AFData = UnityGameFramework.Runtime.GameEntry.GetComponent<AFDataComponent>();
            //HPBar = UnityGameFramework.Runtime.GameEntry.GetComponent<HPBarComponent>();
        }
    }
}
