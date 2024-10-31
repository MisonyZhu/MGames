
using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

namespace App
{
    public class ChannelRequester
    {
        private static bool m_RequestOK;
        
        public  static void Init(Action complete)
        {
            m_RequestOK = false;
            ChannelConfig.LoadConfig();

            if (ChannelConfig.LocalChannelConfig.Id == 0)
            {
                ChannelConfig.ServerChannelInfo = new ServerChannelInfo() { Id = 0,Name =  "Test",};
                m_RequestOK = true;
                complete?.Invoke();
                return;
            }
            if (ChannelConfig.LocalChannelConfig.Id == 1)
            {
                ChannelConfig.ServerChannelInfo = new ServerChannelInfo() { Id = 1,Name =  "TestUpdate",};
                m_RequestOK = true;
                complete?.Invoke();
                return;
            }
            AppEntry.Instance.StartCoroutine(StartRequest());
        }

        public static IEnumerator  StartRequest()
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(ChannelConfig.LocalChannelConfig.ChannelUrl))
            {
                // webRequest.timeout = 3000;
                var operation = webRequest.SendWebRequest();
                yield return operation;
                if (string.IsNullOrEmpty(webRequest.error) && webRequest.result == UnityWebRequest.Result.Success)
                {
                    string channelJson = webRequest.downloadHandler.text;
                    try
                    {
                        ChannelConfig.ServerChannelInfo = JsonUtility.FromJson<ServerChannelInfo>(channelJson);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("序列化渠道信息失败，请检查配置："+channelJson);
                        UIMessage.Instance.ShowTip("请求配置失败，请检查配置！", Application.Quit);
                        throw e;
                    }

                    m_RequestOK = true;
                }
                else
                {
                    //TODO 处理http请求失败
                    UIMessage.Instance.ShowTip("请求配置失败，请检查网络！Error:"+webRequest.error, () => { AppEntry.Instance.StartCoroutine(StartRequest());},
                        Application.Quit);
                }
            }
        }


        public static void Reset()
        {
            m_RequestOK = false;
        }



        

    }
}