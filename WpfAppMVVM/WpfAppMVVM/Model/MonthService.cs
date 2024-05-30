using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Model
{
    internal class MonthService
    {
        Dictionary<int, string> _dict;

        public MonthService() 
        {
            _dict = new Dictionary<int, string>() 
            {
                { 1, "Январь"},
                { 2, "Февраль"},
                { 3, "Март"},
                { 4, "Апрель"},
                { 5, "Май"},
                { 6, "Июнь"},
                { 7, "Июль"},
                { 8, "Август"},
                { 9, "Сентябрь"},
                { 10, "Октябрь"},
                { 11, "Ноябрь"},
                { 12, "Декабрь"},
            };
        }

        public string GetNameMonth(int number) 
        {
            return _dict[number];
        }

        public int GetNumberMonth(string name) 
        {
            foreach (var val in _dict) 
            {
                if(val.Value == name) return val.Key;
            }
            return -1;
        }

    }
}
