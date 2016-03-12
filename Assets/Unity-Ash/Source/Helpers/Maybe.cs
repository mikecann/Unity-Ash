namespace Ash.Helpers
{
    /// <summary>
    /// Represents a potentially null return
    /// Borrowed from https://lostechies.com/derickbailey/2010/09/30/monads-in-c-which-part-is-the-monad/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Maybe<T>
    {
        public static Maybe<T> None = new Maybe<T>(default(T));

        public T Value { get; set; }

        public bool HasValue
        {
            get
            {
                var hasValue = Value != null && !Value.Equals(default(T));
                return hasValue;
            }
        }

        public Maybe(T value)
        {
            Value = value;
        }
    }
}
