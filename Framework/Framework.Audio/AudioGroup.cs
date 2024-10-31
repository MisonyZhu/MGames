

using System.Collections.Generic;
using UnityEngine;

namespace Framework
{

    public class AudioGroup  : IAudioGroup
    {
        private int m_id;
        private string m_name;
        private float m_volume;

        private GameObject m_gameObject;

        private List<AudioObject> m_AudioObjects;

        private List<AudioObject> m_RemoveAudioObjects;

        private AudioSource m_DefaultAudioSource;

        private AudioSource DefaultAudioSource
        {
            get
            {
                //if (m_AudioSources!=null && m_AudioSources.Count > 0)
                //{
                //    return m_AudioSources[0];
                //}
                //else 
                //{
                    
                //}
                return m_DefaultAudioSource;
            }
        }

        
        public AudioGroup(int id,string name, Transform parent,bool is3D = false)
        { 
            m_id = id;
            m_name = name;
            m_gameObject = new GameObject(name+"_Group");
            m_gameObject.transform.parent = parent;
            m_AudioObjects = new List<AudioObject>();
            m_RemoveAudioObjects = new List<AudioObject>();
            m_DefaultAudioSource = (m_gameObject.AddComponent<AudioSource>());
        }

        public int Id => m_id;

        public float Volume => m_volume;
  

        public AudioObject Play(string audioClip, bool isLoop, float volume = 1f, GameObject target = null, float fadeIn = 0, float fadeOut = 0, float playTime = 0)
        {
            AudioSource source = target?target.GetComponent<AudioSource>():DefaultAudioSource;
            source.spatialBlend = (target ? 1 : 0);
            AudioObject audioObject = new AudioObject(source);
            audioObject.AudioGroup = this;
            audioObject.Volum = volume;
            audioObject.FadeOutTime = fadeOut;
            audioObject.FadeInTime = fadeIn;
            audioObject.PlayTime = playTime;
            audioObject.ResourceHandler = ResourceManager.LoadAssetAsync<AudioClip>(audioClip);
            m_AudioObjects.Add(audioObject);
            return audioObject;
        }

        public AudioObject Play(AudioClip audioClip, bool isLoop, float volume = 1f, GameObject target = null, float fadeIn = 0, float fadeOut = 0, float playTime = 0)
        {
            AudioSource source = target ? target.GetComponent<AudioSource>() : DefaultAudioSource;
            source.spatialBlend = (target ? 1 : 0);
            AudioObject audioObject = new AudioObject(source);
            audioObject.AudioGroup = this;
            audioObject.Volum = volume;
            audioObject.FadeOutTime = fadeOut;
            audioObject.FadeInTime = fadeIn;
            audioObject.PlayTime = playTime;
            audioObject.Play(audioClip);
            m_AudioObjects.Add(audioObject);
            return audioObject;
        }

        public void Pause()
        {
            foreach (var audioObject in m_AudioObjects)
            {
                audioObject?.Pause();
            }
        }
        public void UnPause()
        {
            foreach (var audioObject in m_AudioObjects)
            {
                audioObject?.UnPause();
            }
        }

        public void SetVolume(float volume)
        {
            this.m_volume = volume;
            foreach (var audioObject in m_AudioObjects)
            {
                audioObject?.VolumeChange();
            }
        }

        public void Stop()
        {
            foreach (var audioObject in m_AudioObjects)
            {
                audioObject?.Stop();
            }
            m_AudioObjects.Clear();
        }

        public void Stop(AudioObject o)
        {
            if(!m_RemoveAudioObjects.Contains(o))
                m_RemoveAudioObjects.Add(o);
        }

        public void Update(float time)
        {
            if (m_RemoveAudioObjects.Count > 0)
            { 
                foreach (var audioObject in m_RemoveAudioObjects)
                {
                    if (m_AudioObjects.Contains(audioObject))
                        m_AudioObjects?.Remove(audioObject);
                }
                m_RemoveAudioObjects.Clear();
            }
            foreach (var audioObject in m_AudioObjects)
            {
                audioObject?.Update(time);
            }
        }

       
    }
}
