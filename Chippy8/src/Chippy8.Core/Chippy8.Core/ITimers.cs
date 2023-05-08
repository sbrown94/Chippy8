namespace Chippy8.Core
{
    public interface ITimers
    {
        public bool GetDelayTimer();

        public void SetDelayTimer(bool dT);

        public bool GetSoundTimer();

        public void SetSoundTimer(bool sT);
    }
}
