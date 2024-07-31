using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal class TraillerTable : EntityTable
    {
        public TraillerTable(TransportationEntities Context) : base(Context)
        {
        }

        protected override void createEntityTable()
        {
            DataGridTextColumn columnBrandName = new DataGridTextColumn();
            columnBrandName.Header = "Бренд";
            columnBrandName.Binding = new Binding("Brand.Name");
            columnBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnNumber = new DataGridTextColumn();
            columnNumber.Header = "Номер";
            columnNumber.Binding = new Binding("Number");
            columnNumber.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnBrandName);
            ColumnCollection.Add(columnNumber);
        }

        protected override string createMessageBoxText()
        {
            Trailler trailler = SelectedItem as Trailler;
            string messageBoxText = string.Empty;
            if (trailler.Transportations.Count > 0)
            {
                messageBoxText += "Нельзя удалить данный прицеп, так как на него ссылаются из других таблиц. За данным прицепом закреплены следующие заявки:\r\n";
                int i = 0;
                foreach (Transportation transportation in trailler.Transportations)
                {
                    if (i == 3) break;
                    messageBoxText += $"{transportation.RouteName};\r\n";
                    i++;
                }
                int remainder = trailler.Transportations.Count - 3;
                if(remainder > 0) messageBoxText += $"\r\nИ еще {remainder} других заявок.";
                messageBoxText += "\r\nЧтобы удалить данный прицеп, разрешите зависимости.";
            }
            else messageBoxText = $"Прицеп - '{(SelectedItem as Trailler).Number}' будет удален. Продолжить?";
            return messageBoxText;
        }

        protected override string createMessageBoxCaption()
        {
            Trailler trailler = SelectedItem as Trailler;
            string caption;
            if (trailler.Transportations.Count > 0) caption = "Невозможно выполнить действие";
            else caption = "Вы уверены?";
            return caption;
        }

        protected override MessageBoxButton createMessageBoxButton()
        {
            Trailler trailler = SelectedItem as Trailler;
            MessageBoxButton button;
            if (trailler.Transportations.Count > 0) button = MessageBoxButton.OK;
            else button = MessageBoxButton.YesNo;
            return button;
        }

        protected override async Task loadingDependentEntities()
        {
            Trailler trailler = SelectedItem as Trailler;
            var collection = _context.Entry(trailler).Collection(t => t.Transportations);
            if (!collection.IsLoaded) await collection.LoadAsync();
        }
    }
}
