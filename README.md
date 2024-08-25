# tasks-api                            

The app enables the registration and login of users, who can then create, mark as done, tasks within the created groups. Furthermore, the creator of a group as its owner can generate an invitation code and invite others to join their task group.

## Getting started

- Adjust ``appsettings.json`` and ``src/Dockerfile`` (optional)
- Run ``docker-compose up`` in terminal.

A simple Postman collection is available: ``tasks-api.postman_collection.json`` to explore.

## Technologies

- [ASP.NET Core 8](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-8.0)
- [Entity Framework Core 8](https://learn.microsoft.com/en-us/ef/core/)
- [AutoMapper](https://automapper.org/)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/)

## Database

![obraz](https://user-images.githubusercontent.com/74381129/202245256-1aec0e81-b2a4-48e2-b7d6-2c063330e51a.png)

## Available endpoints

Details and possible actions can be found in the ``/docs`` folder.  

![obraz](https://user-images.githubusercontent.com/74381129/202239247-98daf5f6-3a04-4aee-9b20-efbad0c02853.png)
