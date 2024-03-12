using System.Runtime.InteropServices;
using SmartFormat;
using Spectre.Console;
using Swig.Console.Style.Models.Children;

namespace Swig.Console.Style.Layouts.Children;

public abstract class BaseChildLayout : ILayout
{
    protected ILayout Parent { get; private set; }
    private BaseChildModel Model { get; set; }
    
    protected BaseChildLayout(ILayout parent)
    {
        this.Model = new BaseChildModel();
        this.Parent = parent;
    }

    public void DrawParent()
    {
        AnsiConsole.Clear();
        this.Parent.DrawLayout();
    }

    public abstract void DrawLayout();

    protected string FormattedBackLabel =>
        Smart.Format(this.Model.GoBackActionString, Swig.Instance.AreEmojisAllowed);
}