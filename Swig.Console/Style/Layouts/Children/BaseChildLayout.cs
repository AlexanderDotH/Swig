using Spectre.Console;

namespace Swig.Console.Style.Layouts.Children;

public abstract class BaseChildLayout : ILayout
{
    public ILayout Parent { get; private set; }
    
    public BaseChildLayout(ILayout parent)
    {
        Parent = parent;
    }

    public void DrawParent()
    {
        AnsiConsole.Clear();
        this.Parent.DrawLayout();
    }

    public abstract void DrawLayout();
}