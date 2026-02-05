using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using InsightLauncher.ViewModels;

namespace InsightLauncher.Controls;

public partial class DateTimeWeatherControl : UserControl
{
    public DateTimeWeatherControl()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<DateTimeWeatherViewModel>();
    }
}
