using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUI
{
    public class VirtualizingBatchList<T> : IList<T>, INotifyCollectionChanged
    {
        private readonly List<T> _allItems; // Danh sách chứa tất cả dữ liệu
        private readonly List<T> _loadedItems = new List<T>(); // Danh sách các mục đã tải
        private readonly int _batchSize;
        private int _loadedCount = 0;

        public VirtualizingBatchList(List<T> allItems, int batchSize)
        {
            _allItems = allItems ?? throw new ArgumentNullException(nameof(allItems));
            _batchSize = batchSize;
        }

        public async Task LoadNextBatchAsync()
        {
            int startIndex = _loadedCount;
            int itemsToLoad = Math.Min(_batchSize, _allItems.Count - _loadedCount);

            for (int i = startIndex; i < startIndex + itemsToLoad; i++)
            {
                _loadedItems.Add(_allItems[i]);
                _loadedCount++;
                if (i % 10 == 0) // Nhường luồng UI sau mỗi 10 bản ghi
                {
                    await Task.Yield();
                }
            }

            NotifyCollectionChanged();
        }

        public T this[int index]
        {
            get => _loadedItems[index];
            set => throw new NotSupportedException();
        }

        public int Count => _allItems.Count;

        public bool IsReadOnly => true;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void NotifyCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Add(T item) => throw new NotSupportedException();
        public void Clear() => throw new NotSupportedException();
        public bool Contains(T item) => _loadedItems.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _loadedItems.CopyTo(array, arrayIndex);
        public bool Remove(T item) => throw new NotSupportedException();
        public int IndexOf(T item) => _loadedItems.IndexOf(item);
        public void Insert(int index, T item) => throw new NotSupportedException();
        public void RemoveAt(int index) => throw new NotSupportedException();

        public IEnumerator<T> GetEnumerator()
        {
            return _loadedItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
