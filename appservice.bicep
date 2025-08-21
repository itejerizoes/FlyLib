param location string
param prefix string
param storageAccountName string
param sqlConnectionStringSecretUri string
param keyVaultUri string

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: '${prefix}-plan'
  location: location
  sku: { name: 'B1', tier: 'Basic' }
}

resource webApp 'Microsoft.Web/sites@2022-03-01' = {
  name: '${prefix}-api'
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOCKER|097888d200d4f1b0e1da3f5b3f39eb9fe737fd589a15a0cbb74e27e4ffe2de23'
      appSettings: [
        { name: 'StorageAccount', value: storageAccountName }
        { name: 'KeyVaultUri', value: keyVaultUri }
        { name: 'SqlConnectionString__SecretUri', value: sqlConnectionStringSecretUri }
      ]
    }
    httpsOnly: true
  }
}

output webAppUrl string = webApp.defaultHostName