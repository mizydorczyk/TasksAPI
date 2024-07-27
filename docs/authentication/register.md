# Register
Creates user in the database.  
URL: `/api/user/register`  
Method: `POST`  

## Example
```json
{
    "firstName": "example",
    "lastName": "example",
    "email": "example@example.com",
    "password": "example"
}
```

## Success response
Code: `201 CREATED`  

## Error response
Code: `400 BAD REQUEST`  
Content:  
```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "traceId": "00-2928a74623db6fd6924d82ba226e59af-4e210ac7e1bfdee7-00",
    "errors": {
        "Email": [
            "Email must be specified",
            "Incorrect email format",
            "That email is taken"
        ],
        "LastName": [
            "Last name must be specified"
        ],
        "Password": [
            "Password must be specified",
            "Password must be longer than 6 characters"
        ],
        "FirstName": [
            "First name must be specified"
        ]
    }
}
```