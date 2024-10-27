1. How long did you spend on the coding test? What would you add to your solution if you had more time? If you didn't spend much time on the coding test, use this as an opportunity to explain what you would add.
>I spent approximately 4 hours on this technical test. During this time, I focused on meeting the main requirements, such as connecting to the API, filtering by technology, implementing dependency injection, logging, and ensuring both unit and integration test coverage
>If I had more time, I would consider the following improvements:
>* Advanced Error Handling: I would implement custom exceptions to handle potential errors in more detail.
>* Configuration Improvements and environment-specific configuration: Managing sensitive information (e.g., credentials) using secure vaults instead of appsettings.json.
>* Web Interface: I would extend the application to offer a web interface with Angular, providing a more visual and dynamic user experience.
>* Optimized Testing: I would add tests for other potential API error scenarios, such as timeouts or authentication issues.

2. What was the most useful feature added to the latest version of your chosen language? Please include a snippet of code that shows how you've used it.
> One of the most useful features in .NET 8.0 is the  introduction of the `required` attribute for property initializers. This feature helps enforce mandatory values, ensuring complete and secure dependency setup in each class.
>In this project, I used dependency injection to initialize services like `ICuttingMachineService` and `ILogger`, which allowed for a more modular and testable architecture.
>Example of using the required attribute: 
```[csharp]
public class CuttingMachineService : ICuttingMachineService
{
    private readonly HttpClient _client;
    private required ILogger<CuttingMachineService> _logger;

    public CuttingMachineService(HttpClient client, ILogger<CuttingMachineService> logger)
    {
        _client = client;
        _logger = logger;
    }
}
```
>The required attribute applied to the logger ensures that all instances of `CuttingMachineService` are created with an `ILogger<CuttingMachineService>` dependency, helping to maintain control and reliability in the service.

3. How would you track down a performance issue in production? Have you ever had to do this?
>To track down a performance issue in production, I would start by using monitoring tools such as Application Insights, Serilog for event and error logging, or any other tool that may help track request response times, identify bottlenecks, and monitor resource usage in real-time.

>The process might involve these steps:
>* Log Review: I would examine logs to find any unexpected exceptions or warnings.
>* Resource Monitoring: I would use CPU, memory, and network usage metrics to detect any unusual increases.
>* Request Profiling: I would analyze the request routes showing the highest response times.

>Yes, I have had to track down performance issues in previous projects. In one case, identifying a specific endpoint as the source of issues allowed me to optimize queries and significantly improve response times.

4. How would you improve the Lantek API that you just used?
>Some potential improvements I would suggest for the Lantek API are:
>* Pagination: If the number of machines grows, it would be helpful to add pagination to avoid large responses, thus improving performance.
>* Additional Filters: Adding additional filters, such as by manufacturer or creation date, would allow users to obtain more specific data without requiring client-side filtering.
