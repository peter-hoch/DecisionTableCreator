using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    public interface IConditionAction
    {
        int LoadSaveId { get; set; }

        ValueDataType DataType { get; }

        string Text { get; set; }

        bool DefaultBool { get; set; }

        string DefaultText { get; set; }

        ObservableCollection<EnumValue> EnumValues { get; set; }

        string TooltipText { get; set; }

        int SelectedItemIndex { get; set; }

        bool BoolValue { get; set; }

        Brush Background { get; set; }

    }

    /// <summary>
    /// base class for condition and action
    /// </summary>
    public class ConditionActionBase : IConditionAction
    {

        protected ConditionActionBase()
        {
        }

        protected ConditionActionBase(string text, ValueDataType type) : this()
        {
            Text = text;
            DataType = type;
        }

        protected ConditionActionBase(string text, ObservableCollection<EnumValue> enums) : this(text, ValueDataType.Enumeration)
        {
            EnumValues = enums;
        }

        public void Merge(ConditionActionBase clone)
        {
            if (Text != clone.Text)
            {
                Text = clone.Text;
            }
            if (DataType != clone.DataType)
            {
                DataType = clone.DataType;
            }
            if (DefaultText != clone.DefaultText)
            {
                DefaultText = clone.DefaultText;
            }
            DefaultBool = clone.DefaultBool;
            Background = clone.Background;

            EnumValues.Clear();
            foreach (EnumValue value in clone.EnumValues)
            {
                EnumValues.Add(value);
            }
        }

        /// <summary>
        /// clone the object and all its content
        /// </summary>
        /// <param name="clone"></param>
        protected void Clone(IConditionAction clone)
        {
            ConditionActionBase co = (ConditionActionBase)clone;
            co.Text = Text;
            co.DataType = DataType;
            co.DefaultBool = DefaultBool;
            co.DefaultText = DefaultText;
            co.Background = Background;
            co.EnumValues = new ObservableCollection<EnumValue>();
            foreach (EnumValue value in EnumValues)
            {
                co.EnumValues.Add(value.Clone());
            }
        }

        public IList<ValueObject> TestValues
        {
            get
            {
                var result = new List<ValueObject>();
                if (TestCasesRoot != null && TestCasesRoot.TestCases != null && TestCasesRoot.TestCases.Count != 0)
                {
                    if (this is ConditionObject)
                    {
                        GetTestValues(result, TestCase.CollectionType.Conditions);
                    }
                    else
                    {
                        GetTestValues(result, TestCase.CollectionType.Actions);
                    }
                }
                return result;
            }
        }

        private void GetTestValues(List<ValueObject> result, TestCase.CollectionType collectionType)
        {
            int index = CalculateIndexOfConditionOrAction(collectionType);
            foreach (TestCase testCase in TestCasesRoot.TestCases)
            {
                result.Add(testCase.GetValueObject(collectionType, index));
            }
        }

        int CalculateIndexOfConditionOrAction(TestCase.CollectionType collectionType)
        {
            IList<ValueObject> values;
            if (collectionType == TestCase.CollectionType.Conditions)
            {
                values = TestCasesRoot.TestCases[0].Conditions;
            }
            else
            {
                values = TestCasesRoot.TestCases[0].Actions;
            }
            ValueObject value = values.FirstOrDefault(vo => vo.ConditionOrActionParent.Equals(this));
            if (value != null)
            {
                return values.IndexOf(value);
            }
            return -1;
        }

        #region properties

        public TestCasesRoot TestCasesRoot { get; set; }

        public string EditBoxName { get; protected set; }

        /// <summary>
        /// only valid during load and save!!
        /// </summary>
        public int LoadSaveId { get; set; }

        private ValueDataType _dataType;

        public ValueDataType DataType
        {
            get { return _dataType; }
            protected set
            {
                if (value != ValueDataType.Enumeration)
                {
                    throw new NotSupportedException("only enumerations are supported");
                }
                _dataType = value;
                OnPropertyChanged("DataType");
            }
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        public string Name
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Name");
            }
        }

        private bool _defaultBool;

        public bool DefaultBool
        {
            get { return _defaultBool; }
            set
            {
                _defaultBool = value;
                OnPropertyChanged("DefaultBool");
            }
        }

        private string _defaultText;

        public string DefaultText
        {
            get { return _defaultText; }
            set
            {
                _defaultText = value;
                OnPropertyChanged("DefaultText");
            }
        }

        private ObservableCollection<EnumValue> _enumValues = new ObservableCollection<EnumValue>();

        public ObservableCollection<EnumValue> EnumValues
        {
            get { return _enumValues; }
            set
            {
                _enumValues = value;
                OnPropertyChanged("EnumValues");
            }
        }

        private Brush _background = Brushes.White;

        public Brush Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged("Background");
            }
        }

        private string _tooltipText;

        public string TooltipText
        {
            get { return _tooltipText; }
            set
            {
                _tooltipText = value;
                OnPropertyChanged("TooltipText");
            }
        }

        private int _selectedItemIndex;

        public int SelectedItemIndex
        {
            get { return _selectedItemIndex; }
            set
            {
                _selectedItemIndex = value;
                OnPropertyChanged("SelectedItemIndex");
            }
        }


        private Visibility _textBoxVisibility = Visibility.Visible;

        public Visibility TextBoxVisibility
        {
            get { return _textBoxVisibility; }
        }

        private Visibility _checkboxVisibility = Visibility.Collapsed;

        public Visibility CheckboxVisibility
        {
            get { return _checkboxVisibility; }
        }


        private Visibility _comboBoxVisibility = Visibility.Collapsed;

        public Visibility ComboBoxVisibility
        {
            get { return _comboBoxVisibility; }
        }

        private bool _boolValue;

        public bool BoolValue
        {
            get { return _boolValue; }
            set
            {
                _boolValue = value;
                OnPropertyChanged("BoolValue");
            }
        }

        private string _testProperty;

        /// <summary>
        /// for unit testing only
        /// </summary>
        public string TestProperty
        {
            get { return _testProperty; }
            set
            {
                _testProperty = value;
                OnPropertyChanged("TestProperty");
            }
        }



        #endregion

        #region event

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(name);
                PropertyChanged(this, args);
            }
        }

        #endregion

    }

    public class ConditionObject : ConditionActionBase
    {
        private ConditionObject() : base()
        {
            EditBoxName = "Edit condition object";
        }

        public static ConditionObject Create(string text, ValueDataType type)
        {
            ConditionObject ao = new ConditionObject();
            ao.Text = text;
            ao.DataType = type;
            return ao;
        }

        public static ConditionObject Create(string text, ObservableCollection<EnumValue> enums)
        {
            ConditionObject ao = new ConditionObject();
            ao.Text = text;
            ao.DataType = ValueDataType.Enumeration;
            ao.EnumValues = enums;
            return ao;
        }

        public ConditionObject Clone()
        {
            ConditionObject clone = new ConditionObject();
            Clone(clone);
            return clone;
        }



    }

    public class ActionObject : ConditionActionBase
    {
        private ActionObject() : base()
        {
            EditBoxName = "Edit action object";
        }

        public static ActionObject Create(string text, ValueDataType type)
        {
            ActionObject ao = new ActionObject();
            ao.Text = text;
            ao.DataType = type;
            return ao;
        }

        public static ActionObject Create(string text, ObservableCollection<EnumValue> enums)
        {
            ActionObject ao = new ActionObject();
            ao.Text = text;
            ao.DataType = ValueDataType.Enumeration;
            ao.EnumValues = enums;
            return ao;
        }

        public ActionObject Clone()
        {
            ActionObject clone = new ActionObject();
            Clone(clone);
            return clone;
        }

    }
}
