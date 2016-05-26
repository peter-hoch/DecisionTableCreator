/*
 * [The "BSD license"]
 * Copyright (c) 2016 Peter Hoch
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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
