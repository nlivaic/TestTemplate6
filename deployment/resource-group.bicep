param environment string
param projectName string
param location string

targetScope = 'subscription'

// Object containing a mapping for location / region code
var regionCodes = {
  westeurope: 'WE'
}   
// remove space and make sure all lower case
var sanitizedLocation = toLower(replace(location, ' ', ''))
// get the region code
var regionCode = regionCodes[sanitizedLocation]
// naming convention
var rg = 'RG'

var resourceGroup_name = '${regionCode}-${toUpper(environment)}-${toUpper(projectName)}-${rg}'

resource resourceGroup_resource 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: resourceGroup_name
  location: location
}

output resourceGroupName string = resourceGroup_name
