﻿namespace UnitTestExample2023;

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
        _example2.SetSettingTime(_settingTime);
        _example2.SetNow(_now);

        var actual = _example2.TimeUp();
        Assert.AreEqual(false, actual);
        Assert.AreEqual("Time is almost up.", _example2.ActualPrintStr);
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