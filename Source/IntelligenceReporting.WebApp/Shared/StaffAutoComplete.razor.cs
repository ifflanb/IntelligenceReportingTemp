using IntelligenceReporting.Extensions;
using IntelligenceReporting.Queries;
using Microsoft.AspNetCore.Components;

namespace IntelligenceReporting.WebApp.Shared
{
    public partial class StaffAutoComplete : ComponentBase
    {
        [Parameter]
        public string? Class { get; set; }
        private readonly Dictionary<string, object> _attributes;
        private StaffQueryResult? _staffModel;

        public StaffAutoComplete()
        {
            _attributes = new() { { "data-cy", nameof(GetType).ToLower() } };
        }

        protected override async Task OnInitializedAsync()
        {
            _filterSelectionService.CountryId.OnChanged += Refresh;
            _filterSelectionService.StateId.OnChanged += Refresh;
            _filterSelectionService.MultiOfficeId.OnChanged += Refresh;
            _filterSelectionService.BrandId.OnChanged += Refresh;
            _filterSelectionService.OfficeId.OnChanged += Refresh;

            await Refresh();
        }

        private async Task Refresh()
        {
            await OnValueChanged(null);
            StateHasChanged();
        }

        private async Task<IEnumerable<StaffQueryResult>> Search(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Array.Empty<StaffQueryResult>();

            var parameters = new StaffQueryParameters
            {
                StaffName = value,
                StateId = _filterSelectionService.StateId.Value,
                CountryId = _filterSelectionService.CountryId.Value,
                BrandId = _filterSelectionService.BrandId.Value,
                MultiOfficeId = _filterSelectionService.MultiOfficeId.Value,
                OfficeId = _filterSelectionService.OfficeId.Value
            };

            parameters.SetOrderBy(nameof(StaffQueryResult.StaffName));
            parameters.AddOrderBy(nameof(StaffQueryResult.StaffId), true);

            var apiResult = await _apiService.GetStaff(parameters);
            var staff = apiResult.Results;
            return staff;
        }

        private async Task OnValueChanged(object value)
        {
            _staffModel = (StaffQueryResult?)value;
            var staffId = _staffModel?.StaffId;

#if DEBUG
            Console.WriteLine($@"{nameof(GetType)} value changed to ({staffId?.ToString() ?? "null"})");
#endif

            await _filterSelectionService.StaffId.SetValue(staffId);
        }
    }
}
