using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using InsightLauncher.ViewModels;

namespace InsightLauncher.Controls;

public partial class TeamControl : UserControl
{
    public TeamControl()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<TeamViewModel>();
    }
}
