using System;
using System.Collections;
using System.Collections.Generic;

namespace SwiftySend.Helpers
{
    internal static class CollectionExtension
    {

        public static IList<TResult> Select<TResult>(this IEnumerable source, Func<object, TResult> selector)
        {
            var result = new List<TResult>();
            foreach (var item in source)
                result.Add(selector.Invoke(item));
            return result;
        }





    }
}
