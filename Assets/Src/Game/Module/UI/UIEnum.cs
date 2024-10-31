public enum UILayer
{
    Backgroup,      // 背景
    Panel,          // 主面板
    Dialog,         // 对话框
    MessageBox,     // 消息框
    Prompt,         // 提示信息
    Tooltip,        // 悬浮提示
    Loading,        // 加载界面
}

public enum UIHideType
{
    Hide,           // 隐藏
    Destroy,        // 销毁
    WaitDestroy,    // 等待一段时间后销毁
}

public enum UIHideFunc
{
    Deactive,           // 关闭
    MoveOutOfScreen,    // 移出屏幕外
}

public enum UIEscClose
{
    DontClose,      // 不关闭
    Close,          // 关闭
    Block,          // 不关闭且阻止下层界面关闭
}