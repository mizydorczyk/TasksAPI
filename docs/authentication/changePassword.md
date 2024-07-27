# Change password
Changes user's password.  
URL: `/api/user`  
Method: `PATCH`  
Required Headers: `Authorization` with user's JWT  

## Example  
```json
{
    "oldPassword": "example",
    "newPassword": "example"
}
```

## Success response
Code: `200 OK`

## Error response
Code: `400 BAD REQUEST`  
Content:  
```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "traceId": "00-9b52d6b678649e75aa2d0443f44ab3e5-f9305f3c546d7609-00",
    "errors": {
        "NewPassword": [
            "Password must be specified",
            "Password must be longer than 6 characters"
        ]
    }
}
```
Code: `401 ANAUTHORIZED`  
Code: `403 FORBIDDEN` when trying to use blacklist (deleted user's) JWT