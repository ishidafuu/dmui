namespace DM
{
    public interface ISounder
    {
        void PlayDefaultClickSE();
        void PlayClickSE(string name);
        void PlayBGM(string name);
        void StopBGM();
    }
}