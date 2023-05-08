using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq.Tools.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Moq.Tools.Common
{
    public abstract class MoqTestBase: IMoqDependecy
    {
        private IConfiguration _configuration;
        private IHost _host;
        private IMoqDependecy _moqDependecy;

        public abstract void ConfigureServices(IServiceCollection services);

        public abstract IDictionary<string, string> InMemoryConfiguration { get; }

        public abstract IConfiguration Configuration { get; }

        [OneTimeSetUp]
        protected void Setup()
        {
            if (InMemoryConfiguration is null && Configuration is null)
                throw new ArgumentException($"${nameof(InMemoryConfiguration)} And ${nameof(Configuration)}");

            _configuration = Configuration ?? new ConfigurationBuilder()
                                                    .AddInMemoryCollection(InMemoryConfiguration!)
                                                        .Build();

            _host = Host.CreateDefaultBuilder()
                    .ConfigureAppConfiguration(o => o.AddConfiguration(_configuration))
                    .ConfigureServices(s =>
                    {
                        s.AddTransient<IMoqDependecy, MoqDependecy>();
                        ConfigureServices(s);
                    })
                    .Build();

            _moqDependecy = _host.Services.GetRequiredService<IMoqDependecy>();
        }

        public T Get<T>() where T : notnull
        {
            return _moqDependecy.Get<T>();
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
