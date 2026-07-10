# OrderManagementAPI

RESTful API for order management, built with ASP.NET Core, Entity Framework Core, and SQL Server.

Demo: https://ordermanagement-api-vitor-fpcphmf5ejaretc0.brazilsouth-01.azurewebsites.net/swagger

(Hosted on Azure's free tier, so it may take a few seconds to "wake up" on the first request.)

## About the project

API for managing products, customers, and orders. When an order is created, the system checks if there's enough stock available, automatically deducts the quantity from the product, and "freezes" the unit price at the moment of purchase — so if the product's price changes later, past orders stay accurate.

## Stack

- ASP.NET Core 8
- Entity Framework Core
- SQL Server / Azure SQL Database
- Swagger for documentation
- Automatic deployment via GitHub Actions to Azure App Service

## Features

- Full CRUD for Products, Customers, and Orders
- Stock validation when creating orders
- Input data validation (email, required fields, etc.)
- Global exception handling middleware
- DTOs separating what the API receives from what it returns

## Endpoints

**Products** — `GET/POST /api/products`, `GET/PUT/DELETE /api/products/{id}`

**Customers** — `GET/POST /api/customers`, `GET/PUT/DELETE /api/customers/{id}`

**Orders** — `GET/POST /api/orders`, `GET/DELETE /api/orders/{id}`, `PUT /api/orders/{id}/status`

## Running locally

```bash
git clone https://github.com/Vitormjere/OrderManagementAPI.git
cd OrderManagementAPI/OrderManagementAPI
dotnet restore
dotnet ef database update
dotnet run
```

You'll need to update the `DefaultConnection` string in `appsettings.json` to point to your own local SQL Server instance.

## Author

Vitor Miranda Jeremias — [GitHub](https://github.com/Vitormjere)
