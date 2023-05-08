using Microsoft.Extensions.DependencyInjection;
using Moq.Tools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Moq.Tools.Common
{
    internal class MoqDependecy: IMoqDependecy
    {
        private Dictionary<Type, Mock> _dicMockCache = new Dictionary<Type, Mock>();
        private readonly IServiceProvider _service;

        public MoqDependecy(IServiceProvider service)
        {
            this._service = service;
        }

        public T Get<T>()
            where T : notnull
        {
            if (_dicMockCache.TryGetValue(typeof(T), out Mock? mResult))
                return (T)mResult.Object;

            return _service.GetRequiredService<T>();
        }

        public void AddMock<T>() where T : class
        {
            Mock<T> objMock = GetNewMock<T>(GetArgs<T>());
            _dicMockCache.Add(typeof(T), objMock);
        }

        public void AddMock<T>(Action<Mock<T>> action, params object[] args)
            where T : class
        {
            Mock<T> objMock = GetNewMock<T>(args);
            if (action != null) action(objMock);

            _dicMockCache.Add(typeof(T), objMock);
        }

        public void AddMock<T>(Action<Mock<T>> action)
            where T : class
        {
            AddMock(action, GetArgs<T>());
        }

        public void ClearMock()
        {
            _dicMockCache.Clear();
        }

        #region [ Private ]

        private object[] GetArgs<T>()
            where T : class
        {
            var lsTypesContructor = GetConstructorParameterTypes(typeof(T));
            return lsTypesContructor.Select(GetCache).ToArray();
        }

        private Mock<T> GetNewMock<T>(params object[] args)
            where T : class
        {
            return new Mock<T>(args) { CallBase = true };
        }

        private object GetCache(Type type)
        {
            try
            {
                if (_dicMockCache.TryGetValue(type, out Mock? mResult))
                    return mResult.Object;

                return _service.GetService(type)!;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private IEnumerable<Type> GetConstructorParameterTypes(Type type)
        {
            ConstructorInfo constructorInfo = type.GetConstructors().FirstOrDefault()!;
            if (constructorInfo == null)
                return new List<Type>();

            List<Type> parameterTypes = new List<Type>();
            foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
                parameterTypes.Add(parameterInfo.ParameterType);

            return parameterTypes;
        }


        #endregion [ Private ]
    }
}
