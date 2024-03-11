namespace Swig.Console;

public class Program
{
    protected Program() {}
    
    public static void Main(string[] args) => new Swig(args).Run();
}