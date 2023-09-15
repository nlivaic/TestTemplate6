# How to migrate the database

Please refer to solution level readme.md on how to create and apply migrations.

# Architecture

There is a `docker-compose-migration.yml` dedicated just to running the `./migrate.ps1` command from the solution root folder. This was done in order to share `.env` file with the main `docker-compose.yml` file so `DB_USER` and `DB_PASSWORD` would not have to copied separately to this project.