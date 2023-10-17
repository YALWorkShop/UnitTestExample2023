using System.Runtime;

namespace UnitTestExample2023;

public class Example2
{
    public bool TimeUp()
    {
        if (Profile.SettingTime >= DateTime.Now)
        {
            Console.WriteLine("Time is up.");
            return true;
        }
        
        Console.WriteLine("Time is almost up.");
        return false;
    }
}

public static class Profile
{
    public static DateTime SettingTime { get; set; }
}