# Delete group
Deletes a group from the database.  
URL: `/api/group/{groupId}`  
Method: `DELETE`  
Required Headers: `Authorization` with user's JWT  

## Success response
Code: `204 NO CONTENT`  

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
Code: `401 ANAUTHORIZED`  
Code: `403 FORBIDDEN` when trying to use blacklist (deleted user's) JWT  
