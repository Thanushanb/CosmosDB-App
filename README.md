How to make a CosmosDB
Login på Azure
1. az login
Opret Resource Group

2. az group create --name CosmosSupport --location swedencentral

Opret Cosmos DB-konto 

3. export DBACCOUNT="ibas-db-account-$RANDOM"
export RESGRP="CosmosSupportRG"

az cosmosdb create \
  --name $DBACCOUNT \
  --resource-group $RESGRP \
  --enable-free-tier true

Opret SQL-database

4. export DATABASE="CosmosSupportDB"

az cosmosdb sql database create \
  --account-name $DBACCOUNT \
  --resource-group $RESGRP \
  --name $DATABASE

Opret SQL-container** (med partitionsnøglen `/category`)

4. export CONTAINER="cosmossupport"

az cosmosdb sql container create \
  --account-name $DBACCOUNT \
  --resource-group $RESGRP \
  --database-name $DATABASE \
  --name $CONTAINER \
  --partition-key-path "/category"

