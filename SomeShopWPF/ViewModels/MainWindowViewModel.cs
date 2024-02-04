using SomeShopWPF.Infrastructure;
using SomeShopWPF.Services;
using SomeShopWPF.ViewModels.Base;

namespace SomeShopWPF.ViewModels
{
    public class MainWindowViewModel : DialogViewModel
    {
        private readonly IDataBaseWorker _dbWorker;
        private MSSQLAdapter _adapter;

        public MainWindowViewModel(IDataBaseWorker dbWorker)
        {
            _dbWorker = dbWorker;
        }
    }
}
