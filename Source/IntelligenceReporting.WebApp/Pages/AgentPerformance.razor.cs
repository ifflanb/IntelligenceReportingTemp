using IntelligenceReporting.Enums;
using IntelligenceReporting.Extensions;
using IntelligenceReporting.Queries;
using IntelligenceReporting.WebApp.Shared;
using IntelligenceReporting.WebApp.Structs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace IntelligenceReporting.WebApp.Pages
{
    public partial class AgentPerformance : ComponentBase
    {

        #region Declarations

        private Pager? _pager;
        private MudDatePicker? _startPicker;
        private MudDatePicker? _endPicker;
        private IEnumerable<AgentEarningsQueryResult> _staff = Array.Empty<AgentEarningsQueryResult>();
        private AgentEarningsQueryResult _totals = new();
        private AgentEarningsQueryResult _averages = new();
        private int? _brandId;
        private int? _countryId;
        private int? _stateId;
        private int? _multiOfficeId;
        private int? _officeId;
        private int? _staffId;
        private int? _reportMethodOfSaleId;
        private OrderBy? _orderBy;
        private List<int>? _reportPropertyClassIds;
        private bool _loading;
        private int _rowsPerPage = 25;
        private int _numberOfPages = 1;
        private bool _showPreviousNextFirstLastButtons = true;
        private DateTime? _startDate = DateTime.Now.StartOfMonth();
        private DateTime? _endDate = DateTime.Now.EndOfMonth();
        private bool _isStartDateError;
        private bool _isEndDateError;
        private readonly Dictionary<string, object> _startDateCancelAttributes = new() { { "data-cy", "startdatecancel" } };
        private readonly Dictionary<string, object> _startDateOkAttributes = new() { { "data-cy", "startdateok" } };
        private readonly Dictionary<string, object> _startEndCancelAttributes = new() { { "data-cy", "enddatecancel" } };
        private readonly Dictionary<string, object> _startEndOkAttributes = new() { { "data-cy", "enddateok" } };
        private readonly Dictionary<string, object> _generateReportAttributes = new() { { "data-cy", "generatereport" } };
        private readonly Dictionary<string, object> _agentEarningsAttributes = new() { { "data-cy", "agentearningsreport" } };

        private readonly List<BreadcrumbItem> _items = new()
        {
            new BreadcrumbItem("Home", href: "/"),
            new BreadcrumbItem("Agent Performance", href: null, disabled: true)
        };

        private readonly OrderBy[] _orderByItems =
        {
            new(nameof(AgentEarningsQueryResult.Appraisals), true),
            new(nameof(AgentEarningsQueryResult.Listings), true),
            new(nameof(AgentEarningsQueryResult.Auctions), true),
            new(nameof(AgentEarningsQueryResult.Conditionals), "Conditional", true),
            new(nameof(AgentEarningsQueryResult.Unconditionals), "Unconditional", true),
            new(nameof(AgentEarningsQueryResult.Settlements), "Settled Sales", true),
            new(nameof(AgentEarningsQueryResult.Commission), "Commission on Settled Sales", true, true)
        };

        private readonly ReportPropertyClass[] _reportPropertyClassItems =
        {
            new((int)ReportPropertyClassEnum.ResidentialLand, "﻿﻿Residential﻿ & Land"),
            new((int)ReportPropertyClassEnum.Rural, "Rural"),
            new((int)ReportPropertyClassEnum.RuralOver40Acres, "Rural (Over 40 acres)"),
            new((int)ReportPropertyClassEnum.RuralOver20Hectares, "Rural (Over 20 hectares)"),
            new((int)ReportPropertyClassEnum.Commercial, "Commercial"),
            new((int)ReportPropertyClassEnum.CommercialLease, "Commercial Lease"),
            new((int)ReportPropertyClassEnum.Business, "Business")
        };
        #endregion

        #region Methods

        private async Task GetStaff(int pageNumber)
        {
            _loading = true;

            AgentEarningsPagedResults apiResults;
            if (_startDate.HasValue && _endDate.HasValue)
            {
                var parameters = new AgentEarningsQueryParameters
                {
                    PageSize = _rowsPerPage,
                    Page = pageNumber,
                    BrandId = _brandId,
                    CountryId = _countryId,
                    StateId = _stateId,
                    MultiOfficeId = _multiOfficeId,
                    OfficeId = _officeId,
                    StaffId = _staffId,
                    ReportMethodOfSaleId = _reportMethodOfSaleId,
                    StartDate = _startDate.Value,
                    EndDate = _endDate.Value,
                    ReportPropertyClassIds = _reportPropertyClassIds
                };

                if (_orderBy.HasValue)
                {
                    parameters.SetOrderBy(_orderBy.Value.ColumnName, _orderBy.Value.IsDescending);
                }

                apiResults = await _apiService.GetAgentEarnings(parameters);
            }
            else
            {
                apiResults = new AgentEarningsPagedResults();
            }

            _staff = apiResults.Results;
            _totals = apiResults.Total;
            _averages = apiResults.Average;
            _numberOfPages = (int)Math.Ceiling(apiResults.TotalCount / (decimal)_rowsPerPage);
            AddTotalsRow();

            _loading = false;
            _showPreviousNextFirstLastButtons = _numberOfPages > 1;
            _isStartDateError = !_startDate.HasValue;
            _isEndDateError = !_endDate.HasValue;
            if (_pager != null)
            {
                _pager.SetPageNumber(pageNumber);
            }
        }

        private void AddTotalsRow()
        {
            _totals.StaffName = "Total";
            _staff = _staff.Append(_totals);
        }

        #endregion

        #region Events

        protected override void OnInitialized()
        {
            _officeId = _filterSelectionService.OfficeId.Value;
            _staffId = _filterSelectionService.StaffId.Value;

            _filterSelectionService.BrandId.OnChanged += OnBrandChanged;
            _filterSelectionService.CountryId.OnChanged += OnCountryChanged;
            _filterSelectionService.StateId.OnChanged += OnStateChanged;
            _filterSelectionService.MultiOfficeId.OnChanged += OnMultiOfficeChanged;
            _filterSelectionService.OfficeId.OnChanged += OnOfficeChanged;
            _filterSelectionService.StaffId.OnChanged += OnStaffChanged;
            _filterSelectionService.ReportMethodOfSaleId.OnChanged += OnReportMethodOfSaleChanged;
            _filterSelectionService.OrderBy.OnChanged += OnOrderChanged;
            _filterSelectionService.ReportPropertyClassIds.OnChanged += OnReportPropertyClassChanged;
            _pagerService.NotifyPageChanged += OnPageChanged;
            _pagerService.NotifyRowsPerPage += OnRowsPerPageChanged;
        }
        private Task OnReportPropertyClassChanged()
        {
            _reportPropertyClassIds = _filterSelectionService.ReportPropertyClassIds.Values?.ToList();
            return Task.CompletedTask;
        }

        private Task OnOrderChanged()
        {
            _orderBy = _filterSelectionService.OrderBy.Value;
            return Task.CompletedTask;
        }

        private Task OnReportMethodOfSaleChanged()
        {
            _reportMethodOfSaleId = _filterSelectionService.ReportMethodOfSaleId.Value;
            return Task.CompletedTask;
        }

        private Task OnCountryChanged()
        {
            _countryId = _filterSelectionService.CountryId.Value;
            return Task.CompletedTask;
        }

        private async Task OnPageChanged()
        {
            var pageNumber = _pagerService.PageNumber ?? 1;
            await GetStaff(pageNumber);
            StateHasChanged();
        }

        private async Task OnRowsPerPageChanged()
        {
            _rowsPerPage = _pagerService.RowsPerPage ?? 1;
            await GetStaff(1);
            if (_pager != null)
            {
                _pager.SetPageNumber(1);
            }

            StateHasChanged();
        }

        private Task OnBrandChanged()
        {
            _brandId = _filterSelectionService.BrandId.Value;
            return Task.CompletedTask;
        }

        private Task OnMultiOfficeChanged()
        {
            _multiOfficeId = _filterSelectionService.MultiOfficeId.Value;
            return Task.CompletedTask;
        }

        private Task OnOfficeChanged()
        {
            _officeId = _filterSelectionService.OfficeId.Value;
            return Task.CompletedTask;
        }

        private Task OnStaffChanged()
        {
            _staffId = _filterSelectionService.StaffId.Value;
            return Task.CompletedTask;
        }

        private Task OnStateChanged()
        {
            _stateId = _filterSelectionService.StateId.Value;
            return Task.CompletedTask;
        }

        private void Dispose()
        {
            _filterSelectionService.BrandId.OnChanged -= OnBrandChanged;
            _filterSelectionService.CountryId.OnChanged -= OnCountryChanged;
            _filterSelectionService.StateId.OnChanged -= OnStateChanged;
            _filterSelectionService.MultiOfficeId.OnChanged -= OnMultiOfficeChanged;
            _filterSelectionService.OfficeId.OnChanged -= OnOfficeChanged;
            _filterSelectionService.StaffId.OnChanged -= OnStaffChanged;
            _filterSelectionService.ReportMethodOfSaleId.OnChanged -= OnReportMethodOfSaleChanged;
            _filterSelectionService.OrderBy.OnChanged -= OnOrderChanged;
            _filterSelectionService.ReportPropertyClassIds.OnChanged -= OnReportPropertyClassChanged;
            _pagerService.NotifyPageChanged -= OnPageChanged;
            _pagerService.NotifyRowsPerPage -= OnRowsPerPageChanged;
        }

        private async Task GenerateReport(MouseEventArgs args)
        {
#if DEBUG
            Console.WriteLine($@"Generate Report button clicked");
#endif
            await GetStaff(1);
            StateHasChanged();
        }

        private void OnStartDateChanged(DateTime? startDate)
        {
            _isStartDateError = !startDate.HasValue;
            _startDate = startDate;
        }

        private void OnEndDateChanged(DateTime? endDate)
        {
            _isEndDateError = !endDate.HasValue;
            _endDate = endDate;
        }

        private Func<AgentEarningsQueryResult, int, string> _rowClassFunc => (x, i) =>
        {
            var cssClass = string.Empty;

            if (x.StaffName == "Total")
            {
                cssClass += "mock-table-footer";
            }

            return cssClass;
        };

        #endregion

    }
}
