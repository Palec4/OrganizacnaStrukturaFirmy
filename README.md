# Organizačná Štruktúra Firmy - REST API

REST API pre správu organizačnej štruktúry firiem vytvorené v .NET 8 a C#.

## Požiadavky

- .NET 8 SDK
- Microsoft SQL Server Express
- SQL Server Management Studio (SSMS)

## Inštalácia a spustenie

### 1. Klonovanie repozitára
```bash
git clone https://github.com/Palec4/OrganizacnaStrukturaFirmy.git
cd OrganizacnaStrukturaFirmy
```

### 2. Vytvorenie databázy
1. Otvorte SSMS a pripojte sa na `localhost\SQLEXPRESS`
2. Otvorte súbor `Database/init.sql`
3. Spustite skript cez **F5** alebo **Execute**

### 3. Nastavenie connection stringu
V súbore `appsettings.json` skontrolujte connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=OrgStructura;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 4. Spustenie projektu
```bash
dotnet run
```

Alebo otvorte projekt vo Visual Studiu a stlačte **F5**.
