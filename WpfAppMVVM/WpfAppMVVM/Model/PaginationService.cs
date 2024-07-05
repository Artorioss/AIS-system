using System.Collections;
using System.Linq.Expressions;

namespace WpfAppMVVM.Model
{
    public class PaginationService
    {
        private const int pageSize = 100;
        public int CountPages { get; private set; }
        public int CurrentPage { get; private set; }
        public bool CanGetNext { get; private set; }
        public bool CanGetPrevios { get; private set; }
        enum Mode {SearchingObjects, AllObjects }
        Mode mode = Mode.AllObjects;

        private Type type;
        private IQueryable _query;
        private Func<string, IQueryable> _func;
        string _searchValue = string.Empty;

        public void SetQuery(IQueryable query) 
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            type = query.ElementType;
            _query = query;

            UpdatePagination(_query);
        }

        public void SetSearchFunction(Func<string, IQueryable> searchFunc)
        {
            if (searchFunc == null)
                throw new ArgumentNullException(nameof(searchFunc));

            _func = searchFunc;
        }

        public IEnumerable GetDataByValue(string obj) 
        {
            if(_func is null)
                throw new ArgumentNullException("Не установлена функция для поиска.");

            _searchValue = obj;

            if (string.IsNullOrEmpty(obj))
            {
                UpdatePagination(_query);
                mode = Mode.AllObjects;
            }
            else 
            {
                UpdatePagination(_func(obj));
                mode = Mode.SearchingObjects;
            } 
            return GetCurrentPage();
        }

        private void UpdatePagination(IQueryable queryable)
        {
            var totalItems = queryable.Provider.Execute<int>(
                Expression.Call(
                    typeof(Queryable),
                    nameof(Queryable.Count),
                    new Type[] { type },
                    queryable.Expression));

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
                objects = getObjects();
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
                objects = getObjects();
            }
            else throw new Exception("Невозможно получить предыдущую страницу");
            return objects;
        }

        public IEnumerable GetCurrentPage()
        {
            return getObjects();
        }

        private IEnumerable getObjects() 
        {
            if(mode == Mode.AllObjects) return getPage(CurrentPage, _query);
            else return getPage(CurrentPage, _func(_searchValue));
        }

        private IQueryable getPage(int pageNumber, IQueryable query)
        {
            var method = typeof(Queryable).GetMethods()
                                          .First(m => m.Name == nameof(Queryable.Skip) && m.GetParameters().Length == 2)
                                          .MakeGenericMethod(type);

            var skipExpression = Expression.Call(
                null,
                method,
                new Expression[] { query.Expression, Expression.Constant((pageNumber - 1) * pageSize) });

            method = typeof(Queryable).GetMethods()
                                      .First(m => m.Name == nameof(Queryable.Take) && m.GetParameters().Length == 2)
                                      .MakeGenericMethod(type);

            var takeExpression = Expression.Call(
                null,
                method,
                new Expression[] { skipExpression, Expression.Constant(pageSize) });

            return query.Provider.CreateQuery(takeExpression);
        }
    }

}
