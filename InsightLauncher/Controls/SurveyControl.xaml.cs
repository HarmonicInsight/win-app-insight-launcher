using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using InsightLauncher.ViewModels;

namespace InsightLauncher.Controls;

public partial class SurveyControl : UserControl
{
    public SurveyControl()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<SurveyViewModel>();
    }
}
