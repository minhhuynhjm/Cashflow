# CashflowAI Architecture Rules

## Layer Architecture

### 1. Presentation Layer (Web)

- Controllers should NEVER directly access repositories
- Controllers should ONLY interact with services through interfaces
- Controllers should handle HTTP concerns only (routing, request/response, status codes)
- ViewModels should be used for data transfer between controllers and views
- API Controllers should return consistent response formats

### 2. Service Layer

- Services implement business logic and orchestration
- Services should be stateless
- Services should handle transaction management
- Services should validate business rules
- Services should coordinate between multiple repositories if needed
- Services should handle error cases and business exceptions

### 3. Repository Layer

- Repositories handle data access only
- Repositories should be generic and reusable
- Repositories should not contain business logic
- Repositories should handle basic CRUD operations
- Repositories should use Entity Framework Core for data access

## Dependency Injection Rules

1. **Constructor Injection**

   - Use constructor injection for required dependencies
   - Mark optional dependencies with `[FromServices]` attribute
   - Register all dependencies in `Program.cs` or `Startup.cs`

2. **Interface-based Injection**
   - Always inject interfaces, not concrete implementations
   - Use `IUnitOfWork` for transaction management
   - Use specific interfaces for each service (e.g., `ITransactionService`)

## Service Implementation Rules

1. **Service Interface Definition**

   ```csharp
   public interface ITransactionService
   {
       Task<IEnumerable<TransactionDto>> GetTransactionsAsync(int userId);
       Task<TransactionDto> GetTransactionByIdAsync(int id);
       Task<TransactionDto> CreateTransactionAsync(TransactionCreateDto dto);
       Task UpdateTransactionAsync(int id, TransactionUpdateDto dto);
       Task DeleteTransactionAsync(int id);
   }
   ```

2. **Service Implementation**

   ```csharp
   public class TransactionService : ITransactionService
   {
       private readonly IUnitOfWork _unitOfWork;
       private readonly IMapper _mapper;

       public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
       {
           _unitOfWork = unitOfWork;
           _mapper = mapper;
       }

       // Implementation methods
   }
   ```

## Controller Rules

1. **API Controller Structure**

   ```csharp
   [ApiController]
   [Route("api/[controller]")]
   public class TransactionsController : ControllerBase
   {
       private readonly ITransactionService _transactionService;
       private readonly ILogger<TransactionsController> _logger;

       public TransactionsController(
           ITransactionService transactionService,
           ILogger<TransactionsController> logger)
       {
           _transactionService = transactionService;
           _logger = logger;
       }

       // Action methods
   }
   ```

2. **MVC Controller Structure**

   ```csharp
   public class HomeController : Controller
   {
       private readonly ITransactionService _transactionService;
       private readonly ICategoryService _categoryService;
       private readonly ILogger<HomeController> _logger;

       public HomeController(
           ITransactionService transactionService,
           ICategoryService categoryService,
           ILogger<HomeController> logger)
       {
           _transactionService = transactionService;
           _categoryService = categoryService;
           _logger = logger;
       }

       // Action methods
   }
   ```

## Data Transfer Objects (DTOs)

1. **DTO Structure**

   - Create separate DTOs for different operations (Create, Update, Response)
   - Use AutoMapper for object-to-object mapping
   - Include validation attributes on DTOs

2. **DTO Naming Convention**
   - `{Entity}Dto` for basic DTOs
   - `{Entity}CreateDto` for creation
   - `{Entity}UpdateDto` for updates
   - `{Entity}ResponseDto` for responses

## Error Handling

1. **Service Layer**

   - Throw domain-specific exceptions
   - Include meaningful error messages
   - Use custom exception types for different error cases

2. **Controller Layer**
   - Use try-catch blocks to handle exceptions
   - Return appropriate HTTP status codes
   - Log exceptions using ILogger
   - Return consistent error response format

## Response Format

1. **API Responses**

   ```json
   {
     "success": true,
     "data": { ... },
     "message": "Operation successful"
   }
   ```

2. **Error Responses**
   ```json
   {
     "success": false,
     "message": "Error message",
     "errors": [ ... ]
   }
   ```

## Best Practices

1. **Async/Await**

   - Use async/await consistently throughout the application
   - Avoid using `.Result` or `.Wait()`
   - Use `Task<T>` for all asynchronous operations

2. **Logging**

   - Use structured logging
   - Include correlation IDs
   - Log at appropriate levels (Debug, Info, Warning, Error)

3. **Validation**

   - Use FluentValidation for complex validation rules
   - Use Data Annotations for simple validation
   - Validate input at both controller and service levels

4. **Security**

   - Use proper authentication and authorization
   - Validate user permissions in services
   - Sanitize all user input
   - Use HTTPS
   - Implement proper CORS policies

5. **Performance**
   - Use appropriate caching strategies
   - Implement pagination for large datasets
   - Use async/await for I/O operations
   - Optimize database queries
   - Use proper indexing

## Testing

1. **Unit Tests**

   - Test services in isolation
   - Mock repositories and external dependencies
   - Test business logic thoroughly
   - Use xUnit or NUnit

2. **Integration Tests**

   - Test API endpoints
   - Test database operations
   - Test authentication and authorization
   - Use TestServer for API testing

3. **Test Naming Convention**
   - `{MethodName}_{Scenario}_{ExpectedResult}`
   - Use descriptive test names
   - Follow AAA pattern (Arrange, Act, Assert)
