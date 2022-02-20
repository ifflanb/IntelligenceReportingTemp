using System;
using IntelligenceReporting.Enums;
using Xunit;
using IntelligenceReporting.Extensions;

namespace IntelligenceReporting.Entities.Extensions;

public class DateTimeExtensionsTests
{
    [Theory]
    [InlineData("2010-01-01", "2010-01-01")]
    [InlineData("2011-01-31", "2011-01-01")]
    [InlineData("2012-02-01", "2012-01-01")]
    [InlineData("2013-02-28", "2013-01-01")]
    [InlineData("2020-12-01", "2020-01-01")]
    [InlineData("2021-12-31", "2021-01-01")]
    public void TestStartOfYear(string input, string expected)
    {
        // Arrange
        var inputDate = DateTime.Parse(input);
        var expectedDate = DateTime.Parse(expected);

        // Act
        var result = inputDate.StartOfYear();

        // Assert
        Assert.Equal(expectedDate, result);
    }

    [Theory]
    [InlineData("2010-01-01", "2010-12-31")]
    [InlineData("2011-01-31", "2011-12-31")]
    [InlineData("2012-02-01", "2012-12-31")]
    [InlineData("2013-02-28", "2013-12-31")]
    [InlineData("2020-12-01", "2020-12-31")]
    [InlineData("2021-12-31", "2021-12-31")]
    public void TestEndOfYear(string input, string expected)
    {
        // Arrange
        var inputDate = DateTime.Parse(input);
        var expectedDate = DateTime.Parse(expected);

        // Act
        var result = inputDate.EndOfYear();

        // Assert
        Assert.Equal(expectedDate, result);
    }

    [Theory]
    [InlineData("2010-01-01", "2010-01-01")]
    [InlineData("2011-01-31", "2011-01-01")]
    [InlineData("2012-02-01", "2012-01-01")]
    [InlineData("2013-02-28", "2013-01-01")]
    [InlineData("2014-03-01", "2014-01-01")]
    [InlineData("2015-03-31", "2015-01-01")]
    [InlineData("2016-04-01", "2016-04-01")]
    [InlineData("2017-06-30", "2017-04-01")]
    [InlineData("2018-07-01", "2018-07-01")]
    [InlineData("2019-09-30", "2019-07-01")]
    [InlineData("2020-10-01", "2020-10-01")]
    [InlineData("2021-12-31", "2021-10-01")]
    public void TestStartOfQuarter(string input, string expected)
    {
        // Arrange
        var inputDate = DateTime.Parse(input);
        var expectedDate = DateTime.Parse(expected);

        // Act
        var result = inputDate.StartOfQuarter();

        // Assert
        Assert.Equal(expectedDate, result);
    }

    [Theory]
    [InlineData("2010-01-01", "2010-03-31")]
    [InlineData("2011-01-31", "2011-03-31")]
    [InlineData("2012-02-01", "2012-03-31")]
    [InlineData("2013-02-28", "2013-03-31")]
    [InlineData("2014-03-01", "2014-03-31")]
    [InlineData("2015-03-31", "2015-03-31")]
    [InlineData("2016-04-01", "2016-06-30")]
    [InlineData("2017-06-30", "2017-06-30")]
    [InlineData("2018-07-01", "2018-09-30")]
    [InlineData("2019-09-30", "2019-09-30")]
    [InlineData("2020-10-01", "2020-12-31")]
    [InlineData("2021-12-31", "2021-12-31")]
    public void TestEndOfQuarter(string input, string expected)
    {
        // Arrange
        var inputDate = DateTime.Parse(input);
        var expectedDate = DateTime.Parse(expected);

        // Act
        var result = inputDate.EndOfQuarter();

        // Assert
        Assert.Equal(expectedDate, result);
    }

    [Theory]
    [InlineData("2010-01-01", "2010-01-01")]
    [InlineData("2011-01-31", "2011-01-01")]
    [InlineData("2012-02-01", "2012-02-01")]
    [InlineData("2013-02-28", "2013-02-01")]
    [InlineData("2014-03-01", "2014-03-01")]
    [InlineData("2015-03-31", "2015-03-01")]
    [InlineData("2016-04-01", "2016-04-01")]
    [InlineData("2017-06-30", "2017-06-01")]
    [InlineData("2018-07-01", "2018-07-01")]
    [InlineData("2019-09-30", "2019-09-01")]
    [InlineData("2020-10-01", "2020-10-01")]
    [InlineData("2021-12-31", "2021-12-01")]
    public void TestStartOfMonth(string input, string expected)
    {
        // Arrange
        var inputDate = DateTime.Parse(input);
        var expectedDate = DateTime.Parse(expected);

        // Act
        var result = inputDate.StartOfMonth();

        // Assert
        Assert.Equal(expectedDate, result);
    }

    [Theory]
    [InlineData("2010-01-01", "2010-01-31")]
    [InlineData("2011-01-31", "2011-01-31")]
    [InlineData("2012-02-01", "2012-02-29")]
    [InlineData("2013-02-28", "2013-02-28")]
    [InlineData("2014-03-01", "2014-03-31")]
    [InlineData("2015-03-31", "2015-03-31")]
    [InlineData("2016-04-01", "2016-04-30")]
    [InlineData("2017-06-30", "2017-06-30")]
    [InlineData("2018-07-01", "2018-07-31")]
    [InlineData("2019-09-30", "2019-09-30")]
    [InlineData("2020-10-01", "2020-10-31")]
    [InlineData("2021-12-31", "2021-12-31")]
    public void TestEndOfMonth(string input, string expected)
    {
        // Arrange
        var inputDate = DateTime.Parse(input);
        var expectedDate = DateTime.Parse(expected);

        // Act
        var result = inputDate.EndOfMonth();

        // Assert
        Assert.Equal(expectedDate, result);
    }

    [Theory]
    [InlineData("2022-01-01", "2022-01-01")]
    [InlineData("2022-01-02", "2022-01-01")]
    [InlineData("2022-01-03", "2022-01-01")]
    [InlineData("2022-01-04", "2022-01-01")]
    [InlineData("2022-01-05", "2022-01-01")]
    [InlineData("2022-01-06", "2022-01-01")]
    [InlineData("2022-01-07", "2022-01-01")]
    [InlineData("2022-01-08", "2022-01-08")]
    [InlineData("2022-01-09", "2022-01-08")]
    public void TestLastSaturday(string input, string expected)
    {
        // Arrange
        var inputDate = DateTime.Parse(input);
        var expectedDate = DateTime.Parse(expected);

        // Act
        var result = inputDate.LastSaturday();

        // Assert
        Assert.Equal(expectedDate, result);
    }

    [Theory]
    [InlineData("2022-01-01", "2022-01-07")]
    [InlineData("2022-01-02", "2022-01-07")]
    [InlineData("2022-01-03", "2022-01-07")]
    [InlineData("2022-01-04", "2022-01-07")]
    [InlineData("2022-01-05", "2022-01-07")]
    [InlineData("2022-01-06", "2022-01-07")]
    [InlineData("2022-01-07", "2022-01-07")]
    [InlineData("2022-01-08", "2022-01-14")]
    [InlineData("2022-01-09", "2022-01-14")]
    public void TestNextFriday(string input, string expected)
    {
        // Arrange
        var inputDate = DateTime.Parse(input);
        var expectedDate = DateTime.Parse(expected);

        // Act
        var result = inputDate.NextFriday();

        // Assert
        Assert.Equal(expectedDate, result);
    }

    public static object[][] TestGetDateRange_TestData
    {
        get
        {
            object[] X(DateRange option, string date, string startDate, string endDate, string expectedStartDate, string expectedEndDate)
                => new object[] { option, DateTime.Parse(date), DateTime.Parse(startDate), DateTime.Parse(endDate), DateTime.Parse(expectedStartDate), DateTime.Parse(expectedEndDate) };

            var irrelevant = DateTime.Today.ToString("yyyy-MM-dd");

            return new[]
            {
                X(DateRange.LastYear, "2020-12-31", irrelevant, irrelevant, "2019-01-01", "2019-12-31"),
                X(DateRange.LastYear, "2021-01-01", irrelevant, irrelevant, "2020-01-01", "2020-12-31"),
                X(DateRange.LastYear, "2021-12-31", irrelevant, irrelevant, "2020-01-01", "2020-12-31"),
                X(DateRange.LastYear, "2022-01-01", irrelevant, irrelevant, "2021-01-01", "2021-12-31"),

                X(DateRange.ThisYear, "2020-12-31", irrelevant, irrelevant, "2020-01-01", "2020-12-31"),
                X(DateRange.ThisYear, "2021-01-01", irrelevant, irrelevant, "2021-01-01", "2021-12-31"),
                X(DateRange.ThisYear, "2021-12-31", irrelevant, irrelevant, "2021-01-01", "2021-12-31"),
                X(DateRange.ThisYear, "2022-01-01", irrelevant, irrelevant, "2022-01-01", "2022-12-31"),

                X(DateRange.PastYear, "2020-12-31", irrelevant, irrelevant, "2019-12-01", "2020-11-30"),
                X(DateRange.PastYear, "2021-01-01", irrelevant, irrelevant, "2020-01-01", "2020-12-31"),
                X(DateRange.PastYear, "2021-01-31", irrelevant, irrelevant, "2020-01-01", "2020-12-31"),
                X(DateRange.PastYear, "2021-02-01", irrelevant, irrelevant, "2020-02-01", "2021-01-31"),
                X(DateRange.PastYear, "2021-02-28", irrelevant, irrelevant, "2020-02-01", "2021-01-31"),
                X(DateRange.PastYear, "2021-03-01", irrelevant, irrelevant, "2020-03-01", "2021-02-28"),
                X(DateRange.PastYear, "2021-03-31", irrelevant, irrelevant, "2020-03-01", "2021-02-28"),

                X(DateRange.LastQuarter, "2020-12-31", irrelevant, irrelevant, "2020-07-01", "2020-09-30"),
                X(DateRange.LastQuarter, "2021-01-01", irrelevant, irrelevant, "2020-10-01", "2020-12-31"),
                X(DateRange.LastQuarter, "2021-01-31", irrelevant, irrelevant, "2020-10-01", "2020-12-31"),
                X(DateRange.LastQuarter, "2021-03-31", irrelevant, irrelevant, "2020-10-01", "2020-12-31"),
                X(DateRange.LastQuarter, "2021-04-01", irrelevant, irrelevant, "2021-01-01", "2021-03-31"),
                X(DateRange.LastQuarter, "2021-06-30", irrelevant, irrelevant, "2021-01-01", "2021-03-31"),
                X(DateRange.LastQuarter, "2021-07-01", irrelevant, irrelevant, "2021-04-01", "2021-06-30"),
                X(DateRange.LastQuarter, "2021-09-30", irrelevant, irrelevant, "2021-04-01", "2021-06-30"),
                X(DateRange.LastQuarter, "2021-10-01", irrelevant, irrelevant, "2021-07-01", "2021-09-30"),
                X(DateRange.LastQuarter, "2021-12-31", irrelevant, irrelevant, "2021-07-01", "2021-09-30"),

                X(DateRange.ThisQuarter, "2020-12-31", irrelevant, irrelevant, "2020-10-01", "2020-12-31"),
                X(DateRange.ThisQuarter, "2021-01-01", irrelevant, irrelevant, "2021-01-01", "2021-03-31"),
                X(DateRange.ThisQuarter, "2021-01-31", irrelevant, irrelevant, "2021-01-01", "2021-03-31"),
                X(DateRange.ThisQuarter, "2021-03-31", irrelevant, irrelevant, "2021-01-01", "2021-03-31"),
                X(DateRange.ThisQuarter, "2021-04-01", irrelevant, irrelevant, "2021-04-01", "2021-06-30"),
                X(DateRange.ThisQuarter, "2021-06-30", irrelevant, irrelevant, "2021-04-01", "2021-06-30"),
                X(DateRange.ThisQuarter, "2021-07-01", irrelevant, irrelevant, "2021-07-01", "2021-09-30"),
                X(DateRange.ThisQuarter, "2021-09-30", irrelevant, irrelevant, "2021-07-01", "2021-09-30"),
                X(DateRange.ThisQuarter, "2021-10-01", irrelevant, irrelevant, "2021-10-01", "2021-12-31"),
                X(DateRange.ThisQuarter, "2021-12-31", irrelevant, irrelevant, "2021-10-01", "2021-12-31"),

                X(DateRange.LastMonth, "2020-12-31", irrelevant, irrelevant, "2020-11-01", "2020-11-30"),
                X(DateRange.LastMonth, "2021-01-01", irrelevant, irrelevant, "2020-12-01", "2020-12-31"),
                X(DateRange.LastMonth, "2021-01-31", irrelevant, irrelevant, "2020-12-01", "2020-12-31"),
                X(DateRange.LastMonth, "2021-03-31", irrelevant, irrelevant, "2021-02-01", "2021-02-28"),
                X(DateRange.LastMonth, "2021-04-01", irrelevant, irrelevant, "2021-03-01", "2021-03-31"),
                X(DateRange.LastMonth, "2021-06-30", irrelevant, irrelevant, "2021-05-01", "2021-05-31"),
                X(DateRange.LastMonth, "2021-07-01", irrelevant, irrelevant, "2021-06-01", "2021-06-30"),
                X(DateRange.LastMonth, "2021-09-30", irrelevant, irrelevant, "2021-08-01", "2021-08-31"),
                X(DateRange.LastMonth, "2021-10-01", irrelevant, irrelevant, "2021-09-01", "2021-09-30"),
                X(DateRange.LastMonth, "2021-12-31", irrelevant, irrelevant, "2021-11-01", "2021-11-30"),

                X(DateRange.ThisMonth, "2020-12-31", irrelevant, irrelevant, "2020-12-01", "2020-12-31"),
                X(DateRange.ThisMonth, "2021-01-01", irrelevant, irrelevant, "2021-01-01", "2021-01-31"),
                X(DateRange.ThisMonth, "2021-01-31", irrelevant, irrelevant, "2021-01-01", "2021-01-31"),
                X(DateRange.ThisMonth, "2021-03-31", irrelevant, irrelevant, "2021-03-01", "2021-03-31"),
                X(DateRange.ThisMonth, "2021-04-01", irrelevant, irrelevant, "2021-04-01", "2021-04-30"),
                X(DateRange.ThisMonth, "2021-06-30", irrelevant, irrelevant, "2021-06-01", "2021-06-30"),
                X(DateRange.ThisMonth, "2021-07-01", irrelevant, irrelevant, "2021-07-01", "2021-07-31"),
                X(DateRange.ThisMonth, "2021-09-30", irrelevant, irrelevant, "2021-09-01", "2021-09-30"),
                X(DateRange.ThisMonth, "2021-10-01", irrelevant, irrelevant, "2021-10-01", "2021-10-31"),
                X(DateRange.ThisMonth, "2021-12-31", irrelevant, irrelevant, "2021-12-01", "2021-12-31"),

                X(DateRange.LastWeek, "2022-01-21", irrelevant, irrelevant, "2022-01-08", "2022-01-14"),
                X(DateRange.LastWeek, "2022-01-22", irrelevant, irrelevant, "2022-01-15", "2022-01-21"),
                X(DateRange.LastWeek, "2022-01-28", irrelevant, irrelevant, "2022-01-15", "2022-01-21"),
                X(DateRange.LastWeek, "2022-01-29", irrelevant, irrelevant, "2022-01-22", "2022-01-28"),

                X(DateRange.ThisWeek, "2022-01-14", irrelevant, irrelevant, "2022-01-08", "2022-01-14"),
                X(DateRange.ThisWeek, "2022-01-15", irrelevant, irrelevant, "2022-01-15", "2022-01-21"),
                X(DateRange.ThisWeek, "2022-01-21", irrelevant, irrelevant, "2022-01-15", "2022-01-21"),
                X(DateRange.ThisWeek, "2022-01-22", irrelevant, irrelevant, "2022-01-22", "2022-01-28"),

                X(DateRange.Yesterday, "2022-01-15", irrelevant, irrelevant, "2022-01-14", "2022-01-14"),
                X(DateRange.Yesterday, "2022-01-16", irrelevant, irrelevant, "2022-01-15", "2022-01-15"),

                X(DateRange.Today, "2022-01-14", irrelevant, irrelevant, "2022-01-14", "2022-01-14"),
                X(DateRange.Today, "2022-01-15", irrelevant, irrelevant, "2022-01-15", "2022-01-15"),

                X(DateRange.Custom, irrelevant, "2021-01-01", "2021-05-14", "2021-01-01", "2021-05-14"),
                X(DateRange.Custom, irrelevant, "2021-11-11", "2022-02-27", "2021-11-11", "2022-02-27"),
            };
        }
    }

    [Theory]
    [MemberData(nameof(TestGetDateRange_TestData))]
    public void TestGetDateRange(DateRange option, DateTime date, DateTime startDate, DateTime endDate, DateTime expectedStartDate, DateTime expectedEndDate)
    {
        // Act
        var (actualStartDate, actualEndDate) = date.GetDateRange(option, startDate, endDate);

        // Assert
        Assert.Equal(expectedStartDate, actualStartDate);
        Assert.Equal(expectedEndDate, actualEndDate);
    }

}
