FROM microsoft/dotnet:2.2-sdk AS builder

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

WORKDIR /src
COPY TwitterAnalytics.Console/TwitterAnalytics.Console.csproj TwitterAnalytics.Console/
COPY TwitterAnalytics.Domain/TwitterAnalytics.Domain.csproj TwitterAnalytics.Domain/
COPY TwitterAnalytics.BusinessLogic/TwitterAnalytics.BusinessLogic.csproj TwitterAnalytics.BusinessLogic/
COPY TwitterAnalytics.DataAccess/TwitterAnalytics.DataAccess.csproj TwitterAnalytics.DataAccess/
RUN dotnet restore -nowarn:msb3202,nu1503 TwitterAnalytics.Console/TwitterAnalytics.Console.csproj
COPY . .
WORKDIR /src/TwitterAnalytics.Console

RUN dotnet publish TwitterAnalytics.Console.csproj -c Release -o /app -r linux-arm

FROM microsoft/dotnet:2.2-runtime-stretch-slim-arm32v7
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "TwitterAnalytics.Console.dll"]
