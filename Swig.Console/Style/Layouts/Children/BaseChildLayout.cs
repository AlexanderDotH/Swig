using Spectre.Console;

namespace Swig.Console.Style.Layouts.Children;

public abstract class BaseChildLayout : ILayout
{
    protected ILayout Parent { get; private set; }
    
    protected BaseChildLayout(ILayout parent)
    {
        this.Parent = parent;
    }

    public void DrawParent()
    {
        AnsiConsole.Clear();
        this.Parent.DrawLayout();
    }

    public abstract void DrawLayout();
}