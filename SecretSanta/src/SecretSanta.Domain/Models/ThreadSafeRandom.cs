using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Models
{
    public class ThreadSafeRandom
    {
        private static readonly Random global = new Random();
        [ThreadStatic] private static Random local;

        public ThreadSafeRandom()
        {
            if (local == null)
            {
                lock(global)
                {
                    if (local == null)
                    {
                        int seed = global.Next();
                        local = new Random(seed);
                    }
                }
            }
        }

        public int Next()
        {
            return local.Next();
        }

        public int Next(int maxValue)
        {
            return local.Next(maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            return local.Next(minValue, maxValue);
        }
    }
}
