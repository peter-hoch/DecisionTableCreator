using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DecisionTableCreator.TestCases;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator
{
    /// <summary>
    /// Interaction logic for EditAction.xaml
    /// </summary>
    public partial class EditAction : Window
    {
        public EditAction(ActionObject actionObject)
        {
            DataContext = actionObject;
            InitializeComponent();
        }

        ActionObject DataContainer { get { return (ActionObject)DataContext; } }

        private void AppendEnumValue_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataContainer.EnumValues.Add(new EnumValue("new name", "new value"));
        }

        private void AppendEnumValue_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DeleteEnumValue_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid dataGrid = e.Source as DataGrid;
            DependencyObject dep = e.OriginalSource as DependencyObject;
            if (dataGrid != null && dep != null)
            {
                var dataGridRow = WpfTools.SearchForParent(dep, typeof(DataGridRow), false);
                if (dataGridRow != null)
                {
                    int index = dataGrid.ItemContainerGenerator.IndexFromContainer(dataGridRow);
                    if (index >= 0 && index < dataGrid.Items.Count)
                    {
                        DataContainer.EnumValues.RemoveAt(index);
                    }
                }
            }
        }

        private void DeleteEnumValue_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid dataGrid = e.Source as DataGrid;
            DependencyObject dep = e.OriginalSource as DependencyObject;
            if (dataGrid != null && dep != null)
            {
                if (dataGrid.Items.Count > 1)
                {
                    var dataGridCell = WpfTools.SearchForParent(dep, typeof(DataGridCell), false);
                    if (dataGridCell != null)
                    {
                        e.CanExecute = true;
                    }
                }
            }
        }

        private void OnOk_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
