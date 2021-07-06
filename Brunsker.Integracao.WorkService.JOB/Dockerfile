FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
COPY . .
ENTRYPOINT ["dotnet", "Brunsker.Integracao.WorkService.JOB.dll"]

#Instalando Dependencias
RUN apt-get update && apt-get install -y \
    vim \
    tmux \
    libaio1 \
    && rm -rf /var/lib/apt/lists/*


RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone


#Variaveis de ambiente Oracle
ENV ORACLE_SID=BRUNSKER
ENV ORACLE_BASE=/app/instantclient
ENV ORACLE_HOME=/app/instantclient
ENV NLS_LANG=AMERICAN_AMERICA.WE8ISO8859P1
ENV TNS_ADMIN=/app/instantclient
ENV LD_LIBRARY_PATH=/app/instantclient
ENV PATH=/app/instantclient:$PATH
ENV TZ=America/Sao_Paulo




