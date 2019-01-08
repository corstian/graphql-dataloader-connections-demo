## Introduction
The goal of this project is to give clear examples of the usage of the GraphQL DataLoader, a Relay Connection, and a combination.

Previous knowledge of the graphql-dotnet framework is heavily recommended as this library is full of abstractions, and it's quite easy to lose track of the different aspects.

## Implementation Details

**Mock data**

The static `CompanyRepo` and `PersonRepo` classes are used to mock a data store. The respective models attached are `Company` and `Person`. These classes implement the `IId` interface in order to be sure the `ToConnection(this Enumerable<T>, ResolveConnectionContext<object> context)` extension has access to an Id field. This extension method is the place where we figure out which portion of the `Enumerable` to grab. Tailor this to your data store and requirements. This extension method also instantiates a `Connection<T>` which can be returned directly from a connection.

An interesting tidbit about this mockdata is that people represented in this set all seems to be job hopping in between runs.


**Connections**

The `RootQuery` type contains connections for both the `CompanyType` and the `PersonType`. These connections do not make use of the dataloader. Besides these two connections there is another connection which makes use of the dataloader to retrieve information about persons, which is located within the `CompanyType` class.

Last but not least about connections, I'm using a base-64 encoded guid. Saves a few bytes, and it's fairly easy to work with.

**Using the dataloader**

When it comes to usage of the DataLoader it is important to remember a few things. First of all, keep track of your async methods, and ensure you return the correct type. It's easy to return a `Task<T>` instead of `T` when you forget to await something. Secondly it's extremely easy to deadlock the dataloader by doing something like `return await loader.LoadAsync(id);`. My recommended way to return data from the dataloader is the following:

```C#
var loadHandle = loader.LoadAsync(id);
var result = await loadHandle;
```

> This project contains only a single usage of the dataloader which can be found in the `CompanyType` class and is used to load related persons.


**Register types with the DI**

It's important to register the correct types, but you probably figured that out already by the time you're reading this. Check out the Startup.cs file to see if you missed anything.

## Running the project
Hitting ctrl+F5 should do the job to launch without debugger. Make sure you are using the self hosted kestrel server as there's some problems with IIS express when it comes to loading the mock data.

The GraphQL endpoint lives at http://localhost:5000/graph or https://localhost:5001/graph, depending on whether you want to use http or https.

## Query Inspiration
If you're out of inspiration, you can try the following query to retrieve some information and to see how it works. If all goes right (and the order of the list stays the same), you should get a result containing information about 10 people, with the first one being Joseph Wilson.

```gql
query {
  persons(first: 10, after: "vKzjofUTg0Gg2p0YZJnCpA==") {
    totalCount
    edges {
      cursor
      node {
        firstName
        lastName
        gender
        city
        company {
          name
        }
      }
    }
  }
}
```