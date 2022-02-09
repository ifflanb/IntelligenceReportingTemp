namespace IntelligenceReporting.WebApp.Services
{
    public interface IPagerService
    {
        int? PageNumber { get; }
        int? RowsPerPage { get; }
        Task PageChanged(int? value);
        Task RowsPerPageChanged(int? value);
        event Func<Task>? NotifyPageChanged;
        event Func<Task>? NotifyRowsPerPage;
    }

    public class PagerService : IPagerService
    {
      private int? _pageNumber;
      private int? _rowsPerPage;
      public int? PageNumber => _pageNumber;
      public int? RowsPerPage => _rowsPerPage;

      public async Task PageChanged(int? value)
      {
         _pageNumber = value;
         if (NotifyPageChanged != null)
         {
            await NotifyPageChanged.Invoke();
         }
      }

      public async Task RowsPerPageChanged(int? value)
      {
         _rowsPerPage = value;
         if (NotifyRowsPerPage != null)
         {
            await NotifyRowsPerPage.Invoke();
         }
      }

      public event Func<Task>? NotifyPageChanged;
      public event Func<Task>? NotifyRowsPerPage;
   }
}
