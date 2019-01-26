## Assignment 3

For this assignment use the existing projects and infrastructure that is already in place. 
You may need to modify some of the existing classes.
Several of these items may already be completed on the Assignmednt3 branch. Ensure you verify that they are done completely.


- Create a Web API project to expose needed functionality for the SecretSanta app
- Add swagger to the API project
- The controllers should not touch the DbContext directly. Rather they should only act on the interfaces of a service(s). The implementation of these service may act on the DbContext.
- Setup dependency injection 
  - For the data access layer (DAL) services. You will need to make the determination on the proper scope for these services.
  - Ensure the DbContext is also injected.
- The API project must be unit tested
  - Use mock/stub implementations of the services to test your controllers. These tests should not interact with the database or actual services.
  - The generated Startup and Program classes do *not* need to be unit tested
- The controllers should expose the following functionality
  - Create/Update/Delete users
  - Create/Update/Delete groups
  - Add/Remove users from groups
  - Create/Update/Delete gift for a user
  - Query all groups
  - Query all users in a group
  - Query all gifts for a user
- Controllers are expected to handle input error cases and return appropriate responses.
- Controllers should not expose domain objects. They should only expose data transfer objects (DTO).


### Going above and beyond
- Rather than writing your own mock objects use a mocking library like [Moq](https://github.com/moq/moq4)
  - You may also consider simplifying your unit tests by extending Moq with [AutoMocker](https://github.com/moq/Moq.AutoMocker)
- Setup an alternate dependency injection container. Though not strictly needed for this class, other DI frameworks can [offer additional features](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2#default-service-container-replacement). Poke around and see what is out there. [AutoFac](https://github.com/autofac/Autofac) is one of the more popular options.

### Useful Stuff

- [Tutorial: Create a web API with ASP.NET Core MVC](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.2&tabs=visual-studio)
- [Creating Web API Project in VS2017](https://www.mithunvp.com/create-aspnet-mvc-6-web-api-visual-studio-2017/)
- [Getting Started with Swashbuckle (swagger)](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.2&tabs=visual-studio)
- [What is JSON](https://mva.microsoft.com/en-us/training-courses/introduction-to-json-with-c-12742?l=xxtX274UB_8805494542)
- Definitions
  - [Inversion of Control (IoC)](https://deviq.com/inversion-of-control/)
  - [Dependency Inversion Principle](https://deviq.com/dependency-inversion-principle/)
  - ["new is glue"](https://ardalis.com/new-is-glue)
  - [Data Transfer Object (DTO)](https://en.wikipedia.org/wiki/Data_transfer_object)
  - [Data Access Layer (DAL)](https://en.wikipedia.org/wiki/Data_access_layer)
