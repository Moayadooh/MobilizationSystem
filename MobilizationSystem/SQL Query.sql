CREATE DATABASE MobilizationDB;
GO
USE MobilizationDB;
GO

CREATE TABLE Persons (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    IsAvailable BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

CREATE TABLE Resources (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Type NVARCHAR(50) NULL,
    IsAvailable BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

CREATE TABLE MobilizationRequests (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Purpose NVARCHAR(250) NOT NULL,
    RequestDate DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Status INT NOT NULL, -- Enum: Draft, Submitted, Approved, etc.
    CreatedBy NVARCHAR(100) NOT NULL
);

CREATE TABLE MobilizationRequestPersons (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MobilizationRequestId INT NOT NULL,
    PersonId INT NOT NULL,

    CONSTRAINT FK_MRP_Request
        FOREIGN KEY (MobilizationRequestId)
        REFERENCES MobilizationRequests(Id)
        ON DELETE CASCADE,

    CONSTRAINT FK_MRP_Person
        FOREIGN KEY (PersonId)
        REFERENCES Persons(Id)
);

CREATE TABLE MobilizationRequestResources (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MobilizationRequestId INT NOT NULL,
    ResourceId INT NOT NULL,

    CONSTRAINT FK_MRR_Request
        FOREIGN KEY (MobilizationRequestId)
        REFERENCES MobilizationRequests(Id)
        ON DELETE CASCADE,

    CONSTRAINT FK_MRR_Resource
        FOREIGN KEY (ResourceId)
        REFERENCES Resources(Id)
);

CREATE TABLE Approvals (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MobilizationRequestId INT NOT NULL,
    ApprovedBy NVARCHAR(100) NOT NULL,
    ApprovalDate DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    IsApproved BIT NOT NULL,

    CONSTRAINT FK_Approvals_Request
        FOREIGN KEY (MobilizationRequestId)
        REFERENCES MobilizationRequests(Id)
        ON DELETE CASCADE
);

CREATE TABLE AuditLogs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Action NVARCHAR(200) NOT NULL,
    TableName NVARCHAR(100) NOT NULL,
    RecordId INT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

-- Requests
CREATE INDEX IX_MobilizationRequests_Status
    ON MobilizationRequests(Status);

-- Persons
CREATE INDEX IX_Persons_IsAvailable
    ON Persons(IsAvailable);

-- Resources
CREATE INDEX IX_Resources_IsAvailable
    ON Resources(IsAvailable);

-- Audit Logs
CREATE INDEX IX_AuditLogs_CreatedAt
    ON AuditLogs(CreatedAt DESC);

INSERT INTO Persons (FullName) VALUES
('Ahmed Ali'),
('Sara Khan'),
('Mohammed Hassan');

INSERT INTO Resources (Name, Type) VALUES
('Excavator', 'Heavy'),
('Crane', 'Heavy'),
('Pickup Truck', 'Vehicle');
