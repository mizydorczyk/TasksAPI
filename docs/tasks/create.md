# Create task
Creates task in the database.
**URL:** `/api/group/{groupId}/task`
**Method:** `POST`
**Example:**
```json
{
    "name": "example group"
}
```

## Sucess Response
**Code:** `201 CREATED`

## Error response
**Code:** `400 BAD REQUEST`
**Content:**
```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "traceId": "00-cd3a25936646c87d8034d43f12b2e002-1ac520f78b5154e1-00",
    "errors": {
        "Title": [
            "Task title must be specified"
        ]
    }
}
```
**Code:** `404 NOT FOUND`
**Content:**
```
Group not found
```
**Code:** `403 FORBIDDEN`
```
Insufficient permission
```
**Code:** `401 ANAUTHORIZED` 
**Code:** `403 FORBIDDEN` when trying to use blacklist (deleted user's) JWT