using System;
using System.Collections.Generic;
using Emma.Common;

namespace Emma.XamlControls.Tests.Builders
{
    public class ExtensionMethodBuilder
    {
        private string _methodName = "TestMethod";
        private string _returnType = "ReturnType";
        private string _extendingType = "ExtendingType";
        private string _sourceLocation = "SourceLoaction";
        private string _className = "ClassName";
        private ExtensionMethodSourceType _sourceType = ExtensionMethodSourceType.Assembly;
        private object _source;
        private List<string> _paramTypes = new List<string>();

        public ExtensionMethod Build()
        {
            return ExtensionMethod.Create(
                _methodName, 
                _extendingType, 
                _returnType, 
                _paramTypes, 
                _sourceType,
                _source, 
                DateTimeOffset.Now, 
                _sourceLocation,
                _className
            );
        }

        public ExtensionMethodBuilder WithSourceLocation(string sourceLocation)
        {
            _sourceLocation = sourceLocation;
            return this;
        }

        public ExtensionMethodBuilder WithSource(string source, ExtensionMethodSourceType sourceType = ExtensionMethodSourceType.SourceCode)
        {
            _sourceType = sourceType;
            _source = source;
            return this;
        }
    }
}