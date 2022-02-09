using IntelligenceReporting.WebApp.Structs;
using Microsoft.AspNetCore.Components;

namespace IntelligenceReporting.WebApp.Shared
{
    public partial class ReportPropertyClassDropDown : ComponentBase
    {
        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public ReportPropertyClass[]? Items { get; set; }

        private readonly Dictionary<string, object> _attributes;

        public ReportPropertyClassDropDown()
        {
            _attributes = new() { { "data-cy", nameof(GetType).ToLower() } };
        }

        private async Task OnSelectedValuesChanged(IEnumerable<ReportPropertyClass> values)
        {
            var reportPropertyClassIds = values.Select(o => o.Id).ToArray();
#if DEBUG
            Console.WriteLine($@"{nameof(GetType)} value changed to ({string.Join(",", reportPropertyClassIds)?.ToString() ?? "null"})");
#endif
            await _filterSelectionService.ReportPropertyClassIds.SetValues(reportPropertyClassIds);
        }
    }
}