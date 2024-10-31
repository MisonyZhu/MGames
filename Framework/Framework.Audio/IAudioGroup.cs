using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Framework
{



    public interface IAudioGroup
    {
        public int Id { get; }

        public float Volume { get; }

        void SetVolume(float volume);

        AudioObject Play(string audioClip, bool loop = false, float volumn = 1, GameObject target = null, float fadeIn = 0, float fadeOut = 0, float playTime = 0);

        AudioObject Play(AudioClip audioClip, bool loop = false, float volumn = 1, GameObject target = null, float fadeIn = 0, float fadeOut = 0, float playTime = 0);


        void Pause();

        void UnPause();

        void Stop();

        void Stop(AudioObject o);

        void Update(float time);


    }
}
