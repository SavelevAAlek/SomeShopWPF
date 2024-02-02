using System;

namespace SomeShopWPF.Services
{
    public interface IMessageBus
    {
        IDisposable RegisterHandler<T>(Action<T> Handler);

        void Send<T>(T message);
    }
}
