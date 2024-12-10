CREATE PROCEDURE PopulateUsers
AS
BEGIN
    SET NOCOUNT ON;

    -- Declare a table variable to hold sample user data
    DECLARE @UsersToInsert TABLE (
        FirstName NVARCHAR(50),
        LastName NVARCHAR(50)
    );

    -- Insert sample first names and last names into the table variable
    INSERT INTO @UsersToInsert (FirstName, LastName)
    VALUES 
        ('John', 'Smith'),
        ('Jane', 'Doe'),
        ('Alice', 'Johnson'),
        ('Bob', 'Brown'),
        ('Charlie', 'Davis'),
        ('David', 'Miller'),
        ('Eva', 'Wilson'),
        ('Frank', 'Moore'),
        ('Grace', 'Taylor'),
        ('Hannah', 'Anderson'),
        ('Ivy', 'Thomas'),
        ('Jack', 'Jackson'),
        ('Katherine', 'White'),
        ('Leo', 'Harris'),
        ('Mia', 'Martin'),
        ('Noah', 'Thompson'),
        ('Olivia', 'Garcia'),
        ('Paul', 'Martinez'),
        ('Quinn', 'Robinson'),
        ('Rita', 'Clark');

    DECLARE @Email NVARCHAR(100);
    DECLARE @Password NVARCHAR(255) = 'sPZCK1Y/rQTLF+bIOOACYmy/q7c='; -- the pass is 'password'
    DECLARE @PasswordSalt NVARCHAR(255) = 'qJDEblSFmHWvQz2ZWxCVow=='; 

    -- Insert users into the Users table
    DECLARE @FirstName NVARCHAR(50);
    DECLARE @LastName NVARCHAR(50);
    
    DECLARE user_cursor CURSOR FOR 
    SELECT FirstName, LastName FROM @UsersToInsert;

    OPEN user_cursor;
    
    FETCH NEXT FROM user_cursor INTO @FirstName, @LastName;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @Email = LOWER(CONCAT(SUBSTRING(@FirstName, 1, 1), SUBSTRING(@LastName, 1, LEN(@LastName)), CAST(NEWID() AS NVARCHAR(36)), '@example.com'));

        INSERT INTO [dbo].[Users] (FirstName, LastName, Email, [Password], PasswordSalt, CreatedDate, LastModifiedDate, IsActive)
        VALUES (
            @FirstName,
            @LastName,
            @Email,
            @Password,
            @PasswordSalt,
            GETDATE(),
            GETDATE(),
            1 -- Set IsActive to true
        );

        FETCH NEXT FROM user_cursor INTO @FirstName, @LastName;
    END

    CLOSE user_cursor;
    DEALLOCATE user_cursor;
END;