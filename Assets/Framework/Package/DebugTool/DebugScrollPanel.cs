namespace Framework.Debug
{
    public abstract class DebugScrollPanel : DebugPanel
    {
        DraggableScrollView m_DragScrollView = new DraggableScrollView();

        public override void OnGUI()
        {
            using (m_DragScrollView.Scope())
            {
                DrawContent();
            }
        }

        protected abstract void DrawContent();
    }
}