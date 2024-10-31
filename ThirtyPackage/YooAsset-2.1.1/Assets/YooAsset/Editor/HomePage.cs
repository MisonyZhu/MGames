using System;
using UnityEditor;
using UnityEngine;

namespace YooAsset.Editor
{
    internal class HomePageWindow
    {
        [MenuItem("Tools/YooAsset/Home Page", false, 1)]
        public static void OpenWindow()
        {
            Application.OpenURL("https://www.yooasset.com/");
        }
    }
}