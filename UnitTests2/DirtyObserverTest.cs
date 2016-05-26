using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.Utils;
using NUnit.Framework;
using UnitTests2.DirtyObserverTestFiles;

namespace UnitTests2
{
    [TestFixture]
    public class DirtyObserverTest
    {
        [Test]
        public void FirstTest()
        {
            DataContainerTest1 container = new DataContainerTest1();
            DirtyObserver observer = new DirtyObserver(container);
            container.Value1 = 1;
            Assert.That(observer.Dirty == false);
            container.Value2 = 1;
            Assert.That(observer.Dirty == true);

            observer.Reset();
            container.TestObject = null;
            Assert.That(observer.Dirty == true);

            observer.Reset();
            container.TestObject = "test";
            Assert.That(observer.Dirty == true);

            observer.Reset();
            container.TestObject = null;
            Assert.That(observer.Dirty == true);
        }

        [Test]
        public void TestWithChangesInChildren()
        {
            DataContainerTest2 container = new DataContainerTest2();
            container.Children = new ObservableCollection<DataContainerTest2>();
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            container.DirtyObserver.Name = "Parent";

            var child1 = new DataContainerTest2();
            child1.DirtyObserver.Name = "Child1";
            container.Children.Add(child1);
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            var child2 = new DataContainerTest2();
            child1.DirtyObserver.Name = "Child2";
            container.Children.Add(child2);
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            child1.Value2 = 10;
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            child1.Value2 = 10;
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            container.Children.Remove(child1);
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            child1.Value2 = 10;
            Assert.That(container.DirtyObserver.Dirty == false);

            container.DirtyObserver.Reset();
            child2.Value2 = 10;
            Assert.That(container.DirtyObserver.Dirty == true);
        }


        [Test]
        public void TestWithChangesInDirectChildren()
        {
            DataContainerTest2 container = new DataContainerTest2();
            container.Children = new ObservableCollection<DataContainerTest2>();
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            container.DirtyObserver.Name = "Parent";

            Assert.That(container.DirtyObserver.Dirty == false);

            container.DirtyObserver.Reset();
            container.Container1 = new DataContainerTest2();
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            container.Container1.Value2 = 10;
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            container.Container1 = null;
            Assert.That(container.DirtyObserver.Dirty == true);

        }



        [Test]
        public void TestWithCollectionChanges()
        {
            DataContainerTest2 container = new DataContainerTest2();

            container.DirtyObserver.Reset();
            var collection1 = new ObservableCollection<DataContainerTest2>();
            container.Children = collection1;
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            var child1 = new DataContainerTest2();
            container.Children.Add(child1);
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            var child2 = new DataContainerTest2();
            container.Children.Add(child2);
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            container.Children.Remove(child1);
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            container.Children = null;
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            collection1.Add(new DataContainerTest2());
            Assert.That(container.DirtyObserver.Dirty == false);
        }


        [Test]
        public void TestWithCollectionChanges2()
        {
            DataContainerTest2 container = new DataContainerTest2();

            container.DirtyObserver.Reset();
            var collection1 = new ObservableCollection<DataContainerTest2>();
            var coll1Child1 = new DataContainerTest2();
            var coll1Child2 = new DataContainerTest2();
            collection1.Add(coll1Child1);
            collection1.Add(coll1Child2);
            container.Children = collection1;
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            coll1Child2.Value2 = 10;
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            var collection3 = new ObservableCollection<DataContainerTest2>();
            var coll1Child31 = new DataContainerTest2();
            var coll1Child32 = new DataContainerTest2();
            collection3.Add(coll1Child31);
            collection3.Add(coll1Child32);
            container.Children = collection3;
            Assert.That(container.DirtyObserver.Dirty == true);

            container.DirtyObserver.Reset();
            coll1Child2.Value2 = 10;
            Assert.That(container.DirtyObserver.Dirty == false);

            container.DirtyObserver.Reset();
            coll1Child32.Value2 = 10;
            Assert.That(container.DirtyObserver.Dirty == true);

            //not supported
            //container.DirtyObserver.Reset();
            //container.Children.Clear();
            //Assert.That(container.DirtyObserver.Dirty == true);

            //container.DirtyObserver.Reset();
            //coll1Child32.Value2 = 10;
            //Assert.That(container.DirtyObserver.Dirty == false);

        }
    }



}
