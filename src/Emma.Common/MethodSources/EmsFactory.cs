using System.Reflection;
using Emma.Common.ExtensionMethodProviders;

namespace Emma.Common.MethodSources
{
    public static class EmsFactory
    {
        public static ExtensionMethodsSource Create(Assembly asm)
        {
            var emP = new AssemblyEmProvider(asm);
            return new ExtensionMethodsSource(emP, emP);
        }
    }
}