using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal class RoutePointTable : EntityTable
    {
        protected override void createEntityTable()
        {
            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
        }

        protected override string createMessageBoxText()
        {
            RoutePoint routePoint = SelectedItem as RoutePoint;
            string messageBoxText = string.Empty;
            if (routePoint.Routes.Count > 0)
            {
                messageBoxText += "Нельзя удалить данный пункт маршрута, так как на него есть ссылки из других маршрутов. На данный пункт маршрута ссылаются следующие маршруты:\r\n";
                int i = 0;
                foreach (Route route in routePoint.Routes)
                {
                    if (i == 5) break;
                    messageBoxText += $"{route.RouteName};\r\n";
                    i++;
                }
                int remainder = routePoint.Routes.Count - 5;
                if (remainder > 0) messageBoxText += $"\r\nИ еще {remainder} других маршрутов.";
                messageBoxText += "\r\nЧтобы удалить данный пункт маршрута, разрешите зависимости.";
            }
            else messageBoxText = $"Пункт маршрута - '{(SelectedItem as RoutePoint).Name}' будет удален. Продолжить?";
            return messageBoxText;
        }

        protected override string createMessageBoxCaption()
        {
            RoutePoint routePoint = SelectedItem as RoutePoint;
            string caption;
            if (routePoint.Routes.Count > 0) caption = "Невозможно выполнить действие";
            else caption = "Вы уверены?";
            return caption;
        }

        protected override MessageBoxButton createMessageBoxButton()
        {
            RoutePoint routePoint = SelectedItem as RoutePoint;
            MessageBoxButton button;
            if (routePoint.Routes.Count > 0) button = MessageBoxButton.OK;
            else button = MessageBoxButton.YesNo;
            return button;
        }

        protected override async Task loadingDependentEntities()
        {
            RoutePoint routePoint = SelectedItem as RoutePoint;
            var collection = _context.Entry(routePoint).Collection(rp => rp.Routes);
            if (!collection.IsLoaded) await collection.LoadAsync();
        }
    }
}
