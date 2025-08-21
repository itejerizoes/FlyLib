param location string = resourceGroup().location
param prefix string = 'flylib'

module storage 'modules/storage.bicep' = {
  name: 'storage'
  params: {
    location: location
    prefix: prefix
  }
}

module keyvault 'modules/keyvault.bicep' = {
  name: 'keyvault'
  params: {
    location: location
    prefix: prefix
  }
}

module sqldb 'modules/sqldb.bicep' = {
  name: 'sqldb'
  params: {
    location: location
    prefix: prefix
    keyVaultId: keyvault.outputs.keyVaultId
  }
}

module appservice 'modules/appservice.bicep' = {
  name: 'appservice'
  params: {
    location: location
    prefix: prefix
    storageAccountName: storage.outputs.storageAccountName
    sqlConnectionStringSecretUri: sqldb.outputs.sqlConnectionStringSecretUri
    keyVaultUri: keyvault.outputs.keyVaultUri
  }
}