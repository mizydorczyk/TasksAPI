# Get all tasks from the group
Returns task with given id from given group from the database.  
URL: `/api/group/{groupId}/task/{taskId}`  
Method: `GET`  

## Success response
Code: `200 OK`  
```json  
{
    "id": 1,
    "title": "1st task",
    "description": "lorem ipsum",
    "createdDate": "2022-11-14T17:18:36.6556065",
    "deadline": "2023-01-02T20:00:00",
    "isCompleted": false
}
```

## Error response
Code: `403 FORBIDDEN`  
Content:  
```
Insufficient permission
```
Code: `404 NOT FOUND`  
Content:  
```
Group does not exist
```
Code: `404 NOT FOUND`  
Content:  
```
Task not found
```
Code: `401 ANAUTHORIZED`   
Code: `403 FORBIDDEN` when trying to use blacklist (deleted user's) JWT  