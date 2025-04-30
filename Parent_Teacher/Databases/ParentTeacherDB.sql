CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL
);

INSERT INTO Users (Name, Email, Password, Role)
VALUES ('Admin', 'admin@gmail.com', 'admin1230', 'Admin');

ALTER TABLE Users ADD CreatedAt DATETIME DEFAULT GETDATE();

CREATE TABLE Messages (
    Id INT PRIMARY KEY IDENTITY(1,1),

    SenderId INT NOT NULL,
    ReceiverId INT NOT NULL,

    Content NVARCHAR(MAX),
    Timestamp DATETIME NOT NULL DEFAULT GETDATE(),

    FOREIGN KEY (SenderId) REFERENCES Users(Id),
    FOREIGN KEY (ReceiverId) REFERENCES Users(Id)
);

CREATE TABLE Students (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StudentID NVARCHAR(50),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
	Course NVARCHAR(50),
    Section NVARCHAR(50)
);

CREATE TABLE CourseSections (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CourseName NVARCHAR(100) NOT NULL,
    SectionName NVARCHAR(50) NOT NULL
);

-- updated as of 04/30/25 - 7:00pm
