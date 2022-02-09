using IntelligenceReporting.Entities;
using IntelligenceReporting.Queries;
using Microsoft.AspNetCore.Components;

namespace IntelligenceReporting.WebApp.Shared
{
    public partial class OfficeAutoComplete : ComponentBase
    {
        [Parameter]
        public string? Class { get; set; }
        private readonly Dictionary<string, object> _attributes;
        private OfficeQueryResult? _officeModel;

        public OfficeAutoComplete()
        {
            _attributes = new() { { "data-cy", nameof(GetType).ToLower() } };
        }

        protected override async Task OnInitializedAsync()
        {
            _filterSelectionService.CountryId.OnChanged += Refresh;
            _filterSelectionService.StateId.OnChanged += Refresh;
            _filterSelectionService.MultiOfficeId.OnChanged += Refresh;
            await Refresh();
        }

        private async Task Refresh()
        {
            await OnValueChanged(null);
            StateHasChanged();
        }

        private async Task<IEnumerable<OfficeQueryResult>> Search(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Array.Empty<OfficeQueryResult>();

            var selectedBrandId = _filterSelectionService.BrandId.Value;
            var selectedCountryId = _filterSelectionService.CountryId.Value;
            var selectedStateId = _filterSelectionService.StateId.Value;
            var selectedMultiOfficeId = _filterSelectionService.MultiOfficeId.Value;

            var parameters = new OfficeQueryParameters
            {
                OfficeName = value,
                StateId = selectedStateId,
                BrandId = selectedBrandId,
                MultiOfficeId = selectedMultiOfficeId,
                CountryId = selectedCountryId,
            };

            var offices = await _apiService.GetOffices(parameters);
            return offices.Results;
        }

        private async Task OnValueChanged(object value)
        {
            _officeModel = (OfficeQueryResult?)value;
            var officeId = _officeModel?.OfficeId;
#if DEBUG
            Console.WriteLine($@"{nameof(GetType)} value changed to ({officeId?.ToString() ?? "null"})");
#endif
            await _filterSelectionService.OfficeId.SetValue(officeId);
        }
    }
}