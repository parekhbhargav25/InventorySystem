# Inventory Management System

A desktop-based CRUD application built with C#, .NET Core, SQL Server, and Entity Framework for managing inventory, orders, and supplier data.

## Features

- **Authentication System**: Secure login with password hashing and role-based access
- **Inventory Management**: Full CRUD operations for inventory items
- **Supplier Management**: Track supplier information and relationships
- **Order Management**: Create and manage orders with order items
- **Search & Filter**: Search inventory by name or SKU
- **Modern UI**: Cross-platform desktop application using Avalonia UI
- **Database Integration**: SQL Server with Entity Framework Core

## Architecture

The solution follows a clean architecture pattern with the following projects:

- **InventorySystem.Domain**: Core business entities and domain models
- **InventorySystem.Data**: Data access layer with Entity Framework DbContext
- **InventorySystem.Services**: Business logic and service implementations
- **InventorySystem.App**: Avalonia UI application (cross-platform desktop)

## Prerequisites

- .NET 9.0 SDK or later
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or VS Code (optional)

## Setup Instructions

### 1. Clone the Repository

```bash
git clone <repository-url>
cd InventorySystem
```

### 2. Database Configuration

The application uses SQL Server with the following default connection string:
```
Server=localhost;Database=InventorySystem;User Id=sa;Password=Your_password123;TrustServerCertificate=True;
```

You can override this by setting the `INVENTORY_DB` environment variable:

```bash
# Windows
set INVENTORY_DB="Server=localhost;Database=InventorySystem;Integrated Security=true;TrustServerCertificate=True;"

# macOS/Linux
export INVENTORY_DB="Server=localhost;Database=InventorySystem;Integrated Security=true;TrustServerCertificate=True;"
```

### 3. Build and Run

```bash
# Restore packages
dotnet restore

# Build the solution
dotnet build

# Run the application
dotnet run --project InventorySystem.App
```

## Default Login Credentials

- **Username**: `admin`
- **Password**: `Admin@123`

## Database Schema

The application automatically creates the database and tables on first run. The schema includes:

- **Users**: Authentication and user management
- **Suppliers**: Supplier information
- **InventoryItems**: Product inventory with SKU, name, quantity, and pricing
- **Orders**: Order headers with customer information
- **OrderItems**: Order line items linking to inventory

## Key Features

### Authentication
- Secure password hashing using PBKDF2 with SHA-256
- Role-based access control (Admin/Clerk)
- Session management

### Inventory Management
- Add, edit, delete inventory items
- Track SKU, name, description, quantity, and unit price
- Link items to suppliers
- Search and filter functionality

### Data Validation
- Unique SKU constraints
- Required field validation
- Foreign key relationships

## Project Structure

```
InventorySystem/
├── InventorySystem.Domain/
│   └── Entities.cs              # Core domain models
├── InventorySystem.Data/
│   └── InventoryDbContext.cs    # EF Core DbContext and configuration
├── InventorySystem.Services/
│   ├── AuthService.cs           # Authentication logic
│   └── InventoryService.cs      # Inventory business logic
└── InventorySystem.App/
    ├── Views/                   # Avalonia UI views
    ├── ViewModels/              # MVVM view models
    ├── AppServices.cs           # Service container
    └── Program.cs               # Application entry point
```

## Technology Stack

- **.NET 9.0**: Cross-platform runtime
- **Avalonia UI**: Cross-platform desktop UI framework
- **Entity Framework Core 9.0**: ORM for data access
- **SQL Server**: Database engine
- **MVVM Pattern**: Clean separation of concerns

## Development

### Adding New Features

1. Define domain models in `InventorySystem.Domain`
2. Add DbContext configurations in `InventorySystem.Data`
3. Implement business logic in `InventorySystem.Services`
4. Create UI views and view models in `InventorySystem.App`

### Database Migrations

The application uses `EnsureCreated()` for simplicity. For production, consider using EF Core migrations:

```bash
# Add migration
dotnet ef migrations add InitialCreate --project InventorySystem.Data

# Update database
dotnet ef database update --project InventorySystem.Data
```

## Reporting

The system includes basic reporting capabilities:
- Inventory item listings
- Supplier information
- Order summaries

## Security Considerations

- Passwords are hashed using industry-standard PBKDF2
- SQL injection protection through Entity Framework
- Input validation on all user inputs
- Secure connection strings (avoid hardcoding credentials)

## Troubleshooting

### Common Issues

1. **Database Connection Failed**
   - Verify SQL Server is running
   - Check connection string format
   - Ensure database server is accessible

2. **Login Failed**
   - Use default credentials: admin/Admin@123
   - Check if database was properly seeded

3. **Build Errors**
   - Ensure .NET 9.0 SDK is installed
   - Run `dotnet restore` to restore packages

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support and questions, please open an issue in the repository or contact the development team.
