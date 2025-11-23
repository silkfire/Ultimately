namespace Ultimately.Tests;

using Collections;
using Reasons;

using Xunit;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CollectionTests
{
    [Fact]
    public void Collections_Enumerable_ValuesEither()
    {
        var list1 = new List<Option<string>>
                    {
                        Optional.Some("a"),
                        Optional.None<string>("error"),
                        Optional.None<string>("error"),
                        Optional.Some("b"),
                        Optional.None<string>("error"),
                        Optional.Some("c"),
                    };
        var list1Expected = new List<string> { "a", "b", "c" };

        var list2 = Enumerable.Empty<Option<string>>();
        var list2Expected = new List<string>();

        var list3 = new List<Option<string>> { "a".Some() };
        var list3Expected = new List<string> { "a" };

        var list4 = new List<Option<string>> { Optional.None<string>("error") };
        var list4Expected = new List<string>();


        Assert.Equal(list1.Values().ToList(), list1Expected);
        Assert.Equal(list2.Values().ToList(), list2Expected);
        Assert.Equal(list3.Values().ToList(), list3Expected);
        Assert.Equal(list4.Values().ToList(), list4Expected);
    }

    [Fact]
    public void Collections_Enumerable_ExceptionsEither()
    {
        var list1 = new List<Option<string>>
                    {
                        Optional.None<string>("a"),
                        Optional.Some("value"),
                        Optional.Some("value"),
                        Optional.None<string>("b"),
                        Optional.Some("value"),
                        Optional.None<string>("c"),
                    };

        var list1Expected = new List<string> { "a", "b", "c" };

        var list2 = new List<Option<string>>();
        var list2Expected = new List<string>();

        var list3 = new List<Option<string>> { Optional.None<string>("a") };
        var list3Expected = new List<string> { "a" };

        var list4 = new List<Option<string>> { Optional.Some("error") };
        var list4Expected = new List<string>();


        Assert.Equal(list1.Errors().Select(e => e.Message).ToList(), list1Expected);
        Assert.Equal(list2.Errors().Select(e => e.Message).ToList(), list2Expected);
        Assert.Equal(list3.Errors().Select(e => e.Message).ToList(), list3Expected);
        Assert.Equal(list4.Errors().Select(e => e.Message).ToList(), list4Expected);
    }

    [Fact]
    public void Collections_Enumerable_FirstOrNone()
    {
        var full = Enumerable.Range(0, 100);
        var empty = Enumerable.Empty<int>();
        var single = Enumerable.Repeat(0, 1);

        FirstOperator(full, single, empty);

        var fullList = Enumerable.Range(0, 100).ToList();
        var emptyList = Enumerable.Empty<int>().ToList();
        var singleList = Enumerable.Repeat(0, 1).ToList();

        FirstOperator(fullList, singleList, emptyList);

        var fullReadOnlyList = new TestReadOnlyList<int>(Enumerable.Range(0, 100).ToList());
        var emptyReadOnlyList = new TestReadOnlyList<int>(Enumerable.Empty<int>().ToList());
        var singleReadOnlyList = new TestReadOnlyList<int>(Enumerable.Repeat(0, 1).ToList());

        FirstOperator(fullReadOnlyList, singleReadOnlyList, emptyReadOnlyList);
    }

    [Fact]
    public void Collections_Enumerable_LastOrNone()
    {
        var full = Enumerable.Range(0, 100);
        var empty = Enumerable.Empty<int>();
        var single = Enumerable.Repeat(0, 1);

        LastOperator(full, single, empty);

        var fullList = Enumerable.Range(0, 100).ToList();
        var emptyList = Enumerable.Empty<int>().ToList();
        var singleList = Enumerable.Repeat(0, 1).ToList();

        LastOperator(fullList, singleList, emptyList);

        var fullReadOnlyList = new TestReadOnlyList<int>(Enumerable.Range(0, 100).ToList());
        var emptyReadOnlyList = new TestReadOnlyList<int>(Enumerable.Empty<int>().ToList());
        var singleReadOnlyList = new TestReadOnlyList<int>(Enumerable.Repeat(0, 1).ToList());

        LastOperator(fullReadOnlyList, singleReadOnlyList, emptyReadOnlyList);
    }

    [Fact]
    public void Collections_Enumerable_SingleOrNone()
    {
        var full = Enumerable.Range(0, 100);
        var empty = Enumerable.Empty<int>();
        var single = Enumerable.Repeat(0, 1);

        SingleOperator(full, single, empty);

        var fullList = Enumerable.Range(0, 100).ToList();
        var emptyList = Enumerable.Empty<int>().ToList();
        var singleList = Enumerable.Repeat(0, 1).ToList();

        SingleOperator(fullList, singleList, emptyList);

        var fullReadOnlyList = new TestReadOnlyList<int>(Enumerable.Range(0, 100).ToList());
        var emptyReadOnlyList = new TestReadOnlyList<int>(Enumerable.Empty<int>().ToList());
        var singleReadOnlyList = new TestReadOnlyList<int>(Enumerable.Repeat(0, 1).ToList());

        SingleOperator(fullReadOnlyList, singleReadOnlyList, emptyReadOnlyList);
    }

    [Fact]
    public void Collections_Enumerable_ElementAtOrNone()
    {
        var full = Enumerable.Range(0, 100);
        var empty = Enumerable.Empty<int>();
        var single = Enumerable.Repeat(0, 1);

        ElementAtOperator(full, single, empty);

        var fullList = Enumerable.Range(0, 100).ToList();
        var emptyList = Enumerable.Empty<int>().ToList();
        var singleList = Enumerable.Repeat(0, 1).ToList();

        ElementAtOperator(fullList, singleList, emptyList);

        var fullReadOnlyList = new TestReadOnlyList<int>(Enumerable.Range(0, 100).ToList());
        var emptyReadOnlyList = new TestReadOnlyList<int>(Enumerable.Empty<int>().ToList());
        var singleReadOnlyList = new TestReadOnlyList<int>(Enumerable.Repeat(0, 1).ToList());

        ElementAtOperator(fullReadOnlyList, singleReadOnlyList, emptyReadOnlyList);
    }

    [Fact]
    public void Collections_Dictionary_GetValueOrNone()
    {
        var dictionaryA = Enumerable.Range(50, 50).ToDictionary(i => i, i => i.ToString());
        var excludedKeysA = Enumerable.Range(-50, 50);
        GetValueOperator(new TestReadOnlyDictionary<int, string>(dictionaryA), excludedKeysA);
        GetValueOperator(new TestDictionary<int, string>(dictionaryA), excludedKeysA);
        GetValueOperator(dictionaryA.ToList(), excludedKeysA);

        var dictionaryB = new Dictionary<string, Guid>
                          {
                              { "a", Guid.NewGuid() },
                              { "b", Guid.NewGuid() },
                              { "c", Guid.NewGuid() },
                              { "d", Guid.NewGuid() },
                              { "e", Guid.NewGuid() },
                          };
        var excludedKeysB = new List<string> { "h", "i", "j", "k" };

        GetValueOperator(new TestReadOnlyDictionary<string, Guid>(dictionaryB), excludedKeysB);
        GetValueOperator(new TestDictionary<string, Guid>(dictionaryB), excludedKeysB);
        GetValueOperator(dictionaryB.ToList(), excludedKeysB);
    }

    [Fact]
    public void Collections_Reduce()
    {
        const int value = 42;

        var validationRules1 = new List<LazyOption>
                               {
                                   Optional.Lazy(() => (value & 1) == 1, Success.Create("Value is odd"), "Value must be odd"),
                                   Optional.Lazy(() => value / 2 == 77, Success.Create("Value divided by two is 77"), "Value divided by two must equal 77"),
                               };

        var validationRulesReduced1 = validationRules1.Reduce();

        Assert.False(validationRulesReduced1.IsSuccessful);
        validationRulesReduced1.Match(
                                      some: _ => Assert.Fail(),
                                      none: e => Assert.Equal("Value must be odd", e.Message)
                                     );


        var validationRules2 = new List<LazyOption>
                               {
                                   Optional.Lazy(() => value % 2 == 0, Success.Create("Value is even"), "Value must be even"),
                                   Optional.Lazy(() => value == 42, Success.Create("Value is 42"), "Value must be equal to 42"),
                               };

        var validationRulesReduced2 = validationRules2.Reduce();

        Assert.True(validationRulesReduced2.IsSuccessful);



        var validationRules3 = new List<LazyOption>
                               {
                                   Optional.Lazy(() => value % 2 == 0, Success.Create("Value is even"), "Value must be even"),
                                   Optional.Lazy(() => value / 2 == 77, Success.Create("Value divided by two is 77"), "Value divided by two must equal 77"),
                               };

        var validationRulesReduced3 = validationRules3.Reduce();

        Assert.False(validationRulesReduced3.IsSuccessful);
        validationRulesReduced3.Match(
                                      some: _ => Assert.Fail(),
                                      none: e => Assert.Equal("Value divided by two must equal 77", e.Message)
                                     );




        bool validationRun1 = false, validationRun2 = false, validationRun3 = false, validationRun4 = false;

        var validationRules4 = new List<LazyOption>
                               {
                                   Optional.Lazy(() =>
                                   {
                                       validationRun1 = true;

                                       return (value & 1) == 1;
                                   }, Success.Create("SUCCESS"), "ERROR"),

                                   Optional.Lazy(() =>
                                   {
                                       validationRun2 = true;

                                       return (value & 1) == 1;
                                   }, Success.Create("SUCCESS"), "ERROR"),

                                   Optional.Lazy(() =>
                                   {
                                       validationRun3 = true;

                                       return (value & 1) == 1;
                                   }, Success.Create("SUCCESS"), "ERROR"),

                                   Optional.Lazy(() =>
                                   {
                                       validationRun4 = true;

                                       return (value & 1) == 1;
                                   }, Success.Create("SUCCESS"), "ERROR"),
                               };

        var validationRulesReduced4 = validationRules4.Reduce();

        Assert.False(validationRulesReduced4.IsSuccessful);

        Assert.True(validationRun1);
        Assert.False(validationRun2);
        Assert.False(validationRun3);
        Assert.False(validationRun4);
    }

    [Fact]
    public async Task Collections_Reduce_Async()
    {
        const int value = 42;

        var validationRules1 = new List<LazyOptionAsync>
                               {
                                   Optional.LazyAsync(() => Task.FromResult((value & 1) == 1), Success.Create("Value is odd"), "Value must be odd"),
                                   Optional.LazyAsync(() => Task.FromResult(value / 2 == 77), Success.Create("Value divided by two is 77"), "Value divided by two must equal 77"),
                               };

        var validationRulesReduced1 = await validationRules1.ReduceAsync();

        Assert.False(validationRulesReduced1.IsSuccessful);
        validationRulesReduced1.Match(
                                      some: _ => Assert.Fail(),
                                      none: e => Assert.Equal("Value must be odd", e.Message)
                                     );


        var validationRules2 = new List<LazyOptionAsync>
                               {
                                   Optional.LazyAsync(() => Task.FromResult(value % 2 == 0), Success.Create("Value is even"), "Value must be even"),
                                   Optional.LazyAsync(() => Task.FromResult(value == 42), Success.Create("Value is 42"), "Value must be equal to 42"),
                               };

        var validationRulesReduced2 = await validationRules2.ReduceAsync();

        Assert.True(validationRulesReduced2.IsSuccessful);



        var validationRules3 = new List<LazyOptionAsync>
                               {
                                   Optional.LazyAsync(() => Task.FromResult(value % 2 == 0), Success.Create("Value is even"), "Value must be even"),
                                   Optional.LazyAsync(() => Task.FromResult(value / 2 == 77), Success.Create("Value divided by two is 77"), "Value divided by two must equal 77"),
                               };

        var validationRulesReduced3 = await validationRules3.ReduceAsync();

        Assert.False(validationRulesReduced3.IsSuccessful);
        validationRulesReduced3.Match(
                                      some: _ => Assert.Fail(),
                                      none: e => Assert.Equal("Value divided by two must equal 77", e.Message)
                                     );



        bool validationRun1 = false, validationRun2 = false, validationRun3 = false, validationRun4 = false;

        var validationRules4 = new List<LazyOptionAsync>
                               {
                                   Optional.LazyAsync(() =>
                                   {
                                       validationRun1 = true;

                                       return Task.FromResult((value & 1) == 1);
                                   }, Success.Create("SUCCESS"), "ERROR"),

                                   Optional.LazyAsync(() =>
                                   {
                                       validationRun2 = true;

                                       return Task.FromResult((value & 1) == 1);
                                   }, Success.Create("SUCCESS"), "ERROR"),

                                   Optional.LazyAsync(() =>
                                   {
                                       validationRun3 = true;

                                       return Task.FromResult((value & 1) == 1);
                                   }, Success.Create("SUCCESS"), "ERROR"),

                                   Optional.LazyAsync(() =>
                                   {
                                       validationRun4 = true;

                                       return Task.FromResult((value & 1) == 1);
                                   }, Success.Create("SUCCESS"), "ERROR"),
                               };

        var validationRulesReduced4 = await validationRules4.ReduceAsync();

        Assert.False(validationRulesReduced4.IsSuccessful);

        Assert.True(validationRun1);
        Assert.False(validationRun2);
        Assert.False(validationRun3);
        Assert.False(validationRun4);
    }


    private static void FirstOperator(IEnumerable<int> full, IEnumerable<int> single, IEnumerable<int> empty)
    {
        var fullList = full.ToList();
        var singleList = single.ToList();
        var emptyList = empty.ToList();

        Assert.True(fullList.FirstOrNone().HasValue);
        Assert.True(fullList.FirstOrNone(x => x == 50).HasValue);
        Assert.False(fullList.FirstOrNone(x => x == -1).HasValue);

        Assert.Equal(fullList.FirstOrNone().ValueOr(-1), fullList.First());
        Assert.Equal(50, fullList.FirstOrNone(x => x == 50).ValueOr(-1));
        Assert.Equal(51, fullList.FirstOrNone(x => x > 50).ValueOr(-1));
        Assert.Equal(fullList.FirstOrNone(x => x < 50).ValueOr(-1), fullList.First());

        Assert.True(singleList.FirstOrNone().HasValue);
        Assert.True(singleList.FirstOrNone(x => x == 0).HasValue);
        Assert.False(singleList.FirstOrNone(x => x == -1).HasValue);
        Assert.Equal(singleList.FirstOrNone().ValueOr(-1), singleList.First());

        Assert.False(emptyList.FirstOrNone().HasValue);
        Assert.False(emptyList.FirstOrNone(x => x == 50).HasValue);
    }

    private static void LastOperator(IEnumerable<int> full, IEnumerable<int> single, IEnumerable<int> empty)
    {
        var fullList = full.ToList();
        var singleList = single.ToList();
        var emptyList = empty.ToList();

        Assert.True(fullList.LastOrNone().HasValue);
        Assert.True(fullList.LastOrNone(x => x == 50).HasValue);
        Assert.False(fullList.LastOrNone(x => x == -1).HasValue);

        Assert.Equal(fullList.LastOrNone().ValueOr(-1), fullList.Last());
        Assert.Equal(50, fullList.LastOrNone(x => x == 50).ValueOr(-1));
        Assert.Equal(fullList.LastOrNone(x => x > 50).ValueOr(-1), fullList.Last());
        Assert.Equal(49, fullList.LastOrNone(x => x < 50).ValueOr(-1));

        Assert.True(singleList.LastOrNone().HasValue);
        Assert.True(singleList.LastOrNone(x => x == 0).HasValue);
        Assert.False(singleList.LastOrNone(x => x == -1).HasValue);
        Assert.Equal(singleList.LastOrNone().ValueOr(-1), singleList.Last());

        Assert.False(emptyList.LastOrNone().HasValue);
        Assert.False(emptyList.LastOrNone(x => x == 50).HasValue);
    }

    private static void SingleOperator(IEnumerable<int> full, IEnumerable<int> single, IEnumerable<int> empty)
    {
        var fullList = full.ToList();
        var singleList = single.ToList();
        var emptyList = empty.ToList();

        Assert.False(fullList.SingleOrNone().HasValue);
        Assert.True(fullList.SingleOrNone(x => x == 50).HasValue);
        Assert.False(fullList.SingleOrNone(x => x == -1).HasValue);
        Assert.False(fullList.SingleOrNone(x => x > 50).HasValue);
        Assert.False(fullList.SingleOrNone(x => x < 50).HasValue);
        Assert.Equal(50, fullList.SingleOrNone(x => x == 50).ValueOr(-1));

        Assert.True(singleList.SingleOrNone().HasValue);
        Assert.True(singleList.SingleOrNone(x => x == 0).HasValue);
        Assert.False(singleList.SingleOrNone(x => x == -1).HasValue);
        Assert.Equal(singleList.SingleOrNone().ValueOr(-1), singleList.Single());

        Assert.False(emptyList.SingleOrNone().HasValue);
        Assert.False(emptyList.SingleOrNone(x => x == 50).HasValue);
    }

    private static void ElementAtOperator(IEnumerable<int> full, IEnumerable<int> single, IEnumerable<int> empty)
    {
        var fullList = full.ToList();
        var singleList = single.ToList();

        Assert.False(fullList.ElementAtOrNone(-1).HasValue);
        Assert.False(fullList.ElementAtOrNone(fullList.Count).HasValue);

        for (var i = 0; i < fullList.Count; i++)
        {
            Assert.True(fullList.ElementAtOrNone(i).HasValue);
            Assert.Equal(fullList.ElementAtOrNone(i).ValueOr(-1), fullList.ElementAt(i));
        }

        Assert.True(singleList.ElementAtOrNone(0).HasValue);
        Assert.False(singleList.ElementAtOrNone(2).HasValue);
        Assert.False(singleList.ElementAtOrNone(-1).HasValue);
        Assert.Equal(singleList.ElementAtOrNone(0).ValueOr(-1), singleList.Single());

        Assert.False(empty.ElementAtOrNone(0).HasValue);
    }

    private static void GetValueOperator<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEnumerable<TKey> excludedKeys)
    {
        var keyValuePairsList = dictionary.ToList();

        foreach (var pair in keyValuePairsList)
        {
            Assert.True(keyValuePairsList.GetValueOrNone(pair.Key).HasValue);
            Assert.Equal(keyValuePairsList.GetValueOrNone(pair.Key).ValueOr(default(TValue)), pair.Value);
        }

        foreach (var key in excludedKeys)
        {
            Assert.False(keyValuePairsList.GetValueOrNone(key).HasValue);
        }
    }

    private class TestReadOnlyDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary) : IReadOnlyDictionary<TKey, TValue>
    {
        public TValue this[TKey key] => dictionary[key];
        public int Count => dictionary.Count;
        public IEnumerable<TKey> Keys => dictionary.Keys;
        public IEnumerable<TValue> Values => dictionary.Values;
        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();
        public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)dictionary).GetEnumerator();
    }

    private class TestReadOnlyList<TValue>(List<TValue> list) : IReadOnlyList<TValue>
    {
        public TValue this[int index] => list[index];
        public int Count => list.Count;
        public IEnumerator<TValue> GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)list).GetEnumerator();
    }

    private class TestDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary) : IDictionary<TKey, TValue>
    {
        private ICollection<KeyValuePair<TKey, TValue>> Collection => dictionary;

        public TValue this[TKey key]
        {
            get => dictionary[key];
            set => dictionary[key] = value;
        }

        public int Count => dictionary.Count;
        public bool IsReadOnly => Collection.IsReadOnly;
        public ICollection<TKey> Keys => dictionary.Keys;
        public ICollection<TValue> Values => dictionary.Values;
        public void Add(KeyValuePair<TKey, TValue> item) => Collection.Add(item);
        public void Add(TKey key, TValue value) => dictionary.Add(key, value);
        public void Clear() => Collection.Clear();
        public bool Contains(KeyValuePair<TKey, TValue> item) => dictionary.Contains(item);
        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => Collection.CopyTo(array, arrayIndex);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();
        public bool Remove(KeyValuePair<TKey, TValue> item) => Collection.Remove(item);
        public bool Remove(TKey key) => dictionary.Remove(key);
        public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)dictionary).GetEnumerator();
    }
}
