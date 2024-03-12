using System.Runtime.InteropServices;
using SmartFormat;
using Spectre.Console;
using Swig.Console.Style.Models.Children;

namespace Swig.Console.Style.Layouts.Children;

public class RestoreBackupLayout : BaseChildLayout
{
    private RestoreBackupModel Model { get; set; }
    
    public RestoreBackupLayout(ILayout parent) : base(parent)
    {
        this.Model = new RestoreBackupModel();
    }

    public override void DrawLayout()
    {
        string errorRestoreLabel =
            Smart.Format(this.Model.ErrorRestoreString, Swig.Instance.AreEmojisAllowed);
        
        if (Swig.Instance.ProfileManager.RestoreBackup())
        {
            AnsiConsole.MarkupLine(this.Model.SuccessRestoreString);
        }
        else
        {
            AnsiConsole.MarkupLine(errorRestoreLabel);
        }
                
        System.Console.ReadKey();
        AnsiConsole.Clear();
        DrawParent();
    }
}