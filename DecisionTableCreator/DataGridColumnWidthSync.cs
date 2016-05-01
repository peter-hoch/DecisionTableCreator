using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DecisionTableCreator
{
    public class DataGridColumnWidthSync
    {
        public static readonly DependencyProperty DataGridColumnWidthSyncProperty = DependencyProperty.RegisterAttached("DataGridColumnWidthSync", typeof(string), typeof(DataGridColumnWidthSync), new PropertyMetadata(new PropertyChangedCallback(OnDataGridColumnWidthSyncChanged)));

        public static void SetDataGridColumnWidthSync(DependencyObject obj, string scrollGroup)
        {
            obj.SetValue(DataGridColumnWidthSyncProperty, scrollGroup);
        }

        public static string GetDataGridColumnWidthSync(DependencyObject obj)
        {
            return (string)obj.GetValue(DataGridColumnWidthSyncProperty);
        }

        static Dictionary<DataGridColumnHeader, string> _headerColumnsDictionary = new Dictionary<DataGridColumnHeader, string>();

        static Dictionary<string, Size> _headerWidthDictionary = new Dictionary<string, Size>();

        private static void OnDataGridColumnWidthSyncChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGridColumnHeader header = d as DataGridColumnHeader;

            if (header != null)
            {
                if (!string.IsNullOrEmpty((string)e.OldValue))
                {
                    // Remove scrollviewer
                    if (_headerColumnsDictionary.ContainsKey(header))
                    {
                        header.SizeChanged -= new SizeChangedEventHandler(HeaderOnSizeChanged);
                        _headerColumnsDictionary.Remove(header);
                    }
                }

                if (!string.IsNullOrEmpty((string) e.NewValue))
                {

                    if (!_headerColumnsDictionary.ContainsKey(header))
                    {
                        if (header.DataContext != null)
                        {
                            _headerColumnsDictionary.Add(header, header.DataContext.ToString());
                            header.SizeChanged += HeaderOnSizeChanged;
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        private static void HeaderOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            DataGridColumnHeader header = sender as DataGridColumnHeader;

            if (header != null)
            {
                string key = header.DataContext.ToString();
                if (_headerWidthDictionary.ContainsKey(key))
                {
                    _headerWidthDictionary[key] =  sizeChangedEventArgs.NewSize;
                    UpdateSize(key, sizeChangedEventArgs.NewSize);
                }
                else
                {
                    _headerWidthDictionary.Add(key, sizeChangedEventArgs.NewSize);
                }
            }
        }

        private static void UpdateSize(string key, Size newSize)
        {
            var results = _headerColumnsDictionary.Where(kp => kp.Value.Equals(key));
            foreach (KeyValuePair<DataGridColumnHeader, string> keyValuePair in results)
            {
                if (keyValuePair.Key.Width != newSize.Width)
                {
                    keyValuePair.Key.Width = newSize.Width;
                }
            }
        }
    }
}
