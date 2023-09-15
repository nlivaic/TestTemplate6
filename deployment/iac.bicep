param environment string
param projectName string
param db_user string
@secure()
param db_password string
param location string = resourceGroup().location

// Object containing a mapping for location / region code
var regionCodes = {
  westeurope: 'we'
}   
// remove space and make sure all lower case
var sanitizedLocation = toLower(replace(location, ' ', ''))
// get the region code
var regionCode = regionCodes[sanitizedLocation]
// naming convention
var sql = 'sql'
var db = 'db'
var plan = 'plan'
var web = 'web'
var law = 'law'
var ai = 'ai'
var sb = 'sb'

var baseName = '${regionCode}${environment}${projectName}'

var sqlserver_name = toLower('${baseName}${sql}1')
var sqldb_name = toLower('${baseName}${sql}1${db}1')
var app_service_plan_name = toLower('${baseName}${plan}1')
var appService_web_name = toLower('${baseName}${web}1')
var log_analytics_name = toLower('${baseName}${law}1')
var app_insight_name = toLower('${baseName}${ai}1')
var service_bus_name = toLower('${baseName}${sb}1')
var service_bus_RootManageSharedAccessKey_name = 'RootManageSharedAccessKey'
var service_bus_ReadWritePolicy_name = 'ReadWritePolicy'

var db_connection_string_env_var_name = 'TestTemplate6DbConnection'
var db_user_env_var_name = 'DB_USER'
var db_password_env_var_name = 'DB_PASSWORD'
var applicationinsights_connection_string_env_var_name = 'APPLICATIONINSIGHTS_CONNECTION_STRING'
var messageBroker_connectionStrings_env_var_name = 'MessageBroker'

resource sqlserver 'Microsoft.Sql/servers@2022-11-01-preview' = {
  name: sqlserver_name
  location: location
  properties: {
    administratorLogin: db_user
    administratorLoginPassword: db_password
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    restrictOutboundNetworkAccess: 'Disabled'
  }

  resource sqlserver_AllowAllWindowsAzureIps 'firewallRules@2022-11-01-preview' = {
    name: 'AllowAllWindowsAzureIps'
    properties: {
      startIpAddress: '0.0.0.0'
      endIpAddress: '0.0.0.0'
    }
  }
  
  resource sqlDb 'databases@2022-05-01-preview' = {
    name: sqldb_name
    location: location
    sku: {
      name: 'Basic'
      tier: 'Basic'
      capacity: 5
    }
  }
}

resource app_service_plan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: app_service_plan_name
  location: location
  sku: {
    name: 'B1'
    tier: 'Basic'
    size: 'B1'
    family: 'B'
    capacity: 1
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource app_service_web 'Microsoft.Web/sites@2022-09-01' = {
  name: appService_web_name
  location: location
  kind: 'app,linux'
  dependsOn: [
    log_analytics_workspace
  ]
  properties: {
    serverFarmId: app_service_plan.id
    siteConfig: {
      connectionStrings: [
        {
          name: db_connection_string_env_var_name
          connectionString: 'Server=tcp:${sqlserver.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqlserver::sqlDb.name};Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
          type: 'SQLAzure'
        }
        {
          name: messageBroker_connectionStrings_env_var_name
          connectionString: listKeys(
            '${service_bus.id}/AuthorizationRules/${service_bus_ReadWritePolicy_name}',
            service_bus.apiVersion).primaryConnectionString
          type: 'Custom'
        }
      ]
      linuxFxVersion: 'DOTNETCORE|6.0'
    }
  }
}

resource app_service_appsetting 'Microsoft.Web/sites/config@2022-09-01' = {
  parent: app_service_web
  name: 'web'
  properties: {
    appSettings: [
      {
        name: db_user_env_var_name
        value: db_user
      }
      {
        name: db_password_env_var_name
        value: db_password
      }
      {
          name: applicationinsights_connection_string_env_var_name
          value: app_insights.properties.ConnectionString
      }
    ]
    numberOfWorkers: 1
    netFrameworkVersion: 'v4.0'
    linuxFxVersion: 'DOTNETCORE|6.0'
    publishingUsername: appService_web_name
    appCommandLine: 'dotnet TestTemplate6.Api.dll'
    loadBalancing: 'LeastRequests'
    publicNetworkAccess: 'Enabled'
    ipSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 2147483647
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    http20Enabled: false
    minTlsVersion: '1.2'
    ftpsState: 'FtpsOnly'
  }
}

resource app_insights 'Microsoft.Insights/components@2020-02-02' = {
  name: app_insight_name
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: log_analytics_workspace.id
  }
}

resource log_analytics_workspace 'Microsoft.OperationalInsights/workspaces@2020-08-01' = {
  name: log_analytics_name
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 120
    features: {
      searchVersion: 1
      legacy: 0
      enableLogAccessUsingOnlyResourcePermissions: true
    }
  }
}

resource service_bus 'Microsoft.ServiceBus/namespaces@2021-06-01-preview' = {
  name: service_bus_name
  location: location
  sku: {
    name: 'Standard'
    capacity: 1
    tier: 'Standard'
  }

  resource service_bus_RootManageSharedAccessKey 'authorizationrules@2022-10-01-preview' = {
    name: service_bus_RootManageSharedAccessKey_name
    properties: {
      rights: [
        'Listen'
        'Manage'
        'Send'
      ]
    }
  }

  resource service_bus_ReadWritePolicy 'authorizationrules@2022-10-01-preview' = {
    name: service_bus_ReadWritePolicy_name
    properties: {
      rights: [
        'Manage'
        'Listen'
        'Send'
      ]
    }
  }
}

output appServiceWebName string = appService_web_name
output dbConnection string = 'Server=tcp:${sqlserver.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqlserver::sqlDb.name};Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
