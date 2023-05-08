using System;

namespace Moq.Tools.Interfaces
{
    public interface IMoqDependecy
    {
        T Get<T>() where T : notnull;

        void AddMock<T>(Action<Mock<T>> action, params object[] args) where T : class;

        void AddMock<T>(Action<Mock<T>> action) where T : class;

        void ClearMock();
        
    }
}
