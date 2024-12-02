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
To Run locally you will need to create a `.env` file and add the following lines:
```env
# This is optional, but if you want to use DeepL you need to provide an API key. 
# However it is recommended to use DeepL aswell.
deeplApiKey=yourApiKeyHere


# Connection string to your MySQL database
DefaultConnection=Server=localhost;Port=3306;Database=mysql-container;User=root;Password=1234;


# Secret key for JWT
jwtSecret=YourVeryLongAndSecureSecretKeyHere1234567892


# Email configuration, you can use Gmail for this. 
# This will send Emails to users when they register. 
# But also not required.
EMAIL_FROM=your-email@example.com
EMAIL_SMTP_SERVER=smtp.gmail.com
EMAIL_PORT=587
EMAIL_USERNAME=your-email@gmail.com
EMAIL_PASSWORD=your-app-specific-password
```

Without the `.env` file the application will not work.



### Run in Docker container

In order to have the application work with DeepL, you need to provide an API key. 
You can do this by adding the following line to the `docker-compose.yml` file: 
```yml
environment:
    # This is optional, but if you want to use DeepL you need to provide an API key. 
    # However it is recommended to use DeepL aswell.
    - deeplApiKey=yourApiKeyHere

    # Connection string to your MySQL database
    - DefaultConnection=Server=localhost;Port=3306;Database=mysql-container;User=root;Password=1234;

    # Secret key for JWT
    - jwtSecret=YourVeryLongAndSecureSecretKeyHere1234567892

    # Email configuration, you can use Gmail for this. 
    # This will send Emails to users when they register. 
    # But also not required.
    - EMAIL_FROM=your-email@example.com
    - EMAIL_SMTP_SERVER=smtp.gmail.com
    - EMAIL_PORT=587
    - EMAIL_USERNAME=your-email@gmail.com
    - EMAIL_PASSWORD=your-app-specific-password
```
Afterwards you can run the following command in the root directory of the project:
```bash
docker-compose build
```
```bash
docker-compose up
```
You might have to wait for the MySQL Database to finish the migration.
This can take up to 20 seconds. 


## Explore
You can reach the application by going to http://localhost:5070 in your browser by default.

