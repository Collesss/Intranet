using System.DirectoryServices;
using System.Security.Principal;

namespace Intranet.Repository.ActiveDirectory.Extenstions
{
    public static class SearchResultExtensions
    {
        public static string GetStrSID(this SearchResult searchResult) =>
            new SecurityIdentifier((byte[])searchResult.Properties["objectSid"][0], 0).ToString();

        public static string GetStrFirstProp(this SearchResult searchResult, string prop) =>
            searchResult.Properties[prop].Count == 1 ? searchResult.Properties[prop][0].ToString() : null;

        public static string[] GetArrStrProp(this SearchResult searchResult, string prop) =>
            searchResult.Properties[prop].Cast<string>().ToArray();
    }
}
