namespace Swig.Console.Style.Models.Children;

public class UserSetupModel : ILayoutModel
{
    public readonly string WelcomePromptString = "[dodgerblue1]W[/][blueviolet]e[/][slateblue3_1]l[/][royalblue1]c[/][lightslateblue]o[/][mediumpurple]m[/][slateblue1]e[/] to Swig! Before we start, we have something to check\n";
    
    public readonly string CanBeSeenString = "Can you identify {0:choose(True|False):yourself|this} as an Airplane?: :airplane:  ";
    public readonly string ConfirmCanNotBeSeenString = "Are you really sure that this is [red1]not[/] an airplane? ([mediumturquoise]yes[/] = enable emojis): :airplane:";
    public readonly string ConfirmCanBeSeenString = "Are you sure that this [mediumturquoise]is[/] an airplane ([mediumturquoise]yes[/] = enable emojis)? :winking_face:";
}