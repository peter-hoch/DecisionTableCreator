﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using DecisionTableCreator.DynamicTable;
using DecisionTableCreator.TestCases;
using DecisionTableCreator.Utils;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace DecisionTableCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer _timer;
        public bool CalculateStatistics { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            CalculateStatistics = false;
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0,0,0,1);
            _timer.Tick += TimerOnTick;
            _timer.Start();
            SearchForTemplatesAndCreateSubmenu();
            DataContainer.ResetDirty();
        }

        private void SearchForTemplatesAndCreateSubmenu()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DecisionTableCreatorTemplates"));
                if (!di.Exists)
                {
                    di.Create();
                }
                FileInfo fi = new FileInfo(System.IO.Path.Combine(di.FullName, "Sample.file.stg"));
                if (!fi.Exists)
                {
                    File.WriteAllText(fi.FullName, Templates.Resources.Sample_file);
                }
                foreach (FileInfo fileInfo in di.GetFiles("*.stg"))
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = fileInfo.Name;
                    menuItem.Command = WpfCommands.ExportToFileWithExternalTemplate;
                    menuItem.CommandParameter = fileInfo.FullName;
                    DataContainer.ExportToFileItem.Add(menuItem);
                }
            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            try
            {
                if (CalculateStatistics)
                {
                    CalculateStatistics = false;
                    DataContainer.TestCasesRoot.FireStatisticsChanged();
                }
            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        void ShowAndLogMessage(string message, Exception ex, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = null, [CallerFilePath] string fileName = null)
        {
            string text = String.Format("{0} in method({3}) file({4}/{2})"+Environment.NewLine+"{1}", message, ex, lineNumber, memberName, fileName);
            Trace.WriteLine(text);
#if DEBUG
            MessageBox.Show(this, text);
#endif
        }


        DataContainer DataContainer { get { return (DataContainer)DataContext; } }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            try
            {
                DataGrid dataGrid = sender as DataGrid;
                DataGridTextColumn col = e.Column as DataGridTextColumn;

                //// exchange DataGridTextColumn with DataGridTemplateColumn and GridCellControl

                DataGridHeader header = new DataGridHeader(col.Header.ToString(), dataGrid.Columns.Count);

                DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
                templateColumn.Header = header;

                Binding bind = new Binding(col.Header.ToString());
                bind.Mode = BindingMode.TwoWay;

                if (col.Header.Equals(TestCasesRoot.ActionsColumnHeaderName) || col.Header.Equals(TestCasesRoot.ConditionsColumnHeaderName))
                {
                    // this is action or condition column
                    FrameworkElementFactory gridCellControl = new FrameworkElementFactory(typeof(ConditionOrActionGridCellControl));
                    gridCellControl.SetBinding(DataContextProperty, bind);
                    DataTemplate dataTemplate = new DataTemplate();
                    dataTemplate.VisualTree = gridCellControl;
                    templateColumn.CellTemplate = dataTemplate;
                }
                else
                {
                    FrameworkElementFactory gridCellControl = new FrameworkElementFactory(typeof(GridCellControl));
                    gridCellControl.SetBinding(DataContextProperty, bind);
                    DataTemplate dataTemplate = new DataTemplate();
                    dataTemplate.VisualTree = gridCellControl;
                    templateColumn.CellTemplate = dataTemplate;

                }
                e.Column = templateColumn;

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void DataGrid_OnAutoGeneratedColumns(object sender, EventArgs e)
        {
            try
            {
                DataGrid dataGrid = (DataGrid)sender;
                for (int idx = 0; idx < dataGrid.Columns.Count; idx++)
                {
                    DataGridColumn col = dataGrid.Columns[idx] as DataGridColumn;

                    // add sync column width
                    AddToDictionary(dataGrid, col);
                    var widthPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(DataGridColumn.WidthProperty, typeof(DataGridColumn));
                    widthPropertyDescriptor.AddValueChanged(col, ValueChangedHandler);
                }
                var testcasesByDisplayOrder = DataContainer.TestCasesRoot.TestCases.OrderBy(tc => tc.DisplayIndex);
                foreach (TestCase testCase in testcasesByDisplayOrder)
                {
                    var testCaseCol = dataGrid.Columns.FirstOrDefault(col => col.Header.ToString().Equals(testCase.Name));
                    if (testCaseCol != null)
                    {
                        testCaseCol.DisplayIndex = testCase.DisplayIndex;
                    }
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }


        #region SyncColumnWidth and order

        // dictionary for sync column width
        Dictionary<string, List<DataGridColumnContainer>> ColumnsWidthSyncDictionary = new Dictionary<string, List<DataGridColumnContainer>>();


        void AddToDictionary(DataGrid dataGrid, DataGridColumn col)
        {
            DataGridColumnContainer container = new DataGridColumnContainer(dataGrid, col);

            string key = GetColumnKey(container.Column.Header.ToString());
            if (ColumnsWidthSyncDictionary.ContainsKey(key))
            {
                var result = ColumnsWidthSyncDictionary[key].Where(cc => cc.DataGrid.Equals(dataGrid)).ToArray();
                foreach (DataGridColumnContainer cont in result)
                {
                    ColumnsWidthSyncDictionary[key].Remove(cont);
                }
                ColumnsWidthSyncDictionary[key].Add(container);
            }
            else
            {
                List<DataGridColumnContainer> colContainer = new List<DataGridColumnContainer>();
                ColumnsWidthSyncDictionary.Add(key, colContainer);
                colContainer.Add(container);
            }

        }

        string GetColumnKey(string name)
        {
            if (name.Contains("Action") || name.Contains("Condition"))
            {
                return "ConditionOrAction";
            }
            return name;
        }

        private void ValueChangedHandler(object sender, EventArgs eventArgs)
        {
            DataGridColumn col = sender as DataGridColumn;
            DataGridLength value = col.Width;
            foreach (DataGridColumnContainer column in ColumnsWidthSyncDictionary[GetColumnKey(col.Header.ToString())])
            {
                if (!col.Equals(column.Column))
                {
                    column.Column.Width = value;
                }
            }

        }

        private void DataGrid_OnColumnReordering(object sender, DataGridColumnReorderingEventArgs e)
        {
            // do not move the first column
            if (e.Column.Header.Equals(TestCasesRoot.ActionsColumnHeaderName) || e.Column.Header.Equals(TestCasesRoot.ConditionsColumnHeaderName))
            {
                e.Cancel = true;
            }
        }

        private void DataGrid_OnColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid != null)
            {
                // test case columns are not allowed in the first column
                if (e.Column.DisplayIndex == 0)
                {
                    var colName = dataGrid.Columns.FirstOrDefault(c => c.Header.Equals(TestCasesRoot.ActionsColumnHeaderName) || c.Header.Equals(TestCasesRoot.ConditionsColumnHeaderName));
                    if (colName != null)
                    {
                        colName.DisplayIndex = 0;
                    }
                }
                SyncColumnOrder(dataGrid);
            }
        }

        void SyncColumnOrder(DataGrid dataGrid)
        {
            var colInfos = SaveColumnOrder(dataGrid);

            if (dataGrid == DataGridConditions)
            {
                SetColumnOrder(colInfos, DataGridActions);
            }
            else
            {
                SetColumnOrder(colInfos, DataGridConditions);
            }
        }

        private static List<ColumnInfo> SaveColumnOrder(DataGrid dataGrid)
        {
            List<ColumnInfo> colInfos = new List<ColumnInfo>();

            foreach (DataGridColumn column in dataGrid.Columns)
            {
                colInfos.Add(new ColumnInfo(column.DisplayIndex, column.Header));
            }
            return colInfos;
        }

        private void SetColumnOrder(List<ColumnInfo> colInfos, DataGrid dataGrid)
        {
            foreach (ColumnInfo info in colInfos.OrderBy(ci => ci.DisplayIndex))
            {
                if (info.Header.ToString().Equals(TestCasesRoot.ActionsColumnHeaderName) || info.Header.ToString().Equals(TestCasesRoot.ConditionsColumnHeaderName))
                {
                    var selCol = dataGrid.Columns.FirstOrDefault(col => col.Header.ToString().Equals(TestCasesRoot.ActionsColumnHeaderName) || col.Header.ToString().Equals(TestCasesRoot.ConditionsColumnHeaderName));
                    if (selCol != null)
                    {
                        selCol.DisplayIndex = 0;
                    }
                    else
                    {
                        throw new Exception("problem... unable to find header " + info.Header.ToString());
                    }
                }
                else
                {
                    var selCol = dataGrid.Columns.FirstOrDefault(col => col.Header.ToString().Equals(info.Header.ToString()));
                    if (selCol != null)
                    {
                        selCol.DisplayIndex = info.DisplayIndex;
                    }
                    else
                    {
                        throw new Exception("problem... unable to find header " + info.Header.ToString());
                    }
                    for (int idx = 0; idx < DataContainer.TestCasesRoot.TestCases.Count; idx++)
                    {
                        DataContainer.TestCasesRoot.TestCases[idx].DisplayIndex = dataGrid.Columns[idx + 1].DisplayIndex;
                    }
                }

            }
        }



        #endregion

        private void AppendTestCase_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AppendTestCase_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                DataContainer.TestCasesRoot.InsertTestCase();

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void InsertTestCase_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var tcr = DataContainer.TestCasesRoot;
                int index = CalculateColumnIndex(e, true);
                tcr.InsertTestCase(index - 1);

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void InsertTestCase_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                int index = CalculateColumnIndex(e, true);
                // focus must be at least on first testcase
                if (index >= 1)
                {
                    e.CanExecute = true;
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }


        private void EditConditionOrAction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var index = CalculateRowIndex(e);
                if (index >= 0)
                {
                    if (e.Source.Equals(DataGridConditions))
                    {
                        EditCondition(index);
                    }
                    else
                    {
                        EditAction(index);   
                    }
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void EditCondition(int index)
        {
            ConditionObject original = ((ConditionObject) DataContainer.TestCasesRoot.Conditions[index]);
            ConditionObject coClone = original.Clone();
            EditCondition wnd = new EditCondition(coClone);
            bool? result = wnd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                DataContainer.TestCasesRoot.ChangeCondition(index, coClone);
            }
        }

        private void EditAction(int index)
        {
            ActionObject original = ((ActionObject)DataContainer.TestCasesRoot.Actions[index]);
            ActionObject coClone = original.Clone();
            EditAction wnd = new EditAction(coClone);
            bool? result = wnd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                DataContainer.TestCasesRoot.ChangeAction(index, coClone);
            }
        }


        private static int CalculateRowIndex(ExecutedRoutedEventArgs e)
        {
            DataGrid dataGrid = e.Source as DataGrid;
            DependencyObject dep = e.OriginalSource as DependencyObject;
            DataGridRow dataGridRow = WpfTools.SearchForParent(dep, typeof(DataGridRow), false) as DataGridRow;
            if (dataGridRow != null && dataGrid != null)
            {
                int index = dataGrid.ItemContainerGenerator.IndexFromContainer(dataGridRow);
                return index;
            }
            return -1;
        }

        private static int CalculateColumnIndex(RoutedEventArgs e, bool trace = false)
        {
            DependencyObject dep = e.OriginalSource as DependencyObject;
            DataGridCell dataGridCell = WpfTools.SearchForParent(dep, typeof(DataGridCell), trace) as DataGridCell;
            if (dataGridCell != null)
            {
                return dataGridCell.Column.DisplayIndex;
            }
            return -1;
        }


        private void EditCondition_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                ConditionObject condObject = GetConditionOrActionGridCellControlDataContext(e.Source as DataGrid, e.OriginalSource as DependencyObject) as ConditionObject;
                if (condObject != null)
                {
                    e.CanExecute = true;
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void EditConditionOrAction_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                object condObject = GetConditionOrActionGridCellControlDataContext(e.Source as DataGrid, e.OriginalSource as DependencyObject);
                if (condObject != null && (condObject is ConditionObject || condObject is ActionObject))
                {
                    e.CanExecute = true;
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void EditAction_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                ActionObject actionObject = GetConditionOrActionGridCellControlDataContext(e.Source as DataGrid, e.OriginalSource as DependencyObject) as ActionObject;
                if (actionObject != null)
                {
                    e.CanExecute = true;
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Decision Table Creator Files|*.dtc|All Files|*.*";
                var result = dlg.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    DataContainer.TestCasesRoot.Save(dlg.FileName);
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void Save_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Decision Table Creator Files|*.dtc|All Files|*.*";
                var result = dlg.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    DataContainer.TestCasesRoot.Load(dlg.FileName);
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void NewDocument_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewDocument_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                DataContainer.TestCasesRoot.New();

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }


        private void Open_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AppendAction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                ActionObject newAction = ActionObject.Create("new action", new ObservableCollection<EnumValue>() { new EnumValue("new text", "new value") });
                EditAction wnd = new EditAction(newAction);
                bool? result = wnd.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    DataContainer.TestCasesRoot.AppendAction(newAction);
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void AppendAction_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void InsertAction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var index = CalculateRowIndex(e);
                if (index >= 0)
                {
                    ActionObject newAction = ActionObject.Create("new action", new ObservableCollection<EnumValue>() { new EnumValue("new text", "new value") });
                    EditCondition wnd = new EditCondition(newAction);
                    bool? result = wnd.ShowDialog();
                    if (result.HasValue && result.Value)
                    {
                        DataContainer.TestCasesRoot.InsertAction(index, newAction);
                    }
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void InsertAction_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                DataGrid dataGrid;
                e.CanExecute = IsActionsDataGridSelected(e, out dataGrid);

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void AppendCondition_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                ConditionObject newCondition = ConditionObject.Create("new condition", new ObservableCollection<EnumValue>() { new EnumValue("new text", "new value") });
                EditCondition wnd = new EditCondition(newCondition);
                bool? result = wnd.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    DataContainer.TestCasesRoot.AppendCondition(newCondition);
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void AppendCondition_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void InsertCondition_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var index = CalculateRowIndex(e);
                if (index >= 0)
                {
                    ConditionObject conditionObject = ConditionObject.Create("new condition", new ObservableCollection<EnumValue>() { new EnumValue("new text", "new value") });
                    EditCondition wnd = new EditCondition(conditionObject);
                    bool? result = wnd.ShowDialog();
                    if (result.HasValue && result.Value)
                    {
                        DataContainer.TestCasesRoot.InsertCondition(index, conditionObject);
                    }
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void InsertCondition_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                DataGrid dataGrid;
                e.CanExecute = IsConditionsDataGridSelected(e, out dataGrid);

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void DeleteAction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var index = CalculateRowIndex(e);
                if (index >= 0)
                {
                    DataContainer.TestCasesRoot.DeleteActionAt(index);
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void DeleteAction_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                DataGrid dataGrid;
                e.CanExecute = IsActionsDataGridSelected(e, out dataGrid);

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void DeleteCondition_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var index = CalculateRowIndex(e);
                if (index >= 0)
                {
                    DataContainer.TestCasesRoot.DeleteConditionAt(index);
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void DeleteCondition_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                DataGrid dataGrid;
                e.CanExecute = IsConditionsDataGridSelected(e, out dataGrid);

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void MoveConditionOrActionUp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var index = CalculateRowIndex(e);
                if (index >= 0)
                {
                    DataGrid dataGrid;
                    if (IsConditionsDataGridSelected(e, out dataGrid))
                    {
                        DataContainer.TestCasesRoot.MoveConditionUp(index);
                    }
                    else if (IsActionsDataGridSelected(e, out dataGrid))
                    {
                        DataContainer.TestCasesRoot.MoveActionUp(index);
                    }
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void MoveConditionOrActionDown_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var index = CalculateRowIndex(e);
                if (index >= 0)
                {
                    DataGrid dataGrid;
                    if (IsConditionsDataGridSelected(e, out dataGrid))
                    {
                        DataContainer.TestCasesRoot.MoveConditionDown(index);
                    }
                    else if (IsActionsDataGridSelected(e, out dataGrid))
                    {
                        DataContainer.TestCasesRoot.MoveActionDown(index);
                    }
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void MoveConditionOrActionUp_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                DataGrid dataGrid;
                if (IsConditionsDataGridSelected(e, out dataGrid))
                {
                    e.CanExecute = DataContainer.TestCasesRoot.Conditions.Count > 1;
                }
                else if (IsActionsDataGridSelected(e, out dataGrid))
                {
                    e.CanExecute = DataContainer.TestCasesRoot.Actions.Count > 1;
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void MoveConditionOrActionDown_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                DataGrid dataGrid;
                if (IsConditionsDataGridSelected(e, out dataGrid))
                {
                    e.CanExecute = DataContainer.TestCasesRoot.Conditions.Count > 1;
                }
                else if (IsActionsDataGridSelected(e, out dataGrid))
                {
                    e.CanExecute = DataContainer.TestCasesRoot.Actions.Count > 1;
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void ExportHtmlToClipboard_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                try
                {
                    string text = DataContainer.TestCasesRoot.GenerateFromTemplateString(Templates.Resources.HtmlTemplate);
                    PrepareForClipboard prepare = new PrepareForClipboard();
                    string clipboardText = prepare.Prepare(text);

                    DataObject dataObject = new DataObject();
                    dataObject.SetData(DataFormats.Html, new MemoryStream(Encoding.UTF8.GetBytes(clipboardText)));
                    Clipboard.SetDataObject(dataObject, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("unexpected error during export to clipboard" + Environment.NewLine + ex.ToString());
                }

            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void ExportHtmlToClipboard_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        bool IsConditionsDataGridSelected(RoutedEventArgs args, out DataGrid dataGrid)
        {
            dataGrid = args.Source as DataGrid;
            if (dataGrid != null && dataGrid.Columns.Count > 0)
            {
                if (dataGrid.Columns[0].Header.ToString().Equals(TestCasesRoot.ConditionsColumnHeaderName))
                {
                    return true;
                }
            }
            return false;
        }

        bool IsActionsDataGridSelected(RoutedEventArgs args, out DataGrid dataGrid)
        {
            dataGrid = args.Source as DataGrid;
            if (dataGrid != null && dataGrid.Columns.Count > 0)
            {
                if (dataGrid.Columns[0].Header.ToString().Equals(TestCasesRoot.ActionsColumnHeaderName))
                {
                    return true;
                }
            }
            return false;
        }

        object GetGridCellControlDataContext(DataGrid dataGrid, DependencyObject originalSource, bool trace = false)
        {
            if (dataGrid != null)
            {
                if (originalSource != null)
                {
                    if (trace) { Debug.Write("SearchForParent "); }
                    var parent = WpfTools.SearchForParent(originalSource, typeof(GridCellControl), trace);
                    if (trace) { Debug.WriteLine(""); }
                    if (parent != null)
                    {
                        return ((GridCellControl)parent).DataContext;
                    }
                }
            }
            return null;
        }

        object GetConditionOrActionGridCellControlDataContext(DataGrid dataGrid, DependencyObject originalSource, bool trace = false)
        {
            if (dataGrid != null)
            {
                if (originalSource != null)
                {
                    if (trace) { Debug.Write("SearchForParent "); }
                    var parent = WpfTools.SearchForParent(originalSource, typeof(ConditionOrActionGridCellControl), trace);
                    if (trace) { Debug.WriteLine(""); }
                    if (parent != null)
                    {
                        return ((ConditionOrActionGridCellControl)parent).DataContext;
                    }
                }
            }
            return null;
        }

        private void ComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            CalculateStatistics = true;
        }

        private void ExportToFileWithExternalTemplate_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExportToFileWithExternalTemplate_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                ExecutedRoutedEventArgs args = e as ExecutedRoutedEventArgs;
                FileInfo fi = new  FileInfo(e.Parameter.ToString());
                if (fi.Exists)
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.Filter = "Text files|*.txt|Source files|*.c;*.cs;*.cpp;*.h|All files|*.*";
                    dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    bool? result = dlg.ShowDialog(this);
                    if (result.HasValue && result.Value)
                    {
                        string text = DataContainer.TestCasesRoot.GenerateFromtemplate(fi.FullName);
                        File.WriteAllText(dlg.FileName, text);
                    }
                }
                else
                {
                    MessageBox.Show(this, "File " + args.Parameter + " not found", "File not found");
                }
            }
            catch (Exception ex)
            {
                ShowAndLogMessage("exception caught", ex);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DataContainer.DirtyObserver.Reset();
        }
    }
}
