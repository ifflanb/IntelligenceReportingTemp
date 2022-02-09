using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IntelligenceReporting.WebApp.Shared
{
    public partial class Pager : ComponentBase
    {
        [Parameter]
        public int RowsPerPage { get; set; } = 25;

        [Parameter]
        public int NumberOfPages { get; set; } = 1;

        [Parameter]
        public bool ShowPreviousNextFirstLastButtons { get; set; } = true;

        private int PageNumber { get; set; } = 1;
        private bool _shouldRender = true;

        private readonly Dictionary<string, object> _rowsPerPageAttributes = new() { { "data-cy", "rowsperpage" } };
        private readonly Dictionary<string, object> _rowsPerPage25Attributes = new() { { "data-cy", "rowsperpage25" } };
        private readonly Dictionary<string, object> _rowsPerPage50Attributes = new() { { "data-cy", "rowsperpage50" } };
        private readonly Dictionary<string, object> _rowsPerPage100Attributes = new() { { "data-cy", "rowsperpage100" } };
        private readonly Dictionary<string, object> _rowsPerPageAllAttributes = new() { { "data-cy", "rowsperpageAll" } };
        private readonly Dictionary<string, object> _paginationAttributes;

        public Pager()
        {
            _paginationAttributes = new() { { "data-cy", nameof(GetType).ToLower() } };
        }

        public void SetPageNumber(int pageNumber)
        {
#if DEBUG
            Console.WriteLine($@"{nameof(GetType)} page number has been set to ({pageNumber}) - PageChanged won't be rendered.");
#endif
            _shouldRender = false;
            PageNumber = pageNumber;
            StateHasChanged();
            _shouldRender = true;
        }

        private async Task PageChanged(int pageNumber)
        {
            if (_shouldRender)
            {
#if DEBUG
                Console.WriteLine($@"{nameof(GetType)} page number value changed to ({pageNumber})");
#endif
                PageNumber = pageNumber;
                await _pagerService.PageChanged(pageNumber);
            }
        }

        private async Task RowsPerPageChanged()
        {
#if DEBUG
            Console.WriteLine($@"{nameof(GetType)} rows per page value changed to ({RowsPerPage})");
#endif
            await _pagerService.RowsPerPageChanged(RowsPerPage);
        }
    }
}
