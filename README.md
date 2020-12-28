# litter-tracker-api
An API interface to handle the CRUD from the litter tracker app for UbiComp 2020-21 Assignment

# Under the hood
- .NET Core 3.1 Web API
- Using Swashbuckle/NJsonSchema to serve up an OpenAPI document
- Includes app.yaml & dockerfile for Google Cloud hosting
- Includes a sweet Google Datastore base class that I spent too much time doing
- Uses Swagger Authorisation Header, requires a valid firebase JWT token
