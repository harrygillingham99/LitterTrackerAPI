# litter-tracker-api
An app API interface to handle the CRUD from the litter tracker app for UbiComp 2020-21 Assignment.

# Under the hood
- .NET Core 3.1 Web API
- Using Swashbuckle/NSwag to serve up an OpenAPI document
- Includes app.yaml & dockerfile for Google Cloud hosting
- Includes a sweet Google Datastore base class that I spent too much time doing
- Requires valid firebase JWT token in "Authorization" header. Swagger UI provides input for this

