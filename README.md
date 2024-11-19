# KanaTomo
description


## Run
Running instructions.

### Run in Docker container

In order to have the application work with DeepL, you need to provide an API key. 
You can do this by adding the following line to the `docker-compose.yml` file: 
```yml
environment:
    - deeplApiKey=yourApiKeyHere
```

