FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
EXPOSE 80

COPY *.sln .
COPY "/Brunsker.Integracao.Application/Brunsker.Integracao.Application.csproj" "/Brunsker.Integracao.Application/"
COPY "/Brunsker.Integracao.Domain/Brunsker.Integracao.Domain.csproj" "/Brunsker.Integracao.Domain/"
COPY "/Brunsker.Integracao.OracleAdapter/Brunsker.Integracao.OracleAdapter.csproj" "/Brunsker.Integracao.OracleAdapter/"
COPY "/Brunsker.Integracao.RabbitMQ/Brunsker.Integracao.RabbitMQ.csproj" "/Brunsker.Integracao.RabbitMQ/"
COPY "/Brunsker.Integracao.WorkService.Insert/Brunsker.Integracao.WorkService.Insert.csproj" "/Brunsker.Integracao.WorkService.Insert/"
COPY "/Brunsker.Integracao.WorkService.JOB/Brunsker.Integracao.WorkService.JOB.csproj" "/Brunsker.Integracao.WorkService.JOB/"

RUN dotnet restore "/Brunsker.Integracao.WorkService.JOB/Brunsker.Integracao.WorkService.JOB.csproj"

COPY . ./
WORKDIR /app/Brunsker.Integracao.WorkService.JOB
RUN dotnet publish -c Release -o publish 

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/Brunsker.Integracao.WorkService.JOB/publish ./
ENTRYPOINT ["dotnet", "Brunsker.Integracao.WorkService.JOB.dll"]

RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
