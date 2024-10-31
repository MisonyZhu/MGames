//using DarkTonic.MasterAudio;
//using Framework;
//using System;
//using UnityEngine;

//namespace Game
//{
//    internal class AudioGroup : IAudioGroup
//    {
//        private int m_Id;

//        private string m_Name;
//        public int Id => m_Id;

//        MasterAudioGroup m_Group;

//        public AudioGroup(int id,string name,GameObject entity,int variationNum,bool is2D = true)
//        {
//            m_Id = id;
//            m_Name = name;
//            //m_Group = MasterAudio.CreateSoundGroup(m_Name, entity, variationNum, is2D);
            
//        }

//        public void AttachAudioListener(Transform target, Vector3 pos)
//        {
//            throw new NotImplementedException();
//        }

//        public void Pause()
//        {
//            throw new NotImplementedException();
//        }

//        public void Play(string audioClip, bool loop = false, float volumn = 1, GameObject target = null, float fadeIn = 0, float delay = 0, float playTime = 0)
//        {
//            throw new NotImplementedException();
//        }

//        public void Play(AudioClip audioClip, bool loop = false, float volumn = 1, GameObject target = null, float fadeIn = 0, float delay = 0, float playTime = 0)
//        {
//            throw new NotImplementedException();
//        }

//        public void SetVolume(float volume)
//        {
//            throw new NotImplementedException();
//        }

//        public void Stop()
//        {
//            throw new NotImplementedException();
//        }

//        public float Volume()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
