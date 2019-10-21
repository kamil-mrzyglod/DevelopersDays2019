@echo off

if [%1]==[] (
    echo namespace: first parameter required
    goto :error
)

if [%2]==[] (
    echo deployment name: second parameter required
    goto :error
)

if "%3"=="local" (
    set kubemount=/etc/kubernetes
) ELSE (
    set kubemount="C:\%HomePath%\.kube"
)

CALL :to_linux_path %~dp0,unicornlinuxpath
CALL :to_linux_path C:\%HOMEPATH%\.nuget\packages\,usernugetlinuxpath
CALL :to_linux_path "C:\Program Files\dotnet\sdk\NuGetFallbackFolder",systemnugetlinuxpath

echo Stopping any old debug containers
docker stop %2

echo Starting Telepresence
docker run^
 --net=host^
 --rm^
 -it^
 -v %kubemount%:/kube^
 -v /:/host^
 -v /tmp:/tmp^
 -v /var/run/docker.sock:/var/run/docker.sock^
 unicorndeveuwacr.azurecr.io/telepresence^
 --docker-mount=/t9s^
 --namespace=%1^
 --swap-deployment %2^
 --docker-run^
    --name %2^
    --rm^
    -v "%unicornlinuxpath%:/src"^
    -v "%usernugetlinuxpath%:/root/.nuget/fallbackpackages"^
    -v "%systemnugetlinuxpath%:/root/.nuget/fallbackpackages2"^
    -v /home/vsdbg:/vsdbg^
    -e ASPNETCORE_ENVIRONMENT=Development^
    -p 5002:80^
    --entrypoint bash^
    mcr.microsoft.com/dotnet/core/sdk:2.2^
    /src/debug-entrypoint.sh

EXIT /B %ERRORLEVEL% 

:to_linux_path
pushd %~1
set evaluated=%CD%
popd

set linuxpath=%evaluated:C:=/host_mnt/c%
set linuxpath=%linuxpath:D:=/host_mnt/d%
set linuxpath=%linuxpath:\=/%
set %~2=%linuxpath%
EXIT /B 0