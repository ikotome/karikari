ARG PROJECT_NAME=MyApp

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source
ARG PROJECT_NAME
COPY ${PROJECT_NAME}/${PROJECT_NAME}.csproj ./${PROJECT_NAME}/
RUN dotnet restore "${PROJECT_NAME}/${PROJECT_NAME}.csproj"
COPY . .
WORKDIR "/source/${PROJECT_NAME}"
RUN dotnet build "${PROJECT_NAME}.csproj" -c Release -o /app/build

FROM build AS publish
ARG PROJECT_NAME
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet publish "${PROJECT_NAME}.csproj" -c Release -o /app/publish  \
    && dotnet tool install --global dotnet-ef --version 9.0.0 \
    && dotnet ef migrations bundle -o /app/publish/bundle \
    && chmod 755 /app/publish/bundle

FROM base AS final
ARG PROJECT_NAME
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/app/bundle"]