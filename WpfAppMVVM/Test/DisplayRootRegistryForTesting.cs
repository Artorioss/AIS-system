using System.Windows;
using WpfAppMVVM.Model;

namespace Test
{
    internal class DisplayRootRegistryForTesting : IDisplayRootRegistry
    {
        Dictionary<Type, Type> vmToWindowMapping = new Dictionary<Type, Type>();
        private Type _cratedWindowType;
        public bool CheckExistWindowType(Type vmType)
        {
            return vmToWindowMapping.ContainsKey(vmType);
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
            _cratedWindowType = windowType;
            return null;
        }

        public void ClosePresentation(object vm)
        {
            Type window;
            if (!openWindows.TryGetValue(vm, out window))
                throw new InvalidOperationException("UI for this VM is not displayed");
            openWindows.Remove(vm);
        }

        public void RegisterWindowType<VM, Win>()
            where VM : class
            where Win : Window, new()
        {
            var vmType = typeof(VM);
            if (vmType.IsInterface)
                throw new ArgumentException("Cannot register interfaces");
            if (vmToWindowMapping.ContainsKey(vmType))
                return;
            vmToWindowMapping[vmType] = typeof(Win);
        }

        public async Task ShowModalPresentation(object vm)
        {
            CreateWindowInstanceWithVM(vm);
            openWindows[vm] = _cratedWindowType;
        }

        Dictionary<object, Type> openWindows = new Dictionary<object, Type>();
        public void ShowPresentation(object vm)
        {
            if (vm == null)
                throw new ArgumentNullException("vm is null");
            if (openWindows.ContainsKey(vm))
                return;
            CreateWindowInstanceWithVM(vm);
            openWindows[vm] = _cratedWindowType;
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
    }
}
