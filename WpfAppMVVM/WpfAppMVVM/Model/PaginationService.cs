using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Models.Entities;

namespace WpfAppMVVM.Model
{
    public class PaginationService
    {
        private const int pageSize = 100;
        public int CountPages { get; private set; }
        public int CurrentPage { get; private set; }
        public bool CanGetNext { get; private set; }
        public bool CanGetPrevios { get; private set; }

        private Type type;
        private IQueryable _query;

        public void SetQuery(IQueryable query) 
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            type = query.ElementType;
            _query = query;

            var totalItems = query.Provider.Execute<int>(
                Expression.Call(
                    typeof(Queryable),
                    nameof(Queryable.Count),
                    new Type[] { type },
                    query.Expression));

            CountPages = (int)Math.Ceiling((double)totalItems / pageSize);

            CurrentPage = 1;
            CanGetNext = CountPages > 1;
            CanGetPrevios = false;
        }

        public IEnumerable GetNextPage() 
        {
            IEnumerable objects;
            if (CanGetNext)
            {
                CurrentPage += 1;
                CanGetNext = CurrentPage != CountPages;
                CanGetPrevios = true;

                objects = getPage(CurrentPage);
            }
            else throw new Exception("Невозможно получить следующую страницу");
            return objects;
        }

        public IEnumerable GetPreviosPage()
        {
            IEnumerable objects;
            if (CanGetPrevios)
            {
                CurrentPage -= 1;
                CanGetPrevios = CurrentPage != 1;
                CanGetNext = true;

                objects = getPage(CurrentPage);
            }
            else throw new Exception("Невозможно получить предыдущую страницу");
            return objects;
        }

        public IEnumerable GetCurrentPage() 
        {
           return getPage(CurrentPage);
        }

        private IQueryable getPage(int pageNumber)
        {
            var method = typeof(Queryable).GetMethods()
                                          .First(m => m.Name == nameof(Queryable.Skip) && m.GetParameters().Length == 2)
                                          .MakeGenericMethod(type);

            var skipExpression = Expression.Call(
                null,
                method,
                new Expression[] { _query.Expression, Expression.Constant((pageNumber - 1) * pageSize) });

            method = typeof(Queryable).GetMethods()
                                      .First(m => m.Name == nameof(Queryable.Take) && m.GetParameters().Length == 2)
                                      .MakeGenericMethod(type);

            var takeExpression = Expression.Call(
                null,
                method,
                new Expression[] { skipExpression, Expression.Constant(pageSize) });

            return _query.Provider.CreateQuery(takeExpression);
        }
    }

}
