CREATE PROCEDURE [dbo].[UpdateUserStatus]
    @UserUpdates UserUpdateType READONLY
AS
BEGIN
    UPDATE u
    SET u.IsActive = t.IsActive  
    FROM Users u
    JOIN @UserUpdates t ON u.Id = t.UserId;
END
GO