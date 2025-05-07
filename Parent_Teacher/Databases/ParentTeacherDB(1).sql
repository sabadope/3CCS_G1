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
ADD Midterm DECIMAL(5,2) NOT NULL DEFAULT 0,
    Finals DECIMAL(5,2) NOT NULL DEFAULT 0,
    TotalAverage DECIMAL(5,2) NOT NULL DEFAULT 0;


-- Add new columns
ALTER TABLE Students ADD Subject NVARCHAR(100) NOT NULL DEFAULT '';
ALTER TABLE Students ADD Class NVARCHAR(100) NOT NULL DEFAULT '';



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


-- Create CourseSections table
CREATE TABLE CourseSections (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CourseName NVARCHAR(100) NOT NULL,
    SectionName NVARCHAR(50) NOT NULL
);


-- Create SubjectClasses table 
CREATE TABLE SubjectClasses (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SubjectCode NVARCHAR(50) NOT NULL,
    SubjectName NVARCHAR(255) NOT NULL
);



CREATE TABLE Grades (
    Id INT IDENTITY(1,1) PRIMARY KEY,           
    SubjectName NVARCHAR(100) NOT NULL,         
    GradeValue FLOAT NOT NULL CHECK (GradeValue >= 0 AND GradeValue <= 100),  
    StudentId INT NOT NULL,                      
    CONSTRAINT FK_Grades_Students FOREIGN KEY (StudentId)  
        REFERENCES Students(Id)  
        ON DELETE CASCADE         
);





--DOCUMENTATIONS


-- updated as of 05/7/25 - 6:00am

-- sa editstudent ka natapos, bali gawin mo kunin mo nlng ung nasa loob ng music tas extra 
-- ayun malinis na editstudent un, dun knlng mag mod ggs

-- ayos na ung sa createstudent, bali sa edit and sa delte ka magkakatalo ggs




-- Replace 'Users' with 'Students', 'CourseSections', or 'SubjectClasses' as needed
SELECT 
    f.name AS ForeignKeyName,
    OBJECT_NAME(f.parent_object_id) AS ReferencingTable,
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ReferencingColumn
FROM 
    sys.foreign_keys AS f
JOIN 
    sys.foreign_key_columns AS fc ON f.object_id = fc.constraint_object_id
WHERE 
    f.referenced_object_id = OBJECT_ID('Users');


ALTER TABLE Messages DROP CONSTRAINT [FK__Messages__SenderId__5070F446];
ALTER TABLE Messages DROP CONSTRAINT [FK__Messages__ReceiverId__5165187F];


ALTER TABLE SubjectClasses DROP CONSTRAINT FK_SubjectClasses_CourseSections;

ALTER TABLE Performances DROP CONSTRAINT FK__Performan__Subje__12FDD1B2;


DROP TABLE Users;
DROP TABLE Students;
DROP TABLE Messages;
DROP TABLE CourseSections;
DROP TABLE SubejectClasses;
DROP TABLE Grades;