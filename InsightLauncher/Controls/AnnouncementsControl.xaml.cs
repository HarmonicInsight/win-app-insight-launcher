using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using InsightLauncher.ViewModels;

namespace InsightLauncher.Controls;

public partial class AnnouncementsControl : UserControl
{
    public AnnouncementsControl()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<AnnouncementsViewModel>();
    }
}
