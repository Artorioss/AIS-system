using System.Diagnostics.Eventing.Reader;
using System.Windows;
using WpfAppMVVM.Model;

namespace WpfApp
{
    public class DisplayRootRegistry: IDisplayRootRegistry
    {
        Dictionary<Type, Type> vmToWindowMapping = new Dictionary<Type, Type>();

        //Регистрация соответствия между ViewModel и View чтобы знать какое окно должно быть отображено для каждой модели
        public void RegisterWindowType<VM, Win>() where Win : Window, new() where VM : class
        {
            var vmType = typeof(VM);
            if (vmType.IsInterface)
                throw new ArgumentException("Cannot register interfaces");
            if (vmToWindowMapping.ContainsKey(vmType))
                throw new InvalidOperationException(
                    $"Type {vmType.FullName} is already registered");
            vmToWindowMapping[vmType] = typeof(Win);
        }

        public bool CheckExistWindowType(Type vmType)
        {
            return vmToWindowMapping.ContainsKey(vmType);
        }

        public void UnregisterWindowType<VM>()
        {
            var vmType = typeof(VM);
            if (vmType.IsInterface)
                throw new ArgumentException("Cannot register interfaces");
            if (!vmToWindowMapping.ContainsKey(vmType))
                throw new InvalidOperationException(
                    $"Type {vmType.FullName} is not registered");
            vmToWindowMapping.Remove(vmType);
        }

        public Window CreateWindowInstanceWithVM(object vm)
        {
            if (vm == null)
                throw new ArgumentNullException("vm is null");
            Type windowType = null;

            var vmType = vm.GetType();
            while (vmType != null && !vmToWindowMapping.TryGetValue(vmType, out windowType))
                vmType = vmType.BaseType;

            if (windowType == null)
                throw new ArgumentException(
                    $"No registered window type for argument type {vm.GetType().FullName}");

            var window = (Window)Activator.CreateInstance(windowType);
            window.DataContext = vm;
            return window;
        }


        Dictionary<object, Window> openWindows = new Dictionary<object, Window>();
        public void ShowPresentation(object vm)
        {
            if (vm == null)
                throw new ArgumentNullException("vm is null");
            if (openWindows.ContainsKey(vm))
                throw new InvalidOperationException("UI for this VM is already displayed");
            var window = CreateWindowInstanceWithVM(vm);
            window.Show();
            openWindows[vm] = window;
        }

        public void ClosePresentation(object vm)
        {
            Window window;
            if (!openWindows.TryGetValue(vm, out window))
                throw new InvalidOperationException("UI for this VM is not displayed");
            window.Close();
            openWindows.Remove(vm);
        }

        public async Task ShowModalPresentation(object vm)
        {
            var window = CreateWindowInstanceWithVM(vm);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            openWindows[vm] = window;
            await window.Dispatcher.InvokeAsync(() => window.ShowDialog());
        }
    }
}
