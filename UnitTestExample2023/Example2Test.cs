﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestExample2023
{
    public class Example2Test
    {
        [Test]
        public void TimeUpTest_time_is_almost_up()
        {
            var example2 = new Example2();

            var actual = example2.TimeUp();
            Assert.AreEqual(false, actual);
        }
    }

    public class FakeExample2 : Example2
    {
        private DateTime _settingTime;

        public void SetSettingTime(DateTime settingTime)
        {
            _settingTime = settingTime;
        }

        protected override void ConsoleWriteLine(string printStr)
        {
            base.ConsoleWriteLine(printStr);
        }

        protected override DateTime GetNow()
        {
            return base.GetNow();
        }

        protected override DateTime GetSettingTime()
        {
            return _settingTime;
        }
    }
}
