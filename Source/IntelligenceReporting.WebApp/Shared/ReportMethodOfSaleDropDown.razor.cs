using IntelligenceReporting.Entities;
using Microsoft.AspNetCore.Components;

namespace IntelligenceReporting.WebApp.Shared
{
    public partial class ReportMethodOfSaleDropDown : ComponentBase
    {
        [Parameter]
        public string? Class { get; set; }

        private ReportMethodOfSale[]? _reportMethodOfSales = Array.Empty<ReportMethodOfSale>();
        private readonly Dictionary<string, object> _attributes;

        public ReportMethodOfSaleDropDown()
        {
            _attributes = new() { { "data-cy", nameof(GetType).ToLower() } };
        }

        protected override async Task OnInitializedAsync()
        {
            var staticData = await _storage.Get<StaticData>();
            _reportMethodOfSales = staticData.ReportMethodOfSales.OrderBy(c => c.Id).ToArray();
        }

        private async Task OnValueChanged(object value)
        {
            var reportMethodOfSaleId = ((ReportMethodOfSale?)value)?.Id;

#if DEBUG
            Console.WriteLine($@"{nameof(GetType)} value changed to ({reportMethodOfSaleId?.ToString() ?? "null"})");
#endif

            await _filterSelectionService.ReportMethodOfSaleId.SetValue(reportMethodOfSaleId);
        }
    }
}
