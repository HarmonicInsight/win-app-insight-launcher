using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using InsightLauncher.ViewModels;

namespace InsightLauncher.Controls;

public partial class TodoControl : UserControl
{
    public TodoControl()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<TodoViewModel>();
    }
}
