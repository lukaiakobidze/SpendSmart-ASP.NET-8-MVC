# Simple Expense Tracker

A basic Expense Tracker application built with **ASP.NET MVC**, using **Entity Framework** and **SQL Database** for data persistence. This application allows users to efficiently manage their expenses with essential CRUD operations.

## Features

- **Personalized expense tracking**: Working Authorization using ASP.NET CORE.
- **Add Expenses**: Record new expenses with details like amount, category, date and description.
- **Edit & Delete Expenses**: Update or remove existing expense entries.
- **View Expenses by Month**: Filter and view expenses based on selected months.
- **View Expenses by Category**: Organize and analyze expenses based on different categories.

## Technologies Used

- **ASP.NET MVC**
- **Entity Framework (EF Core)**
- **SQL Database**
- **Bootstrap (for styling)**

## Setup Instructions

1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/your-repo-name.git
   ```
2. Navigate to the project folder and open it in Visual Studio.
3. Update the database connection string in `appsettings.json`.
4. Apply database migrations:
   ```sh
   dotnet ef database update
   ```
5. Run the application using Visual Studio or the .NET CLI:
   ```sh
   dotnet run
   ```
6. It should automatically open your browser and the home page, but you can and navigate to *https\://localhost:7090/* to start tracking expenses.

## Future Improvements

- ~~User authentication for personalized expense tracking.~~
- Export expenses to CSV/PDF.
- Add charts and graphs for better expense visualization.

## License

This project is open-source under the **MIT License**.

---

Feel free to contribute or report any issues!

