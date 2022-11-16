# Create group
Creates group in the database.
**URL:** `/api/group/`
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
    "traceId": "00-e9f9ca134233825c9472cf6b383f58d5-9295f4519bb78cd0-00",
    "errors": {
        "Name": [
            "Group name must be specified"
        ]
    }
}
```
**Code:** `401 ANAUTHORIZED` 
**Code:** `403 FORBIDDEN` when trying to use blacklist (deleted user's) JWT