using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SPKCollections
{
    public class MovingBindingList<T> : BindingList<T>
    {
        public MovingBindingList() : base()
        {
        }

        public MovingBindingList(IList<T> list) : base(list)
        {
        }

        public void Move(T item, int index)
        {
            bool raiseListChangedEvents = RaiseListChangedEvents;
            try
            {
                RaiseListChangedEvents = false;
                int oldIndex = IndexOf(item);
                Remove(item);
                InsertItem(index, item);
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemMoved, index, oldIndex));
            }
            finally
            {
                RaiseListChangedEvents = raiseListChangedEvents;
            }
        }
    }
}
