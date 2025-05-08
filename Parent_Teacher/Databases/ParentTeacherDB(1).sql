-- ------------------------------------------
-- 1. USERS TABLE
-- ------------------------------------------
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

-- ------------------------------------------
-- 2. COURSE SECTIONS TABLE
-- ------------------------------------------
CREATE TABLE CourseSections (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CourseName NVARCHAR(100) NOT NULL,
    SectionName NVARCHAR(50) NOT NULL,
    CONSTRAINT UQ_Course_Section UNIQUE (CourseName, SectionName)
);

-- ------------------------------------------
-- 3. SUBJECT CLASSES TABLE
-- ------------------------------------------
CREATE TABLE SubjectClasses (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SubjectCode NVARCHAR(50) NOT NULL,
    SubjectName NVARCHAR(255) NOT NULL,
    CONSTRAINT UQ_Subject_Class UNIQUE (SubjectCode, SubjectName)
);

-- ------------------------------------------
-- 4. STUDENTS TABLE
-- ------------------------------------------
-- Create the Students table with all necessary columns
CREATE TABLE Students (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StudentID NVARCHAR(50),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Course NVARCHAR(50),
    Section NVARCHAR(50),
    ImagePath NVARCHAR(255) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    
    -- Subject 1
    Subject NVARCHAR(100) NOT NULL DEFAULT '',
    Class NVARCHAR(100) NOT NULL DEFAULT '',
    Midterm DECIMAL(5,2) NOT NULL DEFAULT 0,
    Finals DECIMAL(5,2) NOT NULL DEFAULT 0,
    TotalAverage DECIMAL(5,2) NOT NULL DEFAULT 0,
    
    -- Subject 2
    Subject2 NVARCHAR(100) NOT NULL DEFAULT '',
    Class2 NVARCHAR(100) NOT NULL DEFAULT '',
    Midterm2 DECIMAL(5,2) NOT NULL DEFAULT 0,
    Finals2 DECIMAL(5,2) NOT NULL DEFAULT 0,
    TotalAverage2 DECIMAL(5,2) NOT NULL DEFAULT 0,
    
    -- Subject 3
    Subject3 NVARCHAR(100) NOT NULL DEFAULT '',
    Class3 NVARCHAR(100) NOT NULL DEFAULT '',
    Midterm3 DECIMAL(5,2) NOT NULL DEFAULT 0,
    Finals3 DECIMAL(5,2) NOT NULL DEFAULT 0,
    TotalAverage3 DECIMAL(5,2) NOT NULL DEFAULT 0
);

-- Add index to StudentID for fast lookup
CREATE INDEX IX_Students_StudentID ON Students(StudentID);

-- Insert sample data
INSERT INTO Students (StudentID, FirstName, LastName, Course, Section, Subject, Class, Midterm, Finals, TotalAverage,
                     Subject2, Class2, Midterm2, Finals2, TotalAverage2,
                     Subject3, Class3, Midterm3, Finals3, TotalAverage3)
VALUES 
('2023-001', 'John', 'Doe', 'BSIT', 'A1', 'Programming', 'IT101', 85.50, 90.00, 87.75,
 'Database', 'IT102', 88.00, 92.00, 90.00,
 'Networking', 'IT103', 82.00, 88.00, 85.00),

('2023-002', 'Jane', 'Smith', 'BSCS', 'B1', 'Programming', 'CS101', 92.00, 95.00, 93.50,
 'Database', 'CS102', 90.00, 94.00, 92.00,
 'Networking', 'CS103', 88.00, 91.00, 89.50),

('2023-003', 'Mike', 'Johnson', 'BSIT', 'A1', 'Programming', 'IT101', 78.00, 85.00, 81.50,
 'Database', 'IT102', 80.00, 82.00, 81.00,
 'Networking', 'IT103', 75.00, 80.00, 77.50),

('2023-004', 'Sarah', 'Williams', 'BSCS', 'B1', 'Programming', 'CS101', 95.00, 98.00, 96.50,
 'Database', 'CS102', 94.00, 97.00, 95.50,
 'Networking', 'CS103', 93.00, 96.00, 94.50),

('2023-005', 'David', 'Brown', 'BSIT', 'A1', 'Programming', 'IT101', 88.00, 92.00, 90.00,
 'Database', 'IT102', 85.00, 90.00, 87.50,
 'Networking', 'IT103', 87.00, 89.00, 88.00);

-- Create CourseSections table if it doesn't exist
IF OBJECT_ID('CourseSections', 'U') IS NULL
BEGIN
    CREATE TABLE CourseSections (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CourseName NVARCHAR(50) NOT NULL,
        SectionName NVARCHAR(50) NOT NULL
    );

    -- Insert sample course sections
    INSERT INTO CourseSections (CourseName, SectionName)
    VALUES 
    ('BSIT', 'A1'),
    ('BSIT', 'A2'),
    ('BSCS', 'B1'),
    ('BSCS', 'B2');
END

-- ------------------------------------------
-- 5. GRADES TABLE
-- ------------------------------------------
CREATE TABLE Grades (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SubjectName NVARCHAR(100) NOT NULL,
    GradeValue FLOAT NOT NULL CHECK (GradeValue >= 0 AND GradeValue <= 100),
    StudentId INT NOT NULL,
    CONSTRAINT FK_Grades_Students FOREIGN KEY (StudentId)
        REFERENCES Students(Id)
        ON DELETE CASCADE
);

-- Index for performance
CREATE INDEX IX_Grades_StudentId ON Grades(StudentId);

-- ------------------------------------------
-- 6. MESSAGES TABLE
-- ------------------------------------------
CREATE TABLE Messages (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SenderId INT NOT NULL,
    ReceiverId INT NOT NULL,
    Content NVARCHAR(MAX),
    Timestamp DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (SenderId) REFERENCES Users(Id),
    FOREIGN KEY (ReceiverId) REFERENCES Users(Id)
);

-- Index for performance
CREATE INDEX IX_Messages_ReceiverId ON Messages(ReceiverId);
CREATE INDEX IX_Messages_SenderId ON Messages(SenderId);







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
DROP TABLE SubjectClasses;
DROP TABLE Grades;
DROP TABLE Classes;
DROP TABLE Subjects;
DROP TABLE Sections;
DROP TABLE Courses;


DROP TABLE IF EXISTS Classes;
DROP TABLE IF EXISTS Subjects;
DROP TABLE IF EXISTS Sections;
DROP TABLE IF EXISTS Courses;
DROP TABLE IF EXISTS Students;