# Security-Incident-Response-System
Real-time SOC Dashboard and Incident Management System using ASP.NET Core, SignalR, and SQL Server

## Architecture

The application is built with a layered architecture:

1. **Models** (`Models/`) - View models that structure data for the UI
2. **Services** (`Services/`) - Business logic and database access
3. **Controllers** (`Controllers/`) - Handle HTTP requests and responses
4. **Views** (`Views/`) - Razor templates that render HTML
5. **Database** (`Database/`) - SQL scripts for schema and seed data

### Data Flow
User → Browser Request → Controller → Service → Database → SQL Query → Results → View → HTML Response → Browser

## Current Progress

- [x] Database schema and seed data
- [x] View models and service layer
- [x] Dashboard controller and view
- [x] Create incident functionality
- [x] Real-time updates with SignalR
- [ ] AI analysis (future phase)
