# Recommended to build first, so you are certain all the changes done to source code make it into the image.
docker-compose -f docker-compose-migrations.yml build
docker-compose -f docker-compose-migrations.yml up