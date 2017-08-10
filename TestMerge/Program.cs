using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace TestMerge
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmark>();
            Console.ReadLine();
        }
    }

    public class Benchmark
    {
        private readonly MyClass _benchClass = new MyClass();

        [Benchmark]
        public void 用Dictionary處理() => _benchClass.用Dictionary處理();

        [Benchmark]
        public void 用Dictionary處理AsParallel() => _benchClass.用Dictionary處理AsParallel();

        [Benchmark]
        public void 用Zip處理() => _benchClass.用Zip處理();

        [Benchmark]
        public void 用Zip處理AsParallel() => _benchClass.用Zip處理AsParallel();

        [Benchmark]
        public void 用ListWhere處理() => _benchClass.用ListWhere處理();

        [Benchmark]
        public void 用ListWhere處理AsParallel() => _benchClass.用ListWhere處理AsParallel();

        [Benchmark]
        public void 用Join處理() => _benchClass.用Join處理();

        [Benchmark]
        public void 用Join處理AsParallel() => _benchClass.用Join處理AsParallel();
    }

    internal class MyClass
    {
        private readonly int _count = 100;

        // 如果其中一個項目數量不同，最終結果就會不符合預期
        public void 用Zip處理()
        {
            var names = Enumerable.Range(1, _count).Select(i => new NameClass { Id = i, Name = i.ToString() }).ToList();
            var heights = Enumerable.Range(1, _count).Select(i => new HeightClass { Id = i, Height = i }).ToList();
            var weights = Enumerable.Range(1, _count).Select(i => new WeightClass { Id = i, Wieght = i }).ToList();

            var result1 = names.Zip(heights, (n, h) => new NameHeightWeightClass
            {
                Id = n.Id,
                Name = n.Name,
                Height = h.Height
            });

            var result2 = result1.Zip(weights, (n, w) => new NameHeightWeightClass
            {
                Id = n.Id,
                Name = n.Name,
                Height = n.Height,
                Wieght = w.Wieght
            });

            var resultCount = result2.Count();
        }

        public void 用Zip處理AsParallel()
        {
            var names = Enumerable.Range(1, _count).Select(i => new NameClass { Id = i, Name = i.ToString() }).ToList();
            var heights = Enumerable.Range(1, _count).Select(i => new HeightClass { Id = i, Height = i }).ToList();
            var weights = Enumerable.Range(1, _count).Select(i => new WeightClass { Id = i, Wieght = i }).ToList();

            var result1 = names.Zip(heights, (n, h) => new NameHeightWeightClass
            {
                Id = n.Id,
                Name = n.Name,
                Height = h.Height
            });

            var result2 = result1.Zip(weights, (n, w) => new NameHeightWeightClass
            {
                Id = n.Id,
                Name = n.Name,
                Height = n.Height,
                Wieght = w.Wieght
            }).AsParallel();

            var resultCount = result2.Count();
        }

        public void 用ListWhere處理()
        {
            var names = Enumerable.Range(1, _count).Select(i => new NameClass { Id = i, Name = i.ToString() }).ToList();
            var heights = Enumerable.Range(1, _count).Select(i => new HeightClass { Id = i, Height = i }).ToList();
            var weights = Enumerable.Range(1, _count).Select(i => new WeightClass { Id = i, Wieght = i }).ToList();

            var result = names.Select(n =>
            {
                var height = heights.FirstOrDefault(h => h.Id == n.Id);
                var weight = weights.FirstOrDefault(w => w.Id == n.Id);

                return new NameHeightWeightClass
                {
                    Id = n.Id,
                    Name = n.Name,
                    Height = height.Height,
                    Wieght = weight.Wieght
                };
            });

            var resultCount = result.Count();
        }

        public void 用ListWhere處理AsParallel()
        {
            var names = Enumerable.Range(1, _count).Select(i => new NameClass { Id = i, Name = i.ToString() }).ToList();
            var heights = Enumerable.Range(1, _count).Select(i => new HeightClass { Id = i, Height = i }).ToList();
            var weights = Enumerable.Range(1, _count).Select(i => new WeightClass { Id = i, Wieght = i }).ToList();

            var result = names.Select(n =>
            {
                var height = heights.FirstOrDefault(h => h.Id == n.Id);
                var weight = weights.FirstOrDefault(w => w.Id == n.Id);

                return new NameHeightWeightClass
                {
                    Id = n.Id,
                    Name = n.Name,
                    Height = height.Height,
                    Wieght = weight.Wieght
                };
            }).AsParallel();

            var resultCount = result.Count();
        }

        public void 用Dictionary處理()
        {
            var names = Enumerable.Range(1, _count).Select(i => new NameClass { Id = i, Name = i.ToString() }).ToList();
            var heights = Enumerable.Range(1, _count).Select(i => new HeightClass { Id = i, Height = i }).ToDictionary(k => k.Id, v => v);
            var weights = Enumerable.Range(1, _count).Select(i => new WeightClass { Id = i, Wieght = i }).ToDictionary(k => k.Id, v => v);

            var result = names.Select(n =>
            {
                var height = TryGetValue(heights, n.Id);
                var weight = TryGetValue(weights, n.Id);

                return new NameHeightWeightClass
                {
                    Id = n.Id,
                    Name = n.Name,
                    Height = height.Height,
                    Wieght = weight.Wieght
                };
            });
        }

        public void 用Dictionary處理AsParallel()
        {
            var names = Enumerable.Range(1, _count).Select(i => new NameClass { Id = i, Name = i.ToString() }).ToList();
            var heights = Enumerable.Range(1, _count).Select(i => new HeightClass { Id = i, Height = i }).ToDictionary(k => k.Id, v => v);
            var weights = Enumerable.Range(1, _count).Select(i => new WeightClass { Id = i, Wieght = i }).ToDictionary(k => k.Id, v => v);

            var result = names.Select(n =>
            {
                var height = TryGetValue(heights, n.Id);
                var weight = TryGetValue(weights, n.Id);

                return new NameHeightWeightClass
                {
                    Id = n.Id,
                    Name = n.Name,
                    Height = height.Height,
                    Wieght = weight.Wieght
                };
            }).AsParallel();
        }

        private T2 TryGetValue<T1, T2>(Dictionary<T1, T2> dicts, T1 key)
        {
            T2 value;
            dicts.TryGetValue(key, out value);
            return value;
        }

        public void 用Join處理()
        {
            var names = Enumerable.Range(1, _count).Select(i => new NameClass { Id = i, Name = i.ToString() }).ToList();
            var heights = Enumerable.Range(1, _count).Select(i => new HeightClass { Id = i, Height = i }).ToList();
            var weights = Enumerable.Range(1, _count).Select(i => new WeightClass { Id = i, Wieght = i }).ToList();

            var result = (from n in names
                join h in heights on n.Id equals h.Id
                join w in weights on n.Id equals w.Id
                select new NameHeightWeightClass
                {
                    Id = n.Id,
                    Name = n.Name,
                    Height = h.Height,
                    Wieght = w.Wieght
                }).ToList();

            var resultCount = result.Count();
        }

        public void 用Join處理AsParallel()
        {
            var names = Enumerable.Range(1, _count).Select(i => new NameClass { Id = i, Name = i.ToString() }).ToList();
            var heights = Enumerable.Range(1, _count).Select(i => new HeightClass { Id = i, Height = i }).ToList();
            var weights = Enumerable.Range(1, _count).Select(i => new WeightClass { Id = i, Wieght = i }).ToList();

            var result = (from n in names
                join h in heights on n.Id equals h.Id
                join w in weights on n.Id equals w.Id
                select new NameHeightWeightClass
                {
                    Id = n.Id,
                    Name = n.Name,
                    Height = h.Height,
                    Wieght = w.Wieght
                }).AsParallel();

            var resultCount = result.Count();
        }
    }

    internal class NameClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    internal class HeightClass
    {
        public int Id { get; set; }
        public int Height { get; set; }
    }

    internal class WeightClass
    {
        public int Id { get; set; }
        public int Wieght { get; set; }
    }

    internal class NameHeightWeightClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public int Wieght { get; set; }
    }
}
