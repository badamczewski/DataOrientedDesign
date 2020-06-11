using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DoDSamples
{
    public class BlockList<T>
    {
        private ListBlock[] blocks = new ListBlock[32];
        private int blockIndex = 0;
        private int blockSize = 512;
        private ListBlock mainBlock = null;

        public BlockList()
        {
            mainBlock = new ListBlock(blockSize);
            blocks[blockIndex++] = mainBlock;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T value)
        {
            var blockFull = mainBlock.Add(value);
            //
            // Add another block
            //
            if (blockFull)
            {
                mainBlock = new ListBlock(blockSize);
                blocks[blockIndex++] = mainBlock;
            }
        }

        public T Get(int index)
        {
            var blockId = index / blockSize;
            return blocks[blockId].Get(index - (blockId * blockSize));
        }

        public ListBlock GetBlock(int index, out int indexInBlock)
        {
            var blockId = index / blockSize;
            indexInBlock = index - (blockId * blockSize);
            return blocks[blockId];
        }

        public void Set(int index, T value)
        {
            var blockId = index / blockSize;
            blocks[blockId].Set(index - (blockId * blockSize), value);
        }

        public T this[int index]
        {
            get { return Get(index); }
            set { Set(index, value); }
        }

        public class ListBlock
        {
            public ListBlock(int blockSize)
            {
                array = new T[blockSize];
            }

            public T Get(int index)
            {
                return array[index];
            }

            public void Set(int index, T value)
            {
                array[index] = value;
            }

            private T[] array;
            private int index = 0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Add(T value)
            {
                array[index++] = value;
                return (index >= array.Length);
            }
        }

    }

}
