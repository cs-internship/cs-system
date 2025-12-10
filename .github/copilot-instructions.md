# GitHub Copilot Instructions

## 1. Core Principles

As an expert AI assistant for this project, your actions must be guided by these core principles:

*   **Proactive Research:** Never assume. Always begin UI-related tasks by consulting the knowledge base. Your primary directive is to use the provided tools to understand the `bitplatform` ecosystem *before* writing any code.
*   **Structured Planning:** Do not implement changes impulsively. You must first analyze the request, investigate the codebase, and formulate a detailed, step-by-step plan. This plan is your blueprint for success.
*   **Rigorous Verification:** After every implementation phase, you must verify your work. This includes, at a minimum, ensuring the project builds successfully. You are responsible for identifying and fixing errors your changes introduce.
*   **Strict Adherence to Conventions:** The project's quality and maintainability depend on consistency. You must strictly follow all established coding conventions and best practices outlined in this document.

## 2. Technology Stack

You will be working with the following key technologies:

*   **C# 13.0**
*   **ASP.NET Core 9.0**
*   **Blazor**: Component-based web UI framework
*   **.NET MAUI Blazor Hybrid**: Cross-platform app development
*   **ASP.NET Core Identity**: Authentication and authorization
*   **Entity Framework Core**: Data access
*   **SignalR**: Real-time communication
*   **Hangfire**: Background job processing
*   **OData**: Advanced querying capabilities
*   **Bit.BlazorUI**: The primary UI component library
*   **Microsoft.Extensions.AI**: AI integration
*   **TypeScript**: Type-safe JavaScript development
*   **SCSS**: Advanced CSS preprocessing
*   **Mapperly**: High-performance object mapping
*   **SQL Server**: Primary database

## 3. Project Structure

The solution is organized into the following projects. Understand their roles to locate and modify the correct files.

*   **CrystaLearn.Server.Api**: Houses API controllers, mappers, the `DbContext`, EF Core migrations, email templates, action filters, SignalR hubs, and server-specific configuration.
*   **CrystaLearn.Server.Web**: The application's default startup project and entry point. It hosts `App.razor` and configures Blazor Server and server-side rendering (SSR).
*   **CrystaLearn.Server.Shared**: (Also known as Aspire's ServiceDefaults) Contains common code shared between the `Server.Api` and `Server.Web` projects.
*   **CrystaLearn.Shared**: Contains shared DTOs, enums, custom exceptions, shared services, and `.resx` resource files.
*   **CrystaLearn.Test**: Contains integration tests, identity tests, and end-to-end application tests.
*   **CrystaLearn.Core.Test**: Contains unit tests for core services (GitHub, Azure DevOps, TextUtil).
*   **CrystaLearn.Client.Core**: The heart of the client application. Contains all shared Blazor components, pages, layouts, client-side services, and the primary `App.ts` and `App.scss` files.
*   **CrystaLearn.Client.Web**: The Blazor WebAssembly (WASM) standalone project.
*   **CrystaLearn.Client.Maui**: The .NET MAUI Blazor Hybrid project for native mobile and desktop apps.
*   **CrystaLearn.Client.Windows**: The Windows Forms Blazor Hybrid project.
*   **CrystaLearn.Console**: Console application for administrative tasks and batch operations.
*   **CrystaLearn.Core**: Core business logic, domain models, and services shared across the application.

## 4. Available Tooling

-   **DeepWiki**: Provides access to an extensive knowledge base for the `bitfoundation/bitplatform` and `riok/mapperly` repositories.
-   **Website Fetcher**: Gathers information from URLs provided by the user. Prefer the built-in `fetch` tool if available; otherwise, use the `read-website-fast` tool.

## 5. Mandatory Workflow

You **MUST** follow this workflow for every request. Do not deviate.

### Step 1: Deconstruct the Request
Carefully analyze the user's prompt. Identify the core objectives, whether it is a question, a code modification, or a review.

### Step 2: Information Gathering & Codebase Investigation
Before writing code, investigate thoroughly.
*   If the user provides a **URL**, you **MUST** use the `fetch` tool to retrieve its content.
*   If the user provides a **git commit id/hash**, you **MUST** run the `git --no-pager show <commit-id>` command to retrieve its details.
*   If the user taked about current changes in the codebase, you **MUST** run the `git --no-pager diff` and `git --no-pager diff --staged` commands to see the differences.
*   For UI-related tasks, you **MUST** first ask `DeepWiki`: *"What features does BitPlatform offer to help me complete this task? [USER'S ORIGINAL REQUEST]"*
*   For anything related to `Bit.BlazorUI`, `bit Bswup`, `bit Butil`, `bit Besql`, or the bit project template, you **MUST** use the `DeepWiki_ask_question` tool with repository `bitfoundation/bitplatform` to find relevant information.
*   For mapper/mapping entity/dto related tasks, you **MUST** use the `DeepWiki_ask_question` tool with repository `riok/mapperly` to find correct implementation and usage patterns focusing on its static classes and extension methods approach.

### Step 3: Formulate a Detailed Plan
Create a comprehensive, step-by-step plan. This plan must outline:
*   The files you will create or modify.
*   The specific changes you will make (e.g., "Add a `BitButton` to `MyComponent.razor`").
*   A brief justification for each change, referencing your research from DeepWiki and your analysis of the codebase.

### Step 4: Execute the Plan
Implement the changes exactly as described in your plan. Adhere strictly to the **Coding Conventions & Best Practices** during this phase.

### Step 5: Verify, Test, and Refine
After applying changes, you **MUST** verify the integrity of the application.
1.  **Build the Project**: Run a build to ensure your changes have not introduced compilation errors. This is mandatory.
2.  **Fix Build Errors**: If the build fails, you must fix it. For errors related to the `bitplatform`, you **MUST** go back to Step 3 and use `DeepWiki` to find the correct implementation.
3.  **Iterate**: Continue this cycle of implementation and verification until all requirements are met and the solution is in a stable, buildable state.

## 6. Behavioral Directives

*   **Be Decisive**: Do not ask for permission to proceed or for a review of your plan. Directly state your plan and proceed with the implementation.
*   **Execute Commands Individually**: **Never** chain CLI commands with `&&`. Execute each command in a separate step.

## 7. Critical Command Reference

-   **Build the project**: Run `dotnet build` in CrystaLearn.Server.Web project root directory.
-   **Run the project**: Run `dotnet run` in CrystaLearn.Server.Web project root directory.
-   **Run tests**: Run `dotnet test` in the solution root directory or in specific test project directories (CrystaLearn.Test or CrystaLearn.Core.Test).
-   **Add new migrations**: Run `dotnet ef migrations add <MigrationName> --verbose` in CrystaLearn.Server.Api project root directory.
-   **Generate Resx C# code**: Run `dotnet build -t:PrepareResources` in CrystaLearn.Shared project root directory.
-   **Restore packages**: Run `dotnet restore` in the solution root directory.

## 8. Coding Conventions & Best Practices

01. **Follow Project Structure**: Adhere to the defined project layout for all new files and code.
02. **Prioritize Bit.BlazorUI Components**: You **MUST** use components from the `Bit.BlazorUI` library (e.g., `BitButton`, `BitTextField`, `BitChart`) instead of generic HTML elements to ensure UI consistency and leverage built-in features.
03. **Embrace Nullable Reference Types**: All new code must be nullable-aware.
04. **Use Dependency Injection**: Use the `[AutoInject]` attribute in components. For other classes, use constructor injection.
05. **Implement Structured Logging**: Use structured logging for clear, queryable application logs.
06. **Adhere to Security Best Practices**: Implement robust authentication and authorization patterns. Never commit secrets or sensitive data.
07. **Use Async Programming**: Employ `async/await` for all I/O-bound operations to prevent blocking.
08. **Write Modern C#**: Utilize the latest C# features, including implicit and global using statements, file-scoped namespaces, and records.
09. **Respect .editorconfig**: Adhere to the `.editorconfig` settings for consistent code style. Use 4 spaces for indentation, file-scoped namespaces, and PascalCase for constants.
10. **Use Code-Behind Files**: Place component logic in `.razor.cs` files instead of `@code` blocks.
11. **Use Scoped SCSS Files**: Place component styles in `.razor.scss` files for CSS isolation.
12. **Style Bit.BlazorUI Components Correctly**: Use the `::deep` selector in your `.scss` files to style `Bit.BlazorUI` components.
13. **Use Theme Colors**: You **MUST** use `BitColor` theme variables in C#, Razor, and SCSS files (`_bit-css-variables.scss`) to support dark/light modes. Do not use hardcoded colors.
14. **Use Enhanced Lifecycle Methods**: In components inheriting from `AppComponentBase` or pages inheriting from `AppPageBase`, you **MUST** use `OnInitAsync`, `OnParamsSetAsync`, and `OnAfterFirstRenderAsync`.
15. **WrapHandled**: Use `WrapHandled` for event handlers in razor files to prevent unhandled exceptions.
Example 1: `OnClick="WrapHandled(MyMethod)"` instead of `OnClick="MyMethod"`.
Example 2: `OnClick="WrapHandled(async () => await MyMethod())"` instead of `OnClick="async () => await MyMethod()"`.
16. **Use OData Query Options**: Leverage `[EnableQuery]` and `ODataQueryOptions` for efficient data filtering and pagination.
17. **Follow Mapperly Conventions**: Use partial static classes and extensions methods with Mapperly for high-performance object mapping.
18. **Handle Concurrency**: Always use `ConcurrencyStamp` for optimistic concurrency control in update and delete operations.
19. **Write Tests**: Add tests for new functionality in the appropriate test project (CrystaLearn.Test for integration tests, CrystaLearn.Core.Test for unit tests).
20. **Document Your Code**: Add XML documentation comments for public APIs, complex algorithms, and non-obvious implementations.

## Instructions for adding new model/entity to ef-core DbContext / Database
Create the Entity Model
- **Location**: `CrystaLearn.Server.Api's Models folder`
- **Requirements**:
  - Include `Id`, `ConcurrencyStamp` properties
  - Add appropriate navigation properties
  - Use nullable reference types
  - Add data annotations as needed

Create the EntityTypeConfiguration
- **Location**: `CrystaLearn.Server.Api's Data/Configuration folder`
  - Implement `IEntityTypeConfiguration<{EntityName}>`
  - Configure unique indexes, relationships
  - Add `DbSet<{EntityName}>` to AppDbContext
  - Add ef-core migration

## Instructions for adding new DTO and Mapper
Create the DTO
- **Location**: `CrystaLearn.Shared's Dtos folder`
- **Requirements**:
  - Use `[DtoResourceType(typeof(AppStrings))]` attribute
  - Add validation attributes: `[Required]`, `[MaxLength]`, `[Display]`
  - Use `nameof(AppStrings.PropertyName)` for error messages and display names
  - Include `Id`, `ConcurrencyStamp` properties
  - Add calculated properties if needed (e.g., `ProductsCount`)
  - Add `[JsonSerializable(typeof({DtoName}))]` to AppJsonContext.cs

Create the Mapper
- **Location**: `CrystaLearn.Server.Api's Mappers folders`
- **Requirements**:
  - Use `[Mapper]` attribute from Mapperly
  - Create `static partial class {MapperName}Mapper`
  - Add projection method: `public static partial IQueryable<{DtoName}> Project(this IQueryable<{EntityName}> query);`
  - Add mapping methods: `Map()`, `Patch()` for CRUD operations
  - Use `[MapProperty]` for complex mappings if needed

#### Instructions for creating Strongly Typed Http Client Wrapper to Call Backend API
- **Location**: `CrystaLearn.Shared project's Controllers folder`
- **Requirements**:
  - Inherit from `IAppController`
  - Add `[Route("api/[controller]/[action]/")]` attribute
  - Add `[AuthorizedApi]` if authentication required
  - Always Use `CancellationToken` parameters
  - The return type should be `Task<T>` or Task<T> where T is JSON Serializable type like DTO, int, or List<Dto>
  - If Backend API's action returns `IQueryable<T>`, use `Task<List<T>>` as return type with `=> default!`
  - If Backend API's action returns `IActionResult`, use `Produces<T>` attribute to specify the response type with `=> default!`
  - If Backend API accepts ODataQueryOptions, simply ignore it

#### Instructions to create Backend API Controllers
- **Location**: `CrystaLearn.Server.Api's Controllers folder`
- **Requirements**:
  - Inherit from `AppControllerBase`
  - Implement the corresponding IAppController interface
  - Add appropriate authorization attributes
  - Use `[EnableQuery]` for GET endpoints with OData support
  - Implement validation in private methods
  - Use `Project()` for querying and mapping
  - Handle resource not found scenarios using ResourceNotFoundException.

## 9. Testing Guidelines

### Test Project Structure
- **CrystaLearn.Test**: Integration tests, identity tests, and end-to-end tests
- **CrystaLearn.Core.Test**: Unit tests for core services and utilities (GitHub, Azure DevOps, TextUtil)

### Writing Tests
- Use xUnit as the testing framework
- Follow AAA pattern: Arrange, Act, Assert
- Use descriptive test method names that explain what is being tested
- Mock external dependencies appropriately
- Ensure tests are isolated and can run independently
- Use `AppTestServer.cs` for integration tests that require the full application context

### Running Tests
- Run all tests: `dotnet test` from solution root
- Run specific test project: `dotnet test <path-to-test-project>`
- Use `.runsettings` file for test configuration when available

## 10. Documentation Standards

### Code Documentation
- Add XML documentation comments (`///`) for all public APIs
- Document complex algorithms with inline comments
- Keep comments up-to-date with code changes
- Use clear, concise language

### Markdown Documentation
- Follow the structure in `/docs` directory:
  - `/docs/technical` - Technical documentation (architecture, setup, API)
  - `/docs/standards` - Contribution and workflow standards
  - `/docs/roadmap` - Feature roadmaps and planning
- Use relative links for internal documentation references
- Keep README.md updated with major changes

## 11. Contribution Workflow

Before starting work, review these documents:
- [Contribution Guidelines](/docs/standards/contribution.md) - Fork, branch, commit, and PR workflow
- [Working on Issues](/docs/standards/working-on-issues.md) - How to claim and complete issues
- [Setup Development Environment](/docs/technical/setup-dev-environment.md) - Local environment setup

### Branch Strategy
- Base branch: `develop`
- Feature branches: `feature/<feature-name>`
- Bug fix branches: `fix/<bug-name>`
- Always create PRs against `develop` branch

### Commit Messages
- Use descriptive commit messages
- Format: `<Type>: <Short description>`
- Examples: `feat: Add user profile page`, `fix: Resolve authentication issue`

## 12. Security and Secrets Management

- **NEVER** commit secrets, connection strings, access tokens, or sensitive data to source control
- Use `dotnet user-secrets` for local development secrets
- Required secrets for development:
  - `ConnectionStrings:PostgresConnectionString` - Database connection string
  - `GitHub:GitHubAccessToken` - GitHub API access token
  - `AzureDevOps:PersonalAccessToken` - Azure DevOps PAT
- Initialize secrets in these projects:
  - `src/Server/CrystaLearn.Server.Web`
  - `src/Console/CrystaLearn.Console`
  - `src/Core/CrystaLearn.Core.Test`
- See [Setup Development Environment](/docs/technical/setup-dev-environment.md) for detailed instructions