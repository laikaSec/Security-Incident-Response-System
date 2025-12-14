-- Creates a responder (team member) that incidents can be assigned to
CREATE OR ALTER PROCEDURE sp_CreateResponder
    @Name NVARCHAR(200),
    @Email NVARCHAR(255) = NULL,
    @Role NVARCHAR(100) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Responders (Name, Email, Role, IsActive)
    VALUES (@Name, @Email, @Role, @IsActive);

    SELECT SCOPE_IDENTITY() AS NewResponderID;
END;
GO
