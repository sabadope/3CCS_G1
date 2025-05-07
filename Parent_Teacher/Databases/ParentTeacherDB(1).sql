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
CREATE TABLE Students (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StudentID NVARCHAR(50),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Course NVARCHAR(50),
    Section NVARCHAR(50),
    ImagePath NVARCHAR(255) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    Midterm DECIMAL(5,2) NOT NULL DEFAULT 0,
    Finals DECIMAL(5,2) NOT NULL DEFAULT 0,
    TotalAverage DECIMAL(5,2) NOT NULL DEFAULT 0,
    Subject NVARCHAR(100) NOT NULL DEFAULT '',
    Class NVARCHAR(100) NOT NULL DEFAULT ''
);

-- Add index to StudentID for fast lookup
CREATE INDEX IX_Students_StudentID ON Students(StudentID);

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