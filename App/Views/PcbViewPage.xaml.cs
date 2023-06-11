﻿// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using App.Core.Models;
using App.Core.Models.Enums;
using App.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ctWinUI = CommunityToolkit.WinUI.UI.Controls;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PcbViewPage : Page
    {
        private enum DataGridDisplayMode
        {
            Default,
            UserSorted,
            Filtered,
            Grouped,
            Search
        }

        public PcbPaginationViewModel ViewModel { get; }

        public PcbViewPage()
        {
            ViewModel = App.GetService<PcbPaginationViewModel>();
            InitializeComponent();
            Loaded += Page_Loaded;
            Unloaded += Page_Unload;
            ViewModel.FilterOptions = PcbFilterOptions.None;
            ViewModel.SortBy = DataGrid.Columns[5].Tag.ToString();
            DataGrid.SelectionChanged += DataGrid_SelectionChanged;
        }

        private DataGridDisplayMode _displayMode = DataGridDisplayMode.Default;
        private long _token;
        private DataGridColumn _actualSortedColumn;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _token = DataGrid.RegisterPropertyChangedCallback(ctWinUI.DataGrid.ItemsSourceProperty, DataGridItemsSourceChangedCallback);
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            DataGrid.UnregisterPropertyChangedCallback(ctWinUI.DataGrid.ItemsSourceProperty, _token);
            base.OnNavigatingFrom(e);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _displayMode = DataGridDisplayMode.Default;
            DataGrid.ItemsSource = ViewModel.Pcbs; //nötig? weil schon in Xaml gebunden
            DataGrid.Columns[5].SortDirection = ctWinUI.DataGridSortDirection.Descending;
            DataGrid.SelectionChanged += DataGrid_SelectionChanged;
            ViewModel.FilterOptions = PcbFilterOptions.None;
        }

        private void Page_Unload(object sender, RoutedEventArgs e)
        {
            DataGrid.SelectionChanged -= DataGrid_SelectionChanged;
            ViewModel.FilterOptions = PcbFilterOptions.None;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid.RowDetailsVisibilityMode = ctWinUI.DataGridRowDetailsVisibilityMode.Collapsed;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems.First() is not null)
                {
                    _displayMode = DataGridDisplayMode.Default;
                    ViewModel.FilterOptions = PcbFilterOptions.FilterStorageLocation;
                    ViewModel.SelectedComboBox = ComboBoxStorageLocation.SelectedItem as StorageLocation;
                    ViewModel.FilterItems.ExecuteAsync(null);
                }
            }
        }

        private async void DataGrid_Sorting(object sender, ctWinUI.DataGridColumnEventArgs e)
        {
            _displayMode = DataGridDisplayMode.UserSorted;

            _ = ViewModel.IsSortingAscending
                ? ViewModel.IsSortingAscending = false
                : ViewModel.IsSortingAscending = true;
            _ = ViewModel.IsSortingAscending
                ? e.Column.SortDirection = ctWinUI.DataGridSortDirection.Ascending
                : e.Column.SortDirection = ctWinUI.DataGridSortDirection.Descending;
            _actualSortedColumn = e.Column;

            if (e.Column.Tag is not null)
            {
                ViewModel.SortBy = e.Column.Tag.ToString();
            }
            bool isAscending = e.Column.SortDirection is null or (ctWinUI.DataGridSortDirection?)ctWinUI.DataGridSortDirection.Descending;
            await ViewModel.SortByCommand.ExecuteAsync(null); //hier nochmal schauen
        }

        private async void Filter1_Click(object Sender, RoutedEventArgs e)
        {
            _displayMode = DataGridDisplayMode.Filtered;
            ViewModel.FilterOptions = PcbFilterOptions.Filter1;
            ComboBoxStorageLocation.SelectedItem = null;
            await ViewModel.FilterItems.ExecuteAsync(null);
        }

        private async void Filter2_Click(object Sender, RoutedEventArgs e)
        {
            ViewModel.PageNumber = 1;
            _displayMode = DataGridDisplayMode.Filtered;
            ViewModel.FilterOptions = PcbFilterOptions.Filter2;
            ComboBoxStorageLocation.SelectedItem = null;
            await ViewModel.FilterItems.ExecuteAsync(null);
        }

        private async void Filter3_Click(object Sender, RoutedEventArgs e)
        {
            _displayMode = DataGridDisplayMode.Filtered;
            ViewModel.FilterOptions = PcbFilterOptions.Filter3;
            ComboBoxStorageLocation.SelectedItem = null;
            await ViewModel.FilterItems.ExecuteAsync(null);
        }

        private async void FilterClear_Click(object sender, RoutedEventArgs e)
        {
            _displayMode = DataGridDisplayMode.Default;
            ViewModel.FilterOptions = PcbFilterOptions.None;
            ComboBoxStorageLocation.SelectedItem = null;
            await ViewModel.FirstAsyncCommand.ExecuteAsync(null);
        }

        private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            _displayMode = DataGridDisplayMode.Search;
            ViewModel.FilterOptions = PcbFilterOptions.Search;
            ViewModel.QueryText = e.QueryText;
            ComboBoxStorageLocation.SelectedItem = null;
            await ViewModel.FilterItems.ExecuteAsync(null);
        }

        private async void SearchBox_QueryClick(object sender, RoutedEventArgs e)
        {
            _displayMode = DataGridDisplayMode.Search;
            ViewModel.FilterOptions = PcbFilterOptions.Search;
            ViewModel.QueryText = SearchBox.Text;
            ComboBoxStorageLocation.SelectedItem = null;
            await ViewModel.FilterItems.ExecuteAsync(null);
        }

        private void DeleteClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.DeleteCommand.Execute(null);
        }

        void EditClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.NavigateToUpdateCommand.Execute(ViewModel.SelectedItem.PcbId);
        }

        private void NavigateToDetails(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.NavigateToDetailsCommand.Execute(ViewModel.SelectedItem.PcbId);
        }

        private void CreatePcbButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreatePcbPage));
        }

        private void DataGridItemsSourceChangedCallback(DependencyObject sender, DependencyProperty dp)
        {
            if (_actualSortedColumn != null)
            {
                _ = ViewModel.IsSortingAscending
                    ? DataGrid.Columns[_actualSortedColumn.DisplayIndex].SortDirection = ctWinUI.DataGridSortDirection.Ascending
                    : DataGrid.Columns[_actualSortedColumn.DisplayIndex].SortDirection = ctWinUI.DataGridSortDirection.Descending;
            }

            // Remove Display Mode Indicators;
            FilterIndicator.Visibility = Visibility.Collapsed;
            SearchIndicator.Visibility = Visibility.Collapsed;

            // Remove Sort Indicators.
            if (dp == ctWinUI.DataGrid.ItemsSourceProperty)
            {
                foreach (var column in (sender as ctWinUI.DataGrid).Columns)
                {
                    column.SortDirection = null;
                }
            }

            if (_displayMode == DataGridDisplayMode.Filtered)
            {
                FilterIndicator.Visibility = Visibility.Visible;
            }

            if (_displayMode == DataGridDisplayMode.Search)
            {
                SearchIndicator.Visibility = Visibility.Visible;
            }

        }

        void TransferClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowTransferCommand.Execute(null);
        }

        /*private void UIElement_OnLostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("ok");
        }*/


    }
}
