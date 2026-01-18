CREATE TABLE Approvals (
    Id INT IDENTITY PRIMARY KEY,
    MobilizationRequestId INT NOT NULL,
    ApprovedBy NVARCHAR(450) NOT NULL,
    ApprovalDate DATETIME2 NOT NULL,
    IsApproved BIT NOT NULL,
    CONSTRAINT FK_Approvals_MobilizationRequests FOREIGN KEY (MobilizationRequestId) REFERENCES MobilizationRequests(Id)
);

CREATE TABLE AuditLogs (
    Id INT IDENTITY PRIMARY KEY,
    UserName NVARCHAR(450) NOT NULL,
    Action NVARCHAR(200) NOT NULL,
    TableName NVARCHAR(100) NOT NULL,
    RecordId INT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE MobilizationRequestPersons (
    Id INT IDENTITY PRIMARY KEY,
    MobilizationRequestId INT NOT NULL,
    PersonId INT NOT NULL,
    CONSTRAINT FK_MRP_MR FOREIGN KEY (MobilizationRequestId) REFERENCES MobilizationRequests(Id),
    CONSTRAINT FK_MRP_P FOREIGN KEY (PersonId) REFERENCES Persons(Id)
);

CREATE TABLE MobilizationRequestResources (
    Id INT IDENTITY PRIMARY KEY,
    MobilizationRequestId INT NOT NULL,
    ResourceId INT NOT NULL,
    CONSTRAINT FK_MRR_MR FOREIGN KEY (MobilizationRequestId) REFERENCES MobilizationRequests(Id),
    CONSTRAINT FK_MRR_R FOREIGN KEY (ResourceId) REFERENCES Resources(Id)
);

CREATE INDEX IX_Persons_IsAvailable ON Persons(IsAvailable);
CREATE INDEX IX_Resources_IsAvailable ON Resources(IsAvailable);

CREATE INDEX IX_MR_Status ON MobilizationRequests(Status);
CREATE INDEX IX_MRP_PersonId ON MobilizationRequestPersons(PersonId);
CREATE INDEX IX_MRR_ResourceId ON MobilizationRequestResources(ResourceId);

CREATE INDEX IX_MobilizationRequests_Status ON MobilizationRequests(Status);
