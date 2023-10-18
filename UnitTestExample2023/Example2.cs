using System.Runtime;

namespace UnitTestExample2023;

public class Example2
{
    public bool TimeUp()
    {
        if (GetSettingTime() >= DateTime.Now)
        {
            Console.WriteLine("Time is up.");
            return true;
        }
        
        Console.WriteLine("Time is almost up.");
        return false;
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