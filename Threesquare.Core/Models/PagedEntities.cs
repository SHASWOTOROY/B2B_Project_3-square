﻿namespace Threesquare.Core.Models
{
    public class PagedEntities<T>
    {
        public PagedEntities()
        {
            Items = new List<T>();
        }

        public PagedEntities(List<T> entities, int pageNumber, int pageSize, int totalItems)
        {
            Items = entities;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(TotalItems / (double)PageSize);
            }
        }
        public List<T> Items { get; set; }
    }
}
