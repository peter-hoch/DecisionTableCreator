using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    public partial class TestCasesRoot
    {
        public void Save(string savePath)
        {
            LoadSaveVisitor save = new LoadSaveVisitor(savePath);
            save.Save(this);
        }

        public void Load(string filePath)
        {
            Init();
            LoadSaveVisitor load = new LoadSaveVisitor(filePath);
            load.Load(this);
            CreateInfosForDatagrid();
       }

        private void CreateInfosForDatagrid()
        {
            CreateBasicColumnDescriptions();
            AddColumnDescriptionsForTestCases();

            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);

            FireActionsChanged();
            ProcessConditionsChanged();
        }

        public void New()
        {
            Init();
            CreateInfosForDatagrid();
        }

    }
}

