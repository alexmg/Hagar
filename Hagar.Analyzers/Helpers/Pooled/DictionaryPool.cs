﻿using System.Collections.Generic;

namespace Hagar.Analyzers.Helpers.Pooled
{
    internal static class DictionaryPool<TKey, TValue>
    {
        private static readonly Pool<Dictionary<TKey, TValue>> Pool = new Pool<Dictionary<TKey, TValue>>(() => new Dictionary<TKey, TValue>(), x => x.Clear());

        public static Pool<Dictionary<TKey, TValue>>.Pooled Create()
        {
            return Pool.GetOrCreate();
        }
    }
}