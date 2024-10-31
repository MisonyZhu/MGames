using Framework;
using Game;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AudioModule : ModuleBase<AudioModule>
{
    private static IAudioManager AudioManager;

    public override int Priority => ModulePriority.Audio_Priority;

    public override void Init()
    {
        base.Init();

        AudioManager = UnityAudio.Create();
    }


    public override void OnShutDown()
    {
        AudioManager.StopAll(0);
    }

    public override void OnUpdate(float detlaTime)
    {
        AudioManager?.Update(detlaTime);
    }

    public static void Play(int goupId,string audioClip, bool isLoop, float volume = 1f, GameObject target = null, float fadeIn = 0, float fadeOut = 0, float playTime = 0)
    {
         AudioManager.Play(goupId,audioClip, isLoop,volume,target,fadeIn,fadeOut,playTime);
    }

    public static void Play(int goupId, AudioClip audioClip, bool isLoop, float volume = 1f, GameObject target = null, float fadeIn = 0, float fadeOut = 0, float playTime = 0)
    {
        AudioManager.Play(goupId, audioClip, isLoop, volume, target, fadeIn, fadeOut, playTime);
    }
    public static void SetVolume(int groupId, float volueme)
    {
        AudioManager.SetVolume(groupId, volueme);
    }

    public static void Stop(int goupId) 
    {
        AudioManager.Stop(goupId);
    }

    public static void StopAll(int goupId = 0)
    {
        AudioManager.StopAll(goupId);
    }
}