using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Shouldly;

namespace Emma.Core.Tests
{
    public class Spikes
    {
        private string _classText;


        [SetUp]
        public void Setup()
        {
            _classText = File.ReadAllText(@"Support/SampleExtensionsClass.cs");
        }

        [Test]
        public void Spike()
        {
            var model = new CsFileModel(_classText);

            model.AddUsing("abc.def.ghh");

            Console.WriteLine(model.ToString());
        }
        [Test]
        public void Should_add_additional_using_statement()
        {
            var model = new CsFileModel(_classText);

            model.Usings.Length.ShouldBe(2);
            model.ToString().ShouldNotContain("using abc.def.ghi;" + Environment.NewLine);

            model.AddUsing("abc.def.ghi");

            model.Usings.Length.ShouldBe(3);
            model.ToString().ShouldContain("using abc.def.ghi;" + Environment.NewLine);

        }
        [Test]
        public void Should_add_additional_method()
        {
            var model = new CsFileModel(_classText);

            model.Methods.Length.ShouldBeGreaterThan(0);
            // model.ToString().ShouldNotContain("using abc.def.ghi;" + Environment.NewLine);

            model.AddMethod("public int TestMethod(string x) => return int.Parse(x))");

            model.Methods.Length.ShouldBe(3);
            // model.ToString().ShouldContain("using abc.def.ghi;" + Environment.NewLine);

        }
    }

    public class CsFileModel
    {
        private CompilationUnitSyntax _compilationUnitSyntax;

        public string[] Usings
            => _compilationUnitSyntax.Usings.Select(u => u.Name.ToString())
                .ToArray();

        public string[] Methods => RecurseMethodNames(_compilationUnitSyntax.Members);

        private string[] RecurseMethodNames(in SyntaxList<MemberDeclarationSyntax> members)
        {
            
            var x= members
                .Where(m => m.Kind() == SyntaxKind.NamespaceDeclaration || m.Kind() == SyntaxKind.ClassDeclaration)
                .SelectMany(m => m.Kind() == SyntaxKind.NamespaceDeclaration 
                    ? RecurseMethodNames(( m as NamespaceDeclarationSyntax).Members)
                    : RecurseMethodNames((m as ClassDeclarationSyntax).Members))
                ;

            var methodnames = members
                .Where(m => m.Kind() == SyntaxKind.MethodDeclaration)
                .Select(m => m.ToString())
                .ToArray();
            return methodnames;
        }


        public CsFileModel(string classText)
        {
            _compilationUnitSyntax = CSharpSyntaxTree.ParseText(classText).GetCompilationUnitRoot();
        }

        public void AddUsing(string @namespace)
        {
            _compilationUnitSyntax = _compilationUnitSyntax.AddUsings(@namespace.AsUsingDirective());
        }

        public override string ToString()
        {
            return _compilationUnitSyntax.ToString();

        }

        public void AddMethod(string methodText)
        {
            //_compilationUnitSyntax.AddMembers()
        }
    }

    public static class SyntaxFactoryHelper
    {
        public static UsingDirectiveSyntax AsUsingDirective(this string source)
        {
            var usingDirectiveSyntax = SyntaxFactory
                .UsingDirective(SyntaxFactory.ParseName(source.Trim()).WithLeadingTrivia(SyntaxFactory.Space))
                .WithTrailingTrivia(NewlineSyntax())
                ;

            return usingDirectiveSyntax;
        }

        private static SyntaxTrivia NewlineSyntax()
        {
            if (Environment.NewLine.Equals("\r\n"))
            {
                return SyntaxFactory.CarriageReturnLineFeed;
            }
            else
            {
                return SyntaxFactory.LineFeed;
            }
        }
    }

    public class UsingModel
    {
        public string Namespace { get; }

        public UsingModel(string namespc)
        {
            Namespace = namespc;
        }
    }
}
