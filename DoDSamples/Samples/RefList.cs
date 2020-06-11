using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DoDSamples.Samples
{
    public class RefList<T>
    {
        private T[] array = null;
        private int index = 0;
        private int capacity = 4;

        public RefList(int capacity)
        {
            this.capacity = capacity;
            array = new T[capacity];
        }

        public RefList()
        {
            array = new T[capacity];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T value)
        {
            if(index >= array.Length)
            {
                Expand();
            }

            array[index++] = value;
        }

        public T Get(int index)
        {
            return array[index];
        }

        public void Set(int index, T value)
        {
            array[index] = value;
        }

        public void Expand()
        {
            var newCapacity = array.Length * 2;

            T[] newArray = new T[newCapacity];
            Array.Copy(array, newArray, array.Length);
            array = newArray;

            capacity = newCapacity;
        }

        public T this[int index]
        {
            get { return array[index]; }
            set { array[index] = value; }
        }

        public RefEnumerator GetEnumerator() => new RefEnumerator(array, capacity);

        public struct RefEnumerator
        {
            private T[] array;

            private int index;
            private int capacity;

            public RefEnumerator(T[] target, int capacity)
            {
                array = target;
                index = -1;
                this.capacity = capacity;
            }

            public readonly ref T Current
            {
                get
                {
                    if (array is null || index < 0 || index > capacity)
                    {
                        throw new InvalidOperationException();
                    }
                    return ref array[index];
                }
            }

            public void Dispose()
            {
            }

            public bool MoveNext() => ++index < capacity;

            public void Reset() => index = -1;
        }
    }
}
