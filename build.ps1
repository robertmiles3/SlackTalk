#from https://github.com/jbogard/MediatR/blob/master/Build.ps1
function Exec
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}

if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

exec { & dotnet restore }

# PackageVersion set by appveyor.yml csproj patching
exec { & dotnet pack ".\src\SlackTalk\SlackTalk.csproj" -c Release -o ..\..\artifacts }  