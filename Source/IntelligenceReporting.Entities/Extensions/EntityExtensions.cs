using IntelligenceReporting.Queries;

namespace IntelligenceReporting.Extensions
{
    /// <summary>Entity extension methods</summary>
    public static class EntityExtensions
    {
        /// <summary>Add a property to a query's order by</summary>
        public static void AddOrderBy<TParameters>(
            this TParameters parameters,
            string propertyName,
            bool isDescending = false) 
            where TParameters : QueryParameters
        {
            if (parameters.OrderBy != "")
                parameters.OrderBy += ", ";
            parameters.OrderBy += propertyName;
            if (isDescending)
                parameters.OrderBy += " desc";
        }

        /// <summary>Set a query's order by to a property</summary>
        public static void SetOrderBy<TParameters>(
            this TParameters parameters,
            string propertyName,
            bool isDescending = false)
            where TParameters : QueryParameters
        {
            parameters.OrderBy = "";
            parameters.AddOrderBy(propertyName, isDescending);
        }
    }
}
