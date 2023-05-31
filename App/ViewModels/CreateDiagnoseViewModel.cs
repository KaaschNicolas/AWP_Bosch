﻿using App.Contracts.Services;
using App.Controls;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;


namespace App.ViewModels;
public partial class CreateDiagnoseViewModel : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    private string _name;

    private readonly ICrudService<Diagnose> _crudService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;

    public CreateDiagnoseViewModel(ICrudService<Diagnose> crudService, IInfoBarService infoBarService, INavigationService navigationService)
    {
        _crudService = crudService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
    }



    [RelayCommand]
    public async Task Save()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            var response = await _crudService.Create(new Diagnose { Name = _name });
            if (response != null)
            {
                if (response.Code == ResponseCode.Success)
                {
                    _infoBarService.showMessage("Erfolgreich Leiterplatte erstellt", "Erfolg");
                    _navigationService.NavigateTo("App.ViewModels.DiagnoseViewModel");
                }
                else
                {
                    // TODO Fehler in Dict damit man leichter Fehler ändern kann
                    _infoBarService.showError("Leiterplatte konnte nicht erstellt werden", "Error");
                }
            }
            else
            {
                _infoBarService.showError("Leiterplatte konnte nicht erstellt werden", "Error");
            }


        }

    }

    [RelayCommand]
    public void Cancel()
    {
        _navigationService.GoBack();
    }

}

