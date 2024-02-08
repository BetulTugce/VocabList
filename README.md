# VocabList

## Project Description

VocabList is a language learning application developed using ASP.NET Core Web API and Blazor Server technologies. Users can create custom word lists, managing words, meanings, types, and examples for efficient language learning. In the user panel, filtering operations are available on pages that display word lists and the words associated with the selected list.

There is also an administration panel. Basic CRUD operations for users and roles can be performed in the admin panel. In addition, assigning roles to users and endpoints can be done. This means users have permissions to perform actions on endpoints associated with the roles assigned to them.

## Screenshots

![VocabList_KelimeListesi](https://github.com/BetulTugce/VocabList/assets/79140478/314348a8-1579-4cee-b1dc-972262884fd7)

![VocabList_404](https://github.com/BetulTugce/VocabList/assets/79140478/a1750fb5-5b48-4582-bc88-ea64e522623f)

![VocabList_KayıtOl0](https://github.com/BetulTugce/VocabList/assets/79140478/aa475704-5ee1-4f70-a1dd-616e22321f60)

## Screen Recordings

*User Portal*

https://github.com/BetulTugce/VocabList/assets/79140478/ad776aa9-ac5d-4e12-9830-a9689ab4f243

*Admin Dashboard*

https://github.com/BetulTugce/VocabList/assets/79140478/fb0593a3-6002-441f-bb03-5e5f8b07e071

## Technological Infrastructure and Configurations of the Project

I employed ASP.NET Core framework to build the backend of my application. For user management, I relied on the ASP.NET Core Identity framework. As you know, this enables us to handle user authentication, authorization, and role management efficiently. JWT (JSON Web Token) authentication was utilized for identity verification and authorization in the API. JWT-based authentication provides a secure and scalable authorization mechanism. MSSQL Server was used for data storage, I have created a relational database. Entity Framework Core facilitated database operations, making data management straightforward. To simplify code and enhance performance, I opted to utilize Stored Procedures for filtering. I implemented the repository pattern and unit of work pattern in my application. I decided to utilize Blazor framework for the user interface and MudBlazor library to design Blazor components.

## Database Diagram

![VocabListDb_Diagram](https://github.com/BetulTugce/VocabList/assets/79140478/fa0ccb14-cb6e-4e1c-8c6f-e7d9e70d01c0)
<!-- ![VocabListDb_Diagram](https://github.com/BetulTugce/VocabList/assets/79140478/7bb76234-e811-4512-a457-9a6a7a1adbd3) -->

### Relationships

- A user can have multiple word lists, a word list belongs to one user. (One-to-Many Relationship)
- A word list can contain multiple words, a word can belong to one word list. (One-to-Many Relationship)
- A word can contain multiple sentences, a sentence can belong to one word. (One-to-Many Relationship)
- A user can have multiple roles, a role can appear in more than one user. (Many-to-Many Relationship)
- An endpoint belongs to a menu, a menu can contain multiple endpoints. (One-to-Many Relationship)
- An endpoint can have more than one role, likewise a role can be associated with more than one endpoint. (Many-to-Many Relationship)

## Profile Image Upload Feature

The application supports profile image uploading for members. Members can upload profile pictures in JPG or PNG format, with a maximum file size of 10MB. Both the API and UserPortal project handle the validation for this feature. Users perform the image upload operation in the UserPortal project, but the uploaded images are stored in the `wwwroot/ProfileImages` folder in the API project. Only the file name (as a string) is stored in the database. The file name follows the structure of `(uploaded_file_name)_(random_4_digit_number).(file_extension)`. When users upload a new image or wish to delete existing ones, the `ProfileImagePath` column in the database is updated, and any previously uploaded image associated with the user is deleted from the `wwwroot/ProfileImages` folder. When a profile image is requested, the API responds with a byte array, which is then converted to base64 in the UserPortal project for displaying the profile picture.

## Installation

1. **Clone the project:**

    ```bash
    git clone https://github.com/BetulTugce/VocabList.git
    ```

2. **Navigate to the project directory:**

    ```bash
    cd VocabList
    ```

3. **Configure `appsettings.json` File:**
   
   Before running the project, make sure to configure the `appsettings.json` file in the projects. Update the `ConnectionStrings` section with your MSSQL Server connection string and fill in other settings such as tokens, mail configurations, and administrator credentials as specified.

4. **Perform Database Migration:**
   
   To apply database migration, follow these steps:
   - Right-click on the API project and select "Set as Startup Project" to make it the startup project.
   - Open Package Manager Console by navigating to Tools > NuGet Package Manager > Package Manager Console.
   - Choose `VocabList.Repository` as the default project.
   - In the Package Manager Console, execute the command `add-migration InitialCreate` to create a migration for the initial database schema.
   - After the migration is created, apply it to the database by running the command `update-database`.

5. **Create Stored Procedures:**
   
   If you plan to use the provided stored procedures, you need to create them in your MSSQL Server database. Follow the instructions in this file to create the necessary stored procedures.

6. **Run the project:**

   Right-click on the solution and select `Configure Startup Projects`. In the opened window, select `Multiple startup projects`, mark API, UI, and UserPortal projects as `Start`, then click `OK` and press `F5` to run the project.

These steps ensure that the project is properly configured and ready to run with the required database settings and stored procedures.

## Dependencies

- [Microsoft.AspNetCore.Authentication.JwtBearer](https://dotnet.microsoft.com/en-us/apps/aspnet) v7.0.14
- [Microsoft.AspNetCore.OpenApi](https://dotnet.microsoft.com/en-us/apps/aspnet) v7.0.14
- [Microsoft.EntityFrameworkCore.Design](https://learn.microsoft.com/tr-tr/ef/core/) v7.0.14
- [Microsoft.AspNetCore.Identity.EntityFrameworkCore](https://dotnet.microsoft.com/en-us/apps/aspnet) v7.0.14
- [Microsoft.EntityFrameworkCore.SqlServer](https://learn.microsoft.com/tr-tr/ef/core/) v7.0.14
- [Microsoft.EntityFrameworkCore.Tools](https://learn.microsoft.com/tr-tr/ef/core/) v7.0.14
- [AutoMapper.Extensions.Microsoft.DependencyInjection](https://automapper.org/) v12.0.1
- [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) v6.5.0
- [MudBlazor](https://github.com/MudBlazor/MudBlazor) v6.11.2
- [Microsoft.AspNetCore.Components.Authorization](https://dotnet.microsoft.com/en-us/apps/aspnet) v7.0.14
- [Blazored.LocalStorage](https://github.com/Blazored/LocalStorage) v4.4.0

## Configuration

The `appsettings.json` file is not included in the repository as it contains sensitive information, such as API keys. Instead, you should create your own `appsettings.json` file with the following structure for api project:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server_name;Database=your_db_name;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;"
  },
  "Token": {
    "Audience": "www.vocablist.com",
    "Issuer": "www.vocablistapi.com",
    "SecurityKey": "your_security_key"
  },
  "Mail": {
    "Username": "your_email",
    "Password": "your_password",
    "Host": "your_smtp_port_like_587"
  },
  "Administrator": {
    "Email": "your_email",
    "Name": "your_name",
    "Surname": "your_surname",
    "Username": "your_username",
    "Password": "your_password",
    "Role": "Admin",
    "Role2": "Member"
  }
}
```

And you should create your own `appsettings.json` file with the following structure for other projects:

```json
{
  "DetailedErrors": false,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ApiSettings": {
    "BaseUrl": "your_api_url"
  },
  "AllowedHosts": "*"
}
```

## Creating a Stored Procedure in MSSQL Server

To create a Stored Procedure in MSSQL Server, follow these simple steps:

1. Navigate to `Programmability` > `Stored Procedures` > `New` > `Stored Procedure`.
2. Copy one of the provided Stored Procedure below.
3. Paste it into the opened window.
4. Execute it by pressing `F5`.

That's it!

It gets the data from WordLists according to selected filtering options:

```sql
CREATE PROCEDURE GetFilteredWordLists
    @SearchString NVARCHAR(MAX),
    @Page INT,
    @Size INT,
    @Sort NVARCHAR(11) = 'UpdatedDate',
    @OrderBy NVARCHAR(4) = 'DESC',
    @AppUserId NVARCHAR(MAX),
    @TotalCount INT OUTPUT
AS
BEGIN
    -- Skiplenecek eleman sayısını belirler
    --DECLARE @Offset INT = (@Page - 1) * @Size;
	DECLARE @Offset INT = @Page * @Size;

    -- Sıralama sütununu belirler
    DECLARE @OrderByColumn NVARCHAR(50);

    SET @OrderByColumn = 
        CASE 
            WHEN @Sort = 'Name' THEN 'Name'
            WHEN @Sort = 'UpdatedDate' THEN 'ISNULL(UpdatedDate, CreatedDate)' -- Eğer UpdatedDate ise, öncelikle UpdatedDate'e göre sıralar, null ise CreatedDate'e göre sıralar
            ELSE 'CreatedDate'
        END;

    -- Toplam eleman sayısını alır
    SELECT @TotalCount = COUNT(*)
    FROM WordLists
    WHERE
        (@SearchString IS NULL 
        OR 
        (Name LIKE '%' + @SearchString + '%'))
        AND AppUserId = @AppUserId;

    -- Sorgu
    DECLARE @Sql NVARCHAR(MAX);

    SET @Sql = '
        SELECT *
        FROM WordLists
        WHERE
            -- NULL olan parametreleri kontrol et ve eşleşmeyi sağla
            (
                @SearchString IS NULL 
                OR 
                (Name LIKE ''%'' + @SearchString + ''%'')
            )
            AND AppUserId = @AppUserId
        ORDER BY ' + @OrderByColumn + ' ' + @OrderBy + '
        OFFSET @Offset ROWS FETCH NEXT @Size ROWS ONLY;
    ';

    EXEC sp_executesql @Sql, N'@SearchString NVARCHAR(MAX), @Offset INT, @Size INT, @AppUserId NVARCHAR(MAX)', @SearchString, @Offset, @Size, @AppUserId;
END;
```

This is another Stored Procedure for filtering operations that display the words associated with the selected list:

```sql
CREATE PROCEDURE GetFilteredWords
    @SearchString NVARCHAR(MAX),
    @Page INT,
    @Size INT,
    @Sort NVARCHAR(11) = 'UpdatedDate',
    @OrderBy NVARCHAR(4) = 'DESC',
    @WordListId INT,
    @AppUserId NVARCHAR(MAX),
    @TotalCount INT OUTPUT
AS
BEGIN
    -- Skiplenecek eleman sayısını belirler
    --DECLARE @Offset INT = (@Page - 1) * @Size;
	DECLARE @Offset INT = @Page * @Size;

    -- Sıralama sütununu belirler
    DECLARE @OrderByColumn NVARCHAR(50);

    SET @OrderByColumn = 
        CASE 
            WHEN @Sort = 'Value' THEN 'w.Value'
            WHEN @Sort = 'Description' THEN 'w.Description'
            WHEN @Sort = 'Type' THEN 'w.Type'
            WHEN @Sort = 'UpdatedDate' THEN 'ISNULL(w.UpdatedDate, w.CreatedDate)' -- Eğer UpdatedDate ise, öncelikle UpdatedDate'e göre sıralar, null ise CreatedDate'e göre sıralar
            ELSE 'w.CreatedDate'
        END;

    -- Toplam eleman sayısını alır
    SELECT @TotalCount = COUNT(*)
    FROM Words w
    INNER JOIN WordLists wl ON w.WordListId = wl.Id
    WHERE
        (@SearchString IS NULL 
        OR 
        (w.Value LIKE '%' + @SearchString + '%' 
        OR w.Description LIKE '%' + @SearchString + '%' 
        OR w.Type LIKE '%' + @SearchString + '%'))
        AND wl.Id = @WordListId
        AND wl.AppUserId = @AppUserId;

    -- Sorgu
    DECLARE @Sql NVARCHAR(MAX);

    SET @Sql = '
        SELECT w.*
        FROM Words w
        INNER JOIN WordLists wl ON w.WordListId = wl.Id
        WHERE
            -- NULL olan parametreleri kontrol et ve eşleşmeyi sağla
            (
                @SearchString IS NULL 
                OR 
                (w.Value LIKE ''%'' + @SearchString + ''%'' 
                OR w.Description LIKE ''%'' + @SearchString + ''%'' 
                OR w.Type LIKE ''%'' + @SearchString + ''%'')
            )
            AND wl.Id = @WordListId
            AND wl.AppUserId = @AppUserId
        ORDER BY ' + @OrderByColumn + ' ' + @OrderBy + '
        OFFSET @Offset ROWS FETCH NEXT @Size ROWS ONLY;
    ';

    EXEC sp_executesql @Sql, N'@SearchString NVARCHAR(MAX), @Offset INT, @Size INT, @WordListId INT, @AppUserId NVARCHAR(MAX)', @SearchString, @Offset, @Size, @WordListId, @AppUserId;
END;
```

## Support and Communication

If you encounter any issues or have feedback while using the project, feel free to [create an issue on GitHub](https://github.com/BetulTugce/VocabList/issues). Moreover, if you wish to support or contribute to the project, you can do so by starring the [GitHub repository](https://github.com/BetulTugce/VocabList) or making contributions to the codebase.
