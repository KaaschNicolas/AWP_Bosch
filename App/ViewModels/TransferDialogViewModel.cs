﻿using App.Contracts.Services;
using App.Core.Models;
using App.Core.Services.Interfaces;
using App.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public partial class TransferDialogViewModel : ObservableValidator
{
    [ObservableProperty]
    private bool _isEnabled = false;

    [ObservableProperty]
    private User _notedBy;

    [ObservableProperty]
    private DateTime _transferDate = DateTime.Now;

    private StorageLocation _selectedStorageLocation;

    public StorageLocation SelectedStorageLocation
    {
        get => _selectedStorageLocation;
        set
        {
            SetProperty(ref _selectedStorageLocation, value);
            IsEnabled = true;
        }
    }

    [ObservableProperty]
    private Diagnose _selectedDiagnose;

    [ObservableProperty]
    private string _commentPcb;

    [ObservableProperty]
    private bool hasMaxTransfer;

    [ObservableProperty]
    private string _maxTransferError;

    public Pcb SelectedPcb;

    [ObservableProperty]
    private ObservableCollection<StorageLocation> _storageLocations = new();

    [ObservableProperty]
    private ObservableCollection<Diagnose> _diagnoses = new();

    private readonly IAuthenticationService _authenticationService;
    private readonly ICrudService<StorageLocation> _storageLocationCrudService;
    private readonly ICrudService<Diagnose> _diagnoseCrudService;
    private readonly ITransferDataService<Transfer> _transferDataService;
    private readonly IPcbDataService<Pcb> _pcbDataService;
    private readonly IInfoBarService _infoBarService;
    public TransferDialogViewModel(IAuthenticationService authenticationService, IPcbDataService<Pcb> pcbDataService, IInfoBarService infoBarServíce, ITransferDataService<Transfer> transferDataService, ICrudService<StorageLocation> storageLocationCrudService, ICrudService<Diagnose> diagnoseCrudService)
    {
        _authenticationService = authenticationService;
        _diagnoseCrudService = diagnoseCrudService;
        _storageLocationCrudService = storageLocationCrudService;
        _infoBarService = infoBarServíce;
        _transferDataService = transferDataService;
        _pcbDataService = pcbDataService;
        _notedBy = _authenticationService.CurrentUser;
        LoadData();
    }


    private async void LoadData()
    {
        SelectedPcb = WeakReferenceMessenger.Default.Send<CurrentPcbRequestMessage>();
        var response = await _pcbDataService.GetByIdEager(SelectedPcb.Id);
        if (response != null && response.Code == ResponseCode.Success)
        {
            SelectedPcb = response.Data;
        }
        // check if max transfer is exceeded
        if (SelectedPcb.Transfers.Count > 0)
        {
            int maxTransfer = SelectedPcb.PcbType.MaxTransfer;
            int transferCount = SelectedPcb.Transfers.Count;
            HasMaxTransfer = transferCount >= maxTransfer ? true : false;
            MaxTransferError = $"Weitergaben Anzahl: {transferCount} von max. {maxTransfer}";
        }
        //TODO: Error handling
        var resStorageLocations = await _storageLocationCrudService.GetAll();
        if (resStorageLocations.Code == ResponseCode.Success)
        {
            resStorageLocations.Data.ForEach(x => StorageLocations.Add(x));
        }

        var resDiagnoses = await _diagnoseCrudService.GetAll();
        if (resDiagnoses.Code == ResponseCode.Success)
        {
            resDiagnoses.Data.ForEach(x => Diagnoses.Add(x));
        }
    }


    public async void Save()
    {

        Transfer transfer = new Transfer
        {
            PcbId = SelectedPcb.Id,
            Comment = CommentPcb,
            NotedById = NotedBy.Id,
            CreatedDate = TransferDate,
            StorageLocationId = SelectedStorageLocation.Id
        };

        Response<Transfer> response;

        if (SelectedDiagnose != null)
        {
            response = await _transferDataService.CreateTransfer(transfer, SelectedDiagnose.Id);
        }
        else
        {
            response = await _transferDataService.Create(transfer);
        }

        if (response.Code == ResponseCode.Success)
        {

            _infoBarService.showMessage("Weitergabe erfolgreich", "Erfolg");
        }
        else
        {
            _infoBarService.showError("Fehler bei der Weitergabe", "Error");
        }
    }
    private bool CanExecute()
    {
        return false;
    }

}

