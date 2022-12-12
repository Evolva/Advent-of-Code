using System.Collections;

namespace DataStructures
{
    public class CircularBuffer<TContent> : IEnumerable<TContent>
    {
        private readonly TContent[] _data;
        private readonly int _maxSize;

        private int _currentIndex = 0;
        public int CurrentSize { get; private set; }

        public CircularBuffer(int size)
        {
            _maxSize = size;
            _data = new TContent[size];
            CurrentSize = 0;
        }

        public void Add(TContent item)
        {
            _data[_currentIndex] = item;

            _currentIndex = (_currentIndex + 1) % _maxSize;
            CurrentSize = Math.Min(CurrentSize + 1, _maxSize);
        }

        public IEnumerator<TContent> GetEnumerator() => _data.ToList().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _data.GetEnumerator();
    }
}