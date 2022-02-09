using IntelligenceReporting.Entities;
using IntelligenceReporting.Queries;
using Microsoft.AspNetCore.Components;

namespace IntelligenceReporting.WebApp.Shared
{
    public partial class MultiOfficeAutoComplete : ComponentBase
    {
        [Parameter]
        public string? Class { get; set; }

        private readonly Dictionary<string, object> _attributes;

        public MultiOfficeAutoComplete()
        {
            _attributes = new() { { "data-cy", nameof(GetType).ToLower() } };
        }

        private async Task<IEnumerable<MultiOffice>> Search(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Array.Empty<MultiOffice>();

            var parameters = new MultiOfficeQueryParameters { Name = value };
            var offices = await _apiService.GetMultiOffices(parameters);
            return offices.Results;
        }

        private async Task OnValueChanged(object value)
        {
            var id = ((MultiOffice?)value)?.Id;

#if DEBUG
            Console.WriteLine($@"{nameof(GetType)} value changed to ({id?.ToString() ?? "null"}");
#endif

            await _filterSelectionService.MultiOfficeId.SetValue(id);
        }
    }
}
