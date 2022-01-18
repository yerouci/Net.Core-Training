using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
	public class QueryStringParameters
	{
		const int maxPageSize = 50;
		public int Offset { get; set; } = 1;

		private int _pageSize = 10;
		public int Limit
		{
			get
			{
				return _pageSize;
			}
			set
			{
				_pageSize = (value > maxPageSize) ? maxPageSize : value;
			}
		}
	}
}
