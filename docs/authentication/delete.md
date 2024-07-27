# Delete
Deletes user from the database.  
URL: `/api/user/`  
Method: `DELETE`  
Required Headers: `Authorization` with user's JWT  

## Success response
Code: `204 NO CONTENT`  

## Error response
Code: `401 ANAUTHORIZED`  
Code: `403 FORBIDDEN` when trying to use blacklist (deleted user's) JWT  