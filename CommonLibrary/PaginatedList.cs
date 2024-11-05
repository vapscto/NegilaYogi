using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class PaginatedList<T> : List<T>
    {
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;

        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalRecords { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalRecords = count;
            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
           
            PaginatedList<T> PaginatedList = null;
            try
            {
                var count = await source.CountAsync();
                var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                PaginatedList = new PaginatedList<T>(items, count, pageIndex, pageSize);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return PaginatedList;
        }
    }
}
