using NavisTools.Interfaces;
using System.Collections.ObjectModel;

namespace NavisTools.ViewModels
{
    /// <summary>
    /// ViewModel for the Selection Info panel.
    /// Separates presentation logic from the View.
    /// </summary>
    public class SelectionInfoViewModel : ViewModelBase
    {
        private readonly ISelectionService _selectionService;
        private readonly IMeasurementService _measurementService;

        private string _summary;
        private int _count;
        private double _totalVolume;
        private double _totalArea;
        private double _totalLength;

        public SelectionInfoViewModel(ISelectionService selectionService, IMeasurementService measurementService)
        {
            _selectionService = selectionService;
            _measurementService = measurementService;
            Items = new ObservableCollection<SelectionItemViewModel>();

            // Subscribe to selection changes
            if (_selectionService != null)
            {
                _selectionService.SelectionChanged += OnSelectionChanged;
            }

            // Initial update
            UpdateFromSelection();
        }

        #region Properties

        public string Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }

        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        public double TotalVolume
        {
            get => _totalVolume;
            set => SetProperty(ref _totalVolume, value);
        }

        public double TotalArea
        {
            get => _totalArea;
            set => SetProperty(ref _totalArea, value);
        }

        public double TotalLength
        {
            get => _totalLength;
            set => SetProperty(ref _totalLength, value);
        }

        public string VolumeDisplay => $"{TotalVolume:N3} m\u00B3";
        public string AreaDisplay => $"{TotalArea:N2} m\u00B2";
        public string LengthDisplay => $"{TotalLength:N2} m";

        public ObservableCollection<SelectionItemViewModel> Items { get; }

        #endregion

        #region Methods

        private void OnSelectionChanged(object sender, System.EventArgs e)
        {
            UpdateFromSelection();
        }

        public void UpdateFromSelection()
        {
            Items.Clear();

            if (_selectionService == null || !_selectionService.HasSelection)
            {
                Summary = "No items selected";
                Count = 0;
                TotalVolume = 0;
                TotalArea = 0;
                TotalLength = 0;
                NotifyDisplayPropertiesChanged();
                return;
            }

            var selectedItems = _selectionService.SelectedItems;
            Count = _selectionService.SelectionCount;
            Summary = Count == 1 ? "1 item selected" : $"{Count} item(s) selected";

            double totalVolume = 0;
            double totalArea = 0;
            double totalLength = 0;

            foreach (var item in selectedItems)
            {
                var measurement = _measurementService?.GetItemMeasurement(item);
                if (measurement != null)
                {
                    totalVolume += measurement.Volume;
                    totalArea += measurement.Area;
                    totalLength += measurement.Length;

                    Items.Add(new SelectionItemViewModel
                    {
                        DisplayName = measurement.DisplayName,
                        Volume = measurement.Volume,
                        Area = measurement.Area,
                        Length = measurement.Length
                    });
                }
            }

            TotalVolume = totalVolume;
            TotalArea = totalArea;
            TotalLength = totalLength;
            NotifyDisplayPropertiesChanged();
        }

        private void NotifyDisplayPropertiesChanged()
        {
            OnPropertyChanged(nameof(VolumeDisplay));
            OnPropertyChanged(nameof(AreaDisplay));
            OnPropertyChanged(nameof(LengthDisplay));
        }

        public void Cleanup()
        {
            if (_selectionService != null)
            {
                _selectionService.SelectionChanged -= OnSelectionChanged;
            }
        }

        #endregion
    }

    /// <summary>
    /// ViewModel for a single selected item in the list.
    /// </summary>
    public class SelectionItemViewModel : ViewModelBase
    {
        private string _displayName;
        private double _volume;
        private double _area;
        private double _length;

        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        public double Volume
        {
            get => _volume;
            set => SetProperty(ref _volume, value);
        }

        public double Area
        {
            get => _area;
            set => SetProperty(ref _area, value);
        }

        public double Length
        {
            get => _length;
            set => SetProperty(ref _length, value);
        }

        public string VolumeDisplay => Volume > 0 ? Volume.ToString("N3") : "-";
        public string AreaDisplay => Area > 0 ? Area.ToString("N2") : "-";
        public string LengthDisplay => Length > 0 ? Length.ToString("N2") : "-";
    }
}
