-- Stored procedure to create a new incident
CREATE PROCEDURE sp_CreateIncident
    @Title NVARCHAR(200),
    @Description NVARCHAR(MAX),
    @Severity NVARCHAR(20),
    @Status NVARCHAR(20),
    @IncidentTypeID INT,
    @AssignedTo INT = NULL,
    @SourceIP NVARCHAR(50) = NULL,
    @AffectedSystem NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insert the incident record
        INSERT INTO Incidents
        (
            Title,
            Description,
            Severity,
            Status,
            IncidentTypeID,
            AssignedTo,
            SourceIP,
            AffectedSystem
        )
        VALUES
        (
            @Title,
            @Description,
            @Severity,
            @Status,
            @IncidentTypeID,
            @AssignedTo,
            @SourceIP,
            @AffectedSystem
        );

        -- Get the new IncidentID that was just created
        DECLARE @NewIncidentID INT = SCOPE_IDENTITY();

        -- Only log an "assigned" action if there is an assigned responder
        IF @AssignedTo IS NOT NULL
        BEGIN
            INSERT INTO IncidentActions
            (
                IncidentID,
                ResponderID,
                ActionType,
                ActionDescription
            )
            VALUES
            (
                @NewIncidentID,
                @AssignedTo,
                'Created',
                'Incident created and assigned.'
            );
        END

        COMMIT TRANSACTION;

        -- Return the new ID to the caller
        SELECT @NewIncidentID AS NewIncidentID;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Return the error message to help debugging
        SELECT ERROR_MESSAGE() AS ErrorMessage;
    END CATCH
END;
GO
