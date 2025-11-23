namespace Ultimately.Tests;

using Reasons;

using Xunit;

using System;

public class EitherTests
{
    [Fact]
    public void Either_CreateAndCheckExistence()
    {
        var noneStruct = Optional.None<int>("ex");
        var noneNullable = Optional.None<int?>("ex");
        var noneClass = Optional.None<string>("ex");

        Assert.False(noneStruct.HasValue);
        Assert.False(noneNullable.HasValue);
        Assert.False(noneClass.HasValue);

        var someStruct = 1.Some();
        var someNullable = Optional.Some<int?>(1);
        var someNullableEmpty = Optional.Some<int?>(null);
        var someClass = Optional.Some("1");
        var someClassNull = Optional.Some<string>(null);

        Assert.True(someStruct.HasValue);
        Assert.True(someNullable.HasValue);
        Assert.True(someNullableEmpty.HasValue);
        Assert.True(someClass.HasValue);
        Assert.True(someClassNull.HasValue);
    }

    [Fact]
    public void Either_CheckContainment()
    {
        var noneStruct = Optional.None<int>("ex");
        var noneNullable = Optional.None<int?>("ex");
        var noneClass = Optional.None<string>("ex");

        Assert.False(noneStruct.Contains(0));
        Assert.False(noneNullable.Contains(null));
        Assert.False(noneClass.Contains(null));

        Assert.False(noneStruct.Exists(val => true));
        Assert.False(noneNullable.Exists(val => true));
        Assert.False(noneClass.Exists(val => true));

        var someStruct = Optional.Some(1);
        var someNullable = Optional.Some<int?>(1);
        var someNullableEmpty = Optional.Some<int?>(null);
        var someClass = Optional.Some("1");
        var someClassNull = Optional.Some<string>(null);

        Assert.True(someStruct.Contains(1));
        Assert.True(someNullable.Contains(1));
        Assert.True(someNullableEmpty.Contains(null));
        Assert.True(someClass.Contains("1"));
        Assert.True(someClassNull.Contains(null));

        Assert.True(someStruct.Exists(val => val == 1));
        Assert.True(someNullable.Exists(val => val == 1));
        Assert.True(someNullableEmpty.Exists(val => val == null));
        Assert.True(someClass.Exists(val => val == "1"));
        Assert.True(someClassNull.Exists(val => val == null));

        Assert.False(someStruct.Contains(-1));
        Assert.False(someNullable.Contains(-1));
        Assert.False(someNullableEmpty.Contains(1));
        Assert.False(someClass.Contains("-1"));
        Assert.False(someClassNull.Contains("1"));

        Assert.False(someStruct.Exists(val => val != 1));
        Assert.False(someNullable.Exists(val => val != 1));
        Assert.False(someNullableEmpty.Exists(val => val != null));
        Assert.False(someClass.Exists(val => val != "1"));
        Assert.False(someClassNull.Exists(val => val != null));
    }

    [Fact]
    public void Either_StringRepresentation()
    {
        Assert.Equal("None(Error='No value')", Optional.None<int>("No value").ToString());
        Assert.Equal("None(Error='No value')", Optional.None<int>(Error.Create("No value")).ToString());
        Assert.Equal("None(Error='No value', Caused by=Exception: 'An exception occurred')", Optional.None<int>("No value", new Exception("An exception occurred")).ToString());

        Assert.Equal("Some(null)", Optional.Some<int?>(null).ToString());
        Assert.Equal("Some(null)", Optional.Some<string>(null).ToString());

        Assert.Equal("Some(1)", Optional.Some(1).ToString());
        Assert.Equal("Some(1)", Optional.Some<int?>(1).ToString());
        Assert.Equal("Some(1)", Optional.Some("1").ToString());


        var now = DateTime.Now;

        Assert.Equal($"Some({now})", Optional.Some(now).ToString());
        Assert.Equal($"Some({now} | 'Success')", Optional.Some(now, "Success").ToString());
        Assert.Equal($"Some({now} | 'Success')", Optional.Some(now, Success.Create("Success")).ToString());
    }

    [Fact]
    public void Either_GetValue()
    {
        var noneStruct = Optional.None<int>("ex");
        var noneStructNullable = Optional.None<int?>("ex");
        var noneReferenceType = Optional.None<string>("ex");

        Assert.Equal(-1, noneStruct.ValueOr(-1));
        Assert.Equal(-1, noneStructNullable.ValueOr(-1));
        Assert.Equal("-1", noneReferenceType.ValueOr("-1"));

        var someStruct = Optional.Some(1);
        var someNullableStruct = Optional.Some<int?>(1);
        var someNullableStructNull = Optional.Some<int?>(null);
        var someReferenceType = Optional.Some("1");
        var someReferenceTypeNull = Optional.Some<string>(null);

        Assert.Equal(1, someStruct.ValueOr(-1));
        Assert.Equal(1, someNullableStruct.ValueOr(-1));
        Assert.Null(someNullableStructNull.ValueOr(-1));
        Assert.Equal("1", someReferenceType.ValueOr("-1"));
        Assert.Null(someReferenceTypeNull.ValueOr("-1"));
    }

    [Fact]
    public void Either_GetValueLazy()
    {
        var noneStruct = Optional.None<int>("ex");
        var noneStructNullable = Optional.None<int?>("ex");
        var noneReferenceType = Optional.None<string>("ex");

        Assert.Equal(-1, noneStruct.ValueOr(() => -1));
        Assert.Equal(noneStructNullable.ValueOr(() => -1), -1);
        Assert.Equal("-1", noneReferenceType.ValueOr(() => "-1"));


        var someStruct = Optional.Some(1);
        var someNullable = Optional.Some<int?>(1);
        var someNullableEmpty = Optional.Some<int?>(null);
        var someClass = Optional.Some("1");
        var someClassNull = Optional.Some<string>(null);

        Assert.Equal(1, someStruct.ValueOr(() => -1));
        Assert.Equal(1, someNullable.ValueOr(() => -1));
        Assert.Null(someNullableEmpty.ValueOr(() => -1));
        Assert.Equal("1", someClass.ValueOr(() => "-1"));
        Assert.Null(someClassNull.ValueOr(() => "-1"));

        Assert.Equal(1, someStruct.ValueOr(() =>
        {
            Assert.Fail();

            return -1;
        }));

        Assert.Equal(1, someNullable.ValueOr(() =>
        {
            Assert.Fail();

            return -1;
        }));

        Assert.Null(someNullableEmpty.ValueOr(() =>
        {
            Assert.Fail();

            return -1;
        }));

        Assert.Equal("1", someClass.ValueOr(() =>
        {
            Assert.Fail();

            return "-1";
        }));

        Assert.Null(someClassNull.ValueOr(() =>
        {
            Assert.Fail();

            return "-1";
        }));
    }

    [Fact]
    public void Either_AlternativeValue()
    {
        var noneStruct = Optional.None<int>("ex");
        var noneNullable = Optional.None<int?>("ex");
        var noneClass = Optional.None<string>("ex");

        Assert.False(noneStruct.HasValue);
        Assert.False(noneNullable.HasValue);
        Assert.False(noneClass.HasValue);

        var someStruct = noneStruct.Or(1);
        var someNullable = noneNullable.Or(1);
        var someClass = noneClass.Or("1");

        Assert.True(someStruct.HasValue);
        Assert.True(someNullable.HasValue);
        Assert.True(someClass.HasValue);

        Assert.Equal(1, someStruct.ValueOr(-1));
        Assert.Equal(1, someNullable.ValueOr(-1));
        Assert.Equal("1", someClass.ValueOr("-1"));
    }

    [Fact]
    public void Either_AlternativeOption()
    {
        var noneStruct = Optional.None<int>("ex");
        var noneNullable = Optional.None<int?>("ex");
        var noneClass = Optional.None<string>("ex");

        Assert.False(noneStruct.HasValue);
        Assert.False(noneNullable.HasValue);
        Assert.False(noneClass.HasValue);

        var noneStruct2 = noneStruct.Else(Optional.None<int>("ex2"));
        var noneNullable2 = noneNullable.Else(Optional.None<int?>("ex2"));
        var noneClass2 = noneClass.Else(Optional.None<string>("ex2"));

        Assert.False(noneStruct2.HasValue);
        Assert.False(noneNullable2.HasValue);
        Assert.False(noneClass2.HasValue);

        var someStruct = noneStruct.Else(1.Some());
        var someNullable = noneNullable.Else(Optional.Some<int?>(1));
        var someClass = noneClass.Else("1".Some());

        Assert.True(someStruct.HasValue);
        Assert.True(someNullable.HasValue);
        Assert.True(someClass.HasValue);

        Assert.Equal(1, someStruct.ValueOr(-1));
        Assert.Equal(1, someNullable.ValueOr(-1));
        Assert.Equal("1", someClass.ValueOr("-1"));
    }

    [Fact]
    public void Either_AlternativeValueLazy()
    {
        var noneStruct = Optional.None<int>("ex");
        var noneNullable = Optional.None<int?>("ex");
        var noneClass = Optional.None<string>("ex");

        Assert.False(noneStruct.HasValue);
        Assert.False(noneNullable.HasValue);
        Assert.False(noneClass.HasValue);

        var someStruct = noneStruct.Or(() => 1);
        var someNullable = noneNullable.Or(() => 1);
        var someClass = noneClass.Or(() => "1");

        Assert.True(someStruct.HasValue);
        Assert.True(someNullable.HasValue);
        Assert.True(someClass.HasValue);

        Assert.Equal(1, someStruct.ValueOr(() => -1));
        Assert.Equal(1, someNullable.ValueOr(() => -1));
        Assert.Equal("1", someClass.ValueOr(() => "-1"));

        someStruct.Or(() =>
        {
            Assert.Fail();

            return -1;
        });

        someNullable.Or(() =>
        {
            Assert.Fail();

            return -1;
        });

        someClass.Or(() =>
        {
            Assert.Fail();

            return "-1";
        });
    }

    [Fact]
    public void Either_AlternativeOptionLazy()
    {
        var noneStruct = Optional.None<int>("ex");
        var noneNullable = Optional.None<int?>("ex");
        var noneClass = Optional.None<string>("ex");

        Assert.False(noneStruct.HasValue);
        Assert.False(noneNullable.HasValue);
        Assert.False(noneClass.HasValue);

        var noneStruct2 = noneStruct.Else(() => Optional.None<int>("ex2"));
        var noneNullable2 = noneNullable.Else(() => Optional.None<int?>("ex2"));
        var noneClass2 = noneClass.Else(() => Optional.None<string>("ex2"));

        Assert.False(noneStruct2.HasValue);
        Assert.False(noneNullable2.HasValue);
        Assert.False(noneClass2.HasValue);

        var someStruct = noneStruct.Else(() => 1.Some());
        var someNullable = noneNullable.Else(() => Optional.Some<int?>(1));
        var someClass = noneClass.Else(() => "1".Some());

        Assert.True(someStruct.HasValue);
        Assert.True(someNullable.HasValue);
        Assert.True(someClass.HasValue);

        Assert.Equal(1, someStruct.ValueOr(() => -1));
        Assert.Equal(1, someNullable.ValueOr(() => -1));
        Assert.Equal("1", someClass.ValueOr(() => "-1"));

        someStruct.Else(() =>
        {
            Assert.Fail();

            return (-1).Some();
        });
        someNullable.Else(() =>
        {
            Assert.Fail();

            return Optional.Some<int?>(-1);
        });
        someClass.Else(() =>
        {
            Assert.Fail();

            return "-1".Some();
        });
    }

    [Fact]
    public void Either_CreateExtensions()
    {
        var none = Optional.None<int>("ex");
        var some = 1.Some();

        Assert.Equal(-1, none.ValueOr(-1));
        Assert.Equal(1, some.ValueOr(-1));

        var noneLargerThanTen = 1.SomeWhen(x => x > 10, "ex");
        var someLargerThanTen = 20.SomeWhen(x => x > 10, "ex");

        Assert.Equal(-1, noneLargerThanTen.ValueOr(-1));
        Assert.Equal(20, someLargerThanTen.ValueOr(-1));

        var noneNotNull = (null as string).SomeNotNull("ex");
        var someNotNull = "1".SomeNotNull("ex");

        Assert.Equal("-1", noneNotNull.ValueOr("-1"));
        Assert.Equal("1", someNotNull.ValueOr("-1"));

        var noneNullableNotNull = (null as int?).SomeNotNull("ex");
        var someNullableNotNull = (1 as int?).SomeNotNull("ex");

        Assert.IsType<int>(noneNullableNotNull.ValueOr(-1));
        Assert.IsType<int>(someNullableNotNull.ValueOr(-1));
        Assert.Equal(-1, noneNullableNotNull.ValueOr(-1));
        Assert.Equal(1, someNullableNotNull.ValueOr(-1));

        var noneFromNullable = (null as int?).ToOption("ex");
        var someFromNullable = (1 as int?).ToOption("ex");

        Assert.IsType<int>(noneFromNullable.ValueOr(-1));
        Assert.IsType<int>(someFromNullable.ValueOr(-1));
        Assert.Equal(-1, noneFromNullable.ValueOr(-1));
        Assert.Equal(1, someFromNullable.ValueOr(-1));
    }

    [Fact]
    public void Either_Matching()
    {
        var none = Optional.None<string>("ex");
        var some = "val".Some();

        var failure = none.Match(
                                 some: val => val,
                                 none: ex => ex.Message
                                );

        var success = some.Match(
                                 some: val => val,
                                 none: e => e.Message
                                );

        Assert.Equal("ex", failure);
        Assert.Equal("val", success);

        var hasMatched = false;
        none.Match(
                   some: val => Assert.Fail(),
                   none: e => hasMatched = e.Message == "ex"
                  );

        Assert.True(hasMatched);

        hasMatched = false;
        some.Match(
                   some: val => hasMatched = val == "val",
                   none: e => Assert.Fail()
                  );

        Assert.True(hasMatched);

        none.MatchSome(val => Assert.Fail());


        hasMatched = false;
        some.MatchSome(val => hasMatched = val == "val");

        Assert.True(hasMatched);

        some.MatchNone(e => Assert.Fail());

        hasMatched = false;
        none.MatchNone(e => hasMatched = e.Message == "ex");

        Assert.True(hasMatched);
    }

    [Fact]
    public void Either_Transformation()
    {
        var none = Optional.None<string>("ex");
        var some = "val".Some();

        var noneNull = Optional.None<string>("ex");
        var someNull = (null as string).Some();

        var noneUpper = none.Map(x => x.ToUpper());
        var someUpper = some.Map(x => x.ToUpper());

        Assert.False(noneUpper.HasValue);
        Assert.True(someUpper.HasValue);
        Assert.Equal("ex", noneUpper.ValueOr("ex"));
        Assert.Equal("VAL", someUpper.ValueOr("ex"));

        var noneNotNull = none.FlatMap(x => x.SomeNotNull("ex1"));
        var someNotNull = some.FlatMap(x => x.SomeNotNull("ex1"));
        var noneNullNotNull = noneNull.FlatMap(x => x.SomeNotNull("ex1"));
        var someNullNotNull = someNull.FlatMap(x => x.SomeNotNull("ex1"));

        Assert.False(noneNotNull.HasValue);
        Assert.True(someNotNull.HasValue);
        Assert.False(noneNullNotNull.HasValue);
        Assert.False(someNullNotNull.HasValue);
        Assert.Equal("ex", noneNotNull.Match(val => val, e => e.Message));
        Assert.Equal("val", someNotNull.Match(val => val, e => e.Message));
        Assert.Equal("ex", noneNullNotNull.Match(val => val, e => e.Message));
        Assert.Equal("ex1", someNullNotNull.Match(val => val, e => e.Message));
    }

    [Fact]
    public void Either_Flatten()
    {
        var noneNone = Optional.None<Option<string>>("1");
        var someNone = Optional.Some<Option<string>>(Optional.None<string>("2"));
        var someSome = Optional.Some<Option<string>>(Optional.Some("a"));

        Assert.False(noneNone.HasValue);
        Assert.False(noneNone.Flatten().HasValue);
        noneNone.Flatten().Match(
                                 some: val => Assert.Fail(),
                                 none: e => Assert.Equal("1", e.Message)
                                );


        Assert.True(someNone.HasValue);
        Assert.False(someNone.Flatten().HasValue);
        someNone.Flatten().Match(
                                 some: val => Assert.Fail(),
                                 none: e => Assert.Equal("2", e.Message)
                                );

        Assert.True(someSome.HasValue);
        Assert.True(someSome.Flatten().HasValue);
        Assert.Equal("a", someSome.Flatten().ValueOr("b"));
    }

    [Fact]
    public void Either_Filtering()
    {
        var none = Optional.None<string>("ex");
        var some = "val".Some();

        var noneNotVal = none.Filter(x => x != "val", Error.Create("ex1"));
        var someNotVal = some.Filter(x => x != "val", Error.Create("ex1"));
        var noneVal = none.Filter(x => x == "val", Error.Create("ex1"));
        var someVal = some.Filter(x => x == "val", Error.Create("ex1"));

        Assert.False(noneNotVal.HasValue);
        Assert.False(someNotVal.HasValue);
        Assert.False(noneVal.HasValue);
        Assert.True(someVal.HasValue);
        Assert.Equal("ex", noneNotVal.Match( val => val, e => e.Message));
        Assert.Equal("ex1", someNotVal.Match( val => val, e => e.Message));
        Assert.Equal("ex", noneVal.Match( val => val, e => e.Message));
        Assert.Equal("val", someVal.Match( val => val, e => e.Message));

        var noneFalse = none.Filter(x => false, Error.Create("ex1"));
        var someFalse = some.Filter(x => false, Error.Create("ex1"));
        var noneTrue = none.Filter(x => true, Error.Create("ex1"));
        var someTrue = some.Filter(x => true, Error.Create("ex1"));

        Assert.False(noneFalse.HasValue);
        Assert.False(someFalse.HasValue);
        Assert.False(noneTrue.HasValue);
        Assert.True(someTrue.HasValue);
        Assert.Equal("ex", noneFalse.Match( val => val, e => e.Message));
        Assert.Equal("ex1", someFalse.Match( val => val, e => e.Message));
        Assert.Equal("ex", noneTrue.Match( val => val, e => e.Message));
        Assert.Equal("val", someTrue.Match( val => val, e => e.Message));

        var someNull = Optional.Some(null as string);
        Assert.True(someNull.HasValue);
        Assert.False(someNull.NotNull(Error.Create("ex")).HasValue);

        var someNullableNull = Optional.Some(null as int?);
        Assert.True(someNullableNull.HasValue);
        Assert.False(someNullableNull.NotNull(Error.Create("-1")).HasValue);

        var someStructNull = Optional.Some(default(int));
        Assert.True(someStructNull.HasValue);
        Assert.True(someStructNull.NotNull(Error.Create("-1")).HasValue);

        Assert.True(some.HasValue);
        Assert.True(some.NotNull(Error.Create("ex")).HasValue);

        Assert.False(none.HasValue);
        Assert.False(none.NotNull(Error.Create("ex1")).HasValue);


        var noneFalseWithReason = none.Filter(x => false, Error.Create("ex1"), "Predicate execution skipped");
        Assert.Equal("Predicate execution skipped", noneFalseWithReason.Match(val => val, e => e.Message));
        Assert.NotEqual("ex1", noneFalseWithReason.Match(val => val, e => e.Message));
    }

    [Fact]
    public void Either_ToEnumerable()
    {
        var none = Optional.None<string>("ex");
        var some = "a".Some();

        var noneAsEnumerable = none.ToEnumerable();
        var someAsEnumerable = some.ToEnumerable();

        foreach (var _ in noneAsEnumerable)
        {
            Assert.Fail();
        }

        var count = 0;

        foreach (var value in someAsEnumerable)
        {
            Assert.Equal("a", value);

            count += 1;
        }

        Assert.Equal(1, count);

        foreach (var value in someAsEnumerable)
        {
            Assert.Equal("a", value);

            count += 1;
        }

        Assert.Equal(2, count);

        Assert.Empty(noneAsEnumerable);
        Assert.Single(someAsEnumerable);
    }

    [Fact]
    public void Either_Enumerate()
    {
        var none = Optional.None<string>("ex");
        var some = "a".Some();

        foreach (var _ in none)
        {
            Assert.Fail();
        }

        var count = 0;

        foreach (var (value, _) in some)
        {
            Assert.Equal("a", value);

            count += 1;
        }

        Assert.Equal(1, count);

        foreach (var (value, _) in some)
        {
            Assert.Equal("a", value);

            count += 1;
        }

        Assert.Equal(2, count);
    }
}
