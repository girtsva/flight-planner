using System.Collections.Generic;
using FlightPlanner.Models;

namespace FlightPlanner.Core.DTO
{
    public class PageResult
    {
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public List<Flight> Items { get; set; }

        public PageResult(List<Flight> items)
        {
            Page = 0;
            TotalItems = items.Count;
            Items = items;
        }
    }
}
