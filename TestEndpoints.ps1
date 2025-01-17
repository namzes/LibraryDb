param(
    [string] $baseUrl = "https://localhost:7086"
)

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

Write-Host "Get BookInfo"
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

$apiUrl = "$baseUrl/api/Books"

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
    $physicalBookResponse = Invoke-RestMethod -Uri $apiUrl -Method Post -Body $physicalBookJsonData -ContentType "application/json"

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
$returnResponse | Invoke-RestMethod -Uri "$returnApiUrl/1" -Method Patch
Write-Host $returnResponse
$returnResponse | Invoke-RestMethod -Uri "$returnApiUrl/2" -Method Patch
Write-Host $returnResponse
$returnResponse | Invoke-RestMethod -Uri "$returnApiUrl/3" -Method Patch 
Write-Host $returnResponse














# Define the list of movie data
# $movies = @(
#     @{ Title = "Inception"; ReleaseYear = 2010 },
#     @{ Title = "The Dark Knight"; ReleaseYear = 2008 },
#     @{ Title = "The Shawshank Redemption"; ReleaseYear = 1994 },
#     @{ Title = "The Godfather"; ReleaseYear = 1972 }
# )



# $movies | Foreach-Object { Invoke-RestMethod -Uri $apiUrl -Method Post -Body ($_ | ConvertTo-Json) -ContentType "application/json" } | Format-Table


