namespace Framework
{
    public interface IProdureManager<T>
    {
        void Update(float detlaTime);

        void RunProdurce();

        void Reset();

        void Add<TState>() where TState : ProdureBase<T>;

        void Add<TState>(TState state) where TState : ProdureBase<T>;
    }
}