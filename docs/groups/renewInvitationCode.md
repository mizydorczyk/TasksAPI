# Get invitation code
Action available only for group's owner. Returns invitation code.  
URL: `/api/group/{groupId}/renewInvitationCode`  
Method: `PATCH`  
Required Headers: `Authorization` with user's JWT  

## Success response
Code: `200 OK`  

## Error response
Code: `403 FORBIDDEN`  
Content:  
```
Insufficient permission
```
Code: `400 BAD REQUEST`  
Content:  
```
Group does not exist
```
Code: `401 ANAUTHORIZED`   
Code: `403 FORBIDDEN` when trying to use blacklist (deleted user's) JWT   
