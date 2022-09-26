using System.ComponentModel;

namespace MauiApp1;

// Learn more about making custom code visible in the Xamarin.Forms previewer
// by visiting https://aka.ms/xamarinforms-previewer
[DesignTimeVisible(false)]
public partial class TabPage : TabbedPage
{
    public TabPage()
    {
        InitializeComponent();

        Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.TabbedPage.SetIsSmoothScrollEnabled(this, false);
    }

}
