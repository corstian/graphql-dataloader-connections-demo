using GraphQL.Builders;
using GraphQL.Types.Relay.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL_DataLoader_Connection
{
    public static class Extensions
    {
        public static string ToCursor(this Guid guid) => Convert.ToBase64String(guid.ToByteArray());
        public static Guid FromCursor(this string base64Guid) => new Guid(Convert.FromBase64String(base64Guid));

        /*
         * This is just a mock extension. Modify to suit your own needs.
         */
        public static async Task<Connection<TResult>> ToConnection<TResult, TSource>(
            this IEnumerable<TResult> enumerable,
            ResolveConnectionContext<TSource> context)
            where TResult : IId
            where TSource : class
        {
            await Task.Delay(8);    // Again, we have some network activity going on here :)

            IList<TResult> resultset;

            // In this if else tree we select the pages we want
            if (!string.IsNullOrWhiteSpace(context.After))
            {
                resultset = enumerable
                    .SkipWhile(q => q.Id != context.After.FromCursor())
                    .Skip(1) // We skip an additional one because other the 'after' cursor would be included in the results.
                    .Take(context.First.GetValueOrDefault(context.PageSize ?? 10))
                    .ToList();
            }
            else if (!string.IsNullOrWhiteSpace(context.Before))
            {
                throw new NotImplementedException();
            }
            else if (context.First != null)
            {
                resultset = enumerable
                    .Take(context.First.Value)
                    .ToList();
            }
            else if (context.Last != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                // We're out of options. Return a default selection
                resultset = enumerable
                    .Take(context.PageSize ?? 10)
                    .ToList();
            }


            // The rest of this method is used to construct the connection we're returning
            var edges = resultset.Select((item, i) => new Edge<TResult>
            {
                Node = item,
                Cursor = item?.Id.ToCursor()
            })
            .ToList();

            var connection = new Connection<TResult>
            {
                Edges = edges,
                TotalCount = enumerable.Count(),
                PageInfo = new PageInfo
                {
                    StartCursor = edges.FirstOrDefault()?.Cursor,
                    EndCursor = edges.LastOrDefault()?.Cursor,
                    HasPreviousPage = true,
                    HasNextPage = true,
                }
            };

            return connection;
        }
    }
}
