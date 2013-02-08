﻿using System.Diagnostics;
using System.Linq;
using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;

namespace Kunukn.SingleDetectLibrary.Code.StrategyPattern
{
    public class NaiveStrategy : AlgorithmStrategy
    {
        public override string Name
        {
            get { return "Naive Strategy"; }
        }
        
        /// <summary>
        ///  O(n^2)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public override long UpdateSingles(ISingleDetectAlgorithm s)
        {
            var sw = new Stopwatch();
            sw.Start();

            s.Singles.Clear();

            var n = s.Points.Count;
            for (var i = 0; i < n; i++)
            {
                var p1 = s.Points[i];
                var add = true;
                for (var j = 0; j < n; j++)
                {
                    if (i == j) continue;

                    var p2 = s.Points[j];
                    var dist = p1.Distance(p2.X, p2.Y);
                    if (!(dist > s.Rect_.MaxDistance))
                    {
                        add = false;
                        break;
                    }
                }
                if (add) s.Singles.Add(p1);
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
        
        /// <summary>
        /// O(n log n)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="p"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public override long UpdateKnn(ISingleDetectAlgorithm s, IP p, int k)
        {
            var sw = new Stopwatch();
            sw.Start();

            s.Knn.Clear();
            s.Knn.Origin = p;
            s.Knn.K = k;

            var n = s.Points.Count;
            for (var i = 0; i < n; i++)
            {
                var p1 = s.Points[i];
                if (p.Equals(p1)) continue;

                var dist = p.Distance(p1.X, p1.Y);
                s.Knn.NNs.Data.Add(new PDist { Point = p1, Distance = dist });
            }

            s.Knn.NNs.Data.Sort();
            k = k > n ? n : k;
            s.Knn.NNs.Data = s.Knn.NNs.Data.Take(k).ToList();

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
    }
}
