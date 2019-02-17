/*
 * Heavily influenced by https://stackoverflow.com/questions/3049467/is-c-sharp-random-number-generator-thread-safe
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Models
{
    public class ThreadSafeRandom
    {
        private static readonly Random globalRandom = new Random();
        [ThreadStatic] private static Random localRandom;

        public ThreadSafeRandom()
        {
           
        }

        public int Next()
        {
            if (localRandom == null)
            {
                lock (globalRandom)
                {
                    if (localRandom == null)
                    {
                        int seed = globalRandom.Next();
                        localRandom = new Random(seed);
                    }
                }
            }

            return localRandom.Next();
        }
    }
}
