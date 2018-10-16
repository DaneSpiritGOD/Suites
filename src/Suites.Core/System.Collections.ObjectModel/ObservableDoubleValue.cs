﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Collections.ObjectModel
{
    public class ObservableDoubleValue : INotifyPropertyChanged
    {
        private double _value;

        /// <summary>
        /// Value in he chart
        /// </summary>
        public double Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value);
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of ObservableValue class
        /// </summary>
        public ObservableDoubleValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of ObservableValue class with a given value
        /// </summary>
        /// <param name="value"></param>
        public ObservableDoubleValue(double value)
        {
            Value = value;
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
