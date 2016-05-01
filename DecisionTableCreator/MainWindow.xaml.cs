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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DecisionTableCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataGridTextColumn col = e.Column as DataGridTextColumn;

            // excange DataGridTextColumn with DataGridTemplateColumn and GridCellControl
            DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
            templateColumn.Header = col.Header;

            Binding bind = new Binding(col.Header.ToString());
            bind.Mode = BindingMode.TwoWay;

            FrameworkElementFactory gridCellControl = new FrameworkElementFactory(typeof(GridCellControl));
            gridCellControl.SetBinding(DataContextProperty, bind);
            DataTemplate dataTemplate = new DataTemplate();
            dataTemplate.VisualTree = gridCellControl;

            templateColumn.CellTemplate = dataTemplate;

            e.Column = templateColumn;
    }

}
}
