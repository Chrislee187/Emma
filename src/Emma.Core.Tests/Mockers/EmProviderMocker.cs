using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emma.Common;
using Emma.Common.ExtensionMethodProviders;
using Moq;

namespace Emma.Core.Tests.Mockers
{
    public class EmProviderMocker
    {
        private Mock<ICachedEmProvider> _mock;
        private DateTimeOffset _lastUpdated;
        private IEnumerable<ExtensionMethod> _methods;

        public EmProviderMocker()
        {
            _lastUpdated = DateTimeOffset.MinValue;
            _methods = new List<ExtensionMethod>();
            _mock = new Mock<ICachedEmProvider>();
        }

        public EmProviderMocker With(DateTimeOffset lastUpdated, IEnumerable<ExtensionMethod> methods)
        {
            _lastUpdated = lastUpdated;
            _methods = methods;
            return this;
        }

        public Mock<ICachedEmProvider> Build()
        {
            _mock = new Mock<ICachedEmProvider>();
            SetupReturns(_lastUpdated, _methods);

            return _mock;
        }

        private void SetupReturns(DateTimeOffset timestamp, IEnumerable<ExtensionMethod> extensionMethods)
        {
            _mock.Setup(m => m.LastUpdated(false))
                .Returns(Task.FromResult(timestamp));
            _mock.Setup(m => m.Provide(false))
                .Returns(Task.FromResult(extensionMethods));
        }

        public void VerifyProvideWasCalled() => 
            _mock.Verify(m => m.Provide(false), Times.Once);

        public void VerifySetCache(DateTimeOffset srcDate, IEnumerable<ExtensionMethod> srcMethods) =>
            _mock.Verify(m
                    => m.SetCache(
                        It.Is<DateTimeOffset>(dto => dto == srcDate),
                        It.Is<IEnumerable<ExtensionMethod>>(
                            em => em.SequenceEqual(srcMethods.ToArray()))),
                Times.Once);
    }
}