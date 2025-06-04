using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaUI.Commons
{
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        private bool _suppressNotification = false;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
                base.OnCollectionChanged(e);
        }

        public void AddRange(IEnumerable<T> list)
        {
            if (list == null)
                return;

            // Tạm thời tắt thông báo
            _suppressNotification = true;

            // Thêm tất cả các mục từ danh sách vào collection bên trong
            foreach (T item in list)
            {
                Add(item);
            }

            // Bật lại thông báo
            _suppressNotification = false;

            // Phát ra MỘT thông báo DUY NHẤT để báo cho UI rằng collection đã được "reset" (cập nhật hàng loạt)
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public void RemoveRange(int index, int count)
        {
            if (index < 0 || count <= 0 || index + count > Count)
                throw new ArgumentOutOfRangeException();

            _suppressNotification = true;

            for (int i = 0; i < count; i++)
            {
                RemoveAt(index);
            }

            _suppressNotification = false;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

    }
}
