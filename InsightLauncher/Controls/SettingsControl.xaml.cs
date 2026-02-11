using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using InsightLauncher.ViewModels;

namespace InsightLauncher.Controls;

public partial class SettingsControl : UserControl
{
    public SettingsControl()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<SettingsViewModel>();
    }
}
