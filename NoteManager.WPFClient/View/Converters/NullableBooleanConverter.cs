﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace NoteManagerUI.View
{
    public class NullableBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            object result = this.NullableBooleanToFalse(value);
            return result;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            object result = this.NullableBooleanToFalse(value);
            return result;
        }

        private object NullableBooleanToFalse(object value)
        {
            if (value == null)
            {
                return false;
            }
            else
            {
                return value;
            }
        }
    }
}
