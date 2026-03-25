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

## Design Decisions

### Architecture
The application is built upon the existing ASP.NET Core MVC project structure. A dedicated `ApiService` was created to encapsulate all communication with the external product API, separating external data concerns from the main application logic. This makes the application more modular and easier to maintain.

### Data Model
*   **Local Database**: A local SQLite database managed by Entity Framework Core was chosen for its simplicity and portability. It requires no external database server setup.
*   **Order Data Ownership**: The application defines its own `Order` and `OrderItem` models. This is a key decision: the application *owns* the order data. When an order is created, product information and, crucially, the *price at the time of order* are copied into the local database. This ensures that order records are immutable and historically accurate, even if product names or prices change in the external API later.

### Interpreting Open-Ended Requirements
1.  **"Understand how prices are moving"**: This was interpreted as a need for visual trend analysis. The implementation provides a clear line chart on each product's detail page, showing price fluctuations over the last year.
2.  **"Foreman reviews orders"**: This was addressed by creating a simple but effective status-based workflow (`New`, `Approved`, `Rejected`). A dedicated "Orders" page defaults to showing `New` orders, providing a direct to-do list for the foreman. The foreman has the ability to edit quantities or approve/reject the entire order.
3.  **"Make reordering easier"**: A "Reorder" button on approved orders was implemented. This feature streamlines the process of re-purchasing common sets of items by creating a new cart with the same products at their current prices.

### What Was Skipped
For this initial prototype, several features were considered out of scope:
*   **User Authentication**: As requested, a full authentication system was omitted in favor of a simple user name string for each order.
*   **Advanced Reporting**: No complex reporting or data visualization dashboard was built beyond the individual price history charts.
*   **UI/UX Polish**: The focus was on functionality over extensive UI design. The standard Bootstrap framework provides a clean and responsive base, but further branding and custom styling were not implemented.


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
