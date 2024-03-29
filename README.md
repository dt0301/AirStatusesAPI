# Техническое задание

## Цель
Разработать Web API по получению данных о статусе рейсов

## Примечание
- То, что не выделено цветом обязательно для выполнения для Junior, Middle и Senior
- То, что выделено зеленым обязательно для выполнения для Middle и Senior
- То, что выделено жёлтым обязательно для выполнения для Senior

## Описание
Необходимо разработать Web API, с помощью которого можно контролировать статусы рейсов авиакомпании. Клиенты (пользователи или другие приложения) могут авторизоваться в данном приложении и использовать имеющийся функционал в рамках их уровня доступа. Одни могут только считывать данные, другие, помимо считывания, могут и редактировать их. Приложение должно предоставлять доступ к своим методам через API. Данные по рейсам должны храниться в базе данных.

**<span style="color:green">Документацию по приложению необходимо вести в Swagger. Любые изменения в БД и исключения логируются в файл лога с указанием пользователя и времени изменения.</span>**

**<span style="color:yellow">Приложение должно считывать данные из кэша (не из БД). Добавление и редактирование – напрямую в БД. При изменении данных в БД кэш тоже обновляется до актуального состояния.</span>**

## Модели
### Flight (рейс)
Свойство | Тип | Ограничения
--- | --- | ---
ID | int | Primary Key
Origin | string | 256
Destination | string | 256
Departure | DateTimeOffset | Timezone по Origin
Arrival | DateTimeOffset | Timezone по Destination
Status | enum | InTime, Delayed, Cancelled

### User (клиент)
Свойство | Тип | Ограничения
--- | --- | ---
ID | int | Primary Key
Username | string | 256, unique
Password | string | 256, hash
RoleId | int | Foreign Key

### Role (роль)
Свойство | Тип | Ограничения
--- | --- | ---
ID | int | Primary Key
Code | string | 256, unique

## Методы
Название | Ограничения | Описание | Валидация
--- | --- | --- | ---
Авторизация | получение токена по username и password | Username Password
Получение списка рейсов | Авторизация, Доступ для всех ролей | возвращает список всех рейсов, отсортированных по Arrival, Фильтрация по Origin и/или Destination | Origin, Destination
Добавление нового рейса | Авторизация, Доступ только для роли Moderator | добавляет новый рейс в список | Весь объект Flight
Редактирование рейса | Авторизация, Доступ только для роли Moderator | Редактирует статус рейса | Status

## Условия
- Придерживаться SOLID, KISS, DRY
- База данных: EF Core, СУБД – любая, связка с базой через Code First
- Логирование: Serilog
- Cache: Redis Cache/Memory Cache
- Framework: .NET 6
- Паттерны: Mediator, CQRS
- Docs: Swagger или Postman
- Валидация входных параметров: FluentValidation
- Тесты: Unit Testing и/или Integration Testing (xUnit или nUnit)
- Архитектура: Domain Driven Design, проект необходимо разделить на слои: Domain, Application, Infrastructure, Presentation (без фронта), согласно DDD
- Пример: https://github.com/jasontaylordev/CleanArchitecture
- VCS: Git, предоставить ссылку на публичный репозиторий с проектом тестового задания

## Что оценивается
- Качество выполнения требований тестового задания
- Качество написания кода: соблюдение единого стиля, отступов, ведение в системе контроля версий только нужных файлов (без «мусора»)
- Коммиты должны кратко и тезисно описывать проделанную работу в рамках данного коммита
- Уместное и правильное использование паттернов программирования и проектирования
- Архитектурное построение проекта
Задание не ограничивает миддлов и джунов в выполнении требований выше нормы, но будет оцениваться согласно уровню. Поэтому прежде, чем использовать, убедитесь, что качество выполнения не пострадает
