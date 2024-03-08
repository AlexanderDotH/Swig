using System.Text;
using Microsoft.Extensions.Primitives;
using Swig.Shared.Classes;

namespace Swig.Console.Style.Models;

public class MainLayoutModel : ILayoutModel
{
    public string GetTitle()
    {
        StringBuilder titleBuilder = new StringBuilder();

        // Welcome to Swig, what do you want to do?
        titleBuilder.Append(
            "[dodgerblue1]W[/][blueviolet]e[/][slateblue3_1]l[/][royalblue1]c[/][lightslateblue]o[/][mediumpurple]m[/][slateblue1]e[/] ");
        titleBuilder.Append("to Swig, what do [mediumturquoise]you[/] want to do?\n\n");


        Profile profile = Swig.Instance.ProfileManager.Current;

        if (profile == null)
        {
            titleBuilder.Append("There is no profile set!");
        }
        else
        {
            titleBuilder.Append($"Current profile: [mediumturquoise]{profile.Name}[/]");
        }
        
        
        return titleBuilder.ToString();
    }
}