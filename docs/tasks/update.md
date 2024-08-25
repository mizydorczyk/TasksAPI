# Update task
Updates given task.  
URL: `/api/group/{groupId}/task/{taskId}`  
Method: `PATCH`  
Required Headers: `Authorization` with user's JWT  

## Example
```json
{
  "title": null,
  "description": null,
  "deadline": "2024-09-25T18:00:24",
  "isCompleted": null
}
```

## Success response
Code: `200 OK`  

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