using System.Collections;
using System.Collections.Generic;

namespace SomeShopWPF.Services
{
    public interface IDataBaseWorker
    {
        List<IEnumerable> Select();
        void Delete(object item);
        void Insert(object item);
        void Update(object item);

    }
}
