using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suites.MediatR
{
    public class SimpleNotification<T> : INotification
    {
        public T Item { get; }
        public SimpleNotification(T item)
        {
            Item = item;
        }
    }
}
