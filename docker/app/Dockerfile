ARG PROJECT_NAME=MyApp
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source
ARG PROJECT_NAME
ARG PROJECT_LIB_NAME
COPY ${PROJECT_LIB_NAME}/${PROJECT_LIB_NAME}.csproj ./${PROJECT_LIB_NAME}/
COPY ${PROJECT_NAME}/${PROJECT_NAME}.csproj ./${PROJECT_NAME}/
RUN dotnet restore ${PROJECT_NAME}

COPY ./${PROJECT_LIB_NAME}/. ./${PROJECT_LIB_NAME}/
COPY ./${PROJECT_NAME}/. ./${PROJECT_NAME}/
WORKDIR /source/${PROJECT_NAME}
RUN dotnet publish ${PROJECT_NAME}.csproj -c Release -o /app --use-current-runtime true

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0

ARG PROJECT_NAME
ENV PROJECT_NAME ${PROJECT_NAME}
# 日本語の設定
ENV LANG ja_JP.UTF-8  
ENV LANGUAGE ja_JP:ja  
ENV LC_ALL ja_JP.UTF-8
WORKDIR /app
COPY --from=build /app/ ./
CMD dotnet ${PROJECT_NAME}.dll