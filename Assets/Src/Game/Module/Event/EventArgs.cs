namespace Game
{
    interface IEventArgs
    {
        int id { get; }
    }

    struct EventArgs : IEventArgs
    {
        public int id { get; private set; }

        public EventArgs(int id)
        {
            this.id = id;
        }
    }

    struct EventArgs<T> : IEventArgs
    {
        public int id { get; private set; }
        public T value { get; private set; }

        public EventArgs(int id, T value)
        {
            this.id = id;
            this.value = value;
        }
    }

    struct UIEventArgs : IEventArgs
    {
        public int id { get; private set; }
        public BaseUI ui { get; private set; }

        public UIEventArgs(int id, BaseUI ui)
        {
            this.id = id;
            this.ui = ui;
        }
    }
}
