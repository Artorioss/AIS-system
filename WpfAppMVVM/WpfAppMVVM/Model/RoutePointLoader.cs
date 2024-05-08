using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfAppMVVM.Models.Entities;


namespace WpfAppMVVM.Models
{
    public class RoutePointLoader
    {
        private const string _pattern = @"([п,г]\.\s)*[\w\(\)]+(?:\sи\s[\w\(\)]+)?";
        private Regex _regex;
        public List<RoutePoint> Route_Points_Loading { get; set; }
        public List<RoutePoint> Route_Points_Dispatcher { get; set; }

        public RoutePointLoader()
        {
            Route_Points_Loading = new List<RoutePoint>();
            Route_Points_Dispatcher = new List<RoutePoint>();
            _regex = new Regex(_pattern);
        }

        public void setRoutePoints(string text)
        {
            if (text != string.Empty) loadPoints(text);
            else clearData();
        }

        public override string ToString()
        {
            string first = string.Join(", ", Route_Points_Loading.Select(s => s.Name));
            string second = string.Join(", ", Route_Points_Dispatcher.Select(s => s.Name));
            string _generalRoute = $"{first} - {second}";
            return _generalRoute;
        }

        public void AddDispatcher(RoutePoint route_Point_dispatcher)
        {
            Route_Points_Dispatcher.Add(route_Point_dispatcher);
        }

        public void AddDispatcher(string route_Point_dispatcher)
        {
            Route_Points_Dispatcher.Add(new RoutePoint
            {
                Name = route_Point_dispatcher
            });
        }

        public void AddLoading(RoutePoint route_Point_loading)
        {
            Route_Points_Loading.Add(route_Point_loading);
        }

        public void AddLoading(string route_Point_loading)
        {
            Route_Points_Loading.Add(new RoutePoint
            {
                Name = route_Point_loading
            });
        }

        private void loadPoints(string text)
        {
            clearData();
            if (text.Contains('-'))
            {
                string[] strings = text.Split('-');
                if (strings.First() != string.Empty)
                {
                    loadMatchesInList(_regex.Match(strings.First()), Route_Points_Loading);
                }
                if (strings.Last() != string.Empty)
                {
                    loadMatchesInList(_regex.Match(strings.Last()), Route_Points_Dispatcher);
                }
            }
            else
            {
                loadMatchesInList(_regex.Match(text), Route_Points_Loading);
            }
        }

        private void loadMatchesInList(Match match, List<RoutePoint> list)
        {
            while (match.Success)
            {
                list.Add(new RoutePoint { Name = match.Value });
                match = match.NextMatch();
            }
        }

        private void clearData()
        {
            Route_Points_Loading.Clear();
            Route_Points_Dispatcher.Clear();
        }
    }
}
