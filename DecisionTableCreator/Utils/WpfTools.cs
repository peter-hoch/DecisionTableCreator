using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

    }
}
