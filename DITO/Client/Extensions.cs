using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public static class Extensions
    {
        public static byte[] Merge(this IEnumerable<byte[]> blocks, int length)
        {
            var total = new byte[blocks.Sum(b => b.Length)];

            foreach (var block in blocks.Select((data, i) => (data, i)))
            {
                Buffer.BlockCopy(block.data, 0, total, block.i * length, length);
            }

            return total;
        }

        public static IEnumerable<TEntry> Repeat<TEntry>(this IEnumerable<TEntry> entries, Predicate<int> condition)
        {
            if (entries is null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            if (entries.Count() <= 0)
            {
                throw new ArgumentException("Entries must have at least one element.");
            }

            var enumerator = entries.GetEnumerator();
            var counter = 1;
            do
            {
                if (!enumerator.MoveNext())
                {
                    enumerator.Reset();
                    enumerator.MoveNext();
                }

                yield return enumerator.Current;

            } while (condition(counter++));
        }
    }
}
