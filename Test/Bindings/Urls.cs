using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Bindings
{
    internal static class Urls
    {
        internal static object Algorithm;

        internal static class Items
        {
            public static QueryString GetList(
                int? pageSize,
                int? pageNumber,
                string searchTerm,
                string sortBy,
                string sortDirection,
                string moduleSystemName = null) =>
                QueryString.Create(new[]
                {
                    new KeyValuePair<string, string>("pageSize", pageSize.ToString()),
                    new KeyValuePair<string, string>("pageNumber", pageNumber.ToString()),
                    new KeyValuePair<string, string>("searchTerm", searchTerm),
                    new KeyValuePair<string, string>("sortBy", sortBy),
                    new KeyValuePair<string, string>("sortDirection", sortDirection),
                    new KeyValuePair<string, string>("moduleSystemName", moduleSystemName),
                });
        }

        internal static class AlgorithmConfiguration
        {
            private const string Collection = "algorithmConfigurations";
            private const string Root = "api/v1/algorithmConfigurations";
            private const string ModificationRoot = "api/v1/algorithms";

            public static string CreateItem(string algorithmSystemName)
            {
                var algorithmPart = Join(ModificationRoot, algorithmSystemName);

                return Join(algorithmPart, Collection);
            }

            public static string DeleteItem(string algorithmSystemName, string systemName)
            {
                return UpdateItem(algorithmSystemName, systemName);
            }

            public static string UpdateItem(string algorithmSystemName, string systemName)
            {
                var algorithmPart = Join(ModificationRoot, algorithmSystemName);
                var algorithmConfigurationsPart = Join(Collection, systemName);

                return Join(algorithmPart, algorithmConfigurationsPart);
            }

            public static string Item(string systemName) => BuildItemUrl(Root, systemName);
        }
        public static string BuildItemUrl(string collection, string id) => Join(collection, Escape(id));

        private static string Escape(string value) => Uri.EscapeUriString(value);

        public static string Join(string left, string right) => $"{left}/{right}";
    }
}
