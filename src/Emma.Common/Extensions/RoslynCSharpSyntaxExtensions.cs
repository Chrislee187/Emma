using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Emma.Common.Extensions
{
    public static class RoslynCSharpSyntaxExtensions
    {
        public static bool IsStatic(this MemberDeclarationSyntax syntax)
            => syntax.Modifiers.Any(m => m.Kind() == SyntaxKind.StaticKeyword);

        public static bool IsPublic(this MemberDeclarationSyntax syntax)
            => syntax.Modifiers.Any(m => m.Kind() == SyntaxKind.PublicKeyword);

        public static bool IsThis(this BaseParameterSyntax syntax)
            => syntax.Modifiers.Any(m => m.Kind() == SyntaxKind.ThisKeyword);

        public static bool IsExtensionMethod(this MethodDeclarationSyntax syntax) 
            => syntax.IsPublic() && syntax.IsStatic() 
                                 && (syntax.ParameterList.Parameters.Any() 
                                     && syntax.ParameterList.Parameters.First().IsThis());

        public static string Name(this MethodDeclarationSyntax syntax) => syntax.Identifier.Text.Trim();
        public static string Name(this TypeSyntax syntax) => syntax.GetText().ToString().Trim();

    }
}