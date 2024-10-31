using UnityEngine;

namespace Framework.Debug
{
    public class DebugSettingPanel : DebugScrollPanel
    {
        protected override void DrawContent()
        {
            GUILayout.Label("<b>Minimize</b>");
            using (new GUILayout.VerticalScope("box"))
            {
                var dt = DebugTool.Instance;
                dt.showFPS.Set(DrawToggle("ShowFPS", dt.showFPS));
                dt.showMono.Set(DrawToggle("ShowMono", dt.showMono));
                dt.showNative.Set(DrawToggle("ShowNative", dt.showNative));
                dt.showLua.Set(DrawToggle("ShowLua", dt.showLua));
#if UNITY_ANDROID && !UNITY_EDITOR
                dt.showPSS.Set(DrawToggle("ShowPSS", dt.showPSS));
#endif
            }
        }
    }
}