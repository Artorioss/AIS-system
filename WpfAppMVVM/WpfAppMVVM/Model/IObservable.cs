namespace WpfAppMVVM.Model
{
    internal interface IObservable
    {
        public void RegisterObserver(IObserver observer);
        public void UnregisterObserver(IObserver observer);
        public void NotifyObservers();
    }
}
