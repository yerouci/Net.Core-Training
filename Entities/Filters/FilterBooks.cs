using Entities.Models;
using System;
using DynamicExpression;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Entities.ModelsDTO;

namespace Entities.Filters
{
    public static class FilterBooks
    {
        public static Expression<Func<BookDTO, bool>> GetFiltersExpression(BookParameters filters)
        {

            var criteriaExpression = new CriteriaExpression();

            if (filters.AuthorId != null)
            {
                criteriaExpression.Equal("AuthorId", filters.AuthorId);
            }

            if (filters.EditorialName != null)
            {
                criteriaExpression.Equal("EditorialName", filters.EditorialName);
            }

            if (filters.Before != null)
            {
                criteriaExpression.LessThan("Date", filters.Before);
            }

            if (filters.After != null)
            {
                criteriaExpression.GreaterThan("Date", filters.After);
            }

            var builder = new CriteriaBuilder();
            var expression = builder.Build<BookDTO>(criteriaExpression);

            return expression;
        }
    }
}
