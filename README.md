🌐 CosmosDB Support WebApp

🎯 Projektets formål

Formålet med dette projekt er at udvikle en .NET Blazor WebApp, der kan oprette og vise supporthenvendelser gemt i en Azure CosmosDB-database.
Applikationen demonstrerer integrationen mellem en webapplikation og en cloud-baseret NoSQL-database,


⚙️ Sådan oprettes CosmosDB-databasen

🚀 1. Login på Azure

Først logger du ind på din Azure-konto for at kunne udføre kommandoer:

- az login


🌐 2. Opret en Resource Group

- az group create --name CosmosSupport --location swedencentral


☁️ Opret Cosmos DB-konto 

- export DBACCOUNT="ibas-db-account-$RANDOM"
  export RESGRP="CosmosSupportRG"

  az cosmosdb create \
  --name $DBACCOUNT \
  --resource-group $RESGRP \
  --enable-free-tier true


🐘 Opret SQL-database

- export DATABASE="CosmosSupportDB"

  az cosmosdb sql database create \
  --account-name $DBACCOUNT \
  --resource-group $RESGRP \
  --name $DATABASE


🏗️ Opret SQL-container** (med partitionsnøglen `/category`)

- export CONTAINER="cosmossupport"

  az cosmosdb sql container create \
  --account-name $DBACCOUNT \
  --resource-group $RESGRP \
  --database-name $DATABASE \
  --name $CONTAINER \
  --partition-key-path "/category"

