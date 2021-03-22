using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Emma.Common.Adapters;
using Emma.Common.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Emma.Common
{
    public static class ExtensionMethodParser
    {
        public static ExtensionMethod Parse(MethodInfo mi, DateTime lastUpdated = default) => 
            new MethodInfoExtensionMethod(mi, lastUpdated);

        public static IEnumerable<ExtensionMethod> Parse(IEnumerable<MethodInfo> mis, DateTime lastUpdated = default) => 
            mis.Select(m => Parse(m, lastUpdated));

        public static IEnumerable<ExtensionMethod> Parse(Assembly asm) => 
            Parse(asm.ExtensionMethods(), new FileInfo(asm.Location).LastWriteTimeUtc);

        public static IEnumerable<ExtensionMethod> Parse(string sourceCode, string sourceLocation = default, DateTimeOffset lastUpdated = default) => 
            ParseSyntax(
                CSharpSyntaxTree.ParseText(sourceCode).GetCompilationUnitRoot().Members, 
                sourceLocation, 
                lastUpdated);

        public static async Task<IEnumerable<ExtensionMethod>> Parse(string folder)
        {
            return await Task.Run(() =>
            {
                var walker = new FolderWalker(folder).Where(f => f.EndsWith(".cs"));
                var ems = new List<ExtensionMethod>();
                foreach (var fn in walker)
                {
                    ems.AddRange(Parse(File.ReadAllText(fn), fn, new FileInfo(fn).LastWriteTimeUtc));
                }

                return ems;
            });
        }

        private static IEnumerable<ExtensionMethod> ParseSyntax(
            SyntaxList<MemberDeclarationSyntax> members, 
            string sourceLocation,
            DateTimeOffset lastUpdated) => 
                new MemberSyntaxListExtensionMethods(members, sourceLocation, lastUpdated).ToArray();
    }

    public class FolderWalker : IEnumerable<string>
    {
        private readonly string _root;

        public FolderWalker(string root)
        {
            _root = root;
        }

        
        public IEnumerator<string> GetEnumerator()
        {
            var stk = new Stack<string>();
            stk.Push(_root);

            while (stk.Any())
            {
                var current = stk.Pop();
                if (Directory.Exists(current))
                {
                    foreach (var file in Directory.GetFiles(current))
                    {
                        yield return Path.GetFullPath(file);
                    }
                    foreach (var dir in Directory.GetDirectories(current))
                    {
                        stk.Push(Path.GetFullPath(dir));
                    }
                }
                else
                {
                    yield return current;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}