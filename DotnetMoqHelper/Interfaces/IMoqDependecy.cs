using Moq;
using System;

namespace DotnetMoqHelper.Interfaces
{
    public interface IMoqDependecy
    {
        T Get<T>() where T : notnull;

        void AddMock<T>() where T : class;

        void AddMock<T>(Action<Mock<T>> action, params object[] args) where T : class;

        void AddMock<T>(Action<Mock<T>> action) where T : class;

        void ClearMock();

        object GetCacheOrService(Type type);

    }
}
