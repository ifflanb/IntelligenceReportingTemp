using IntelligenceReporting.WebApp.Structs;
using Microsoft.AspNetCore.Components;

namespace IntelligenceReporting.WebApp.Shared
{
    public partial class OrderByDropDown : ComponentBase
    {
        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public OrderBy[]? Items { get; set; }

        private OrderBy _selectedItem;

        private readonly Dictionary<string, object> _attributes;

        public OrderByDropDown()
        {
            _attributes = new() { { "data-cy", nameof(GetType).ToLower() } };
        }

        protected override async Task OnInitializedAsync()
        {
            var selectedItem = Items?.FirstOrDefault(o => o.IsSelected) ?? Items?.First();
            if (selectedItem.HasValue)
            {
                _selectedItem = selectedItem.Value;
                await _filterSelectionService.OrderBy.SetValue(_selectedItem);
            }
        }

        private async Task SelectedItemChanged()
        {
#if DEBUG
            Console.WriteLine($@"{nameof(GetType)} value changed to ({_selectedItem.Description})");
#endif
            await _filterSelectionService.OrderBy.SetValue(_selectedItem);
        }
    }
}
