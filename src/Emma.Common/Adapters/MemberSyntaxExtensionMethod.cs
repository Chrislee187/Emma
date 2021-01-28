using System;
using System.Linq;
using Emma.Common.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Emma.Common.Adapters
{
    internal class MemberSyntaxExtensionMethod : ExtensionMethod
    {
        
        public static ExtensionMethod Create(MethodDeclarationSyntax member,
            DateTimeOffset lastUpdated,
            string className,
            string sourceLocation)
        {
            if (!member.IsExtensionMethod())
            {
                throw new ArgumentException($"member '{member.Name()}' is not an extension method.");
            }

            var extendingType = member.ParameterList.Parameters
                .First().Type.Name();
            var returnType = member.ReturnType.Name();

            var prms = member.ParameterList.Parameters
                .Skip(1)
                .Select(p => p.Type?.Name())
                .ToArray();

            return ExtensionMethod.Create(
                member.Name(),
                extendingType,
                returnType,
                prms,
                ExtensionMethodSourceType.SourceCode,
                member.ToString(),
                lastUpdated,
                sourceLocation,
                className);

        }
        protected MemberSyntaxExtensionMethod(MethodDeclarationSyntax member, 
            DateTimeOffset lastUpdated, 
            string className, 
            string sourceLocation)
        {
            if (!member.IsExtensionMethod())
            {
                throw new ArgumentException($"member '{member.Name()}' is not an extension method.");
            }

            var extendingType = member.ParameterList.Parameters
                .First().Type.Name();
            var returnType = member.ReturnType.Name();

            var prms = member.ParameterList.Parameters
                .Skip(1)
                .Select(p => p.Type?.Name())
                .ToArray();

            Name = member.Name();
            ExtendingType = NormaliseDotNetType(extendingType);
            ReturnType = NormaliseDotNetType(returnType);
            ParamTypes = prms.Select(NormaliseDotNetType).ToArray();
            SourceType = ExtensionMethodSourceType.SourceCode;
            Source = member.ToString();
            LastUpdated = lastUpdated;
            SourceLocation = sourceLocation;
            ClassName = className;

        }
    }
}