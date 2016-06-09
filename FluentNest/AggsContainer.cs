﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nest;

namespace FluentNest
{
    public class AggsContainer<T>
    {
        private readonly AggregationsHelper aggs;

        public AggsContainer(AggregationsHelper aggs)
        {
            this.aggs = aggs;
        }

        public int GetCardinality(Expression<Func<T, object>> fieldGetter, Expression<Func<T, object>> filterRule = null)
        {
            return aggs.GetCardinality(fieldGetter, filterRule);
        }

        public K GetSum<K>(Expression<Func<T, K>> fieldGetter, Expression<Func<T, object>> filterRule = null)
        {
            return aggs.GetSum(fieldGetter, filterRule);
        }

        public K GetAverage<K>(Expression<Func<T, K>> fieldGetter)
        {
            return aggs.GetAverage(fieldGetter);
        }

        public K GetMin<K>(Expression<Func<T, K>> fieldGetter)
        {
            return aggs.GetMin(fieldGetter);
        }

        public K GetMax<K>(Expression<Func<T, K>> fieldGetter)
        {
            return aggs.GetMax(fieldGetter);
        }

        public int? GetCount(Expression<Func<T, object>> fieldGetter, Expression<Func<T, object>> filterRule = null)
        {
            return aggs.GetCount(fieldGetter, filterRule);
        }

        public IEnumerable<V> GetDistinct<V>(Expression<Func<T, V>> fieldGetter)
        {
            return aggs.GetDistinct(fieldGetter);
        }

        public IList<PercentileItem> GetPercentile(Expression<Func<T, Object>> fieldGetter)
        {
            return aggs.GetPercentile<T>(fieldGetter);
        }

        public StatsAggregate GetStats(Expression<Func<T, Object>> fieldGetter)
        {
            return aggs.GetStats(fieldGetter);
        }

        public K GetFirstBy<K>(Expression<Func<T, K>> fieldGetter,
            Expression<Func<T, object>> filterRule = null)
        {
            return aggs.GetFirstBy(fieldGetter, filterRule);
        }
    }
}
