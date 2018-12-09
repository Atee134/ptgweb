FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
COPY . .

RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*

CMD ASPNETCORE_URLS=http://*:$PORT dotnet PtgWeb.dll