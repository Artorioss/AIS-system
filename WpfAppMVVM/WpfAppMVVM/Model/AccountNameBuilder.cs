using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.Model
{
    internal class AccountNameBuilder
    {
        private const string _prefix = "Организация перевозки груза по ";
        private StringBuilder _stringBuilder = new StringBuilder(_prefix, 128);

        private string _routeName;
        public string RouteName
        {
            get => _routeName;
            set
            {
                _routeName = value;
                UpdateString();
            }
        }

        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                UpdateString();
            }
        }

        private Car _car;
        public Car Car
        {
            get => _car;
            set
            {
                _car = value;
                UpdateString();
            }
        }

        private Brand _carBrand;
        public Brand CarBrand
        {
            get => _carBrand;
            set
            {
                _carBrand = value;
                UpdateString();
            }
        }

        private Trailler _trailler;
        public Trailler Trailler
        {
            get => _trailler;
            set
            {
                _trailler = value;
                UpdateString();
            }
        }

        private Brand _traillerBrand;
        public Brand TraillerBrand
        {
            get => _traillerBrand;
            set
            {
                _traillerBrand = value;
                UpdateString();
            }
        }

        private Driver _driver;
        public Driver Driver
        {
            get => _driver;
            set
            {
                _driver = value;
                UpdateString();
            }
        }

        public AccountNameBuilder()
        {

        }

        public AccountNameBuilder(string routeName, DateTime date, Car car, Trailler trailler, Driver driver)
        {
            _routeName = routeName;
            _date = date;
            _car = car;
            _trailler = trailler;
            _driver = driver;

            UpdateString();
        }

        private void UpdateString()
        {
            _stringBuilder.Clear();
            _stringBuilder.Append(_prefix);
            if(_routeName != null) _stringBuilder.Append(_routeName).Append(' ');
            if(_date != null) _stringBuilder.Append(_date.ToString("d")).Append(' ');
            if (_car != null || _carBrand != null) 
            {
                _stringBuilder.Append("а/м ");
                if (_carBrand != null) _stringBuilder.Append(_carBrand.Name).Append(' ');
                if (_car != null) _stringBuilder.Append("№").Append(_car.Number).Append(' ');
            }
            
            if (_trailler != null || _traillerBrand != null) 
            {
                _stringBuilder.Append("п/п ");
                if(_traillerBrand != null) _stringBuilder.Append(_traillerBrand.Name).Append(' ');
                if(_trailler != null) _stringBuilder.Append("№").Append(_trailler.Number).Append(' ');
            }
            if (Driver != null) _stringBuilder.Append(Driver.Name); 
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}
