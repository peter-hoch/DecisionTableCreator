﻿/*
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using DecisionTableCreator.TestCases;

namespace DecisionTableCreator.Utils
{
    public class WpfTools
    {
        public static DependencyObject SearchForParent(DependencyObject dep, Type typeofParent, bool trace)
        {
            if (trace) { Debug.Write(" " + dep.GetType().Name); }
            if (dep.GetType() == typeofParent)
            {
                return dep;
            }
            var parent = VisualTreeHelper.GetParent(dep);
            if (parent != null)
            {
                return SearchForParent(parent, typeofParent, trace);
            }
            return null;
        }

        public static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            if (childCount != 0)
            {
                for (int idx = 0; idx < childCount; idx++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, idx);
                    Trace.WriteLine(child.GetType().Name);
                    T result = child as T;
                    if (result != null)
                    {
                        return result;
                    }
                    else
                    {
                        T found = FindVisualChild<T>(child);
                        if (found != null)
                        {
                            return found;
                        }
                    }
                }
            }
            return null;
        }


        public static DataGridCell GetDataGridCell(DataGrid dataGrid, DataGridRow rowContainer, int column)
        {
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = WpfTools.FindVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    /* if the row has been virtualized away, call its ApplyTemplate() method
                     * to build its visual tree in order for the DataGridCellsPresenter
                     * and the DataGridCells to be created */
                    rowContainer.ApplyTemplate();
                    presenter = WpfTools.FindVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                if (presenter != null)
                {
                    DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    if (cell == null)
                    {
                        /* bring the column into view
                         * in case it has been virtualized away */
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                        cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    }
                    return cell;
                }
            }
            return null;
        }


        public static void SetFocusOnNewCreatedColumn(DataGrid DataGrid, int rowIndex)
        {
            DataGrid.Focus();

            DataGridRow rowContainer = DataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            if (rowContainer == null)
            {
                DataGrid.SelectedIndex = rowIndex;
                DataGrid.ScrollIntoView(DataGrid.SelectedItem);
                rowContainer = DataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            }
            if (rowContainer != null)
            {
                rowContainer.ApplyTemplate();
                DataGridCellsPresenter presenter = WpfTools.FindVisualChild<DataGridCellsPresenter>(rowContainer);
                DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(0) as DataGridCell;
                if (cell == null)
                {
                    /* bring the column into view in case it has been virtualized away */
                    DataGrid.ScrollIntoView(rowContainer, DataGrid.Columns[0]);
                    cell = presenter.ItemContainerGenerator.ContainerFromIndex(0) as DataGridCell;
                }
                if (cell != null)
                {
                    cell.Focus();
                }
            }
        }
    }
}
