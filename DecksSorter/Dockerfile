FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR /DecksSorter
COPY bin/Release/net5.0/publish/ .
ENTRYPOINT ["dotnet", "DecksSorter.dll"]
