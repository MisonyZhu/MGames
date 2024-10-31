using Framework;
using System;
using UnityEngine;
using static UnityEngine.Windows.WebCam.VideoCapture;


namespace Framework
{
    public enum EAudioObjectState
    {
        Loading,
        Playing,
        PlayOver
    }

    public class AudioObject : IDisposable
    {
        private ResourceHandler m_ResourceHandler;

        private GameObject m_Target;

        private AudioSource m_AudioSource;

        private float m_Duration;

        private bool m_Pause;

        public IAudioGroup AudioGroup { get; set; }

        public ResourceHandler ResourceHandler
        {
            get
            {
                return m_ResourceHandler;
            }

            set
            {
                AudioState = EAudioObjectState.Loading;
                value.Completed += (handler) => { Play(handler); };
                m_ResourceHandler = value;
            }
        }


        public bool IsLoop { get; set; }

        public float Volum { get; set; }

        public float FadeInTime { get; set; }

        public float FadeOutTime { get; set; }

        public float PlayTime { get; set; }

        public EAudioObjectState AudioState { get;private set; }

        internal AudioObject(AudioSource source)
        {
            m_AudioSource = source;
        }

        void Play(IResourceHandler hanlder)
        {
            m_AudioSource.clip = hanlder.AssetObject as AudioClip;
            m_AudioSource.volume = Volum * AudioGroup.Volume;
            m_AudioSource.time = PlayTime;
            m_AudioSource.loop = IsLoop;
            
            m_AudioSource.Play();
        }

        public void Play(AudioClip clip)
        {
            m_AudioSource.clip = clip;
            m_AudioSource.volume = Volum * AudioGroup.Volume;
            m_AudioSource.time = PlayTime;
            m_AudioSource.loop = IsLoop;
            AudioState = EAudioObjectState.Playing;

            m_AudioSource.Play();
        }

        public void Pause()
        {
            m_Pause = true;
            m_AudioSource.Pause();
        }

        public void UnPause()
        {
            m_Pause = false; 
            m_AudioSource.UnPause();
        }
    

        public void VolumeChange()
        {
            m_AudioSource.volume = Volum*AudioGroup.Volume;
        }

        public void Stop()
        {
            m_AudioSource.Stop();
            m_ResourceHandler?.Release();
            m_ResourceHandler = null;

            AudioGroup.Stop(this);
        }

        internal void Update(float detlaTime)
        {
            if (AudioState == EAudioObjectState.Playing && !m_Pause)
            {
                m_Duration += detlaTime;
                if (m_Duration > m_AudioSource.clip.length)
                {
                    AudioState = EAudioObjectState.PlayOver;
                    Stop();
                }
            }
        }

        public void Dispose()
        {
            m_ResourceHandler?.Release();
            m_ResourceHandler = null;
        }
    }
}
