using System;
using System.Collections.Generic;
using System.Text;

namespace DoDSamples
{
    public readonly struct ArrayEnumerableByRef<T>
    {
        private readonly T[] _target;

        public ArrayEnumerableByRef(T[] target) => _target = target;

        public Enumerator GetEnumerator() => new Enumerator(_target);

        public struct Enumerator
        {
            private readonly T[] _target;

            private int _index;

            public Enumerator(T[] target)
            {
                _target = target;
                _index = -1;
            }

            public readonly ref T Current
            {
                get
                {
                    if (_target is null || _index < 0 || _index > _target.Length)
                    {
                        throw new InvalidOperationException();
                    }
                    return ref _target[_index];
                }
            }

            public bool MoveNext() => ++_index < _target.Length;

            public void Reset() => _index = -1;
        }
    }

    public static class ArrayExtensions
    {
        public static ArrayEnumerableByRef<T> ToEnumerableByRef<T>(this T[] array) => new ArrayEnumerableByRef<T>(array);
        public static ArrayEnumerableByRef<T> ToEnumerableByRef<T>(this List<T> list) => new ArrayEnumerableByRef<T>(list.ToArray());
    }
}
