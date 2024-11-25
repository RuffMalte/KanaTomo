# KanaTomo
description


## Run
Running instructions.

### Run locally
To Run locally and have the DeepL translation feature work, you need to provide an API key.
You can add a API key by creating a .env file and adding the following line:
```env
deeplApiKey=yourApiKeyHere
```


### Run in Docker container

In order to have the application work with DeepL, you need to provide an API key. 
You can do this by adding the following line to the `docker-compose.yml` file: 
```yml
environment:
    - deeplApiKey=yourApiKeyHere
```

