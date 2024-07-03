namespace WpfAppMVVM.Model
{
    public class MonthService
    {
        private Dictionary<int, string> _months;

        public MonthService() 
        {
            _months = new Dictionary<int, string>() 
            {
                {1, "Январь" },
                {2, "Февраль" },
                {3, "Март" },
                {4, "Апрель" },
                {5, "Май" },
                {6, "Июнь" },
                {7, "Июль" },
                {8, "Август" },
                {9, "Сентябрь" },
                {10, "Октябрь" },
                {11, "Ноябрь" },
                {12, "Декабрь" },
            };
        }

        public List<string> GetMonths(List<int> months) 
        {
            List<string> list = new List<string>();
            foreach (int i in months) list.Add(_months[i]);
            return list;
        }

        public string GetMonth(int month)
        {
            if (month == 0 || month > 12) return string.Empty;
            return _months[month];
        } 
        public int GetMonth(string month) 
        {
            for (int i = 1; i < 12; i++) if (_months[i] == month) return i;
            return -1;  
        }
    }
}
