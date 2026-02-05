using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InsightLauncher.Models;
using InsightLauncher.Services;

namespace InsightLauncher.ViewModels;

public partial class SurveyViewModel : ObservableObject
{
    private readonly ISurveyService _surveyService;

    [ObservableProperty]
    private ObservableCollection<Survey> _surveys = new();

    [ObservableProperty]
    private Survey? _selectedSurvey;

    [ObservableProperty]
    private string? _selectedOptionId;

    [ObservableProperty]
    private string _textAnswer = string.Empty;

    [ObservableProperty]
    private bool _isSubmitting;

    [ObservableProperty]
    private bool _isSubmitted;

    public int CompletedCount => Surveys.Count(s => s.IsCompleted);
    public int TotalCount => Surveys.Count;
    public ObservableCollection<Survey> PendingSurveys => new(Surveys.Where(s => !s.IsCompleted));

    public SurveyViewModel(ISurveyService surveyService)
    {
        _surveyService = surveyService;
        LoadSurveysAsync();
    }

    [RelayCommand]
    private void SelectSurvey(Survey survey)
    {
        SelectedSurvey = survey;
        SelectedOptionId = null;
        TextAnswer = string.Empty;
        IsSubmitted = false;
    }

    [RelayCommand]
    private void ClearSelection()
    {
        SelectedSurvey = null;
        SelectedOptionId = null;
        TextAnswer = string.Empty;
        IsSubmitted = false;
    }

    [RelayCommand]
    private void SelectOption(string optionId)
    {
        SelectedOptionId = optionId;
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (SelectedSurvey == null) return;

        var answer = SelectedSurvey.Type == SurveyType.Text
            ? TextAnswer
            : SelectedOptionId;

        if (string.IsNullOrEmpty(answer)) return;

        IsSubmitting = true;
        try
        {
            await _surveyService.SubmitAnswerAsync(SelectedSurvey.Id, answer);
            SelectedSurvey.IsCompleted = true;
            IsSubmitted = true;

            OnPropertyChanged(nameof(CompletedCount));
            OnPropertyChanged(nameof(PendingSurveys));

            // 2秒後に選択をクリア
            await Task.Delay(2000);
            ClearSelection();
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    private async Task LoadSurveysAsync()
    {
        var surveys = await _surveyService.GetSurveysAsync();
        Surveys.Clear();
        foreach (var s in surveys)
        {
            Surveys.Add(s);
        }
        OnPropertyChanged(nameof(CompletedCount));
        OnPropertyChanged(nameof(TotalCount));
        OnPropertyChanged(nameof(PendingSurveys));
    }
}
