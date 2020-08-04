
namespace Shop.Tests.Bulders
{
    public abstract class Builder<T> where T: new()
    {
        protected T _object;

        public Builder()
        {
            _object = new T();
        }
        private T Build()
        {
            return _object;
        }

        public static implicit operator T(Builder<T> builder)
        {
            return builder.Build();
        }
    }
}
