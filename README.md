# Staff Management Project

## Objective

**Staff Management** application using **ASP.NET Web API** to manage staff information, provide advanced search functionality, and export search results to Excel or PDF. The application must include comprehensive testing.

## Functional Requirements

### Staff Management

Implement CRUD operations for staff with the following attributes:

- **Staff ID**: String (max length: 8 characters)
- **Full Name**: String (max length: 100 characters)
- **Birthday**: Date
- **Gender**: Integer (1 = Male, 2 = Female)

**Operations**:

- **Add**: Create a new staff record.
- **Edit**: Update existing staff details.
- **Delete**: Remove a staff record.
- **Search**: Retrieve staff by ID or other basic criteria.

### Advanced Search

Create an advanced search form allowing filtering by:

- **Staff ID**
- **Gender**
- **Birthday Range** (From date, To date)

### Reports

- Export advanced search results to:
  - **Excel** file
  - **PDF** file

## Technical Requirements

- **Backend**: Use **ASP.NET Web API** with **C#**.
- **Database**: Choose a suitable database (e.g., SQL Server, SQLite) for storing staff data.
- **Input Validation**:
  - Ensure Staff ID is unique and max 8 characters.
  - Full Name max 100 characters.
  - Valid date for Birthday.
  - Gender must be 1 (Male) or 2 (Female).
- **API Design**:
  - Follow RESTful principles.
  - Use appropriate HTTP methods (GET, POST, PUT, DELETE).
  - Return standard HTTP status codes.
- **Export Functionality**:
  - Use libraries like **EPPlus** for Excel or **iTextSharp**/**PdfSharp** for PDF generation.
- **Error Handling**:
  - Implement proper exception handling and meaningful error messages.
- **Code Quality**:
  - Follow clean code principles.
  - Use dependency injection for service management.
  - Implement a repository pattern for data access.

## Testing Requirements

### Unit Tests

- Test business logic and service layer (e.g., validation, data processing).
- Use a testing framework like **xUnit**, **NUnit**, or **MSTest**.
- Mock dependencies using **Moq** or similar libraries.
- Aim for at least 80% code coverage.

### Integration Tests

- Test API endpoints with the database.
- Verify data persistence and retrieval.
- Use an in-memory database (e.g., SQLite) or a test database for isolation.

### End-to-End Tests

- Test the full application flow (API calls, database, and export functionality).
- Use tools like **Postman**, **SpecFlow**, or **Selenium** for API testing.
- Validate advanced search and export functionality.

## Deliverables

- Source code for the ASP.NET Web API project.
- Unit, integration, and end-to-end test suites.
- A brief README explaining:
  - Project setup instructions.
  - How to run the application and tests.
  - Dependencies and libraries used.
- Sample export files (Excel and PDF) generated from the advanced search.

## Optional Enhancements

- Add authentication/authorization (e.g., JWT-based) to secure API endpoints.
- Implement pagination for search results.
- Add logging using a library like **Serilog** or **NLog**.

## Tools and Libraries

- **ASP.NET Core**: For Web API development.
- **Entity Framework Core**: For database operations.
- **EPPlus** or **ClosedXML**: For Excel export.
- **iTextSharp** or **PdfSharp**: For PDF export.
- **xUnit/NUnit/MSTest**: For unit testing.
- **Moq**: For mocking dependencies.
- **Swagger**: For API documentation.

## Acceptance Criteria

- All CRUD operations work as expected with proper validation.
- Advanced search filters staff by ID, gender, and birthday range.
- Search results can be exported to Excel and PDF without errors.
- Tests (unit, integration, end-to-end) pass successfully.
- Code is well-documented and follows clean code practices.
- Application is deployable and runs without errors.