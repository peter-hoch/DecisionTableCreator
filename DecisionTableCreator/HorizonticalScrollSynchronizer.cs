using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DecisionTableCreator
{

    public class HorizonticalScrollSynchronizer : DependencyObject
    {
        public static readonly DependencyProperty HorizonticalScrollGroupProperty = DependencyProperty.RegisterAttached("HorizonticalScrollGroup", typeof(string), typeof(HorizonticalScrollSynchronizer), new PropertyMetadata(new PropertyChangedCallback(OnScrollGroupChanged)));

        /// <summary>
        /// dictionary of all registered scroll viewers.
        /// the string is the name of the group
        /// 
        /// </summary>
        private static Dictionary<ScrollViewer, string> _scrollViewers = new Dictionary<ScrollViewer, string>();

        /// <summary>
        /// Contains the latest horizontal scroll offset for each scroll group.
        /// </summary>
        private static Dictionary<string, double> _horizontalScrollOffsets = new Dictionary<string, double>();

        public static void SetHorizonticalScrollGroup(DependencyObject obj, string scrollGroup)
        {
            obj.SetValue(HorizonticalScrollGroupProperty, scrollGroup);
        }
        public static string GetHorizonticalScrollGroup(DependencyObject obj)
        {
            return (string)obj.GetValue(HorizonticalScrollGroupProperty);
        }

        /// <summary>
        /// scroll group changed - add scrollviewer to collection
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>

        private static void OnScrollGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;
            if (scrollViewer != null)
            {
                if (!string.IsNullOrEmpty((string)e.OldValue))
                {
                    // Remove scrollviewer
                    if (_scrollViewers.ContainsKey(scrollViewer))
                    {
                        scrollViewer.ScrollChanged -= new ScrollChangedEventHandler(ScrollViewer_ScrollChanged);
                        _scrollViewers.Remove(scrollViewer);
                    }
                }

                if (!string.IsNullOrEmpty((string)e.NewValue))
                {
                    // If group already exists, set scrollposition of new scrollviewer to the scrollposition of the group
                    if (_horizontalScrollOffsets.Keys.Contains((string)e.NewValue))
                    {
                        scrollViewer.ScrollToHorizontalOffset(_horizontalScrollOffsets[(string)e.NewValue]);
                    }
                    else
                    {
                        _horizontalScrollOffsets.Add((string)e.NewValue, scrollViewer.HorizontalOffset);
                    }

                    // Add scrollviewer
                    _scrollViewers.Add(scrollViewer, (string)e.NewValue);
                    scrollViewer.ScrollChanged += new ScrollChangedEventHandler(ScrollViewer_ScrollChanged);
                }
            }
        }

        private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0)
            {
                var changedScrollViewer = sender as ScrollViewer;
                Scroll(changedScrollViewer);
            }
        }


        /// <summary>
        /// sync scroll viewers
        /// </summary>
        /// <param name="changedScrollViewer"></param>
        private static void Scroll(ScrollViewer changedScrollViewer)
        {
            var group = _scrollViewers[changedScrollViewer];
            _horizontalScrollOffsets[group] = changedScrollViewer.HorizontalOffset;

            foreach (var scrollViewer in _scrollViewers.Where((s) => s.Value == group && s.Key != changedScrollViewer))
            {
                if (scrollViewer.Key.HorizontalOffset != changedScrollViewer.HorizontalOffset)
                {
                    scrollViewer.Key.ScrollToHorizontalOffset(changedScrollViewer.HorizontalOffset);
                }
            }
        }
    }
}
