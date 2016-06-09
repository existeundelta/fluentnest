﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Nest;

namespace FluentNest
{
    public static class GroupBys
    {
        public static AggregationContainerDescriptor<T> GroupBy<T>(this AggregationContainerDescriptor<T> innerAggregation, Expression<Func<T, Object>> fieldGetter) where T : class
        {
            AggregationContainerDescriptor<T> v = new AggregationContainerDescriptor<T>();
            var fieldName = fieldGetter.GetAggName(AggType.GroupBy);
            v.Terms(fieldName, tr =>
            {
                TermsAggregationDescriptor<T> trmAggDescriptor = new TermsAggregationDescriptor<T>();
                trmAggDescriptor.Field(fieldGetter);
                trmAggDescriptor.Size(int.MaxValue);
                return trmAggDescriptor.Aggregations(x => innerAggregation);
            });

            return v;
        }

        public static AggregationContainerDescriptor<T> GroupBy<T>(this AggregationContainerDescriptor<T> innerAggregation, String key) where T : class
        {
            AggregationContainerDescriptor<T> v = new AggregationContainerDescriptor<T>();
            v.Terms(key, tr =>
            {
                TermsAggregationDescriptor<T> trmAggDescriptor = new TermsAggregationDescriptor<T>();
                trmAggDescriptor.Field(key);
                trmAggDescriptor.Size(int.MaxValue);
                return trmAggDescriptor.Aggregations(x => innerAggregation);
            });

            return v;
        }


        public static AggregationContainerDescriptor<T> GroupBy<T>(this AggregationContainerDescriptor<T> innerAggregation, IEnumerable<String> keys) where T : class
        {
            var reversedAndLowered = keys.Select(x => x.FirstCharacterToLower()).Reverse().ToList();
            var aggregations = reversedAndLowered.Aggregate(innerAggregation, (s, i) => s.GroupBy(i));
            return aggregations;
        }

        public static IEnumerable<K> GetGroupBy<T,K>(this AggregationsHelper aggs, Expression<Func<T, Object>> fieldGetter, Func<KeyedBucket, K> objectTransformer)
        {
            var aggName = fieldGetter.GetAggName(AggType.GroupBy);
            var itemsTerms = aggs.Terms(aggName);
            return itemsTerms.Buckets.Select(objectTransformer);
        }

        public static IEnumerable<KeyedBucket> GetGroupBy<T>(this AggregationsHelper aggs, Expression<Func<T, Object>> fieldGetter)
        {
            var aggName = fieldGetter.GetAggName(AggType.GroupBy);
            var itemsTerms = aggs.Terms(aggName);
            return itemsTerms.Buckets;
        }

        public static IEnumerable<KeyedBucket> GetGroupBy(this AggregationsHelper aggs, string key)
        {
            var itemsTerms = aggs.Terms(key);
            return itemsTerms.Buckets;
        }

        public static IDictionary<V, KeyedBucket> GetDictionary<T,V>(this AggregationsHelper aggs, Expression<Func<T, Object>> fieldGetter) where V:struct,IConvertible
        {
            var aggName = fieldGetter.GetAggName(AggType.GroupBy);
            var itemsTerms = aggs.Terms(aggName);
            if ((typeof(V).IsEnum))
            {
                return itemsTerms.Buckets.ToDictionary(x => Filters.Parse<V>(x.Key));
            }
            return itemsTerms.Buckets.ToDictionary(x => (V)Convert.ChangeType(x.Key, typeof(V)));
        }

        public static IDictionary<String, K> GetDictionary<T, K>(this AggregationsHelper aggs, Expression<Func<T, Object>> fieldGetter, Func<KeyedBucket, K> objectTransformer)
        {
            var aggName = fieldGetter.GetAggName(AggType.GroupBy);
            var itemsTerms = aggs.Terms(aggName);
            return itemsTerms.Buckets.ToDictionary(x => x.Key, objectTransformer);
        }
    }
}
