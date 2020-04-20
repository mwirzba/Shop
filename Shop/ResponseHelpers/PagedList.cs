using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Respn
{
	public class PagedList<T> : List<T>
	{
		public int CurrentPage { get; private set; }
		public int TotalPages { get; private set; }
		public int PageSize { get; private set; }
		public int TotalCount { get; private set; }
		public bool HasPrevious => CurrentPage > 1;
		public bool HasNext => CurrentPage < TotalPages;

		public PagedList(List<T> items, int count, int pageNumber, int pageSize)
		{
			TotalCount = count;
			CurrentPage = pageNumber;
			PageSize = pageSize;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);
			AddRange(items);
		}
	
		public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
		{
			var count = source.Count();
			var items = await source.Skip((pageNumber - 1) * pageSize)
						.Take(pageSize).ToListAsync();

			return new PagedList<T>(items, count, pageNumber, pageSize);
		}
	}

	public static class PagedListMapper<TIn,TOut>
	{
		public static PagedList<TOut> Map(PagedList<TIn> pagedList,IMapper mapper)
		{
			var productdto = mapper.Map<IEnumerable<TIn>,List<TOut>>(pagedList);

			PagedList<TOut> paggedOut = new PagedList<TOut>(productdto, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize);

			return paggedOut;
		}
	}
}
