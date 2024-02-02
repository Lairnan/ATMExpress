namespace ConsoleApplication.Behaviors;

public static class Extension
{
    public static int TryGetInt(string enterText)
    {
        do
        {
            Console.Write(enterText);
            var input = Console.ReadLine();
            if (int.TryParse(input, out var result)) return result;
            Console.WriteLine(Translate.GetString("input_error"));
        } while (true);
    }
}