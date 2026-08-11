# ---- build stage ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Restore before copying the rest so Docker caches NuGet across code-only changes.
COPY RetailECommerce.csproj ./
RUN dotnet restore RetailECommerce.csproj

COPY . .
RUN dotnet publish RetailECommerce.csproj -c Release -o /app/publish /p:UseAppHost=false

# ---- runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

# QuestPDF renders through SkiaSharp, which needs fontconfig and at least one
# installed font. The stock aspnet image ships neither, so PDF report export
# throws a native load error without this.
RUN apt-get update \
 && apt-get install -y --no-install-recommends libfontconfig1 fonts-dejavu-core \
 && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

# Shell form is required: Render injects $PORT at runtime, and the JSON array
# form would pass "${PORT}" through as a literal string without expanding it.
ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://0.0.0.0:${PORT:-10000} dotnet RetailECommerce.dll"]
