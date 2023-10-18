namespace UnitTestExample2023;

public class Example2Test
{
    private readonly DateTime _now = new(2023, 12, 25, 0, 0, 0);
    private readonly DateTime _settingTime = new(2023, 12, 24, 23, 59, 59);
    private FakeExample2 _example2;

    [SetUp]
    public void SetUp()
    {
        _example2 = new FakeExample2();
    }

    [Test]
    public void TimeUpTest_time_is_almost_up()
    {
        GivenSettingTime(_settingTime);
        GivenGetNow(_now);

        TimeUpShouldBe(false, "Time is almost up.");
    }
    [Test]
    public void TimeUpTest_time_is_up_equal()
    {
        GivenSettingTime(_now);
        GivenGetNow(_now);

        TimeUpShouldBe(true, "Time is up.");
    }


    [Test]
    public void TimeUpTest_time_is_up_later_than()
    {
        var laterSettingTime = new DateTime(2023, 12, 25, 01, 20, 0);
        GivenSettingTime(laterSettingTime);
        GivenGetNow(_now);

        TimeUpShouldBe(true, "Time is up.");
    }




    private void TimeUpShouldBe(bool expected, string expectedPrintString)
    {
        var actual = _example2.TimeUp();
        Assert.AreEqual(expected, actual);
        Assert.AreEqual(expectedPrintString, _example2.ActualPrintStr);
    }

    private void GivenGetNow(DateTime now)
    {
        _example2.SetNow(now);
    }

    private void GivenSettingTime(DateTime settingTime)
    {
        _example2.SetSettingTime(settingTime);
    }
}

public class FakeExample2 : Example2
{
    private DateTime _now;
    private DateTime _settingTime;

    public string ActualPrintStr { get; private set; }

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
        ActualPrintStr = printStr;
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