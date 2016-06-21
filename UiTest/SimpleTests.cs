using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WPFUIItems;

namespace UiTest
{
    [TestFixture]
    public class SimpleTests
    {
        [Test]
        public void StartApplicationTest()
        {
            string workPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);

            Application application = Application.Launch(Path.Combine(workPath, "DecisionTableCreator.exe"));
            Window window = application.GetWindow(SearchCriteria.ByAutomationId("MainWindow"), InitializeOption.NoCache);

            Menu menuItemFile = window.Get(SearchCriteria.ByText("File")) as Menu;
            Menu menuItemExit = menuItemFile.SubMenu("Exit");
            menuItemExit.Click();
        }

        [TestCase(5)]
        public void AddAndRemoveCondition(int repeatCount)
        {
            string workPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);

            ProcessStartInfo info = new ProcessStartInfo(Path.Combine(workPath, "DecisionTableCreator.exe"));
            Application application = Application.AttachOrLaunch(info);
            Window mainWnd = application.GetWindow(SearchCriteria.ByAutomationId("MainWindow"), InitializeOption.NoCache);

            for (int repeatIdx = 0; repeatIdx < repeatCount; repeatIdx++)
            {
                for (int condIdx = 0; condIdx < 2; condIdx++)
                {
                    AddCondition(mainWnd, application);
                }
                for (int tcIdx = 0; tcIdx < 2; tcIdx++)
                {
                    AddTestCase(mainWnd, application);
                }

                for (int idx = 0; idx < 2; idx++)
                {
                    Menu menuItemEdit = mainWnd.Get(SearchCriteria.ByText("Edit")) as Menu;
                    Menu menuItemDeleteTestCase = menuItemEdit.SubMenu(SearchCriteria.ByAutomationId("MenuItemDeleteMostRightTestCase"));
                    menuItemDeleteTestCase.Click();

                    menuItemEdit = mainWnd.Get(SearchCriteria.ByText("Edit")) as Menu;
                    Menu menuItemDeleteCond = menuItemEdit.SubMenu(SearchCriteria.ByAutomationId("MenuItemDeleteBottomCondition"));
                    menuItemDeleteCond.Click();
                }
            }
            Menu menuItemFile = mainWnd.Get(SearchCriteria.ByText("File")) as Menu;
            Menu menuItemExit = menuItemFile.SubMenu("Exit");
            menuItemExit.Click();

            Window saveWindow = mainWnd.ModalWindow(SearchCriteria.ByText("Save"));
            Button btnNo = saveWindow.Get<Button>("No");
            btnNo.Click();
        }

        static Window GetCurrentPopUpMenu()
        {

            List<Window> windows = WindowFactory.Desktop.DesktopWindows();
            foreach (Window w in windows)
            {
                if (w.Name == "") return w;
            }
            return null;
        }


        private static void AddCondition(Window mainWnd, Application application)
        {
            Menu menuItemEdit = mainWnd.Get(SearchCriteria.ByText("Edit")) as Menu;
            Menu menuItemAppendCond = menuItemEdit.SubMenu(SearchCriteria.ByAutomationId("MenuItemAppendCondition"));
            menuItemAppendCond.Click();

            Window editCondWnd = application.GetWindow(SearchCriteria.ByAutomationId("EditConditionWindow"), InitializeOption.NoCache);
            Button btnOk = editCondWnd.Get(SearchCriteria.ByText("Ok")) as Button;
            btnOk.Click();
        }

        private static void AddTestCase(Window mainWnd, Application application)
        {
            Menu menuItemEdit = mainWnd.Get(SearchCriteria.ByText("Edit")) as Menu;
            Menu menuItemAppendCond = menuItemEdit.SubMenu(SearchCriteria.ByAutomationId("MenuItemAppendTestCase"));
            menuItemAppendCond.Click();

        }
        private static void DeleteTestCase(Window mainWnd, Application application)
        {
            Menu menuItemEdit = mainWnd.Get(SearchCriteria.ByText("Edit")) as Menu;
            Menu menuItemAppendCond = menuItemEdit.SubMenu(SearchCriteria.ByAutomationId("MenuItemAppendTestCase"));
            menuItemAppendCond.Click();

        }
    }
}
