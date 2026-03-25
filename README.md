# EBCaseStudy Procurement Application

This is a web application built for a fictional building supplies company to help their procurement team browse products, manage orders, and analyze pricing data.

## Features

*   **Product Catalog**: Browse products from the company's centralized API. Products can be filtered by category and sorted by name or category.
*   **Price History**: View a product's price history over the last 12 months with an interactive chart on the product detail page.
*   **Shopping Cart**: Add products to a shopping cart before placing an order.
*   **Order Management**:
    *   Place orders with products from the catalog. Prices are captured at the time of order.
    *   View a list of all past and present orders.
    *   Filter orders by status (`New`, `Approved`, `Rejected`).
*   **Order Review Workflow**:
    *   New orders can be reviewed by a foreman.
    *   The foreman can edit quantities or remove items from an order.
    *   The foreman can approve or reject orders.
*   **Reordering**: Previously approved orders can be easily reordered with a single click, creating a new cart with the same items at current prices.

## Technical Stack

*   **Backend**: ASP.NET Core MVC (.NET 8)
*   **Database**: SQLite with Entity Framework Core
*   **Frontend**: Razor Views, Bootstrap, Chart.js
*   **API Communication**: `HttpClient` is used to communicate with the external REST API for product and pricing data.

## How to Run the Application

1.  **Prerequisites**:
    *   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

2.  **Clone the repository**:
    ```bash
    git clone https://github.com/bkzarok/EBCaseStudy.git
    cd EBCaseStudy
    ```

3.  **Run the application**:
    *   Navigate to the `EBCaseStudy` directory in your terminal.
    *   Execute the command:
        ```bash
        dotnet run
        ```
    *   The application will build, and the terminal will display the URL where the application is running (e.g., `https://localhost:7123`).

4.  **Access the application**:
    *   Open a web browser and navigate to the URL from the previous step.

## Database

The application uses a SQLite database (`EBCaseStudy.db`) which is created automatically in the `EBCaseStudy` project directory when the application is run for the first time. The database schema is managed using EF Core migrations.
