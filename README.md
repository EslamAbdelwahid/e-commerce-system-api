# E-Commerce API Platform

## Project Description
A robust backend API solution for e-commerce applications built with ASP.NET Core Web API. This project demonstrates the implementation of industry-standard architecture patterns and modern web development practices.

## Core Capabilities

### Customer Experience
- Browse complete product catalog
- Filter products by category or manufacturer
- Virtual shopping cart functionality
- Streamlined checkout process
- Secure online payment processing via Stripe
- User account management with authentication

### Technical Stack

#### Architecture & Design
- **Clean Architecture** (Onion pattern)
- **Design Patterns**
  * Repository Pattern with Generic Implementation
  * Unit of Work for Transaction Management
  * Specification Pattern for Query Flexibility

#### Performance Enhancements
- Redis-powered basket storage for lightning-fast operations
- Strategic response caching for high-demand endpoints
- Smart pagination for efficient data retrieval

#### Security Framework
- Token-based authentication with JWT
- ASP.NET Core Identity integration
- Comprehensive exception handling middleware

## Setup Guide

### System Requirements
- .NET 6.0+ SDK
- SQL Server instance
- Redis server installation
- Active Stripe account

### Deployment Instructions

1. **Get the code**
   ```
   git clone https://github.com/YourUsername/e-commerce-api.git
   cd e-commerce-api
   ```

2. **Configure database connections**
   
   Edit `appsettings.json` to include your connection details:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=your_database_server;Database=ECommerceDB;Trusted_Connection=True;MultipleActiveResultSets=true",
     "Redis": "localhost:6379"
   }
   ```

3. **Set up payment processing**
   
   Add your Stripe credentials to `appsettings.json`:
   ```json
   "StripeSettings": {
     "PublishableKey": "pk_test_your_key",
     "SecretKey": "sk_test_your_key"
   }
   ```

4. **Initialize the database**
   ```
   dotnet ef database update
   ```

5. **Launch the application**
   ```
   dotnet run
   ```

## API Reference

### User Management
- `POST /api/Accounts/login` - Authenticate user
- `POST /api/Accounts/register` - Create new account
- `GET /api/Accounts/getcurrentuser` - Retrieve user profile
- `GET /api/Accounts/getaddress` - Fetch shipping address
- `PUT /api/Accounts/updateaddress` - Modify shipping address

### Cart Operations
- `GET /api/Baskets` - Retrieve current cart
- `POST /api/Baskets` - Create/update cart
- `DELETE /api/Baskets` - Remove cart

### Order Processing
- `GET /api/Orders` - List all user orders
- `GET /api/Orders/{id}` - View specific order details
- `POST /api/Orders` - Place new order
- `GET /api/Orders/DeliveryMethods` - List shipping options

### Payment Processing
- `POST /api/Payments/{basketId}` - Process transaction

### Product Catalog
- `GET /api/Products` - Browse products (paginated)
- `GET /api/Products/{id}` - View product details
- `GET /api/Products/brands` - List available brands
- `GET /api/Products/types` - List product categories

## Exception Management
The platform implements a sophisticated exception handling system that captures all errors and generates appropriate HTTP responses with meaningful status codes.

## Project Background
This API was developed as a professional learning exercise to demonstrate competence in ASP.NET Core and modern web API development techniques.
