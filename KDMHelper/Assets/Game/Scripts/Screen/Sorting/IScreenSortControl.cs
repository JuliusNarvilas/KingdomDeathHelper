using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Screen.Sorting
{
    public enum ESortType
    {
        None,
        Ascending,
        Descending
    }

    public interface IScreenSortControl<T> : IScreenSortReceiver
    {
        IOrderedEnumerable<T> Sort(IOrderedEnumerable<T> i_Source);
    }

    public interface IScreenSortReceiver
    {
        void Append(ScreenSortInfo i_SortInfo);
        void Remove(ScreenSortInfo i_SortInfo);
        void Clear();
        void TriggerOnChange();
    }
}
