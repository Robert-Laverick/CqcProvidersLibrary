# Example Care Quality Commission API Reader Library.

Simple example of a library that reads data from the Care Quality Commission API. 

Main endpoint is CqcProvidersRepository which requires and subscription key and an optional database connection string to locally caching data.

Test as included in the CqcProvidersLibrary.Tests project. 

Tests requiring a subscription key read the key from client secrets.

Tests requiring a database connection string read the connection string from client secrets, but will default to a local database with trusted connection.