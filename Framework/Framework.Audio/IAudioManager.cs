using System;
using UnityEngine;


namespace Framework
{
    public interface IAudioManager
    {

        void Play(int groupId, string audioClip, bool loop = false, float volumn = 1f, GameObject target = null, float fadeIn = 0f, float fadeOut = 0f, float playTime = 0f);

        void Play(int groupId, AudioClip audioClip, bool loop = false, float volumn = 1f, GameObject target = null, float fadeIn = 0f, float fadeOut = 0f, float playTime = 0f);

        void SetVolume(int groupID, float volume);

        float GetVolume(int groupID);

        void Pause(int groupId);

        void UnPause(int groupId);

        void Stop(int groupId);

        void StopAll(int groupId);

        void Update(float detlaTime);

    }
}
