﻿using App.Contracts.Services;
using App.Controls;
using App.Core.Models;
using App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;


namespace App.Services;

/// <summary>
/// A <see langword="class"/> that implements the <see cref="IDialogService"/> <see langword="interface"/> using WinUI APIs.
/// </summary>
public sealed class DialogService : IDialogService
{
    /// <inheritdoc/>
    private readonly FrameworkElement? rootElement = App.MainWindow.Content as FrameworkElement;

    public async Task<bool?> ConfirmDeleteDialogAsync(string title, string content, string confirmButtonText, string cancelButtonText)
    {
        if (rootElement != null)

        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = confirmButtonText,
                DefaultButton = ContentDialogButton.Close,
                RequestedTheme = rootElement.RequestedTheme,
                CloseButtonText = cancelButtonText,
                XamlRoot = rootElement.XamlRoot

            };
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.None)
            {
                return null;
            }

            return (result == ContentDialogResult.Primary);
        }
        return null;
    }
    public async Task<Tuple<Transfer, int?>?> ShowCreateTransferDialog(string title, string confirmButtonText, string cancelButtonText)
    {
        try
        {
            if (rootElement != null)
            {
                var dialog = new ContentDialog
                {
                    Title = title,
                    Content = new TransferDialog(),
                    PrimaryButtonText = confirmButtonText,
                    DefaultButton = ContentDialogButton.Primary,
                    RequestedTheme = rootElement.RequestedTheme,
                    CloseButtonText = cancelButtonText,
                    XamlRoot = rootElement.XamlRoot
                };
                var result = await dialog.ShowAsync();
                var view = (TransferDialog)dialog.Content;

                if (result == ContentDialogResult.None)
                {
                    return null;
                }

                if (result == ContentDialogResult.Primary)
                {

                    TransferDialogViewModel tdVM = (TransferDialogViewModel)view.ViewModel;

                    int? diagnoseId = ((Diagnose)tdVM.SelectedDiagnose) != null
                        ? ((Diagnose)tdVM.SelectedDiagnose).Id
                        : null;
                    return Tuple.Create(
                        new Transfer
                        {
                            NotedById = tdVM.NotedBy.Id,
                            CreatedDate = tdVM.TransferDate,
                            StorageLocationId = ((StorageLocation)tdVM.SelectedStorageLocation).Id,
                            Comment = tdVM.Comment
                        }, diagnoseId);

                }
            }
        }catch(Exception ex){

        Debug.WriteLine(ex);
        }

        return null;
    }

    public async Task<Comment> AddCommentDialog(string title, string confirmButtonText, string cancelButtonText)
    {
        if (rootElement != null)
        {
            TextBox comment = new TextBox
            {
                Header = "Anmerkung",
                PlaceholderText = "Text eintragen"
            };

            var dialog = new ContentDialog
            {
                Title = title,
                Content = comment,
                PrimaryButtonText = confirmButtonText,
                DefaultButton = ContentDialogButton.Primary,
                RequestedTheme = rootElement.RequestedTheme,
                CloseButtonText = cancelButtonText,
                XamlRoot = rootElement.XamlRoot

            };
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.None)
            {
                return null;
            }
            if (result == ContentDialogResult.Primary)
            {

                return new Comment
                {
                    Content = comment.Text
                };
            }

        }
        return null;
    }

    public async Task<Device> AddRestrictionDialog(string title, string confirmButtonText, string cancelButtonText)
    {
        if (rootElement != null)
        {
            TextBox restriction = new TextBox
            {
                Header = "Einschränkung",
                PlaceholderText = "Text eintragen"
            };

            var dialog = new ContentDialog
            {
                Title = title,
                Content = restriction,
                PrimaryButtonText = confirmButtonText,
                DefaultButton = ContentDialogButton.Primary,
                RequestedTheme = rootElement.RequestedTheme,
                CloseButtonText = cancelButtonText,
                XamlRoot = rootElement.XamlRoot

            };
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.None)
            {
                return null;
            }
            if (result == ContentDialogResult.Primary)
            {

                return new Device
                {
                    Name = restriction.Text
                };
            }

        }
        return null;
    }



    public async void UnAuthorizedDialogAsync(string title, string content, XamlRoot xamlRoot)
    {

        if (xamlRoot != null)
        {
            ContentDialog unAuthorizedDialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                XamlRoot = xamlRoot
            };

            await unAuthorizedDialog.ShowAsync();
        }

    }

}