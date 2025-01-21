# This script is used to test the endpoints
param(
    [string]$environment = "Development",
    [string]$launchProfile = "https-Development",
    [string]$connectionStringKey = "BooksDb",
    [bool]$dropDatabase = $false,
    [bool]$createDatabase = $false,
    [string] $baseUrl = "https://localhost:7086"
)

$projectName = Get-ChildItem -Recurse -Filter "*.csproj" | Select-Object -First 1 | ForEach-Object { $_.Directory.Name }

# Get the base URL of the project
$launchSettings = Get-Content -LiteralPath ".\$projectName\Properties\launchSettings.json" | ConvertFrom-Json
$baseUrl = ($launchSettings.profiles.$launchProfile.applicationUrl -split ";")[0] # Can also set manually -> $baseUrl = "https://localhost:7253"

#Install module SqlServer
if (-not (Get-Module -ErrorAction Ignore -ListAvailable SqlServer)) {
    Write-Verbose "Installing SqlServer module for the current user..."
    Install-Module -Scope CurrentUser SqlServer -ErrorAction Stop
}
Import-Module SqlServer

# Set the environment variable
$env:ASPNETCORE_ENVIRONMENT = $environment



# Read the connection string from appsettings.Development.json
$appSettings = Get-Content ".\$projectName\appsettings.$environment.json" | ConvertFrom-Json
$connectionString = $appSettings.ConnectionStrings.$connectionStringKey
Write-Host "Database Connection String: $connectionString" -ForegroundColor Blue


# Get the database name from the connection string
if ($connectionString -match "Database=(?<dbName>[^;]+)")
{
    $databaseName = $matches['dbName']
    Write-Host "Database Name: $databaseName" -ForegroundColor Blue
}else{
    Write-Host "Database Name not found in connection string" -ForegroundColor Red
    exit
}


# Check if the database exists
$conStringDbExcluded = $connectionString -replace "Database=[^;]+;", ""
$queryDbExists = Invoke-Sqlcmd -ConnectionString $conStringDbExcluded -Query "Select name FROM sys.databases WHERE name='$databaseName'"
if($queryDbExists){
    if($dropDatabase -or (Read-Host "Do you want to drop the database? (y/n)").ToLower() -eq "y") { 

        # Drop the database
        Invoke-Sqlcmd -ConnectionString $connectionString -Query  "USE master;ALTER DATABASE $databaseName SET SINGLE_USER WITH ROLLBACK IMMEDIATE;DROP DATABASE $databaseName;"
        Write-Host "Database $databaseName dropped." -ForegroundColor Green
    }
}

# Create the database from the model
if(Select-String -LiteralPath ".\$projectName\Program.cs" -Pattern "EnsureCreated()"){
    Write-Host "The project uses EnsureCreated() to create the database from the model." -ForegroundColor Yellow
} else {
    if($createDatabase -or (Read-Host "Should dotnet ef migrate and update the database? (y/n)").ToLower() -eq "y") { 

        dotnet ef migrations add "UpdateModelFromScript_$(Get-Date -Format "yyyyMMdd_HHmmss")" --project ".\$projectName\$projectName.csproj"
        dotnet ef database update --project ".\$projectName\$projectName.csproj"
    }
}

# Run the application
if((Read-Host "Start the server from Visual studio? (y/n)").ToLower() -ne "y") { 
    Start-Process -FilePath "dotnet" -ArgumentList "run --launch-profile $launchProfile --project .\$projectName\$projectName.csproj" -WindowStyle Normal    
    Write-Host "Wait for the server to start..." -ForegroundColor Yellow 
}

# Continue with the rest of the script
Read-Host "Press Enter to continue when the server is started..."



### =============================================================
### =============================================================
### =============================================================

##### Post BookInfo ############################################################################

$apiUrl = "$baseUrl/api/BookInfos"

Write-Host "Posting BookInfos"

$books = @(
    @{
        Title = "Harry Potter and the Sorcerer's Stone"
        Description = "A young boy discovers he's a wizard."
        Rating = 9
        BooksInInventory = 5
        Authors = @(
            @{
                FirstName = "J.K."
                LastName = "Rowling"
            }
        )
    },
    @{
        Title = "The Hobbit"
        Description = "A hobbit embarks on a grand adventure."
        Rating = 8
        BooksInInventory = 3
        Authors = @(
            @{
                FirstName = "J.R.R."
                LastName = "Tolkien"
            }
        )
    },
    @{
        Title = "1984"
        Description = "A dystopian novel about totalitarianism."
        Rating = 10
        BooksInInventory = 2
        Authors = @(
            @{
                FirstName = "George"
                LastName = "Orwell"
            }
        )
    },
    @{
        Title = "To Kill a Mockingbird"
        Description = "A story about racism and injustice."
        Rating = 9
        BooksInInventory = 4
        Authors = @(
            @{
                FirstName = "Harper"
                LastName = "Lee"
            }
        )
    },
    @{
        Title = "Pride and Prejudice"
        Description = "A romance novel set in early 19th-century England."
        Rating = 8
        BooksInInventory = 6
        Authors = @(
            @{
                FirstName = "Jane"
                LastName = "Austen"
            }
        )
    }
)

$books | ForEach-Object { 
    $response = Invoke-RestMethod -Uri $apiUrl -Method Post -Body ($_ | ConvertTo-Json -Depth 3) -ContentType "application/json"
    Write-Host "Book posted successfully: $($response.Title)"
    $response
} | Format-Table -Property Title, Description, Rating, BooksInInventory, @{Name="Authors"; Expression={($_.Authors | ForEach-Object { "$($_.FirstName) $($_.LastName)" }) -join ", "}}

##### Get BookInfos ##########################################################################################

Write-Host "Getting BookInfos"
Invoke-RestMethod -Uri $apiUrl -Method Get | Format-Table -Property Title, Description, Rating, BooksInInventory, @{Name="Authors"; Expression={($_.Authors | ForEach-Object { "$($_.FirstName) $($_.LastName)" }) -join ", "}}

##### Get Book #############################################################################################

Write-Host "Getting specific BookInfo"
Invoke-RestMethod -Uri "$apiUrl/1" -Method Get | Format-Table -Property Title, Description, Rating, BooksInInventory, @{Name="Authors"; Expression={($_.Authors | ForEach-Object { "$($_.FirstName) $($_.LastName)" }) -join ", "}}

##### Post Authors ################################################################################################ 

$authorApiUrl = "$baseUrl/api/Authors"

$authors = @(
    @{
        FirstName = "Ulf"
        LastName = "Lundell"
        BookInfoIdLinks = @(1, 2)
    },
    @{ 
        FirstName = "George"
        LastName = "Orwell"
        BookInfoIdLinks = @(3)
    }
)

foreach ($author in $authors) {
    try {
        $authorJsonData = $author | ConvertTo-Json -Depth 3
        $authorResponse = Invoke-RestMethod -Uri $authorApiUrl -Method Post -Body $authorJsonData -ContentType "application/json"

        Write-Host "Author posted successfully: $($author.FirstName) $($author.LastName)"
        $authorResponse | Format-Table -Property FirstName, LastName, BookInfoIdLinks
    }
    catch {
        Write-Host "Error posting author: $($author.FirstName) $($author.LastName)"
        Write-Host "Error message: $($_.Exception.Message) Author already exists."
    }
}

###### Post Books #######################################################################################

$apiBookUrl = "$baseUrl/api/Books"

Write-Host "Posting Books"

$physicalBooks = @(
    @{
        Isbn = "978-3-16-148410-0"
        Edition = 1
        ReleaseYear = 1997
        BookInfoId = 1 
    },
    @{
        Isbn = "978-1-56619-909-4"
        Edition = 2
        ReleaseYear = 1945
        BookInfoId = 2  
    },
    @{
        Isbn = "978-0-7432-7356-5"
        Edition = 3
        ReleaseYear = 1949
        BookInfoId = 3  
    },
    @{
        Isbn = "978-0-7432-7356-6"
        Edition = 4
        ReleaseYear = 1960
        BookInfoId = 4  
    },
    @{
        Isbn = "978-1-56619-909-5"
        Edition = 5
        ReleaseYear = 1813
        BookInfoId = 5  
    }
)

foreach ($physicalBook in $physicalBooks) {
    $physicalBookJsonData = $physicalBook | ConvertTo-Json -Depth 3
    $physicalBookResponse = Invoke-RestMethod -Uri $apiBookUrl -Method Post -Body $physicalBookJsonData -ContentType "application/json"

    Write-Host "Physical copy posted successfully for BookInfo ID: $($physicalBook.BookInfoId)"
    $physicalBookResponse | Format-Table -Property Isbn, Edition, ReleaseYear, BookInfoId
}


####### Post Customers ##################################################################################

$customerApiUrl = "$baseUrl/api/Customers"

Write-Host "Posting Customers"

$customers = @(
    @{
        FirstName = "John"
        LastName = "Doe"
        Address = "123 Main St"
        BirthDate = "1990-05-14"
    },
    @{
        FirstName = "Jane"
        LastName = "Smith"
        Address = "456 Oak St"
        BirthDate = "1985-11-22"
    },
    @{
        FirstName = "Alice"
        LastName = "Johnson"
        Address = "789 Pine St"
        BirthDate = "1992-08-30"
    }
)

foreach ($customer in $customers) {
    $customerJsonData = $customer | ConvertTo-Json -Depth 3
    $customerResponse = Invoke-RestMethod -Uri $customerApiUrl -Method Post -Body $customerJsonData -ContentType "application/json"

    Write-Host "Customer posted successfully: $($customer.FirstName) $($customer.LastName)"
    $customerResponse | Format-Table -Property FirstName, LastName, Address, BirthDate
}

##### Post Loans ##################################################################################

$loanApiUrl = "$baseUrl/api/Loans" 

Write-Host "Posting Loans"

$loans = @(
    @{
        BookId = 1  
        LoanCardId = 1 
    },
    @{
        BookId = 2 
        LoanCardId = 2  
    },
    @{
        BookId = 3  
        LoanCardId = 3 
    },
    @{
        BookId = 3
        LoanCardId = 1  
    }
)

foreach ($loan in $loans) {
    try {
        $loanJsonData = $loan | ConvertTo-Json -Depth 3
        $loanResponse = Invoke-RestMethod -Uri $loanApiUrl -Method Post -Body $loanJsonData -ContentType "application/json"

        Write-Host "Loan created successfully: BookId $($loan.BookId) - LoanCardId $($loan.LoanCardId)"
        $loanResponse | Format-Table -Property LoanId, BookId, bookTitle, CustomerId, CustomerName, LoanDate, expectedReturnDate, IsLate, Returned
    }
    catch {
        Write-Host "Error posting loan: $($loan.BookId) - LoanCardId $($loan.LoanCardId)" 
        Write-Host "Error message: $($_.Exception.Message) Unable to loan book that is already loaned."
    }
}

###### Return Loans ################################################################################

Write-Host "Getting BookInfos"
$returnApiUrl = "$baseUrl/api/Loans/return" 
for ($i = 1; $i -lt 4; $i++){
    $loanResponse = Invoke-RestMethod -Uri "$returnApiUrl/$i" -Method Patch
    Write-Host "Loan returned successfully. LoanId: $i"
    $loanResponse | Format-Table -Property LoanId, BookId, bookTitle, CustomerId, CustomerName, LoanDate, expectedReturnDate, IsLate, Returned

}

###### Delete Customers ##################################################################################

Write-Host "Deleting Customers"
for ($i = 1; $i -le 3; $i++) {
    try {
        Invoke-RestMethod -Uri "$customerApiUrl/$i" -Method Delete
        Write-Host "Customer with ID $i deleted successfully."
    } catch {
        Write-Host "Failed to delete Customer with ID $i. Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}

#############
Write-Host "Verifying LoanCards cascading delete"
$loanCardResponse = Invoke-RestMethod -Uri "$baseUrl/api/LoanCards" -Method Get
if ($null -eq $loanCardResponse-or $loanCardResponse.Count -eq 0) {
    Write-Host "No books found in the database."
} else {
    $loanCardResponse | Format-Table -Property Id, LoanCardNumber, Customer
}
#############
Write-Host "Verifying BookLoanCards cascading delete"
$bookLoanCardResponse = Invoke-RestMethod -Uri "$baseUrl/api/BookLoanCards" -Method Get
if ($null -eq $bookLoanCardResponse -or $bookLoanCardResponse.Count -eq 0) {
    Write-Host "No books found in the database."
} else {
    $bookLoanCardResponse | Format-Table -Property BookId, LoanCardId
}


######## Delete books ###########################################################################

Write-Host "Deleting BookInfos"
for ($i = 1; $i -le 5; $i++) {
    try {
        Invoke-RestMethod -Uri "$apiUrl/$i" -Method Delete
        Write-Host "BookInfo with ID $i deleted successfully."
    } catch {
        Write-Host "Failed to delete Book with ID $i. Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "Verifying Books cascading delete"
$bookResponse = Invoke-RestMethod -Uri "$baseUrl/api/Books" -Method Get
if ($null -eq $bookResponse -or $bookResponse.Count -eq 0) {
    Write-Host "No books found in the database."
} else {
    $bookResponse | Format-Table -Property BookTitle, Id, Isbn, Edition, ReleaseYear, IsAvailable
}

#####

Write-Host "Verifying BookInfoAuthor cascading delete"
$bookInfoAuthorResponse = Invoke-RestMethod -Uri "$baseUrl/api/BookInfoAuthors" -Method Get
if ($null -eq $bookInfoAuthorResponse -or $bookInfoAuthorResponse.Count -eq 0) {
    Write-Host "No books found in the database."
} else {
    $bookInfoAuthorResponse | Format-Table -Property Id, BookInfoId, BookTitle, AuthorId, AuthorName
}

######## Delete Authors ###################################################################

Write-Host "Deleting Authors"
for ($i = 1; $i -le 6; $i++) {
    try {
        Invoke-RestMethod -Uri "$authorApiUrl/$i" -Method Delete
        Write-Host "Author with ID $i deleted successfully."
    } catch {
        Write-Host "Failed to delete Author with ID $i. Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}

####### DROP DATABASE ######################################
if ($dropDatabase -or (Read-Host "Do you want to drop the database after all actions are complete? (y/n)").ToLower() -eq "y") {
    # Drop the database
    $dropMethod = Read-Host "Do you want to drop the database using SQL (1) or EF Core (2)? Enter 1 or 2"

if ($dropMethod -eq "1") {
    Invoke-Sqlcmd -ConnectionString $connectionString -Query "USE master;ALTER DATABASE $databaseName SET SINGLE_USER WITH ROLLBACK IMMEDIATE;DROP DATABASE $databaseName;"
    Write-Host "Database $databaseName dropped via SQL at the end of operations." -ForegroundColor Green
    dotnet ef migrations remove --project ".\$projectName\$projectName.csproj"
    Write-Host "Migration history cleaned." -ForegroundColor Green
} elseif ($dropMethod -eq "2") {
    dotnet ef database drop --force
    Write-Host "Database dropped via EF Core." -ForegroundColor Green
    dotnet ef migrations remove --project ".\$projectName\$projectName.csproj"
    Write-Host "Migration history cleaned." -ForegroundColor Green
} else {
    Write-Host "Invalid option selected. Please choose either 1 for SQL or 2 for EF Core." -ForegroundColor Red
}
    
}

Write-Host "If you want to run the script again, don't forget to close the projects terminal window."


# Define the list of movie data
# $movies = @(
#     @{ Title = "Inception"; ReleaseYear = 2010 },
#     @{ Title = "The Dark Knight"; ReleaseYear = 2008 },
#     @{ Title = "The Shawshank Redemption"; ReleaseYear = 1994 },
#     @{ Title = "The Godfather"; ReleaseYear = 1972 }
# )



# $movies | Foreach-Object { Invoke-RestMethod -Uri $apiUrl -Method Post -Body ($_ | ConvertTo-Json) -ContentType "application/json" } | Format-Table


