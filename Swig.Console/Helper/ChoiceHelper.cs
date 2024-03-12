namespace Swig.Console.Helper;

public class ChoiceHelper
{
    public static void Choice(string choice, string answer, Action action)
    {
        if (choice.SequenceEqual(answer))
            action.Invoke();
    }
}