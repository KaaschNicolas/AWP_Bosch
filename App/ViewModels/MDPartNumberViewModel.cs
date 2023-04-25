﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Contracts.Services;
using App.Core.Models;
using App.Models;
using App.Services;
using App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using WinUIEx;

namespace App.ViewModels;

public class MDPartNumberViewModel : ObservableRecipient, INavigationAware
{

    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<PartNumber> Source { get; } = new ObservableCollection<PartNumber>();

    public MDPartNumberViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}

