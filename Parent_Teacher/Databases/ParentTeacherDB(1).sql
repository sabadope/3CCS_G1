-- Create Users table
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Insert default admin user
INSERT INTO Users (Name, Email, Password, Role)
VALUES ('Admin', 'admin@gmail.com', 'admin1230', 'Admin');

-- Create CourseSections table
CREATE TABLE CourseSections (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CourseName NVARCHAR(100) NOT NULL,
    SectionName NVARCHAR(50) NOT NULL
);

-- Create Students table
CREATE TABLE Students (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StudentID NVARCHAR(50),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Course NVARCHAR(50),
    Section NVARCHAR(50),
    ImagePath NVARCHAR(255) NULL
    
);

ALTER TABLE Students
ADD CreatedAt DATETIME NOT NULL DEFAULT GETDATE();

ALTER TABLE Students
ADD ParentImagePath NVARCHAR(255) NULL;


-- Create Messages table (after Users is created)
CREATE TABLE Messages (
    Id INT PRIMARY KEY IDENTITY(1,1),

    SenderId INT NOT NULL,
    ReceiverId INT NOT NULL,

    Content NVARCHAR(MAX),
    Timestamp DATETIME NOT NULL DEFAULT GETDATE(),

    FOREIGN KEY (SenderId) REFERENCES Users(Id),
    FOREIGN KEY (ReceiverId) REFERENCES Users(Id)
);

-- Create SubjectClasses table 
CREATE TABLE SubjectClasses (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SubjectCode NVARCHAR(50) NOT NULL,
    SubjectName NVARCHAR(255) NOT NULL
);

-- updated as of 05/4/25 - 1:00am