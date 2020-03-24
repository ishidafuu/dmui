namespace DM
{
    public class UIImplements
    {
        public IPrefabLoader PrefabLoader { get; }
        public ISounder Sounder { get; }
        public IFadeCreator FadeCreator { get; }
        public ILoadingCreator LoadingCreator { get; }
        public IToastCreator ToastCreator { get; }

        public UIImplements(IPrefabLoader prefabLoader, ISounder sounder, IFadeCreator fadeCreator, 
            ILoadingCreator loadingCreator, IToastCreator toastCreator)
        {
            PrefabLoader = prefabLoader;
            Sounder = sounder;
            FadeCreator = fadeCreator;
            LoadingCreator = loadingCreator;
            ToastCreator = toastCreator;
        }
    }
}