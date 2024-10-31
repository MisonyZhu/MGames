using UnityEngine;
using UnityEngine.Rendering;

namespace Framework.Debug
{
    public class DebugEnvironmentInfomationPanel : DebugScrollPanel
    {
        protected override void DrawContent()
        {
            GUILayout.Label("<b>Environment Information</b>");
            GUILayout.BeginVertical("box");
            {
                DrawItem("Product Name:", Application.productName);
                DrawItem("Company Name:", Application.companyName);
                DrawItem("Game Identifier:", Application.identifier);
                //DrawItem("Game Version:", HUResources.HotUpdateManager.localVersion.ToString());
                DrawItem("Application Version:", Application.version);
                DrawItem("Unity Version:", Application.unityVersion);
                DrawItem("Platform:", Application.platform.ToString());
                DrawItem("System Language:", Application.systemLanguage.ToString());
                DrawItem("Cloud Project Id:", Application.cloudProjectId);
                DrawItem("Build Guid:", Application.buildGUID);
                DrawItem("Target Frame Rate:", Application.targetFrameRate.ToString());
                DrawItem("Internet Reachability:", Application.internetReachability.ToString());
                DrawItem("Background Loading Priority:", Application.backgroundLoadingPriority.ToString());
                DrawItem("Is Playing:", Application.isPlaying.ToString());
                DrawItem("Splash Screen Is Finished:", SplashScreen.isFinished.ToString());
                DrawItem("Run In Background:", Application.runInBackground.ToString());
                DrawItem("Install Name:", Application.installerName);
                DrawItem("Install Mode:", Application.installMode.ToString());
                DrawItem("Sandbox Type:", Application.sandboxType.ToString());
                DrawItem("Is Mobile Platform:", Application.isMobilePlatform.ToString());
                DrawItem("Is Console Platform:", Application.isConsolePlatform.ToString());
                DrawItem("Is Editor:", Application.isEditor.ToString());
                DrawItem("Is Focused:", Application.isFocused.ToString());
            }
            GUILayout.EndVertical();
        }
    }
}