using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace DecisionTableCreator.Utils
{

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ObserveForDirtyAttribute : Attribute
    {
    }

    public delegate void DirtyChangedDelegate();

    public interface INotifyDirtyChanged : INotifyPropertyChanged
    {
        event DirtyChangedDelegate DirtyChanged;
        void ResetDirty();
        void FireDirtyChanged();
    }

    public class ObservedItem
    {
        public string Name { get; private set; }

        public INotifyCollectionChanged Collection { get; set; }

        public INotifyPropertyChanged Property { get; set; }

        public INotifyDirtyChanged DirtyChanged { get; set; }

        public PropertyInfo PropertyInfo { get; private set; }

        public ObservedItem(string name, PropertyInfo propInfo)
        {
            Name = name;
            PropertyInfo = propInfo;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", Name, Property != null ? "prop" : "", Collection != null ? "coll" : "");
        }
    }

    /// <summary>
    /// check property changes in order to support a dirty flag
    /// </summary>
    public class DirtyObserver : INotifyPropertyChanged
    {
        public INotifyDirtyChanged ObservedContainer { get; private set; }

        public List<ObservedItem> RelevantPropertyList { get; private set; }

        public DirtyObserver(INotifyDirtyChanged container)
        {
            RelevantPropertyList = new List<ObservedItem>();
            ObservedContainer = container;
            var properties = container.GetType().GetProperties();
            foreach (PropertyInfo propInfo in properties)
            {
                var attribute = propInfo.GetCustomAttributes(typeof(ObserveForDirtyAttribute), false).FirstOrDefault() as ObserveForDirtyAttribute;
                if (attribute != null)
                {
                    ObservedItem item = new ObservedItem(propInfo.Name, propInfo);
                    if (propInfo.PropertyType == typeof(DirtyObserver))
                    {
                        throw new ArgumentOutOfRangeException("Attribute ObserveForDirty is not allowed on item DirtyObserver");
                    }
                    UpdatePropertyChanged(container, item);
                    UpdateCollectionChanged(container, item);
                    RelevantPropertyList.Add(item);
                }
            }
            ObservedContainer.PropertyChanged += ObservedContainerOnPropertyChanged;
        }

        private void PropertyChangedOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Dirty = true;
        }


        private void ObservedContainerOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            ObservedItem item = RelevantPropertyList.FirstOrDefault(prop => prop.Name.Equals(propertyChangedEventArgs.PropertyName));
            if (item != null)
            {
                Dirty = true;
                UpdateCollectionChanged(sender, item);
                UpdatePropertyChanged(sender, item);
            }
        }

        private void UpdateCollectionChanged(object container, ObservedItem item)
        {
            if (item.Collection != null)
            {
                DisconnectFromOldItems(item.Collection as IList);
                item.Collection.CollectionChanged -= CollectionOnCollectionChanged;
            }
            var value = item.PropertyInfo.GetValue(container) as INotifyCollectionChanged;
            if (value != null)
            {
                ConnectToNewItems(value as IList);
                value.CollectionChanged += CollectionOnCollectionChanged;
            }
            item.Collection = value;
        }

        private void UpdatePropertyChanged(object container, ObservedItem item)
        {
            var value = item.PropertyInfo.GetValue(container);
            if (value != null && value is DirtyObserver)
            {
                throw new ArgumentOutOfRangeException("Attribute ObserveForDirty is not allowed on item DirtyObserver");
            }
            if (item.Property != null)
            {
                item.Property.PropertyChanged -= PropertyChangedOnPropertyChanged;
                item.Property = null;
            }

            if (item.DirtyChanged != null)
            {
                item.DirtyChanged.DirtyChanged -= DirtyChangedOnDirtyChanged;
                item.DirtyChanged = null;
            }

            var dirtyChanged = value as INotifyDirtyChanged;
            if (dirtyChanged != null)
            {
                dirtyChanged.DirtyChanged += DirtyChangedOnDirtyChanged;
                item.DirtyChanged = dirtyChanged;
            }
            else
            {
                var propertyChanged = value as INotifyPropertyChanged;
                if (propertyChanged != null)
                {
                    propertyChanged.PropertyChanged += PropertyChangedOnPropertyChanged;
                    item.Property = propertyChanged;
                }
            }
        }

        private void DirtyChangedOnDirtyChanged()
        {
            Dirty = true;
        }

        private void CollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            Dirty = true;
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    ConnectToNewItems(notifyCollectionChangedEventArgs.NewItems);
                    DisconnectFromOldItems(notifyCollectionChangedEventArgs.OldItems);
                    break;
                case NotifyCollectionChangedAction.Move:
                    //nothing to do
                    break;
                case NotifyCollectionChangedAction.Reset:
                    throw new NotSupportedException("this command is not supported - deleted item are not provided");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DisconnectFromOldItems(IList newItems)
        {
            if (newItems != null)
            {
                foreach (object item in newItems)
                {
                    var dirtyChanded = item as INotifyDirtyChanged;
                    if (dirtyChanded != null)
                    {
                        dirtyChanded.DirtyChanged -= DirtyChandedOnDirtyChanged;
                    }
                }
            }
        }

        private void ConnectToNewItems(IList newItems)
        {
            if (newItems != null)
            {
                foreach (object item in newItems)
                {
                    var dirtyChanded = item as INotifyDirtyChanged;
                    if (dirtyChanded != null)
                    {
                        dirtyChanded.DirtyChanged += DirtyChandedOnDirtyChanged;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Observable collection members must implement INotifyDirtyChanged");
                    }
                }
            }
        }

        private void DirtyChandedOnDirtyChanged()
        {
            Dirty = true;
        }


        public void Reset()
        {
            Dirty = false;
        }

        private bool _dirty = false;

        [ObserveForDirty]
        public bool Dirty
        {
            get { return _dirty; }
            private set
            {
                _dirty = value;
                OnPropertyChanged("Dirty");
                ObservedContainer.FireDirtyChanged();
            }
        }

        public string Name { get; set; }

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
}
