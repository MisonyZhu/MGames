using UnityEngine;


namespace Framework
{
    public class Audio
    {
        internal static IAudioManager Audiolmpl;

        public static void SetAudio(IAudioManager lmpl)
        {
            Audiolmpl = lmpl;
        }


    }
}
