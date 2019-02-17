﻿/*
 * Heavily influenced by https://stackoverflow.com/questions/3049467/is-c-sharp-random-number-generator-thread-safe
 */

using SecretSanta.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class RandomService : IRandomService
    {
        private static readonly Random globalRandom = new Random();
        [ThreadStatic] private static Random localRandom;

        public RandomService()
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
        }

        public int Next()
        {
            return localRandom.Next();
        }

        public int Next(int maxValue)
        {
            return localRandom.Next(maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            return localRandom.Next(minValue, maxValue);
        }
    }
}