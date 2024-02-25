using Intranet.Api.Dto.Common.Both;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Intranet.Api.Dto.ValidationAttributes
{
    public class QueryValidationAttribute<T> : ValidationAttribute
    {
        private readonly string[] _fields;

        private IEnumerable<string> GetFields()
        {
            string[] defaultTypes = new string[] { "Enum", "Char", "String", "Boolean", "Int16", "Int32", "Int64", "UInt16", "UInt32", "UInt64", "Single", "Double", "Decimal" };

            IEnumerable<string> GetFields(PropertyInfo property, string prefix = "") =>
                defaultTypes.Contains(property.PropertyType.Name)
                ? new string[] { $"{prefix}{property.Name}" }
                : property.PropertyType.GetProperties().SelectMany(prop => GetFields(prop, $"{prefix}{property.Name}."));

            return typeof(T).GetProperties().SelectMany(prop => GetFields(prop)).ToList();
        }

        public QueryValidationAttribute() 
        {
            _fields = GetFields().ToArray();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
                return new ValidationResult("Query can't be null.");

            if (value is not QueryDto sortAndFilter)
                throw new ArgumentException("The attribute can only be used on the QueryRequestDto class.", nameof(value));


            if (!IsValidPage(sortAndFilter.Page, out ValidationResult validResultPage))
                return validResultPage;

            if (!IsValidSort(sortAndFilter.Sorts, out ValidationResult validResultSorts))
                return validResultSorts;

            if (!IsValidFilter(sortAndFilter.Filters, out ValidationResult validResultFilters))
                return validResultFilters;


            return ValidationResult.Success;
        }


        private bool IsValidPage(PageDto page, out ValidationResult validationResult)
        {
            if (page is null)
            {
                validationResult = new ValidationResult("Page can't be null.", new string[] { "Page" });
                return false;
            }

            if (page.PageSize <= 0)
            {
                validationResult = new ValidationResult("Page.PageSize can't less or equal 0.", new string[] { "Page.PageSize" });
                return false;
            }

            if (page.PageNumber <= 0)
            {
                validationResult = new ValidationResult("Page.PageNumber can't less or equal 0.", new string[] { "Page.PageNumber" });
                return false;
            }


            validationResult = ValidationResult.Success;
            return true;
        }


        private bool IsValidSort(IEnumerable<SortDto> sorts, out ValidationResult validationResult)
        {
            if(sorts is null)
            {
                validationResult = new ValidationResult("Sorts can't be null.", new string[] { "Sorts" });
                return false;
            }

            string[] fieldsSort = sorts.Select(sort => sort.Field).ToArray();

            if (!fieldsSort.All(fieldSort => _fields.Contains(fieldSort)))
            {
                validationResult = new ValidationResult($"Sorts list contains not accepted fields. Accepted fields: {string.Join(", ", _fields)}.", new string[] { "Sorts" });
                return false;
            }

            if (fieldsSort.GroupBy(fieldSort => fieldSort).Any(groupedFields => groupedFields.Count() > 1))
            {
                validationResult = new ValidationResult("Sort list can't contains duplicated fields.", new string[] { "Sorts" });
                return false;
            }

            validationResult = ValidationResult.Success;
            return true;
        }

        private bool IsValidFilter(IEnumerable<FilterDto> filters, out ValidationResult validationResult)
        {
            if (filters is null)
            {
                validationResult = new ValidationResult("Filters can't be null.", new string[] { "Filters" });
                return false;
            }

            string[] fieldsFilter = filters.Select(filter => filter.Field).ToArray();

            string[] stringsSearchFilter = filters.Select(filter => filter.StringSearch).ToArray();


            if (!fieldsFilter.All(fieldFilter => _fields.Contains(fieldFilter)))
            {
                validationResult = new ValidationResult($"Filter list can't contains not accepted fields. Accepted fields: {string.Join(", ", _fields)}.", new string[] { "Filters" });
                return false;
            }

            if (fieldsFilter.GroupBy(fieldFiltert => fieldFiltert)
                .Any(groupedFields => groupedFields.Count() > 1))
            {
                validationResult = new ValidationResult("Filter list can't contains duplicated fields.", new string[] { "Filters" });
                return false;
            }

            string regexStr = "^[a-zA-Z*@_0-9\\-а-яА-ЯёЁ ]+$";
            Regex regex = new Regex(regexStr);

            if (!stringsSearchFilter.All(regex.IsMatch))
            {
                validationResult = new ValidationResult($"Filter list contains not accepted SearchString. Regex for accepted SearchString: {regexStr}.", new string[] { "Filters" });
                return false;
            }


            validationResult = ValidationResult.Success;
            return true;
        }
    }
}
