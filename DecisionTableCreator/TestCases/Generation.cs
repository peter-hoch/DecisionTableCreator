using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.TestCases
{
    /// <summary>
    /// the interface for code generation
    /// </summary>
    public interface ITestCasesRoot
    {
        ObservableCollection<TestCase> TestCases { get; }
        ObservableCollection<ConditionObject> Conditions { get; }
        ObservableCollection<ActionObject> Actions { get; }
    }

    public interface ITestCase
    {
        String Name { get; }
        ObservableCollection<ValueObject> Conditions { get; }
        ObservableCollection<ValueObject> Actions { get; }
    }

    public interface IConditionActionObject
    {
        string Name { get; }
        IList<ValueObject> TestValues { get; }
        Background Background { get; }
        ObservableCollection<EnumValue> EnumValues { get; }
        string Comment { get; }
    }

    public interface IValueObject
    {
        ObservableCollection<EnumValue> EnumValues { get; }
        Background Background { get; }
        object Value { get; }
    }

    public interface IEnumValue
    {
        string Name { get; }
        String Value { get; }
        bool IsDefault { get; }
        bool IsInvalid { get; }
        bool DontCare { get; }
    }

    public interface IBackground
    {
        BackgroundColor BackgroundColor { get; }
        string HtmlColor { get; }
    }

}
