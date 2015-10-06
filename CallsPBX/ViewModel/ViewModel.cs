using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CallsPBX.Models;
using System.Data;
using System.Windows.Input;
using System.Windows;

namespace CallsPBX.ViewModel
{
    class ViewModel : INotifyPropertyChanged, IDisposable
    {
        private Nullable<DateTime> _beginPeriod;
        private Nullable<DateTime> _endPeriod;
        private DataService _dataService;
        private DataTable _callsTable;
        private bool _isConnect;

        public Nullable<DateTime> BeginPeriod
        {
            get
            {
                if (_beginPeriod == null)
                {
                    _beginPeriod = DateTime.Now.AddMinutes(-5);
                }
                return _beginPeriod;
            }
            set
            {
                _beginPeriod = value;
            }
        }
        public Nullable<DateTime> EndPeriod
        {
            get
            {
                if (_endPeriod == null)
                {
                    _endPeriod = DateTime.Now;
                }
                return _endPeriod;
            }
            set
            {
                _endPeriod = value;
            }
        }
        public string InNumber { get; set; }
        public string OutNumber { get; set; }
        public DataTable Calls
        {
            get
            {
                return _callsTable;
            }
            set
            {
                if (_callsTable != value)
                {
                    _callsTable = value;
                    OnPropertyChanged("Calls");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ICommand KeyEnterCommand { get; set; }
        public ICommand ClickSettingsCommand { get; set; }
        public ViewModel()
        {
            _dataService = new DataService();
            //OpenConnection();
            KeyEnterCommand = new RelayCommand(arg => KeyEnterMethod());
            ClickSettingsCommand = new RelayCommand(arg => ClickSettingsMethod());
        }

        private void ClickSettingsMethod()
        {
            if (_isConnect == false)
            {
                OpenConnection();
            }
        }

        private void KeyEnterMethod()
        {
            if (_isConnect == false)
            {
                OpenConnection();
            }

            if (_isConnect == true)
            {
                Calls = _dataService.GetDataCalls(BeginPeriod, EndPeriod, InNumber, OutNumber);
            }
        }

        private void OpenConnection()
        {
            ConnectData connectData = new ConnectData();
            connectData.Host = Properties.Settings.Default.Host;
            connectData.Database = Properties.Settings.Default.Database;
            connectData.Username = Properties.Settings.Default.Username;
            connectData.Password = Properties.Settings.Default.Password;
            _isConnect = false;

            try
            {
                _isConnect = _dataService.OpenConnection(connectData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error connecting!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Dispose()
        {
            _dataService.Dispose();
        }
    }
}
