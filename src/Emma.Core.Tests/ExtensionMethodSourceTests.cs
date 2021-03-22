using System;
using System.Linq;
using Emma.Common.MethodSources;
using Emma.Core.Tests.Builders;
using Emma.Core.Tests.Mockers;
using NUnit.Framework;

namespace Emma.Core.Tests
{
    [TestFixture]
    public class ExtensionMethodSourceTests
    {
        private EmProviderMocker _cacheMocker;
        private EmProviderMocker _sourceMocker;

        [SetUp]
        public void SetUp()
        {
            _cacheMocker = new EmProviderMocker();
            _sourceMocker = new EmProviderMocker();
        }

        [Test]
        public void First_use_of_source_inits_local_with_original()
        {
            var srcDate = DateTimeOffset.Now;
            var srcMethods = 
                new ExtensionMethodsBuilder()
                .Build()
                .ToList();
            
            _sourceMocker.With(srcDate, srcMethods);

            var src = new ExtensionMethodsSource(
                _sourceMocker.Build().Object, 
                _cacheMocker.Build().Object);

            var x = src.Methods;
            _sourceMocker.VerifyProvideWasCalled();

            _cacheMocker.VerifySetCache(srcDate, srcMethods);
        }
    }
}