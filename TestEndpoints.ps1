# This script is used to test the endpoints
param(
    [string]$environment = "Development",
    [string]$launchProfile = "https-Development",
    [string]$connectionStringKey = "BooksDb",
    [bool]$dropDatabase = $false,
    [bool]$createDatabase = $false
)

# Get the project name
$projectName = Get-ChildItem -Recurse -Filter "*.csproj" | Select-Object -First 1 | ForEach-Object { $_.Directory.Name } # Projectname can also be set manually

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
#
#
$baseUrl = "https://localhost:7069"
#####################################
##
##Post authors API
#$apiAuthorEndpoint = "$baseUrl/api/Authors"
#$authors = @(
#    @{
#        FirstName = "James"
#        LastName = "Clear"
#    },
#    @{
#        FirstName = "Yuval Noah"
#        LastName = "Harari"
#    },
#    @{
#        FirstName = "George"
#        LastName = "Orwell"
#    },
#    @{
#        FirstName = "F. Scott"
#        LastName = "Fitzgerald"
#    },
#    @{
#        FirstName = "Eckhart"
#        LastName = "Tolle"
#    }
#)
#
#
#foreach ($author in $authors) {
#    
#        # Convert the author to JSON format
#        $jsonBody = $author | ConvertTo-Json -Depth 1
#
#        # Send the POST request
#        $response = Invoke-RestMethod -Uri $apiAuthorEndpoint -Method Post -Body $jsonBody -ContentType "application/json"
#
#        # Log the success
#        Write-Host "Successfully created author:" $author.Id $author.FirstName $author.LastName
#        Write-Host "Response:" $response
#}

##################################

##Post books API
#
# Define the API endpoint
#$apiBookEndPoint = "$baseUrl/api/Books"  
# 
#$books = @(
#    @{
#        Title = "Atomic Habits"
#        ReleaseYear = 2018
#        ISBN = "978-0-7352-1123-4"
#        Rating = 3
#        AuthorsIds = @(1, 2)  
#    },
#    @{
#        Title = "Sapiens: A Brief History of Humankind"
#        ReleaseYear = 2011
#        ISBN = "978-0-06-231611-0"
#        Rating = 4
#        AuthorsIds = @(2)  
#    },
#    @{
#        Title = "1984"
#        ReleaseYear = 1949
#        ISBN = "978-0-452-28423-4"
#        Rating = 4
#        AuthorsIds = @(3)  
#    },
#    @{
#        Title = "The Great Gatsby"
#        ReleaseYear = 1925
#        ISBN = "978-0-7432-7356-5"
#        Rating = 5
#        AuthorsIds = @(4) 
#    },
#    @{
#        Title = "The Power of Now"
#        ReleaseYear = 1997
#        ISBN = "978-1-57731-480-6"
#        Rating = 3
#        AuthorsIds = @(5) 
#    }
#)
#
#foreach ($book in $books) {
#    try {
#        $jsonBody = $book | ConvertTo-Json  
#
#        $response = Invoke-RestMethod -Uri $apiBookEndPoint -Method Post -Body $jsonBody -ContentType "application/json"
#
#        Write-Host "Successfully created book: $($book.Title)"
#        Write-Host "Response: $($response)"
#    } catch {
#        Write-Host "Failed to create book: $($book.Title)"
#        Write-Host "Error: $($_.Exception)"
#    }
#}

#################################

#Get all books

#$allBooksResponse = Invoke-RestMethod -Uri "$baseUrl/api/Books" -Method Get
#
#$allBooksResponse | Format-Table -AutoSize

##################################

##Get book by ID
#$bookId = 1
#$bookIdResponse = Invoke-RestMethod -Uri "$baseUrl/api/Books/$bookId" -Method Get
#
#$bookIdResponse | Format-Table -AutoSize

##################################
#Create Copy

#$copies = @(
#    @{BookId = 1},
#    @{BookId = 1},
#    @{BookId = 2},
#    @{BookId = 2},
#    @{BookId = 3},
#    @{BookId = 4},
#    @{BookId = 5}
#)
#
#foreach ($copy in $copies) {
#    $jsonBody = $copy | ConvertTo-Json 
#
#    $response = Invoke-RestMethod -Uri $baseUrl/api/Copies/ -Method Post -Body $jsonBody -ContentType "application/json"
#    
#    Write-Host "Successfully created copy:" $copy.Id 
#    Write-Host "Response:" $response
#}
#################################
#Create Borrowers

#$borrowers = @(
#    @{
#        FirstName = "Alexander"
#        LastName = "Hoang"
#    },
#    @{
#        FirstName = "Marshall"
#        LastName = "Mathers"
#    },
#    @{
#        FirstName = "Micheal"
#        LastName = "Jacksson"
#    }
#)
#
#foreach ($borrower in $borrowers) {
#    $jsonBody = $borrower | ConvertTo-Json
#
#    $response = Invoke-RestMethod -Uri $baseUrl/api/Borrowers/ -Method Post -Body $jsonBody -ContentType "application/json"
#    Write-Host "Successfully created borrower:" $borrower.Id 
#    Write-Host "Response:" $response
#}
##################################

#Create BookLoan

#$bookLoan = @{
#    CopyId = 2
#    BorrowerId = 2
#}
#
#$bookLoanBody = $bookLoan | ConvertTo-Json
#
#$bookLoanResponse = Invoke-RestMethod -Uri $baseUrl/api/BookLoans/ -Method Post -Body $bookLoanBody -ContentType "application/json"
#
#Write-Host "Successfully created bookloan:" $bookLoanBody.Id 
#Write-Host "Response:" $bookLoanResponse
##################################

#Check Loans

#$borrowerLoanCard = 90001
#
#$borrowerLoansResponse = Invoke-RestMethod -Uri $baseUrl/api/BookLoans/borrower/$borrowerLoanCard/loans -Method Get -ContentType "application/json"
#$borrowerLoansResponse | Format-Table  


##################################

#Return BookLoan

#$bookLoanId = 2
#
#$returnBookLoanBody = $bookLoanId | ConvertTo-Json
#
#$loanReturnResponse = Invoke-RestMethod -Uri $baseUrl/api/BookLoans/$bookLoanId/returnloan -Method Put -Body $returnBookLoanBody -ContentType "application/json"
#Write-Host "Successfully returned the loan:"
#Write-Host "Response:" $loanReturnResponse

##################################

#Delete Borrower
$deleteBorrowerId = 1

$delBorrowerResponse = Invoke-RestMethod -Uri $baseUrl/api/Borrowers/$deleteBorrowerId -Method Delete -ContentType "application/json"
Write-Host "Successfully deleted the borrower:" $delBorrowerResponse
#################################
#Delete Copy
$deleteCopyId = 5

$delCopyResponse = Invoke-RestMethod -Uri $baseUrl/api/Copies/$deleteCopyId -Method Delete -ContentType "application/json"
Write-Host "Successfully deleted the copy:" $delCopyResponse.StatusCode
#Delete Book
#################################
$deleteBookId = 4

$delBookResponse = Invoke-RestMethod -Uri $baseUrl/api/Books/$deleteBookId -Method Delete -ContentType "application/json"
Write-Host "Successfully deleted the book:" $delBookResponse.StatusCode
#################################
#Delete Author
$deleteAuthorId = 1
$delAuthorResponse = Invoke-RestMethod -Uri $baseUrl/api/Authors/$deleteAuthorId -Method Delete -ContentType "application/json"
Write-Host "Successfully deleted the Author:" $delAuthorResponse.StatusCode