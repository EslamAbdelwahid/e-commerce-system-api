# My E-Commerce API Project

## What is this?
This is a backend API I built for online stores using ASP.NET Core. I created it to practice my web development skills and learn more about building real-world applications.

## What can it do?

### For shoppers:
- See all products in the store
- Filter stuff by type or brand
- Add items to a shopping cart
- Check out and place orders
- Pay securely online through Stripe
- Create accounts and log in

### Tech stuff I used:

#### Architecture stuff:
- Onion Architecture (also called Clean Architecture)
- Some useful patterns I learned about:
  * Generic Repository Pattern
  * Unit of Work Pattern
  * Specification Pattern

#### Making it fast:
- Used Redis for storing shopping carts
- Added caching for things people request a lot
- Made pages load in smaller chunks for better performance

#### Keeping it secure:
- JWT tokens for logging in
- ASP.NET Core Identity for user accounts
- Error handling that doesn't reveal too much

## How to set it up

### What you'll need:
- .NET 6.0 or newer
- SQL Server
- Redis
- Stripe account for payments

### Steps:

1. **Get the code**
   ```
   git clone https://github.com/YourUsername/e-commerce-api.git
   cd e-commerce-api
   ```

2. **Set up your database**
   
   Go to `appsettings.json` and change these lines:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=your_server;Database=ECommerceDB;Trusted_Connection=True;MultipleActiveResultSets=true",
     "Redis": "localhost:6379"
   }
   ```

3. **Add your Stripe keys**
   
   In the same file, add:
   ```json
   "StripeSettings": {
     "PublishableKey": "pk_test_your_key",
     "SecretKey": "sk_test_your_key"
   }
   ```

4. **Set up the database**
   ```
   dotnet ef database update
   ```

5. **Start it up**
   ```
   dotnet run
   ```

## API Endpoints

### User stuff:
- `POST /api/Accounts/login` - Log in
- `POST /api/Accounts/register` - Sign up
- `GET /api/Accounts/getcurrentuser` - Get your info
- `GET /api/Accounts/getaddress` - Get your address
- `PUT /api/Accounts/updateaddress` - Change your address

### Shopping cart:
- `GET /api/Baskets` - See what's in your cart
- `POST /api/Baskets` - Add/update items
- `DELETE /api/Baskets` - Empty your cart

### Orders:
- `GET /api/Orders` - See your orders
- `GET /api/Orders/{id}` - Look at a specific order
- `POST /api/Orders` - Place an order
- `GET /api/Orders/DeliveryMethods` - See shipping options

### Payments:
- `POST /api/Payments/{basketId}` - Pay for your order

### Products:
- `GET /api/Products` - Browse products
- `GET /api/Products/{id}` - Look at one product
- `GET /api/Products/brands` - See all brands
- `GET /api/Products/types` - See all product types

## Error Handling
I added some code that catches errors and sends back helpful messages without crashing the app.

## Why I made this
I built this project to learn more about ASP.NET Core and how to make web APIs. It was a fun challenge to put everything together and see it work!
