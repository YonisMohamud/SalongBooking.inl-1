# SalongBooking API

Ett bokningssystem för frisörsalonger där kunder kan boka, ändra och avboka tider online. Systemet hanterar även frisörernas scheman och skickar påminnelser till kunder.

## 🚀 Funktioner

- ✅ **CRUD-operationer** för Kunder, Frisörer, Tjänster och Bokningar
- ✅ **Filter & Sortering** med query-parametrar
- ✅ **Paginering** för alla list-endpoints
- ✅ **Extern API-integration** för e-postbekräftelser (mock)
- ✅ **Validering** med FluentValidation
- ✅ **Seed-data** med Bogus (endast om databasen är tom)
- ✅ **Swagger UI** för API-dokumentation
- ✅ **Postman Collection** med exempel

## 📋 Krav

- .NET 9.0 SDK
- SQL Server (LocalDB eller SQL Server Express)
- Visual Studio 2022 eller VS Code

## 🛠️ Installation

1. Klona repositoryt:
```bash
git clone <repository-url>
cd SalongBooking
```

2. Restaurera NuGet-paket:
```bash
dotnet restore
```

3. Uppdatera connection string i `appsettings.json` om det behövs:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SalongBookingDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

4. Skapa databasen (körs automatiskt vid första start):
```bash
dotnet ef database update --project SalongBooking.Data --startup-project SalongBooking.API
```

5. Starta applikationen:
```bash
dotnet run --project SalongBooking.API
```

6. Öppna Swagger UI: `https://localhost:7000` (eller den port som visas)

## 📚 API Endpoints

### Customers
- `GET /api/customers` - Hämta alla kunder (med filter, sortering och paginering)
- `GET /api/customers/{id}` - Hämta kund efter ID
- `POST /api/customers` - Skapa ny kund
- `PUT /api/customers/{id}` - Uppdatera kund
- `DELETE /api/customers/{id}` - Ta bort kund

### Hairdressers
- `GET /api/hairdressers` - Hämta alla frisörer (med filter, sortering och paginering)
- `GET /api/hairdressers/{id}` - Hämta frisör efter ID
- `POST /api/hairdressers` - Skapa ny frisör
- `PUT /api/hairdressers/{id}` - Uppdatera frisör
- `DELETE /api/hairdressers/{id}` - Ta bort frisör

### Services
- `GET /api/services` - Hämta alla tjänster (med filter, sortering och paginering)
- `GET /api/services/{id}` - Hämta tjänst efter ID
- `POST /api/services` - Skapa ny tjänst
- `PUT /api/services/{id}` - Uppdatera tjänst
- `DELETE /api/services/{id}` - Ta bort tjänst

### Bookings
- `GET /api/bookings` - Hämta alla bokningar (med filter, sortering och paginering)
- `GET /api/bookings/{id}` - Hämta bokning efter ID
- `POST /api/bookings` - Skapa ny bokning
- `PUT /api/bookings/{id}` - Uppdatera bokning
- `DELETE /api/bookings/{id}` - Ta bort bokning
- `POST /api/bookings/{id}/cancel` - Avboka bokning
- `GET /api/bookings/customer/{customerId}` - Hämta bokningar för kund
- `GET /api/bookings/hairdresser/{hairdresserId}` - Hämta bokningar för frisör
- `GET /api/bookings/date/{date}` - Hämta bokningar för datum

## 🔍 Query Parameters

### Filter
Använd `filter`-parametern för att söka:
```
GET /api/customers?filter=Anna
```

### Sortering
Använd `sort`-parametern för att sortera (asc/desc):
```
GET /api/customers?sort=desc
```

### Paginering
Använd `page` och `pageSize` för paginering:
```
GET /api/customers?page=1&pageSize=10
```

## 📦 Projektstruktur

```
SalongBooking/
├── SalongBooking.API/          # Web API projekt
│   ├── Controllers/             # API Controllers
│   ├── DTOs/                    # Data Transfer Objects
│   ├── Validators/              # FluentValidation validators
│   ├── Mappings/                # AutoMapper profiles
│   └── SeedData.cs              # Bogus seed-data
├── SalongBooking.Domain/        # Domänmodeller
│   └── Entities/                # Entiteter
├── SalongBooking.Data/          # Data access layer
│   ├── Repositories/            # Repository pattern
│   └── ApplicationDbContext.cs  # EF Core DbContext
└── SalongBooking.Services/      # Business logic layer
    ├── Interfaces/              # Service interfaces
    └── *.cs                     # Service implementations
```

## 🏗️ Arkitektur

Projektet följer **Lager-arkitektur** (Layered Architecture):

- **Controllers** → Anropar endast Services
- **Services** → Innehåller affärslogik och anropar Repositories
- **Repositories** → Hanterar dataåtkomst via EF Core
- **DTOs** → Separerar API-modeller från entitetsmodeller
- **AutoMapper** → Mappar mellan DTOs och entiteter

## 🧪 Testdata

Vid första start fylls databasen automatiskt med testdata (endast om databasen är tom):
- 5 Tjänster
- 5 Frisörer
- 20 Kunder
- 50 Bokningar

## 📧 E-postintegration

Systemet använder en mock API (JSONPlaceholder) för att simulera e-postskickning. I produktion bör detta ersättas med en riktig e-posttjänst.

## 🔐 Säkerhet

- Lösenord hashas med SHA256 (i produktion bör starkare hashing användas, t.ex. BCrypt)
- Validering av all inkommande data med FluentValidation
- CORS konfigurerad för utveckling

## 📝 Postman Collection

En Postman Collection finns i `SalongBooking.API/Postman/SalongBooking.postman_collection.json`. 

För att använda den:
1. Importera filen i Postman
2. Uppdatera `baseUrl`-variabeln till din API-URL
3. Testa alla endpoints

## 🛠️ Teknologier

- **.NET 9.0** - Framework
- **ASP.NET Core Web API** - API framework
- **Entity Framework Core 9.0** - ORM
- **AutoMapper** - Object mapping
- **FluentValidation** - Validering
- **Bogus** - Testdata generation
- **Swagger/OpenAPI** - API dokumentation

## 📄 Licens

Detta projekt är skapat för utbildningssyfte.

