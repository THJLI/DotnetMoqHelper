using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotnetMoqHelper.Interfaces;
using NUnit.Framework;
using System;
using Moq;

namespace DotnetMoqHelper.Common
{
    public abstract class MoqTestBase
    {
        private IHost _host;
        private IMoqDependecy _moqDependecy;

        public abstract void ConfigureServices(IServiceCollection services);

        protected abstract void AfterSetup();
        
        public abstract IConfiguration Configuration { get; }

        [OneTimeSetUp]
        protected void Setup()
        {
            if (Configuration is NotImplementedException || Configuration is null)
                throw new ArgumentException(nameof(Configuration));
            
            _host = Host.CreateDefaultBuilder()
                    .ConfigureAppConfiguration(o => o.AddConfiguration(Configuration))
                    .ConfigureServices(s =>
                    {
                        s.AddTransient<IMoqDependecy, MoqDependecy>();
                        ConfigureServices(s);
                    })
                    .Build();

            _moqDependecy = _host.Services.GetRequiredService<IMoqDependecy>();
            AfterSetup();
        }

        public T DepGet<T>() where T : notnull
        {
            return _moqDependecy.Get<T>();
        }

        public virtual void AddMock<T>() where T : class
        {
            _moqDependecy.AddMock<T>(); 
        }

        public virtual void AddMock<T>(Action<Mock<T>> action, params object[] args) where T : class
        {
            _moqDependecy.AddMock<T>(action, args);
        }

        public virtual void AddMock<T>(Action<Mock<T>> action) where T : class
        {
            _moqDependecy.AddMock(action);
        }

        public virtual void ClearMock()
        {
            _moqDependecy.ClearMock();
        }

    }
}
