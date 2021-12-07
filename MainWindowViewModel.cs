using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PanelHidingIssue
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel(MainWindow wnd)
        {
            _mainWindow = wnd;
            _columnSplitterWidth = _mainWindow.ColumnSplitter.Width.Value;
            _columnPanelWidth = _mainWindow.ColumnPanel.Width.Value;
        }

        private MainWindow _mainWindow;

        private double _columnPanelWidth;
        private double _columnSplitterWidth;
        private bool _isPanelVisible = true;

        public bool IsPanelVisible
        {
            get => _isPanelVisible;

            set
            {
                _isPanelVisible = value;
                OnPropertyChanged();
                TogglePanelVisibility(value);
            }
        }
        private void TogglePanelVisibility(bool isVisible)
        {
            if (isVisible)
            {
                // Restore saved parameters:
                _mainWindow.ColumnPanel.Width = new GridLength(_columnPanelWidth, GridUnitType.Star);
                _mainWindow.ColumnPanel.MaxWidth = double.PositiveInfinity;

                _mainWindow.ColumnSplitter.Width = new GridLength(_columnSplitterWidth, GridUnitType.Auto);
                _mainWindow.ColumnSplitter.MaxWidth = double.PositiveInfinity;
                return;
            }

            // Save parameters:
            _columnSplitterWidth = _mainWindow.ColumnSplitter.Width.Value;
            _columnPanelWidth = _mainWindow.ColumnPanel.Width.Value;

            // Hide panel:
            _mainWindow.ColumnPanel.Width = new GridLength(0);
            _mainWindow.ColumnPanel.MaxWidth = 0;
            _mainWindow.ColumnSplitter.Width = new GridLength(0);
            _mainWindow.ColumnSplitter.MaxWidth = 0;
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}