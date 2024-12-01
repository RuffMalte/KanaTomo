# KanaTomo

KanaTomo is a web application designed to help learners of the Japanese language. The mission is to provide a comprehensive and user-friendly platform that combines tools and resources to enhance translation and learning.

### Key Features:

- **Translation Services**: Utilizing both JISHO and DeepL APIs, KanaTomo offers accurate and context-aware translations for Japanese text.
- **Anki Card Creation**: Easily create and manage Anki flashcards directly within the application.
- **User-Friendly Interface**: An intuitive, responsive design.
- **API Design**: A RESTful API design that allows for easy use for your own project.

### Technology Stack:

- Backend: ASP.NET Core
- Frontend: HTML, CSS (Bootstrap), JavaScript
- Database: MySQL
- APIs: JISHO, DeepL

## Run
Running instructions.

### Run locally
To Run locally and have the DeepL translation feature work, you need to provide an API key.
You can add a API key by creating a .env file and adding the following line:
```env
deeplApiKey=yourApiKeyHere
DefaultConnection=Server=localhost;Port=3306;Database=mysql-container;User=root;Password=1234;
jwtSecret=YourVeryLongAndSecureSecretKeyHere1234567892
```

### Run in Docker container

In order to have the application work with DeepL, you need to provide an API key. 
You can do this by adding the following line to the `docker-compose.yml` file: 
```yml
environment:
    - deeplApiKey=yourApiKeyHere
    - DefaultConnection=Server=host.docker.internal;Port=3306;Database=mysql-container;User=root;Password=1234;
    - jwtSecret=YourVeryLongAndSecureSecretKeyHere1234567892
```

