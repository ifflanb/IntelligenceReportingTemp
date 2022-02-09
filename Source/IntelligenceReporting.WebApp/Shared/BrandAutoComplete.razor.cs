using IntelligenceReporting.Entities;
using Microsoft.AspNetCore.Components;

namespace IntelligenceReporting.WebApp.Shared
{
    public partial class BrandAutoComplete : ComponentBase
    {
        [Parameter]
        public string? Class { get; set; }

        private StaticData? _staticData;
        private readonly Dictionary<string, object> _attributes;

        public BrandAutoComplete()
        {
            _attributes = new() { { "data-cy", nameof(GetType).ToLower() } };
        }

        private async Task<IEnumerable<Brand>> Search(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Array.Empty<Brand>();

            _staticData ??= await _storage.Get<StaticData>();

            var brands = _staticData.Brands
                .Where(o => o.BrandName.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(o => o.BrandName)
                .ToArray();
            return brands;
        }

        private async Task OnValueChanged(object value)
        {
            var brandId = ((Brand?)value)?.Id;

#if DEBUG
            Console.WriteLine($@"{nameof(GetType)} value changed to ({brandId?.ToString() ?? "null"})");
#endif

            await _filterSelectionService.BrandId.SetValue(brandId);
        }
    }
}
