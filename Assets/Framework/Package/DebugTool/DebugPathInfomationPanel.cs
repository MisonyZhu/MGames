using UnityEngine;

namespace Framework.Debug
{
    public class DebugPathInfomationPanel : DebugScrollPanel
    {
        protected override void DrawContent()
        {
            GUILayout.Label("<b>Path Information</b>");
            GUILayout.BeginVertical("box");
            {
                DrawItem("Data Path:", Application.dataPath);
                DrawItem("Persistent Data Path:", Application.persistentDataPath);
                DrawItem("Streaming Assets Path:", Application.streamingAssetsPath);
                DrawItem("Temporary Cache Path:", Application.temporaryCachePath);
            }
            GUILayout.EndVertical();
        }
    }
}