using IntelligenceReporting.Entities;
using Microsoft.AspNetCore.Components;

namespace IntelligenceReporting.WebApp.Shared
{
    public partial class CountryDropDown : ComponentBase
    {
        [Parameter]
        public string? Class { get; set; }

        private Country[]? _countries = Array.Empty<Country>();
        private readonly Dictionary<string, object> _attributes;

        public CountryDropDown()
        {
            _attributes = new() { { "data-cy", nameof(GetType).ToLower() } };
        }

        protected override async Task OnInitializedAsync()
        {
            var staticData = await _storage.Get<StaticData>();
            _countries = staticData.Countries.OrderBy(c => c.CountryName).ToArray();
        }

        private async Task OnValueChanged(object value)
        {
            var countryId = ((Country?)value)?.Id;

#if DEBUG
            Console.WriteLine($@"{nameof(GetType)} value changed to ({countryId?.ToString() ?? "null"})");
#endif

            await _filterSelectionService.CountryId.SetValue(countryId);
        }
    }
}
