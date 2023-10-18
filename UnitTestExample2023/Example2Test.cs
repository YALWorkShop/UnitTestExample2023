using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestExample2023
{
    public class Example2Test
    {
        private readonly DateTime _settingTime = new DateTime(2023, 12, 24, 23, 59, 59);
        private readonly DateTime _now = new DateTime(2023, 12, 25, 0, 0, 0);

        [Test]
        public void TimeUpTest_time_is_almost_up()
        {
            var example2 = new FakeExample2();
            example2.SetSettingTime(_settingTime);
            example2.SetNow(_now);

            var actual = example2.TimeUp();
            Assert.AreEqual(false, actual);
            Assert.AreEqual("Time is up.", example2.ActualPrintStr);
        }
    }

    public class FakeExample2 : Example2
    {
        private DateTime _settingTime;
        private DateTime _now;
        private string _actualPrintStr;

        public string ActualPrintStr => _actualPrintStr;

        public void SetSettingTime(DateTime settingTime)
        {
            _settingTime = settingTime;
        }

        public void SetNow(DateTime now)
        {
            _now = now;
        }

        protected override void ConsoleWriteLine(string printStr)
        {
            _actualPrintStr = printStr;
        }

        protected override DateTime GetNow()
        {
            return _now;
        }

        protected override DateTime GetSettingTime()
        {
            return _settingTime;
        }
    }
}
