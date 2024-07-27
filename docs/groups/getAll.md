# Get all user's groups
Returns all groups with tasks user belongs to from the database.  
URL: `/api/group/`  
Method: `GET`  

## Success response
Code: `200 OK`  
```json  
[
    {
        "id": 1,
        "name": "example group nr 1",
        "tasks": []
    },
    {
        "id": 2,
        "name": "example group nr 2",
        "tasks": [
            {
                "id": 1,
                "title": "1th task",
                "description": "lorem ipsum",
                "createdDate": "2022-11-14T17:18:36.6556065",
                "deadline": "2023-01-02T20:00:00",
                "isCompleted": false
            }
        ]
    }
]
```

## Error response
Code: `401 ANAUTHORIZED`  
Code: `403 FORBIDDEN` when trying to use blacklist (deleted user's) JWT  