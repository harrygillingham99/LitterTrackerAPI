# LitterTracker - UbiComp-Assignment-2

Received 90/100

# Under the hood
- .NET Core 3.1 Web API
- Using Swashbuckle/NSwag to serve up an OpenAPI document
- Includes app.yaml & dockerfile for Google Cloud hosting
- Includes a sweet Google Datastore base class that I spent too much time doing
- Requires valid firebase JWT token in "Authorization" header. Swagger UI provides input for this
- Uses Google Storage Buckets for image storage
