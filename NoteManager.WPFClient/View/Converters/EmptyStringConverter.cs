using System;
using System.Windows.Data;

namespace NoteManagerUI.View
{
    public class EmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            object result = this.EmptyStringToNull(value);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            object result = this.EmptyStringToNull(value);
            return result;
        }

        /// <summary>
        /// Rendre les string vide comme NULL
        /// </summary>
        private object EmptyStringToNull(object value)
        {
            string stringValue = (string)value;

            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }
            return value;
        }
    }
}
