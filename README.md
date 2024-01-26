# DotnetMoqHelper
This tool is an ideal solution for accelerating test development, offering a range of intuitive and powerful functionalities. With methods like ConfigureServices, AfterSetup, DepGet, AddMock, and ClearMock, MoqTestBase simplifies service configuration, test initialization, dependency management, and mock implementation. Aimed at making the testing process more agile and less prone to errors, this project is perfect for projects seeking efficiency and precision in unit testing. Whether it's to set up dynamic test services, manage mocks flexibly, or easily access injected dependencies, MoqTestBase is the ideal choice for developers looking to optimize their test workflow in C# environments using Moq and NUnit.

# Key Features

* Automated Testing Tools: Simplify your testing process with our automated tools, designed for modern Agile Software Testing practices.
* Efficient Testing Practices: With MoqTestBase, experience a boost in your testing efficiency, saving time and resources.
* Mock Management: Easy-to-use functionality for creating and managing mocks, enhancing the flexibility and robustness of your tests.
* Dependency Injection Testing: Test components reliant on dependency injection effortlessly, ensuring comprehensive coverage.

# Getting Started

The `MoqTestBase` abstract class provides a common base for unit tests that use the Moq framework.

### ConfigureServices

This abstract method is used to configure additional services that will be used in your test. The implementation should add the required services to the service collection passed as a parameter.

### AfterSetup

This abstract method is called after the class setup has been completed. It is an entry point where you can perform any additional actions necessary to initialize your test.

### Configuration

Abstract property that should return the configuration required for your test.

### DepGet

Generic method that retrieves a service registered in the service collection. It is used to retrieve instances of dependencies that have been registered on `Mock Cache objects` or  `Dependency Injection`

### AddMock

Method that adds a Moq instance of a class to the service container. This allows you to substitute a real dependency with a fake dependency in your test. There are three overloads for this method, allowing you to configure the behavior of the added Moq object.

### ClearMock

Method that removes all Moq dependencies added by the `AddMock` method. This allows you to clear the service container and start with a clean slate for the next test.

## Usage

To use this class, you should create a test class that inherits from `MoqTestBase`. This test class should implement the `ConfigureServices` and `Configuration` methods, as well as the abstract `AfterSetup` method if necessary.

Example usage:

```csharp
public class MyTest : MoqTestBase
{
    private IDictionary<string, string> _dicConfiguration = new Dictionary<string, string>{ {"Key", "Value"} };
    public override IConfiguration Configuration => _dicConfiguration.ToConfiguration();

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IMyDependency, MyDependency>();
        services.AddTransient<IMyDependency2, MyDependency2>();
    }

    protected override void AfterSetup()
    {
        // Additional code for test initialization
        AddMock<IHttpContextAccessor>();
    }

    [Test]
    public void MyTestShouldExecuteSuccessfully()
    {
        // Test using the dependencies registered in the service container
        ClearMock();
        
        AddMock<IMyDependency>();
        AddMock<IMyDependency2>(c =>
        {
            c.Setup(s => s.Change()).Returns(() => "");
        });
        
        var dep = DepGet<IMyDependency>();
        await dep.ExecuteAsync();
        
        Assert.Pass();
    }
}
```
