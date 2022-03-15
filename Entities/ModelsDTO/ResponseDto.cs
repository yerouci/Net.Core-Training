using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ModelsDTO
{
    public class ResponseDto<T>
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public IEnumerable<T> Data { get; set; }

    }
}
