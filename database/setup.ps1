param(
    [string]$DatabaseName = 'library_management',
    [string]$PgHost = 'localhost',
    [int]$PgPort = 5432,
    [string]$PgUser = 'postgres',
    [string]$AdminUsername = 'admin',
    [string]$AdminPassword = '1234',
    [string]$PsqlPath = 'C:\Program Files\PostgreSQL\18\bin\psql.exe'
)
$ErrorActionPreference = 'Stop'
if ($DatabaseName -notmatch '^[a-z][a-z0-9_]*$') { throw 'Use a lowercase database name with letters, digits, and underscores.' }
if ([string]::IsNullOrWhiteSpace($AdminUsername) -or [string]::IsNullOrEmpty($AdminPassword)) {
    throw 'Administrator username and password are required.'
}
$connectionArgs = @('-h', $PgHost, '-p', $PgPort, '-U', $PgUser, '-w', '-v', 'ON_ERROR_STOP=1')
$exists = & $PsqlPath @connectionArgs -d postgres -tAc "SELECT 1 FROM pg_database WHERE datname = '$DatabaseName'"
if ($LASTEXITCODE -ne 0) { throw 'Cannot connect to PostgreSQL. Set PGPASSWORD before running setup.' }
if ($exists -ne '1') {
    & $PsqlPath @connectionArgs -d postgres -c "CREATE DATABASE $DatabaseName"
    if ($LASTEXITCODE -ne 0) { throw 'Could not create the library database.' }
}
& $PsqlPath @connectionArgs -d $DatabaseName -f (Join-Path $PSScriptRoot 'schema.sql')
if ($LASTEXITCODE -ne 0) { throw 'Could not initialize library tables.' }

$salt = New-Object byte[] 16
$rng = [Security.Cryptography.RandomNumberGenerator]::Create()
try { $rng.GetBytes($salt) } finally { $rng.Dispose() }
$derive = [Security.Cryptography.Rfc2898DeriveBytes]::new(
    $AdminPassword, $salt, 210000, [Security.Cryptography.HashAlgorithmName]::SHA256)
try { $hash = $derive.GetBytes(32) } finally { $derive.Dispose() }
$stored = 'pbkdf2$210000$' + [Convert]::ToBase64String($salt) + '$' + [Convert]::ToBase64String($hash)
@'
INSERT INTO Librarian (Name, Username, Password)
VALUES ('Administrator', :'username', :'password')
ON CONFLICT (Username) DO NOTHING;
'@ | & $PsqlPath @connectionArgs -d $DatabaseName -v "username=$AdminUsername" -v "password=$stored"
if ($LASTEXITCODE -ne 0) { throw 'Could not create the administrator account.' }
Write-Output "Database $DatabaseName is ready. Existing accounts and data were preserved."
