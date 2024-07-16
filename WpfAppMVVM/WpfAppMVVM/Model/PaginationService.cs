using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace WpfAppMVVM.Model
{
    public class PaginationService
    {
        private const int pageSize = 5;
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

        public void SetQuery<T>(IQueryable<T> query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            type = typeof(T);
            _query = query;

            UpdatePagination(_query);
        }

        public void SetSearchFunction(Func<string, IQueryable> searchFunc)
        {
            if (searchFunc == null)
                throw new ArgumentNullException(nameof(searchFunc));

            _func = searchFunc;
        }

        public async Task<IEnumerable> GetDataByValueAsync(string obj) 
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
            return await GetCurrentPageAsync();
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

        public async Task<IEnumerable> GetNextPageAsync() 
        {
            IEnumerable objects;
            if (CanGetNext)
            {
                CurrentPage += 1;
                CanGetNext = CurrentPage != CountPages;
                CanGetPrevios = true;
                objects = await getObjectsAsync();
            }
            else throw new Exception("Невозможно получить следующую страницу");
            return objects;
        }

        public async Task<IEnumerable> GetPreviosPageAsync()
        {
            IEnumerable objects;
            if (CanGetPrevios)
            {
                CurrentPage -= 1;
                CanGetPrevios = CurrentPage != 1;
                CanGetNext = true;
                objects = await getObjectsAsync();
            }
            else throw new Exception("Невозможно получить предыдущую страницу");
            return objects;
        }

        public async Task<IEnumerable> GetCurrentPageAsync()
        {
            return await getObjectsAsync();
        }

        private async Task<IEnumerable> getObjectsAsync()
        {
            if (mode == Mode.AllObjects)
                return await CallGenericGetPageAsync(CurrentPage, _query, type);
            else
                return await CallGenericGetPageAsync(CurrentPage, _func(_searchValue), type);
        }

        private async Task<IEnumerable> CallGenericGetPageAsync(int pageNumber, IQueryable query, Type type)
        {
            var method = typeof(PaginationService).GetMethod(nameof(getPage), BindingFlags.NonPublic | BindingFlags.Instance)
                                                   .MakeGenericMethod(type);
            var resultTask = (Task)method.Invoke(this, new object[] { pageNumber, query });
            await resultTask.ConfigureAwait(false);

            var resultProperty = resultTask.GetType().GetProperty("Result");
            var result = resultProperty.GetValue(resultTask);

            return (IEnumerable)result;
        }

        private async Task<List<T>> getPage<T>(int pageNumber, IQueryable<T> query)
        {
            return await getQueryPage<T>(pageNumber, query).ToListAsync();
        }

        private IQueryable<T> getQueryPage<T>(int pageNumber, IQueryable query)
        {
            var method = typeof(Queryable).GetMethods()
                                          .First(m => m.Name == nameof(Queryable.Skip) && m.GetParameters().Length == 2)
                                          .MakeGenericMethod(typeof(T));

            var skipExpression = Expression.Call(
                null,
                method,
                new Expression[] { query.Expression, Expression.Constant((pageNumber - 1) * pageSize) });

            method = typeof(Queryable).GetMethods()
                                      .First(m => m.Name == nameof(Queryable.Take) && m.GetParameters().Length == 2)
                                      .MakeGenericMethod(typeof(T));

            var takeExpression = Expression.Call(
                null,
                method,
                new Expression[] { skipExpression, Expression.Constant(pageSize) });

            return query.Provider.CreateQuery<T>(takeExpression);
        }
    }
}
