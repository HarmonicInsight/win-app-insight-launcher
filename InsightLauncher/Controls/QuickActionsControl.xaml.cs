using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using InsightLauncher.ViewModels;

namespace InsightLauncher.Controls;

public partial class QuickActionsControl : UserControl
{
    public QuickActionsControl()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<QuickActionsViewModel>();
    }
}
