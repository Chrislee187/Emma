using System;
using System.Threading.Tasks;

namespace Emma.Common.Utils
{
    public static class AsyncHelper
    {
        public static T RunSynchronously<T>(Func<Task<T>> func)
        {
            return Task.Run(async () => await func()).Result;
        }


    }
}