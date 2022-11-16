# Delete task
Deletes a task from the database.
**URL:** `/api/group/{groupId}/task/{taskId}`
**Method:** `DELETE`
**Required Headers:** `Authorization` with user's JWT

## Sucess Response
**Code:** `204 NO CONTENT`

## Error response
**Code:** `403 FORBIDDEN`
**Content:**
```
Insufficient permission
```
**Code:** `404 NOT FOUND`
**Content:**
```
Group does not exist
```
**Code:** `404 NOT FOUND`
**Content:**
```
Task does not exist
```
**Code:** `401 ANAUTHORIZED` 
**Code:** `403 FORBIDDEN` when trying to use blacklist (deleted user's) JWT 
