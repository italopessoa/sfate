using System;
using BAU.Api.Utils;
using Xunit;

namespace BAU.Test.Utils
{
    /// <summary>
    /// Teste for DataTimeExtensions class
    /// </summary>
    public class DataTimeExtensionsTest
    {
        [Fact]
        public void NextDayOfWeek()
        {
            DateTime monday = new DateTime(2017, 12, 11);//monday

            DateTime expectedNextTuesday = monday.AddDays(1);
            DateTime expectedNextWednesday = expectedNextTuesday.AddDays(1);
            DateTime expectedNextThursday = expectedNextWednesday.AddDays(1);
            DateTime expectedNextFriday = expectedNextThursday.AddDays(1);
            DateTime expectedNextSaturday = expectedNextFriday.AddDays(1);
            DateTime expectedNextSunday = expectedNextSaturday.AddDays(1);
            DateTime expectedNextMonday = expectedNextSunday.AddDays(1);

            Assert.Equal(expectedNextTuesday, monday.NextDayOfWeek(DayOfWeek.Tuesday));
            Assert.Equal(expectedNextWednesday, monday.NextDayOfWeek(DayOfWeek.Wednesday));
            Assert.Equal(expectedNextThursday, monday.NextDayOfWeek(DayOfWeek.Thursday));
            Assert.Equal(expectedNextFriday, monday.NextDayOfWeek(DayOfWeek.Friday));
            Assert.Equal(expectedNextSaturday, monday.NextDayOfWeek(DayOfWeek.Saturday));
            Assert.Equal(expectedNextSunday, monday.NextDayOfWeek(DayOfWeek.Sunday));
            Assert.Equal(expectedNextMonday, monday.NextDayOfWeek(DayOfWeek.Monday));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void NextDayOfWeek_NWeeksAhead(int weeks)
        {
            DateTime monday = new DateTime(2017, 12, 11);//monday
            monday = monday.NextDayOfWeek(DayOfWeek.Monday, weeks); //monday.NextDayOfWeek(DayOfWeek.Monday).NextDayOfWeek(DayOfWeek.Monday);

            DateTime expectedNextTuesday = monday.AddDays(1);
            DateTime expectedNextWednesday = expectedNextTuesday.AddDays(1);
            DateTime expectedNextThursday = expectedNextWednesday.AddDays(1);
            DateTime expectedNextFriday = expectedNextThursday.AddDays(1);
            DateTime expectedNextSaturday = expectedNextFriday.AddDays(1);
            DateTime expectedNextSunday = expectedNextSaturday.AddDays(1);
            DateTime expectedNextMonday = expectedNextSunday.AddDays(1);

            Assert.Equal(expectedNextTuesday, monday.NextDayOfWeek(DayOfWeek.Tuesday));
            Assert.Equal(expectedNextWednesday, monday.NextDayOfWeek(DayOfWeek.Wednesday));
            Assert.Equal(expectedNextThursday, monday.NextDayOfWeek(DayOfWeek.Thursday));
            Assert.Equal(expectedNextFriday, monday.NextDayOfWeek(DayOfWeek.Friday));
            Assert.Equal(expectedNextSaturday, monday.NextDayOfWeek(DayOfWeek.Saturday));
            Assert.Equal(expectedNextSunday, monday.NextDayOfWeek(DayOfWeek.Sunday));
            Assert.Equal(expectedNextMonday, monday.NextDayOfWeek(DayOfWeek.Monday));
        }

        [Fact]
        public void PreviousDayOfWeek()
        {
            DateTime monday = new DateTime(2017, 12, 11);//monday

            DateTime expectedPreviousSunday = monday.AddDays(-1);
            DateTime expectedPreviousSaturday = expectedPreviousSunday.AddDays(-1);
            DateTime expectedPreviousFriday = expectedPreviousSaturday.AddDays(-1);
            DateTime expectedPreviousThursday = expectedPreviousFriday.AddDays(-1);
            DateTime expectedPreviousWednesday = expectedPreviousThursday.AddDays(-1);
            DateTime expectedPreviousTuesday = expectedPreviousWednesday.AddDays(-1);
            DateTime expectedPreviousMonday = expectedPreviousTuesday.AddDays(-1);

            Assert.Equal(expectedPreviousSunday, monday.PreviousDayOfWeek(DayOfWeek.Sunday));
            Assert.Equal(expectedPreviousSaturday, monday.PreviousDayOfWeek(DayOfWeek.Saturday));
            Assert.Equal(expectedPreviousFriday, monday.PreviousDayOfWeek(DayOfWeek.Friday));
            Assert.Equal(expectedPreviousThursday, monday.PreviousDayOfWeek(DayOfWeek.Thursday));
            Assert.Equal(expectedPreviousWednesday, monday.PreviousDayOfWeek(DayOfWeek.Wednesday));
            Assert.Equal(expectedPreviousTuesday, monday.PreviousDayOfWeek(DayOfWeek.Tuesday));
            Assert.Equal(expectedPreviousMonday, monday.PreviousDayOfWeek(DayOfWeek.Monday));
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void PreviousDayOfWeek_NWeeksBefore(int weeks)
        {
            DateTime monday = new DateTime(2017, 12, 11);//monday
            monday = monday.PreviousDayOfWeek(DayOfWeek.Monday, weeks);

            DateTime expectedPreviousSunday = monday.AddDays(-1);
            DateTime expectedPreviousSaturday = expectedPreviousSunday.AddDays(-1);
            DateTime expectedPreviousFriday = expectedPreviousSaturday.AddDays(-1);
            DateTime expectedPreviousThursday = expectedPreviousFriday.AddDays(-1);
            DateTime expectedPreviousWednesday = expectedPreviousThursday.AddDays(-1);
            DateTime expectedPreviousTuesday = expectedPreviousWednesday.AddDays(-1);
            DateTime expectedPreviousMonday = expectedPreviousTuesday.AddDays(-1);

            Assert.Equal(expectedPreviousSunday, monday.PreviousDayOfWeek(DayOfWeek.Sunday));
            Assert.Equal(expectedPreviousSaturday, monday.PreviousDayOfWeek(DayOfWeek.Saturday));
            Assert.Equal(expectedPreviousFriday, monday.PreviousDayOfWeek(DayOfWeek.Friday));
            Assert.Equal(expectedPreviousThursday, monday.PreviousDayOfWeek(DayOfWeek.Thursday));
            Assert.Equal(expectedPreviousWednesday, monday.PreviousDayOfWeek(DayOfWeek.Wednesday));
            Assert.Equal(expectedPreviousTuesday, monday.PreviousDayOfWeek(DayOfWeek.Tuesday));
            Assert.Equal(expectedPreviousMonday, monday.PreviousDayOfWeek(DayOfWeek.Monday));
        }

        [Fact]
        public void NextBusinessDay()
        {
            DateTime monday = new DateTime(2017, 12, 11);
            DateTime nextMonday = monday.NextDayOfWeek(DayOfWeek.Monday);
            DateTime friday = monday.NextDayOfWeek(DayOfWeek.Friday);
            DateTime nextBusinessDay = monday.AddDays(1);
            Assert.Equal(nextBusinessDay, monday.NextBusinessDay());
            Assert.Equal(nextMonday, friday.NextBusinessDay());
        }

        [Fact]
        public void PreviousBusinessDay()
        {
            DateTime monday = new DateTime(2017, 12, 11);
            DateTime previousBusinessDay = monday.PreviousDayOfWeek(DayOfWeek.Friday);
            DateTime previousThursday = previousBusinessDay.PreviousDayOfWeek(DayOfWeek.Thursday);
            Assert.Equal(previousBusinessDay, monday.PreviousBusinessDay());
            Assert.Equal(previousThursday, previousBusinessDay.PreviousBusinessDay());
        }
    }
}
