using Avalonia.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace lab6
{
    public class WeatherDataService
    {
        private readonly WeatherService _weatherService;

        public WeatherDataService()
        {
            _weatherService = new WeatherService();
        }

        public async Task<string> GetWeatherDataAsync(bool isSleep)
        {
            return await Task.Run(() => _weatherService.GetDataSync(isSleep));
        }
    }
    public class WeatherData
    {
        public string Name { get; set; }
        public Main main { get; set; }
        public Wind wind { get; set; }
        public struct Main
        {
            public double Temp { get; set; }
            public double TempInCelsius
            {
                get { return (Temp - 273.17); }
            }
            public int Humidity { get; set; }
            public double pressure { get; set; }
            public double sea_level {  get; set; }
        }
        public struct Wind
        {
            public double Speed { get; set; }
        }
    }


    public class Weather : INotifyPropertyChanged
    {
        private readonly WeatherDataService _weatherDataService;

        private string _sity = "...";
        public string Sity
        {
            get { return _sity; }
            set
            {
                _sity = value;
                OnPropertyChanged();
            }
        }

        private string _temperature = "...";
        public string Temperature
        {
            get { return _temperature; }
            set
            {
                _temperature = value;
                OnPropertyChanged();
            }
        }

        private string _humidity = "...";
        public string Humidity
        {
            get { return _humidity; }
            set
            {
                _humidity = value;
                OnPropertyChanged();
            }
        }

        private string _wind = "...";
        public string Wind
        {
            get { return _wind; }
            set
            {
                _wind = value;
                OnPropertyChanged();
            }
        }
        private string _pressure = "...";
        public string Pressure
        {
            get { return _pressure; }
            set
            {
                _pressure = value;
                OnPropertyChanged();
            }
        }
        private string _seaLevel = "...";
        public string SeaLevel
        {
            get { return _seaLevel; }
            set
            {
                _seaLevel = value;
                OnPropertyChanged();
            }
        }
        public string Data { get; set; }

        public Weather()
        {
            _weatherDataService = new WeatherDataService();
            MainAsync().ConfigureAwait(false);
            setData(DateTime.Now);

        }

        private async Task MainAsync()
        {
            string weatherData = await _weatherDataService.GetWeatherDataAsync(true);
            // Обработка полученных данных
            WeatherData weatherDataObject = JsonConvert.DeserializeObject<WeatherData>(weatherData);
            Sity = weatherDataObject.Name;
            double temp = Math.Round(weatherDataObject.main.TempInCelsius);
            Temperature = temp.ToString() + "°";
            Humidity = weatherDataObject.main.Humidity.ToString() + "%";
            Wind = weatherDataObject.wind.Speed.ToString() + " m/s";
            Pressure = weatherDataObject.main.pressure.ToString() + " mm";
            SeaLevel = weatherDataObject.main.sea_level.ToString() + " m";
        }

        private void setData(DateTime date)
        {
            string temp = date.ToString();
            string[] tokens = temp.Split('.');
            int dayOfWeek = (int)date.DayOfWeek;
            string dayOfWeekString = "";
            switch (dayOfWeek)
            {
                case 1:
                    dayOfWeekString = "Понедельник";
                    break;
                case 2:
                    dayOfWeekString = "Вторник";
                    break;
                case 3:
                    dayOfWeekString = "Среда";
                    break;
                case 4:
                    dayOfWeekString = "Четверг";
                    break;
                case 5:
                    dayOfWeekString = "Пятница";
                    break;
                case 6:
                    dayOfWeekString = "Суббота";
                    break;
                case 7:
                    dayOfWeekString = "Воскресенье";
                    break;
                default:
                    break;
            }
            Data = dayOfWeekString + ", " + tokens[0];
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
