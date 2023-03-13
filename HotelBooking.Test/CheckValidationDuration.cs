using HotelBooking.Data.Comparer;
using HotelBooking.Data.ViewModel;

namespace HotelBooking.Test
{
    [TestFixture]
    public class CheckValidationDuration
    {
        private List<DurationVM> list;
        private List<DurationVM> list1;
        private List<DurationVM> list3;
        [SetUp]
        public void Setup()
        {
            list = new List<DurationVM>()
            {
               new DurationVM{From = new DateTime(2023, 04,01),To = new DateTime(2023,04,03) },
            };
            list1 = new List<DurationVM>()
            {
               new DurationVM{From = new DateTime(2023, 04,01),To = new DateTime(2023,04,03) },
               new DurationVM{From = new DateTime(2023,04,03), To = new DateTime(2023,04,04) }
            };
            list3 = new List<DurationVM>()
            {
               new DurationVM{From = new DateTime(2023, 04,01),To = new DateTime(2023,04,03) },
               new DurationVM{From = new DateTime(2023,04,03), To = new DateTime(2023,04,04) },
               new DurationVM{From = new DateTime(2023,04,10), To = new DateTime(2023,04,12) },
               new DurationVM{From = new DateTime(2023,04,12), To = new DateTime(2023,04,15) },
               new DurationVM{From = new DateTime(2023,04,20), To = new DateTime(2023,04,25) },
            };

        }

        [Test]
        public void Valid_duration_when_checkin_similar_with_checkout_had()
        {
            var duration = new DurationVM { From = new DateTime(2023, 4, 03), To = new DateTime(2023, 04, 04) };
            var result = Check.CheckValidDuration(duration, list);
            Assert.IsTrue(result);
        }

        [Test]
        public void Valid_duration_when_checkout_similar_with_chekin_had()
        {
            var duration = new DurationVM { From = new DateTime(2023, 3, 31), To = new DateTime(2023, 4, 01) };
            var result = Check.CheckValidDuration(duration, list);
            Assert.IsTrue(result);
        }

        [Test]
        public void Invalid_duration_when_checkin_have_and_duration_less_than()
        {
            var duration = new DurationVM { From = new DateTime(2023, 4, 01), To = new DateTime(2023, 4, 02) };
            var result = Check.CheckValidDuration(duration, list);
            Assert.IsFalse(result);
        }

        [Test]
        public void Invalid_duration_when_duration_had()
        {
            var duration = new DurationVM { From = new DateTime(2023, 04, 01), To = new DateTime(2023, 04, 03) };
            var result = Check.CheckValidDuration(duration, list);
            Assert.IsFalse(result);
        }

        [Test]
        public void Invalid_duration_when_duration_in_exist_duration()
        {
            var duration = new DurationVM { From = new DateTime(2023, 03, 31), To = new DateTime(2023, 04, 02) };
            var result = Check.CheckValidDuration(duration, list);
            Assert.IsFalse(result);
        }

        [Test]
        public void Invalid_duration_when_checkin_in_exist_duration()
        {
            var duration = new DurationVM { From = new DateTime(2023, 4, 2), To = new DateTime(2023, 4, 10) };
            var result = Check.CheckValidDuration(duration, list);
            Assert.IsFalse(result);
        }

        [Test]
        public void Valid_duration_when_check_in_and_check_out_have_many_durations_in()
        {
            var duration = new DurationVM { From = new DateTime(2023, 3, 31), To = new DateTime(2023, 4, 1) };
            var result = Check.CheckValidDuration(duration, list1);
            Assert.IsTrue(result);
        }

        [Test]
        public void Invalid_duration_when_check_in_and_check_out_have_one_duration_in()
        {
            var duration = new DurationVM { From = new DateTime(2023, 4, 2), To = new DateTime(2023, 4, 4) };
            var result = Check.CheckValidDuration(duration, list1);
            Assert.IsFalse(result);
        }

        [Test]
        public void Invalid_duration_when_check_out_not_valid()
        {
            var duration = new DurationVM { From = new DateTime(2023, 3, 31), To = new DateTime(2023, 4, 2) };
            var result = Check.CheckValidDuration(duration, list1);
            Assert.IsFalse(result);
        }

        [Test]
        public void Invalid_duration_when_check_in_not_valid()
        {
            var duration = new DurationVM { From = new DateTime(2023, 04, 02), To = new DateTime(2023, 4, 12) };
            var result = Check.CheckValidDuration(duration, list1);
            Assert.IsFalse(result);
        }

        [Test]
        public void Invalid_duration_when_check_in_check_out_not_valid()
        {
            var duration = new DurationVM { From = new DateTime(2023, 04, 01), To = new DateTime(2023, 4, 04) };
            var result = Check.CheckValidDuration(duration, list1);
            Assert.IsFalse(result);
        }

        [Test]
        public void Valid_duration_shorter()
        {
            var duration = new DurationVM { From = new DateTime(2023, 04, 05), To = new DateTime(2023, 4, 07) };
            var result = Check.CheckValidDuration(duration, list3);
            Assert.IsTrue(result);
        }

        [Test]
        public void Valid_when_checkin_and_checkout_similar()
        {
            var duration = new DurationVM { From = new DateTime(2023, 04, 04), To = new DateTime(2023, 4, 10) };
            var result = Check.CheckValidDuration(duration, list3);
            Assert.IsTrue(result);
        }

        [Test]
        public void Valid_when_checkin_like_chekout_latest()
        {
            var duration = new DurationVM { From = new DateTime(2023, 04, 25), To = new DateTime(2023, 04, 30) };
            var result = Check.CheckValidDuration(duration, list3);
            Assert.IsTrue(result);
        }

        [Test]
        public void Invalid_when_checkin_like_chekout_latest()
        {
            var duration = new DurationVM { From = new DateTime(2023, 04, 24), To = new DateTime(2023, 04, 30) };
            var result = Check.CheckValidDuration(duration, list3);
            Assert.IsFalse(result);
        }

        [Test]
        public void Invalid_when_duration_have_child_duration_in()
        {
            var duration = new DurationVM { From = new DateTime(2023, 04, 11), To = new DateTime(2023, 04, 20) };
            var result = Check.CheckValidDuration(duration, list3);
            Assert.IsFalse(result);
        }

        [Test]
        public void Invalid_when_duration_have_child_durations_in()
        {
            var duration = new DurationVM { From = new DateTime(2023, 04, 11), To = new DateTime(2023, 04, 25) };
            var result = Check.CheckValidDuration(duration, list3);
            Assert.IsFalse(result);
        }

        [Test]
        public void Valid_when_duration_have_no_duration_in()
        {
            var duration = new DurationVM { From = new DateTime(2023, 04, 17), To = new DateTime(2023, 04, 20) };
            var result = Check.CheckValidDuration(duration, list3);
            Assert.IsTrue(result);
        }

        [Test]
        public void Invalid_when_duration_have_one_checkin()
        {
            var duration = new DurationVM { From = new DateTime(2023, 04, 2), To = new DateTime(2023, 04, 9) };
            var result = Check.CheckValidDuration(duration, list3);
            Assert.IsFalse(result);
        }

    }
}
