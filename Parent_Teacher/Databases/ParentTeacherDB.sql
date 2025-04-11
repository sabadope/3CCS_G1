CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL
);

INSERT INTO Users (Name, Email, Password, Role)
VALUES ('Admin', 'admin@gmail.com', 'admin1230', 'Admin');

