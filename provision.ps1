# Read from variables.ps1 only as part of local development. Detect local environment by trying 
# to read from "rg" environment variable. If "rg" environment variable is not defined, 
# deduce there aren't any relevant environment variables defined and read from variables.ps1.
# Pipeline should have "rg" and all the other relevant variables defined, so no need for variables.ps1
if ($RG -like '') {
    . .\deployment\variables.ps1
}

echo $RG

az account set -s $SUBSCRIPTION

# Resource group
$jsonResultRg = az deployment sub create --location $LOCATION --template-file ./deployment/resource-group.bicep --parameters environment=$ENVIRONMENT projectName=$PROJECT_NAME location=$LOCATION | ConvertFrom-Json
$resourceGroupName = $jsonResultRg.properties.outputs.resourceGroupName.value

# Resources
az deployment group create --resource-group $resourceGroupName --template-file .\deployment\iac.bicep --parameters environment=$ENVIRONMENT projectName=$PROJECT_NAME db_user=$DB_ADMIN db_password=$DB_ADMIN_PASSWORD db_admin=$DB_ADMIN db_admin_password=$DB_ADMIN_PASSWORD
