using System;
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
}
