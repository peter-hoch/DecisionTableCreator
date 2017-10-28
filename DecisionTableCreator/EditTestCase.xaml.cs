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

namespace DecisionTableCreator
{
    /// <summary>
    /// Interaction logic for EditTestCase.xaml
    /// </summary>
    public partial class EditTestCase : Window
    {
        public EditTestCase(EditTestCaseDataContainer dataContainer)
        {
            InitializeComponent();
            DataContext = dataContainer;
        }

        private void OnOk_OnClick(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            DialogResult = true;
            Close();
        }

        private void OnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            Close();
        }

    }
}
