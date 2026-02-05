using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using InsightLauncher.ViewModels;

namespace InsightLauncher.Controls;

public partial class RecentFilesControl : UserControl
{
    public RecentFilesControl()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<RecentFilesViewModel>();
    }
}
