using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using InsightLauncher.ViewModels;

namespace InsightLauncher.Controls;

public partial class CalendarControl : UserControl
{
    public CalendarControl()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<CalendarViewModel>();
    }
}
