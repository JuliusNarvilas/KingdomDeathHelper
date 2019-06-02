using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Display.Table
{
    public enum ESortType
    {
        None,
        Ascending,
        Descending
    }

    public interface ITableSortControl<T> : ITableSortReceiver
    {
        IOrderedEnumerable<T> Sort(IOrderedEnumerable<T> i_Source);
    }

    public interface ITableSortReceiver
    {
        void TriggerDisabledSortInfo(TableSortInfo i_SortInfo);
        void TriggerEnabledSortInfo(TableSortInfo i_SortInfo);

        void ApplySort(TableSortInfo i_SortInfo);
        void RemoveSort(TableSortInfo i_SortInfo);
        void ClearSortInfo();
        void TriggerOnSortChange();
    }
}
