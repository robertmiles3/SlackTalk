FROM microsoft/dotnet:2.2.104-sdk-alpine3.8
WORKDIR /src

ARG VERSION
ARG VERSION_SUFFIX
ARG VERSION_FULL=$VERSION$VERSION_SUFFIX
ARG NUGET_KEY

COPY src/SlackTalk ./
RUN dotnet restore
RUN dotnet pack -p:Version=$VERSION_FULL -p:PackageVersion=$VERSION_FULL -p:AssemblyVersion=$VERSION -p:FileVersion=$VERSION -p:InformationalVersion=$VERSION_FULL -c Release -o /src/out
RUN dotnet nuget push /src/out/*.nupkg --source https://api.nuget.org/v3/index.json --api-key $NUGET_KEY