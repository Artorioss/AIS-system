using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Models.Entities;

namespace WpfAppMVVM.Model.Entities
{
    public class Brand
    {
        private int _brandId;
        public int BrandId
        {
            get { return _brandId; }
            set 
            {
                _brandId = value; 
                //OnPropertyChanged(nameof(BrandId)); 
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value;
                //OnPropertyChanged(nameof(Name));
            }
        }

        private string _russianBrandName;
        public string RussianBrandName
        {
            get { return _russianBrandName; }
            set
            { 
                _russianBrandName = value;
                //OnPropertyChanged(nameof(RussianBrandName));
            }
        }

        public ICollection<Car> Cars { get; set; }
        public ICollection<Trailler> Traillers { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
