# Inventory Management System

A WPF-based Inventory Management System built with C# and .NET 8.0, featuring category-based product management, user authentication, and SQL Server database integration.

## Features

- **User Authentication**
  - Admin login with username/password
  - User registration with password hashing (SHA256)
  - Secure user login system

- **Category Management**
  - Electronics
  - Clothing
  - Books
  - Furniture
  - Toys
  - Groceries

- **Inventory Operations**
  - Add new items
  - Update existing items
  - Delete items
  - View all items with DataGrid display

- **User Interface**
  - Modern WPF design with gradient backgrounds
  - Category cards with images
  - Responsive layouts

## Project Structure

```
├── Database/
│   ├── DATABASE.sql          # SQL Server database schema
│   └── DATABASE_REPORT.docx  # Database documentation
│
└── InventoryManagementSystem/
    ├── InventoryManagementSystem.sln
    └── InventoryManagementSystem/
        ├── App.xaml                 # Application entry point
        ├── DatabaseConfig.cs        # Centralized connection string
        ├── MainWindow.xaml          # Admin login page
        ├── user_admin_login.xaml    # User/Admin selection
        ├── user_login.xaml          # User login page
        ├── registration.xaml        # User registration
        ├── categories.xaml          # Category dashboard
        ├── ELECTRONICS.xaml         # Electronics inventory
        ├── clothing.xaml            # Clothing inventory
        ├── books.xaml               # Books inventory
        ├── furniture.xaml           # Furniture inventory
        ├── TOYS.xaml                # Toys inventory
        ├── GROCORIES.xaml           # Groceries inventory
        ├── items show.xaml          # User items view
        ├── user_interface.xaml      # User dashboard
        └── img/                     # UI images
```

## Prerequisites

- **Visual Studio 2022** (or later)
- **.NET 8.0 SDK**
- **SQL Server** (LocalDB or SQL Server Express)

## Database Setup

1. Open SQL Server Management Studio
2. Run the `Database/DATABASE.sql` script to create the database and tables
3. Update the connection string in `DatabaseConfig.cs`:

```csharp
public static string ConnectionString { get; } = 
    "Data Source=YOUR_SERVER_NAME;Initial Catalog=InventoryManagements;Integrated Security=True;Trust Server Certificate=True";
```

## How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/rayyan123571/visual-programming-Project.git
   ```

2. Open `InventoryManagementSystem/InventoryManagementSystem.sln` in Visual Studio

3. Update the database connection string in `DatabaseConfig.cs`

4. Build and run the project (F5)

## Default Credentials

- **Admin Login:**
  - Username: `admin`
  - Password: `aumc`

## Technologies Used

- **Framework:** .NET 8.0 WPF (Windows Presentation Foundation)
- **Language:** C#
- **Database:** Microsoft SQL Server
- **ORM:** ADO.NET with Microsoft.Data.SqlClient
- **Security:** SHA256 password hashing

## Screenshots

The application features:
- Login screens with background images
- Category selection with card-based UI
- Data management with DataGrid controls
- Form-based CRUD operations

## Authors

- Student Project - Visual Programming Course

## License

This project is for educational purposes.
