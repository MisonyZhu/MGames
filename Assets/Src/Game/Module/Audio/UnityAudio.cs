using Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Game
{
    internal enum EAudioGroup
    { 
        Bgm = 1,            //_2D
        UI,                 //_2D
        Battle,             //_3D
        //xxxx
    }
    internal class UnityAudio : IAudioManager    
    {
        private Dictionary<int,IAudioGroup> m_AllGroups = new();

        private GameObject m_GameObject;

        UnityAudio()
        {
            m_GameObject  = new GameObject("AudioModule");
            m_GameObject.transform.position = Vector3.zero;      

            for (EAudioGroup i = EAudioGroup.Bgm; i <= EAudioGroup.Battle; i++)
            {               
                m_AllGroups.Add((int)i, new AudioGroup((int)i, i.ToString(),m_GameObject.transform));
            }
        }

        public static UnityAudio Create()
        {
            UnityAudio audio = new UnityAudio();
            
            return audio;
        }

        IAudioGroup GetGroups(int groupId)
        {
            if (m_AllGroups.ContainsKey(groupId))
            {
                return m_AllGroups[groupId];
            }
            else
            {
                LogModule.LogError("不存在的音频Group，请检查");
                return null;
            }
        }

        public void AttachAudioListener(Transform target, Vector3 pos)
        {
            throw new NotImplementedException();
        }

        public float GetVolume(int groupID)
        {
            m_AllGroups.TryGetValue(groupID, out var group);
            if (group != null)
                return group.Volume;
            return 0f;
        }

        public void Pause(int groupId)
        {
            m_AllGroups.TryGetValue(groupId, out var group);
            if (group != null)
                group.Pause();
        }

        public void UnPause(int groupId)
        {
            m_AllGroups.TryGetValue(groupId, out var group);
            if (group != null)
                group.UnPause();
        }

        public void Play(int groupId, string audioClip, bool loop = false, float volumn = 1, GameObject target = null, float fadeIn = 0, float delay = 0, float playTime = 0)
        {
            m_AllGroups.TryGetValue(groupId, out var group);
            if (group != null)
                group.Play(audioClip, loop, volumn, target, fadeIn, delay, playTime);
        }

        public void Play(int groupId, AudioClip audioClip, bool loop = false, float volumn = 1, GameObject target = null, float fadeIn = 0, float delay = 0, float playTime = 0)
        {
            m_AllGroups.TryGetValue(groupId, out var group);
            if (group != null)
                group.Play(audioClip, loop, volumn, target, fadeIn, delay, playTime);
        }

        public void SetVolume(int groupID, float volume)
        {
            m_AllGroups.TryGetValue(groupID, out var group);
            if (group != null)
                group.SetVolume(volume);
        }

        public void Stop(int groupId)
        {
            m_AllGroups.TryGetValue(groupId, out var group);
            if (group != null)
                group.Stop();
        }

        public void StopAll(int groupId)
        {
            foreach (var group in m_AllGroups.Values)
            {
                group.Stop();
            }
        }

       

        public void Update(float detlaTime)
        {
            foreach (var group in m_AllGroups.Values)
            {
                group.Update(detlaTime);
            }
        }
    }
}
