using System;

namespace Emma.Core
{
    public class QueryMatchers
    {
        public static bool String(string text, string compare, bool matchCase, StringMatchMode matchMode = StringMatchMode.Equals)
        {
            var left = text;
            var right = compare;

            if (matchCase)
            {
                left = left.ToLowerInvariant();
                right = right.ToLowerInvariant();
            }

            switch (matchMode)
            {
                case StringMatchMode.Equals:
                    return right.Equals(left);
                case StringMatchMode.StartsWith:
                    return right.StartsWith(left);
                case StringMatchMode.EndsWith:
                    return right.EndsWith(left);
                case StringMatchMode.Contains:
                    return right.Contains(left);
                default:
                    throw new ArgumentOutOfRangeException(nameof(matchMode), matchMode, null);
            }
        }
    }
}