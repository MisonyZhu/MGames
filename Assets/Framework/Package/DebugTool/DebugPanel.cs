using System;
using System.IO;
using UnityEngine;

namespace Framework.Debug
{
  public abstract class DebugPanel
  {
    public string panelName;
    public const float TITLE_WIDTH = 240f;
    public const float BUTTON_WIDTH = 100f;
    public const float BUTTON_HEIGHT = 24f;

    public virtual void OnEnable()
    {
    }

    public virtual void OnDisable()
    {
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnLeave()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void OnGUI()
    {
    }

    public void DrawItem(string title, string content) => this.DrawItem(title, content, 240f);

    public void DrawItem(string title, int content) => this.DrawItem(title, content.ToString(), 240f);

    public void DrawItem(string title, float content) => this.DrawItem(title, content.ToString(), 240f);

    public void DrawItem(string title, bool content) => this.DrawItem(title, content ? "True" : "False", 240f);

    public void DrawItem<T>(string title, T content) where T : class => this.DrawItem(title, (object) content != null ? content.ToString() : "Null", 240f);

    public void DrawItem(string title, string content, float titleWidth)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label(title, GUILayout.Width(titleWidth));
      GUILayout.Label(content);
      GUILayout.EndHorizontal();
    }

    public bool DrawItemButton(string title, string content, string button) => this.DrawItemButton(title, content, button, 240f, 100f);

    public bool DrawItemButton(
      string title,
      string content,
      string button,
      float titleWidth,
      float buttonWidth)
    {
      using (new GUILayout.HorizontalScope(Array.Empty<GUILayoutOption>()))
      {
        GUILayout.Label(title, GUILayout.Width(titleWidth));
        GUILayout.Label(content);
        return GUILayout.Button(button, GUILayout.Width(buttonWidth));
      }
    }

    public bool DrawToggle(string title, bool value)
    {
      using (new GUILayout.HorizontalScope(Array.Empty<GUILayoutOption>()))
      {
        GUILayout.Label(title, GUILayout.Width(240f));
        return GUILayout.Button(value ? "True" : "False", GUILayout.Width(100f)) ? !value : value;
      }
    }

    public int IntSlider(string title, int value, int minValue, int maxValue)
    {
      using (new GUILayout.HorizontalScope(Array.Empty<GUILayoutOption>()))
      {
        GUILayout.Label(title, GUILayout.Width(240f));
        value = Mathf.RoundToInt(GUILayout.HorizontalSlider((float) value, (float) minValue, (float) maxValue, GUILayout.ExpandWidth(true)));
        int result;
        if (int.TryParse(GUILayout.TextField(value.ToString(), GUILayout.Width(100f)), out result) && value != result)
          value = result;
        return value;
      }
    }

    public string debugDir
    {
      get
      {
        string dir = Application.persistentDataPath + "/DebugTool/";
        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);
        return dir;
      }
    }

    public int DrawPage(int page, int total, int onePage)
    {
      using (new GUILayout.HorizontalScope(Array.Empty<GUILayoutOption>()))
      {
        if (GUILayout.Button("<<", GUILayout.ExpandWidth(true), GUILayout.Height(24f)))
          page = 0;
        if (GUILayout.Button("<", GUILayout.ExpandWidth(true), GUILayout.Height(24f)) && page > 0)
          --page;
        int num = Mathf.Max((total - 1) / onePage + 1, 1);
        page = Mathf.Clamp(page, 0, num - 1);
        GUI.skin.label.alignment = TextAnchor.UpperCenter;
        GUILayout.Label(string.Format("{0}/{1}", (object) (page + 1), (object) num), GUILayout.Width(80f));
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        if (GUILayout.Button(">", GUILayout.ExpandWidth(true), GUILayout.Height(24f)) && page < num - 1)
          ++page;
        if (GUILayout.Button(">>", GUILayout.ExpandWidth(true), GUILayout.Height(24f)))
          page = num - 1;
        return page;
      }
    }

    protected static void DrawSplitter(Color color)
    {
      Rect rect = GUILayoutUtility.GetRect(1f, 1f);
      if (Event.current.type != EventType.Repaint)
        return;
      GUI.color = color;
      GUI.DrawTexture(rect, (Texture) Texture2D.whiteTexture);
      GUI.color = Color.white;
    }

    protected static void DrawSplitter() => DebugPanel.DrawSplitter(new Color(0.12f, 0.12f, 0.12f, 1.333f));
  }
}
