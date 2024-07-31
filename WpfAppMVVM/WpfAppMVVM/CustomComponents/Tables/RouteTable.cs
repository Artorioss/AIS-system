using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal class RouteTable : EntityTable
    {
        public RouteTable(TransportationEntities Context) : base(Context)
        {
        }

        protected override void createEntityTable()
        {
            DataGridTextColumn columnRouteName = new DataGridTextColumn();
            columnRouteName.Header = "Маршрут";
            columnRouteName.Binding = new Binding("RouteName");
            columnRouteName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnCount = new DataGridTextColumn();
            columnCount.Header = "Кол-во перевозок";
            columnCount.Binding = new Binding("Transportations.Count");
            columnCount.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);

            ColumnCollection.Add(columnRouteName);
            ColumnCollection.Add(columnCount);
        }

        protected override string createMessageBoxText()
        {
            Route route = SelectedItem as Route;
            string messageBoxText = string.Empty;
            if (route.Transportations.Count > 0)
            {
                messageBoxText += "Нельзя удалить данный маршрут, так как на него ссылаются из других таблиц. За данным маршрутом закреплены следующие заявки:\r\n";
                int i = 0;
                foreach (Transportation transportation in route.Transportations)
                {
                    if (i == 3) break;
                    messageBoxText += $"{transportation.RouteName};\r\n";
                    i++;
                }
                int remainder = route.Transportations.Count - 3;
                if(remainder > 0) messageBoxText += $"\r\nИ еще {remainder} других заявок.";
                messageBoxText += "\r\nЧтобы удалить данный маршрут, разрешите зависимости.";
            }
            else messageBoxText = $"Маршрут - '{(SelectedItem as Route).RouteName}' будет удален. Продолжить?";
            return messageBoxText;
        }

        protected override string createMessageBoxCaption()
        {
            Route route = SelectedItem as Route;
            string caption;
            if (route.Transportations.Count > 0) caption = "Невозможно выполнить действие";
            else caption = "Вы уверены?";
            return caption;
        }

        protected override MessageBoxButton createMessageBoxButton()
        {
            Route route = SelectedItem as Route;
            MessageBoxButton button;
            if (route.Transportations.Count > 0) button = MessageBoxButton.OK;
            else button = MessageBoxButton.YesNo;
            return button;
        }

        protected override async Task loadingDependentEntities()
        {
            Route route = SelectedItem as Route;
            var collection = _context.Entry(route).Collection(r => r.Transportations);
            if (!collection.IsLoaded) await collection.LoadAsync();
        }
    }
}
