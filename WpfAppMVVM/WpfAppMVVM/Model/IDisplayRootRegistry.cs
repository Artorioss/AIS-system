using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppMVVM.Model
{
    public interface IDisplayRootRegistry
    {
        public void RegisterWindowType<VM, Win>() where Win : Window, new() where VM : class;
        public bool CheckExistWindowType(Type vmType);
        public void UnregisterWindowType<VM>();
        public Window CreateWindowInstanceWithVM(object vm);
        public void ShowPresentation(object vm);
        public void ClosePresentation(object vm);
        public Task ShowModalPresentation(object vm);
    }
}
