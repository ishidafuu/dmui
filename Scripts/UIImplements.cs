namespace DM
{
    public class UIImplements
    {
        public IPrefabLoader PrefabLoader { get; }
        public ISounder Sounder { get; }
        public IFadeCreator FadeCreator { get; }

        public UIImplements(IPrefabLoader prefabLoader, ISounder sounder, IFadeCreator fadeCreator)
        {
            PrefabLoader = prefabLoader;
            Sounder = sounder;
            FadeCreator = fadeCreator;
        }
    }
}