﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace SqlRepo.Testing
{
    public static class ExpressionAssertionExtensions
    {
        private static readonly ExpressionHelper Helper = new ExpressionHelper();

        public static bool AreAllEqual<TEntity>(this Expression<Func<TEntity, object>>[] source,
            Expression<Func<TEntity, object>>[] compareTo)
        {
            return !source.Where((t, i) => !t.IsEqual(compareTo[i]))
                          .Any();
        }

        public static bool HasMemberName<TEntity, TMember>(this Expression<Func<TEntity, TMember>> expression,
            string expected)
        {
            return Helper.GetMemberName(expression) == expected;
        }

        public static bool HasValue<TEntity, TMember>(this Expression<Func<TEntity, TMember>> expression,
            object expected)
        {
            return Helper.GetExpressionValue(expression) == expected;
        }

        public static bool IsComparisonWith<TEntity, TMember>(
            this Expression<Func<TEntity, TMember>> expression,
            string member,
            string @operator,
            string @value)
        {
            var memberName = Helper.GetMemberName(expression);
            var actualOperator = Helper.GetOperator(expression);
            var expressionValue = Helper.GetExpressionValue(expression);
            return memberName == member && actualOperator == @operator
                                        && expressionValue.ToString() == @value;
        }

        public static bool IsEqual<TEntity, TMember>(this Expression<Func<TEntity, TMember>> expression,
            Expression<Func<TEntity, TMember>> compareTo)
        {
            if(expression.Equals(compareTo) || ReferenceEquals(expression, compareTo))
            {
                return true;
            }

            if(compareTo == null)
            {
                return false;
            }

            var memberNameMatches = Helper.GetMemberName(expression) == Helper.GetMemberName(compareTo);
            var operatorMatches = Helper.GetOperator(expression) == Helper.GetOperator(compareTo);
            var valueMatches = IsValuelessExpression(expression) || Helper.GetExpressionValue(expression)
                                                                          .Equals(Helper.GetExpressionValue(
                                                                              compareTo));

            return memberNameMatches && operatorMatches && valueMatches;
        }

        public static bool IsEqual<TEntity>(this Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, bool>> compareTo)
        {
            return expression.IsEqual<TEntity, bool>(compareTo);
        }

        private static bool IsValuelessExpression<TEntity, TMember>(
            Expression<Func<TEntity, TMember>> expression)
        {
            return expression.Body.NodeType == ExpressionType.MemberAccess
                   || expression.Body.NodeType == ExpressionType.Convert;
        }
    }
}