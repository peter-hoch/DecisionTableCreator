/*
 * [The "BSD license"]
 * Copyright (c) 2016 Peter Hoch
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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
