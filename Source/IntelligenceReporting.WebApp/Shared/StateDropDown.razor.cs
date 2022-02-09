using IntelligenceReporting.Entities;
using Microsoft.AspNetCore.Components;

namespace IntelligenceReporting.WebApp.Shared
{
    public partial class StateDropDown : ComponentBase
    {
        [Parameter]
        public string? Class { get; set; }
        private State? _stateModel = null;
        private State[]? _states = Array.Empty<State>();
        private readonly Dictionary<string, object> _attributes;

        public StateDropDown()
        {
            _attributes = new() { { "data-cy", nameof(GetType).ToLower() } };
        }

        protected override async Task OnInitializedAsync()
        {
            _filterSelectionService.CountryId.OnChanged += Refresh;
            await Refresh();
        }

        private async Task Refresh()
        {
            var staticData = await _storage.Get<StaticData>();
            var selectedCountryId = _filterSelectionService.CountryId.Value;

            _states = staticData.States
                .Where(s => !selectedCountryId.HasValue || s.CountryId == selectedCountryId)
                .OrderBy(s => s.StateName).ToArray();

            await OnValueChanged(null);
            StateHasChanged();
        }

        private async Task OnValueChanged(object? value)
        {
            _stateModel = (State?)value;
            var stateId = _stateModel?.Id;

#if DEBUG
            Console.WriteLine($@"{nameof(GetType)} value changed to ({stateId?.ToString() ?? "null"})");
#endif

            await _filterSelectionService.StateId.SetValue(stateId);
        }
    }
}
