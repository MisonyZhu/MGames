namespace Framework.Debug
{
    public class DebugPanelRoot : DebugPanelGroup
    {
        protected override void CalcToolbarNames()
        {
            m_ToolbarNames = new string[panels.Count + 1];
            for (int i = 0; i < panels.Count; ++i)
            {
                m_ToolbarNames[i] = string.Format("<b>{0}</b>", panels[i].panelName);
            }
            m_ToolbarNames[panels.Count] = "<b>Close</b>";
        }

        protected override void OnToolbarIndex(int index)
        {
            if (index == panels.Count)
            {
                DebugTool.Instance.minimize = true;
                return;
            }
            base.OnToolbarIndex(index);
        }
    }
}
