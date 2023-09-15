# New solution - quickstart guide

> All the steps outlined here are detailed further on down in the document.

1. Run docker-desktop
2. Run `./configure.ps1`
3. Open solution using Visual Studio, set docker-compose as Startup project and run the solution
4. Run `./create_migration.ps1 '' 'Initial migration'`
5. Run `./migrate.ps1`
6. Run the solution again.
6. Go to http://localhost:44395/index.html

At this point only `.gitignore` has been committed locally. Now you can make some changes to the source code, push it to GitHub and get it deployed to your provisioned Azure resources:

1. Go to GitHub, create a new repository and execute commands locally.
2. `git push -u origin master`
3. You can remove or update some of the endpoints and models if you like.
4. Open `release_pipeline.yml` and set the value for `adoProject` variable. No need to touch anything if you are ok with the name and ADO project has the same name.
5. Set up [Azure Service Connection](#azure-service-connection). Copy the name from ADO to `release_pipeline.yml` lines 73 and 87.
6. `git checkout -b feature/initial-code-commit; git add *; git commit -m "Initial code commit."; git push -u origin feature/initial-code-commit`
7. Create and approve PR.
8. `git checkout master; git pull`
9. Now configure the pipelines on ADO. Add three new pipelines (`pr_pipeline`, `build_pipeline`, `release_pipeline`) based off of YAML files with the same name.
10. Configure the pipeline variables for `release_pipeline`. More on that in section [Release pipeline Database Migrations and Provisioning resources](#release-pipeline-database-migrations-and-provisioning-resources). One detail here: you probably won't know all the details on the first ever run, so it might be easiest to provision manually this one time:
    1. Make sure you do `az login` first and log in to the correct subscription.
    2. Populate `variables.ps1`
    3. Execute `. ./provision.ps1` to provision Azure 
    resources
    4. Then copy stuff over to the pipeline variables.

11. Create a new feature branch `git checkout -b feature/my-first-feature`. Do your work, create a PR and let the `pr_pipeline` do its work.
12. Approve PR. Let `build_pipeline` and `release_pipeline` do their work.
13. Provision Azure resources - `release_pipeline` will do the work here as well.
    * Manual provisioning: if you want to test your infrastructure out regardless of the pipeline, run `. ./provision.ps1` and this will provision everything to Azure. Make sure you do `az login` first and log in to the correct subscription. Open `variables.ps1` and make sure everything is properly defined.

At this point you have a local environment and Azure Service Bus fully set up, along with ADO pipelines ready deploy your code to a working AppService. Start working on your features!

# Before You Get Started

## Install a Docker host

E.g. Docker Desktop:

    choco install docker-desktop

## Configuration

### Set configuration

Most of the stuff is in the `.env` file. This is a git ignored file, but it has the relevant structure in it. Some of the fields are prepopulated, others have to be provided by you. Execute `./configure.ps1` from the root folder and follow the instructions. It helps if you have the following beforehand:

- Connection strings and details for the Service Bus (details above).
- Username and password for the email are provided as default values, but you can provide whatever values you want.

### Database configuration

Database connection string for both `Api`, `WorkerServices` and `Migrations` projects is in the `.env` file. This was a deliberate choice, because I wanted the templated project to have a connection string automatically generated and in line with the name of the solution. You will notice there are two connection strings: `ConnectionStrings__TestTemplate6DbConnection` is used by `Api` and `WorkerServices`. `Migrations` has a separate one `ConnectionStrings__TestTemplate6Db_Migrations_Connection` because it is accessing the dockerized database from outside.

Username and password for the database are provided as default values, but you can provide whatever values you want.
Make sure you set the database-related variables (prefixed `DB_`) before you run the solution for the first time, otherwise the database will be configured with given administrator password and a username and password for the application user. If you don't change those values before running the solution you will have to delete the `testtemplate6.sql` container and accompanying volumes. If you change `DB_PASSWORD`, make sure the same value is set in `InitializeTestTemplate6Db.sql` for the login as well.

When you first run the solution, an SQL script found in `src/InitializeTestTemplate6Db.sql` is executed, creating the database with an admin account (password in `DB_ADMIN_PASSWORD`), login and user (`DB_USER` and `DB_PASSWORD`). User is then assigned to read, write and DDL roles.

Application is accessing the database as a `DB_USER`/`DB_PASSWORD`, with a generated connection string found in `ConnectionStrings__TestTemplate6DbConnection` and `ConnectionStrings__TestTemplate6Db_Migrations_Connection`.

# Running The Application

Make sure to set the `docker-compose` as the startup project. The application can be reached by default on `localhost:44395`. You can change this in the `docker-compose.yml`. Just go to `/swagger/index.html` to see the initial API.

At this point you have several things up and running:

- API (dockerized)
- Worker service (dockerized)
- Empty Sql Server database (dockerized)
- Azure Service bus with several topics, subscriptions and queues
- nginx reverse proxy in `docker-compose.yml`

Now it is time to create some tables in the database. From the root of your solution, first run `.\create_migration.ps1 '' '0001_Initial'` and then `./migrate.ps1`. Now you have to go to the SSMS and register your database server there. It is accessible on localhost, port 1433, with the username and password you set in your `.env` file under `DB_USER` and `DB_PASSWORD`.

# Additional Stuff

## Branching strategy

Feature branches strategy is supported out of the box. This strategy expects all development to go through branches and committing directly to `master` is not allowed. Supported branches:

* `feature/`
* `fix/`

## Pipelines

### Naming the ADO project

`release_pipeline.yml` - `project` property on lines 43, 53, 63 should be the name of your ADO project.

### Azure YAML pipelines:

* `pr_pipeline`
* `build_pipeline`
* `release_pipeline`

All pipelines build and deploy all applications (`Api` and `WorkerServices`) in the solution.

When creating ADO pipelines, name them just like the files are named (minus the `.yml` suffix). Naming the pipelines same as the files is important because the `release_pipeline` is triggered by a successful `build_pipeline` run. If you decide to name your ADO pipelines differently, make sure you change two things in `release_pipeline.yml` - update `source` on line 8 and `definition` on line 40, 50, 60 to match the **build** pipeline name in ADO (if needed).

### Pipeline configuration

#### Azure Service Connection

In `release_pipeline.yml:72` there is an Azure subscription name (property name `azureSubscription` with initial value `AzureConnection_TestTemplate6`) - make sure the name is the same as what is in Azure.

If you are logged into ADO and Azure with different usernames, then you will need to go through additional steps to hook up ADO and Azure: more details [here](https://www.devcurry.com/2019/08/service-connection-from-azure-devops-to.html). The previous link describes the process nicely, but in case it is down try [this](https://learn.microsoft.com/en-us/azure/devops/pipelines/library/connect-to-azure?view=azure-devops#create-an-azure-resource-manager-service-connection-with-an-existing-service-principal) one.

#### Release pipeline Database Migrations and Provisioning resources

`release_pipeline` deploys to resource group and resources based on pipeline variables:
* `DB_PASSWORD` - administrator password of your choosing.
* `DB_USER` - administrator username of your choosing.
* `SUBSCRIPTION` - Azure subscription identifier
* `LOCATION` - must match names of regions Azure can understand, e.g. `westeurope`.
* `ENVIRONMENT` - a moniker of your choosing to describe what environment you are deploying to.
* `PROJECT_NAME` - a moniker of your choosing to denote the project.

#### First deployment run

* `pr_pipeline` - on your first PR, the `pr_pipeline` will get triggered.
* `build_pipeline` - once you merge the PR, the build pipeline will get triggered. It is similar to `pr_pipeline`, except it uploads artifacts.
* `release_pipeline` - once the `build_pipeline` is done, `release_pipeline` will get triggered, but it will stall. You need to manually give a few permissions, it should start running from there on.

### Branches

**All** pipelines work with `master` branch . If you are using `main`, remember to do a search and replace.

## Provisioning resources on Azure manually

Even though the pipelines are built in such a way to take advantage of Bicep files to provision stuff on Azure, you can run those scripts on your own by executing `. ./provision.ps1`. Before running that script, look into `variables.ps1` file - it has all the parameters needed to provision, but you can change values if you wish. Make sure the variable values here are the same ones as in the release pipeline, otherwise you will end up with two different resource groups.

## Project naming

All projects have a prefix `TestTemplate6` and pipelines latch onto that detail. If you want to start renaming projects, you should also do a search and replace across all the files in the solution. Be careful!

## Versioning

We are using semver and GitVersion. Each commit message gets a suffix (defined in `./githooks/prepare-commit-msg` and recognized in `GitVersion.yml`). `feature/` branch gets a suffix saying GitVersion should bump minor version. `fix/` branch gets a suffix saying GitVersion should bump patch version. Bumping major version needs to be done manually by tagging a commit. We do not embed the version in the assemblies yet. GitVersion depends on `--no-ff` merges to be able to deduce version successfully. Make sure your ADO project enforces this, do not allow developers to merge PRs differently! 

## Generating cert for your local development box

The template does not use HTTPS, however it can easily be added. There is a `.conf` file in there which you need to tweak to your liking. Then you need to generate `.crt` and `.key` files for Api. These make up the self-signed certificate, and the commands to create the certificate are below, with a dummy password of `rootpw`:

1. Go to **solution root folder** and execute below lines from **WSL2**:

   sudo openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout api-local.testtemplate6.key -out api-local.testtemplate6.crt -config api-local.testtemplate6.conf -passin pass:rootpw

   sudo openssl pkcs12 -export -out api-local.testtemplate6.pfx -inkey api-local.testtemplate6.key -in api-local.testtemplate6.crt

2. Add the certificate to your computers CA store: go to ./nginx, right-click on `.pfx` files and install to `LocalMachine` -> `Trusted Root Certification Authorities`.

For more details consult: https://bit.ly/3eWOHH2

## Hosts file

You can tell nginx to work with the `localhost`, however this might become a problem if you have multiple services running. To sidestep the issue you can keep the nginx.conf as it is, but that will require a change to `hosts` file.

    # Development DNS
    127.0.0.1	    api-local.testtemplate6.com
    127.0.0.1	    id-local.testtemplate6.com

## Migrations

For migrations to work `.env` file must be properly set up with database credentials and connection string configured.

### Creating migrations

The below commands must be executed from solution root folder using Powershell. If this is the first migration in your project, execute:

    .\create_migration.ps1 '' '0001_Initial'

Every next migration must contain the name of the migration immediately preceeding it:

    .\create_migration.ps1 '0001_Initial' '0002_Second'

### Applying migrations

Command must be executed from solution root folder using Powershell. You will notice it is executing from a Docker container and Docker compose - the reason is this way there is only one `.env` which can be shared by all executeable projects in the solution (`Ąpi`, `Migrations`, `WorkerServices`).

    ./migrate.ps1
