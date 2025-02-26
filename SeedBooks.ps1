# This script is used to test the endpoints
param(
    [string]$environment = "Development",
    [string]$launchProfile = "https-Development",
    [string]$connectionStringKey = "BooksDb",
    [bool]$dropDatabase = $false,
    [bool]$createDatabase = $false,
    [string] $baseUrl
)

$projectName = Get-ChildItem -Recurse -Filter "*.csproj" | Select-Object -First 1 | ForEach-Object { $_.Directory.Name }

# Get the base URL of the project
$launchSettings = Get-Content -LiteralPath ".\$projectName\Properties\launchSettings.json" | ConvertFrom-Json

if ([string]::IsNullOrEmpty($baseUrl)){
    $baseUrl = ($launchSettings.profiles.$launchProfile.applicationUrl -split ";")[0] # Can also set manually -> $baseUrl = "https://localhost:7253"
}
if ([string]::IsNullOrEmpty($baseUrl))
{
    Write-Error "Please provide a domain Url"
    Exit
}

#Install module SqlServer
if (y-not (Get-Module -ErrorAction Ignore -ListAvailable SqlServer)) {
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
    $myServer = Start-Process -FilePath "dotnet" -ArgumentList "run --launch-profile $launchProfile --project .\$projectName\$projectName.csproj" -WindowStyle Normal -PassThru 
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

$books = Get-Content -Path ".\bookinfos.json" | ConvertFrom-Json

$books | ForEach-Object { 
    $response = Invoke-RestMethod -Uri $apiUrl -Method Post -Body ($_ | ConvertTo-Json -Depth 3) -ContentType "application/json"
    Write-Host "Book posted successfully: $($response.Title)"
    $response
} | Format-Table -Property Title, Description, Rating, BooksInInventory, @{Name="Authors"; Expression={($_.Authors | ForEach-Object { "$($_.FirstName) $($_.LastName)" }) -join ", "}}

###### Post Books #######################################################################################

$apiBookUrl = "$baseUrl/api/Books"

Write-Host "Posting Books"

$physicalBooks = Get-Content -Path ".\physicalbooks.json" | ConvertFrom-Json
foreach ($physicalBook in $physicalBooks) {
    $physicalBookJsonData = $physicalBook | ConvertTo-Json -Depth 3
    $physicalBookResponse = Invoke-RestMethod -Uri $apiBookUrl -Method Post -Body $physicalBookJsonData -ContentType "application/json"

    Write-Host "Physical copy posted successfully for BookInfo ID: $($physicalBook.BookInfoId)"
    $physicalBookResponse | Format-Table -Property Isbn, Edition, ReleaseYear, BookInfoId
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

Stop-Process -InputObject $myServer

