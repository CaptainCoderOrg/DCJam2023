using System;
using static Stat;
using static DualStat;

[Flags]
public enum Stat
{
    Body = 1, //0b0000_0001,
    Mind = 2, //0b0000_0010,
    Sun =  4, //0b0000_0100,
    Moon = 8, //0b0000_1000,
    Yin  = 16, //0b0001_0000,
    Yang = 32, //0b0010_0000
}

public enum DualStat
{
    BodyMind = Stat.Body | Stat.Mind,
    SunMoon = Stat.Sun | Stat.Moon,
    YinYang = Stat.Yin | Stat.Yang
}

public static class StatExtensions
{
    public static (Stat Left, Stat Right) Parts(this DualStat dualStat)
    {
        return dualStat switch
        {
            BodyMind => (Body, Mind),
            SunMoon => (Sun, Moon),
            YinYang => (Yin, Yang),
            _ => throw new NotImplementedException()
        };
    }
}
