using cfg;
using Framework;
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    public class ConfigModule : ModuleBase<ConfigModule>
    {
        private const string CONFIG_PATH = "Res/Config/Table";
        public override int Priority => ModulePriority.Config_Priority;

        public static Tables Cfg
        {
            get; private set;
        }

        public static void PreLoad()
        {
            Cfg = new cfg.Tables(file=> LoadTable(file));
        }

        static JSONNode LoadTable(string path)
        {
            var handler = ResourceModule.LoadAssetAsync<TextAsset>($"{CONFIG_PATH}/{path}.json");
            handler.WaitForAsyncComplete();
            var textAsset = handler.AssetObject as TextAsset;
            handler.Release();
            return JSON.Parse(textAsset.text);
        }

        public override void OnUpdate(float detlaTime)
        {
           
        }

        public override void OnShutDown()
        {
            Cfg = null;
        }
    }
}
