-- Create the database
CREATE DATABASE SecurityIncidentDB;

USE SecurityIncidentDB;
GO

-- Table 1: Stores the types of incidents (Phishing, Malware, etc.)
CREATE TABLE IncidentTypes (
    TypeID INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(200),
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- Insert common incident types
INSERT INTO IncidentTypes (TypeName, Description) VALUES
('Phishing', 'Suspicious phishing email or website'),
('Malware', 'Malicious software detected on system'),
('Unauthorized Access', 'Attempted or successful unauthorized login'),
('Data Breach', 'Potential or confirmed data exposure'),
('DDoS Attack', 'Distributed denial of service attack'),
('Insider Threat', 'Suspicious activity by authorized user'),
('Policy Violation', 'Security policy non-compliance'),
('Vulnerability', 'Identified system or application vulnerability');

-- Table 2: Stores the SOC team members who respond to incidents
CREATE TABLE Responders (
    ResponderID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Role NVARCHAR(50),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- Insert sample team members (use your name!)
INSERT INTO Responders (Name, Email, Role) VALUES
('Miguel Silva', 'ms15766@nyu.edu', 'Security Analyst'),
('John Smith', 'jsmith@company.com', 'Senior Analyst'),
('Sarah Johnson', 'sjohnson@company.com', 'Incident Manager'),
('Alex Chen', 'achen@company.com', 'Security Engineer');

-- Table 3: The main table - stores each security incident
CREATE TABLE Incidents (
    IncidentID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Severity NVARCHAR(20) CHECK (Severity IN ('Critical', 'High', 'Medium', 'Low')),
    Status NVARCHAR(20) CHECK (Status IN ('New', 'In Progress', 'Resolved', 'Closed')) DEFAULT 'New',
    IncidentTypeID INT,
    AssignedTo INT,
    DetectedDate DATETIME DEFAULT GETDATE(),
    ResolvedDate DATETIME NULL,
    SourceIP NVARCHAR(50),
    AffectedSystem NVARCHAR(200),
    FOREIGN KEY (IncidentTypeID) REFERENCES IncidentTypes(TypeID),
    FOREIGN KEY (AssignedTo) REFERENCES Responders(ResponderID)
);

-- Table 4: Audit trail - keeps a record of all actions taken on each incident
CREATE TABLE IncidentActions (
    ActionID INT PRIMARY KEY IDENTITY(1,1),
    IncidentID INT,
    ResponderID INT,
    ActionType NVARCHAR(50),
    ActionDescription NVARCHAR(MAX),
    Timestamp DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IncidentID) REFERENCES Incidents(IncidentID),
    FOREIGN KEY (ResponderID) REFERENCES Responders(ResponderID)
);

-- Insert sample incidents for testing
INSERT INTO Incidents (Title, Description, Severity, Status, IncidentTypeID, AssignedTo, SourceIP, AffectedSystem)
VALUES 
('Multiple Failed Login Attempts', 'User account experienced 15 failed login attempts from unknown IP', 'High', 'New', 3, 1, '192.168.1.100', 'Web Server 01'),
('Suspicious Email Campaign', 'Phishing emails detected targeting finance department', 'Medium', 'In Progress', 1, 2, 'External', 'Email System'),
('Malware Detection on Endpoint', 'Antivirus flagged suspicious executable on workstation', 'Critical', 'New', 2, 1, '10.0.0.45', 'Workstation-Finance-12');

