﻿using HotelBooking.Data.Comparer;
using HotelBooking.Data.ViewModel;

namespace HotelBooking.Test
{
    public class CheckValidationDuration
    {
        private List<DurationVM> list;
        [SetUp]
        public void Setup()
        {
            list = new List<DurationVM>()
            {
               new DurationVM{From = new DateTime(2023, 5, 31),To = new DateTime(2023,6,2) },
               new DurationVM{From = new DateTime(2023, 6,2),To = new DateTime(2023,6,4) },
               new DurationVM{From = new DateTime(2023, 4, 15),To = new DateTime(2023,4,17) },
               new DurationVM{From = new DateTime(2023, 4, 10),To =new DateTime(2023,4,12) },
               new DurationVM{From = new DateTime(2023, 4, 8),To =new DateTime(2023,4,10) },
            };
        }

        [Test]
        public void Invalid_duration_when_checkin_and_checkout_had()
        {
            var duration = new DurationVM { From = new DateTime(2023, 5, 31), To = new DateTime(2023, 6, 2) };
            var result = Check.CheckValidDuration(duration, list);
            Assert.IsTrue(!result);
        }

        [Test]
        public void Invalid_duration_when_checkin_have_and_duration_longer_than()
        {
            var duration = new DurationVM { From = new DateTime(2023, 4, 15), To = new DateTime(2023, 4, 18) };
            var result = Check.CheckValidDuration(duration, list);
            Assert.IsTrue(!result);
        }

        [Test]
        public void Valid_duration_when_checkin_have_and_duration_less_than()
        {
            var duration = new DurationVM { From = new DateTime(2023, 4, 12), To = new DateTime(2023, 4, 15) };
            var result = Check.CheckValidDuration(duration, list);
            Assert.IsTrue(result);
        }

        [Test]
        public void Valid_duration_when_checkin_does_not_have()
        {
            var duration = new DurationVM { From = new DateTime(2023, 6, 4), To = new DateTime(2023, 6, 10) };
            var result = Check.CheckValidDuration(duration, list);
            Assert.IsTrue(result);
        }

        [Test]
        public void Valid_duration_when_checkin_earliest()
        {
            var duration = new DurationVM { From = new DateTime(2023, 3, 4), To = new DateTime(2023, 3, 5) };
            var result = Check.CheckValidDuration(duration, list);
            Assert.IsTrue(result);
        }

        [Test]
        public void Invalid_duration_when_check_in_and_check_out_have_many_durations_in()
        {
            var duration = new DurationVM { From = new DateTime(2023, 4, 7), To = new DateTime(2023, 4, 13) };
            var result = Check.CheckValidDuration(duration, list);
            Assert.IsFalse(result);
        }

        [Test]
        public void Invalid_duration_when_check_in_and_check_out_have_one_duration_in()
        {
            var duration = new DurationVM { From = new DateTime(2023, 4, 7), To = new DateTime(2023, 4, 9) };
            var result = Check.CheckValidDuration(duration, list);
            Assert.IsFalse(result);
        }
    }
}
