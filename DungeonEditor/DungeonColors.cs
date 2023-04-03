public static class DungeonColors
{
    private static Dictionary<char, ConsoleColor> s_Colors = new();
    static DungeonColors(){
        s_Colors['#'] = ConsoleColor.DarkGray;
        s_Colors['.'] = ConsoleColor.White;
        s_Colors['+'] = ConsoleColor.DarkYellow;
    }
    public static void SetColor(char ch)
    {
        if (s_Colors.TryGetValue(ch, out ConsoleColor color))
        {
            Console.ForegroundColor = color;
        }
        else
        {
            Console.ResetColor();
        }
    }
}