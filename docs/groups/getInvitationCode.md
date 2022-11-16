# Get invitation code
Action available only for group's owner. Returns invitation code.
**URL:** `/api/group/{groupId}/getInvitationCode`
**Method:** `GET`
**Required Headers:** `Authorization` with user's JWT

## Sucess Response
**Code:** `200 OK`
**Example:**
```
3x4mpl3
```

## Error response
**Code:** `403 FORBIDDEN`
**Content:**
```
Insufficient permission
```
**Code:** `400 BAD REQUEST` 
**Content:**
```
Group does not exist
```
**Code:** `401 ANAUTHORIZED` 
**Code:** `403 FORBIDDEN` when trying to use blacklist (deleted user's) JWT 
