using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Antlr4.StringTemplate.Misc;
using DecisionTableCreator.TestCases;

namespace DecisionTableCreator.ErrorDialog
{
    /// <summary>
    /// Interaction logic for TemplateErrorDialog.xaml
    /// </summary>
    public partial class TemplateErrorDialog : Window
    {
        ErrorDialogDataContainer DataContainer { get { return DataContext as ErrorDialogDataContainer; } }

        public TemplateErrorDialog(StringTemplateResult result)
        {
            InitializeComponent();
            DataContainer.ErrorMessages = new ObservableCollection<TemplateMessageWrapper>();
            DataContainer.ErrorCount = result.ErrorListener.ErrorCount;
            foreach (TemplateMessage msg in result.ErrorListener.ErrorMessages)
            {
                DataContainer.ErrorMessages.Add(new TemplateMessageWrapper(msg));
            }
        }
    }
}
