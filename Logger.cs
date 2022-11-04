namespace MessagePackTools;

public static class Logger
{
    public static void Write(object? value)
    {
        Console.Write(value);
    }

    public static void WriteLine(object? value)
    {
        Console.WriteLine(value);
    }

    public static void WriteLine()
    {
        Console.WriteLine();
    }
}
