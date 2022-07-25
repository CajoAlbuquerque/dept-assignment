# MovieTrailersAPI
A very simple web API to featch movie trailers and details. Does not require any authentication, simple query the data and watch the trailers. Developed as part of Dept's interview process for a backend developer role.

## How to run - Docker
The application is containerized, so start by building the image:
```
docker build -t movie-trailers-api .
```

Then run the image inside a container:
```
docker run -it --rm -p 8000:8000 -p 8001:8001 --env-file=.env movie-trailers-api
```

Then hit the Swagger menu at http://localhost:8000/swagger.
