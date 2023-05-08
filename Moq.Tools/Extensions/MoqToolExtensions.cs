using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moq.Tools.Extensions
{
    public static class MoqToolExtensions
    {

        public static IConfiguration ToConfiguration(this IDictionary<string, string> dicValues)
        {
            return new ConfigurationBuilder()
                            .AddInMemoryCollection(dicValues!)
                                .Build();
        }

    }
}
