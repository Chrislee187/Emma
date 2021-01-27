using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using Emma.Core;
using Emma.Core.MethodSources;
using NUnit.Framework;

namespace Emma.XamlControls.Tests
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class UnitTest1
    {
        private MainEmmaToolWindowControl _control;

        [SetUp]
        public void SetUp()
        {
            var mis = ExtensionMethodParser.Parse(typeof(MainEmmaToolWindowControl).Assembly);
            var src = new ExtensionMethodsSource() { Methods = mis, LastUpdated = DateTimeOffset.Now};
            
            
            var library = new ExtensionMethodLibrary(src);
            _control = new MainEmmaToolWindowControl
            {
                DataContext = new MainEmmaToolWindowViewModel(library)
            };
        }
        
        [Test]
        public void TestMethod1()
        {
        }
    }
}
