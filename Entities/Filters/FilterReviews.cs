using Entities.Models;
using System;
using DynamicExpression;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Entities.ModelsDTO;

namespace Entities.Filters
{
    public static class FilterReviews
    {
        public static Expression<Func<ReviewDTO, bool>> GetFiltersExpression(ReviewsParameters filters)
        {

            var criteriaExpression = new CriteriaExpression();            

            if (filters.reviewType != null)
            {
                criteriaExpression.Equal("Qualification", filters.reviewType);
            }

            var builder = new CriteriaBuilder();
            var expression = builder.Build<ReviewDTO>(criteriaExpression);

            return expression;
        }
    }
}
