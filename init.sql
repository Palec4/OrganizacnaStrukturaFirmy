CREATE DATABASE OrgStructure;
GO
USE OrgStructure;
GO

CREATE TABLE Employees (
    Id          INT PRIMARY KEY IDENTITY(1,1),
    CompanyId   INT NOT NULL,
    Title       NVARCHAR(50) NULL,
    FirstName   NVARCHAR(100) NOT NULL,
    LastName    NVARCHAR(100) NOT NULL,
    Phone       NVARCHAR(20) NOT NULL,
    Email       NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Companies (
    Id          INT PRIMARY KEY IDENTITY(1,1),
    Code        NVARCHAR(50) NOT NULL UNIQUE,
    Name        NVARCHAR(200) NOT NULL,
    ManagerId   INT NULL
);
GO

CREATE TABLE Divisions (
    Id          INT PRIMARY KEY IDENTITY(1,1),
    Code        NVARCHAR(50) NOT NULL UNIQUE,
    Name        NVARCHAR(200) NOT NULL,
    CompanyId   INT NOT NULL,
    ManagerId   INT NULL
);
GO

CREATE TABLE Projects (
    Id          INT PRIMARY KEY IDENTITY(1,1),
    Code        NVARCHAR(50) NOT NULL UNIQUE,
    Name        NVARCHAR(200) NOT NULL,
    DivisionId  INT NOT NULL,
    ManagerId   INT NULL
);
GO

CREATE TABLE Departments (
    Id          INT PRIMARY KEY IDENTITY(1,1),
    Code        NVARCHAR(50) NOT NULL UNIQUE,
    Name        NVARCHAR(200) NOT NULL,
    ProjectId   INT NOT NULL,
    ManagerId   INT NULL
);
GO

-- Foreign keys
ALTER TABLE Employees
    ADD CONSTRAINT FK_Employees_Company 
        FOREIGN KEY (CompanyId) REFERENCES Companies(Id);

ALTER TABLE Companies
    ADD CONSTRAINT FK_Companies_Manager 
        FOREIGN KEY (ManagerId) REFERENCES Employees(Id);

ALTER TABLE Divisions
    ADD CONSTRAINT FK_Divisions_Company 
        FOREIGN KEY (CompanyId) REFERENCES Companies(Id),
        CONSTRAINT FK_Divisions_Manager 
        FOREIGN KEY (ManagerId) REFERENCES Employees(Id);

ALTER TABLE Projects
    ADD CONSTRAINT FK_Projects_Division 
        FOREIGN KEY (DivisionId) REFERENCES Divisions(Id),
        CONSTRAINT FK_Projects_Manager 
        FOREIGN KEY (ManagerId) REFERENCES Employees(Id);

ALTER TABLE Departments
    ADD CONSTRAINT FK_Departments_Project 
        FOREIGN KEY (ProjectId) REFERENCES Projects(Id),
        CONSTRAINT FK_Departments_Manager 
        FOREIGN KEY (ManagerId) REFERENCES Employees(Id);
GO