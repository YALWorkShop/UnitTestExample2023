using System.Runtime;

namespace UnitTestExample2023;

public class Example2
{
    public bool TimeUp()
    {
        if (GetSettingTime() >= GetNow())
        {
            ConsoleWriteLine("Time is up.");
            return true;
        }

        ConsoleWriteLine("Time is almost up.");
        return false;
    }

    private static void ConsoleWriteLine(string printStr)
    {
        Console.WriteLine(printStr);
    }

    protected virtual DateTime GetNow()
    {
        return DateTime.Now;
    }

    protected virtual DateTime GetSettingTime()
    {
        return Profile.SettingTime;
    }
}

public static class Profile
{
    public static DateTime SettingTime { get; set; }
}