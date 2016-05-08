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

namespace DecisionTableCreator
{
    /// <summary>
    /// Interaction logic for EditAction.xaml
    /// </summary>
    public partial class EditAction : Window
    {
        public EditAction(ActionObject actionObject)
        {
            InitializeComponent();
            DataContext = actionObject;
        }
    }
}
