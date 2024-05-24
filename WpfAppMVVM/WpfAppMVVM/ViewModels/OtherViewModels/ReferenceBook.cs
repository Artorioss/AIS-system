using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using WpfAppMVVM.Models;
using WpfAppMVVM.Models.Entities;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal abstract class ReferenceBook: BaseViewModel
    {
        protected TransportationEntities _context;
        protected Mode _mode;

        public ReferenceBook() 
        {
            _context = (Application.Current as App)._context;
        }

        protected void CreateAction(object obj)
        {
            if (dataIsCorrect())
            {
                if (_mode == Mode.Editing)
                {
                    updateEntity();
                }
                _context.SaveChanges();
                (obj as Window)?.Close();
            }
            else
            {
                MessageBox.Show("Неправильно заполнены поля!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected abstract bool dataIsCorrect();
        protected abstract void updateEntity();
    }
}
