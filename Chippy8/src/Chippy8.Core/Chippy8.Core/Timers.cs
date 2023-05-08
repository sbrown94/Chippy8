namespace Chippy8.Core
{
    public class Timers : ITimers
    {
        private bool delayTimer; // delay timer. Active when delayReg is not 0
        private bool soundTimer; // sound timer. Active when soundReg is not 0

        public Timers()
        {
            delayTimer = false;
            soundTimer = false;
        }

        public bool GetDelayTimer() => delayTimer;

        public bool GetSoundTimer() => soundTimer;

        public void SetDelayTimer(bool dT) => delayTimer = dT;

        public void SetSoundTimer(bool sT) => soundTimer = sT;
    }
}
