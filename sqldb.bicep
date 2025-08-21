param location string
param prefix string
param keyVaultId string

resource sqlServer 'Microsoft.Sql/servers@2022-02-01-preview' = {
  name: '${prefix}-sqlsrv'
  location: location
  properties: {
    administratorLogin: 'flylibadmin'
    administratorLoginPassword: 'P@ssw0rd123!' // Reemplazar por referencia a KeyVault en producción
  }
}

resource sqlDb 'Microsoft.Sql/servers/databases@2022-02-01-preview' = {
  name: '${sqlServer.name}/flylibdb'
  location: location
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
  }
}

resource secret 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: keyVaultId
  name: 'SqlConnectionString'
  properties: {
    value: 'Server=tcp:${sqlServer.name}.database.windows.net,1433;Initial Catalog=flylibdb;User ID=flylibadmin;Password=P@ssw0rd123!;'
  }
}

output sqlConnectionStringSecretUri string = secret.properties.secretUri