💻 Opret Azure Cosmos DB (SQL API) med Azure CLI

Denne guide viser, hvordan du opretter en komplet Azure Cosmos DB-løsning med Resource Group, Cosmos DB-konto, SQL Database og SQL Container.

🎯 Formålet er at demonstrere forståelse for opsætning, struktur og best practices i Azure-miljøer.


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

