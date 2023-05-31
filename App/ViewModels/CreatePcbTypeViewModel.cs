﻿using App.Contracts.Services;
using App.Controls;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels;

public partial class CreatePcbTypeViewModel : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    [RegularExpression(@"^[0-9]{10}$")]
    [MinLength(10, ErrorMessage = ValidationErrorMessage.MinLength)]
    private string _pcbPartNumber;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    [MaxLength(100, ErrorMessage = ValidationErrorMessage.MaxLength)]
    private string _description;

    [ObservableProperty]
    private double _maxTransfer;

    private readonly ICrudService<PcbType> _crudService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;
    public CreatePcbTypeViewModel(ICrudService<PcbType> crudService, IInfoBarService infoBarService, INavigationService navigationService)
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
            var response = await _crudService.Create(new PcbType { PcbPartNumber = _pcbPartNumber, Description = _description, MaxTransfer = (int)_maxTransfer });
            if (response != null)
            {
                if (response.Code == ResponseCode.Success)
                {
                    _infoBarService.showMessage("Sachnummer erfolgreich erstellt", "Erfolg");
                    _navigationService.NavigateTo("App.ViewModels.PcbTypeViewModel");
                }
                else
                {
                    _infoBarService.showError("Sachnummer konnte nicht erstellt werden", "Error");
                }

            }
            else
            {
                _infoBarService.showError("Sachnummer konnte nicht erstellt werden", "Error");
            }
        }

    }

    [RelayCommand]
    public void Cancel()
    {
        _navigationService.GoBack();
    }

}
