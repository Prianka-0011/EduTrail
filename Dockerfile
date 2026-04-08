FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
RUN apt-get update
RUN apt-get update && \
    apt-get install -y nodejs npm
WORKDIR /edu-trail
COPY . .
# restore entire /EduTrail.sln
RUN dotnet restore ./EduTrail.sln 
# dotnet build entire /EduTrail.sln
RUN dotnet build ./EduTrail.sln -c Debug


WORKDIR ./src/WebSpa
RUN npm install -g yarn
RUN  yarn install
RUN yarn build

WORKDIR /edu-trail 
EXPOSE 2029
ENTRYPOINT ["dotnet", "watch", "--project", "src/EduTrail.Backend/EduTrail.API/EduTrail.API.csproj", "run", "--urls=https://0.0.0.0:2029"]