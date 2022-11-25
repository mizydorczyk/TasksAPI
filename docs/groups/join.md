# Join
User providing valid invitation code is able to join a group.
**URL:** `/api/group/join`
**Method:** `GET`
**Required Headers:** `Authorization` with user's JWT and `Invitation Code` with invitation code

## Sucess Response
**Code:** `200 OK`

## Error response
**Code:** `400 BAD REQUEST` 
**Content:**
```
Invitation code is invalid or has already expired
```
**Code:** `401 ANAUTHORIZED` 
**Code:** `403 FORBIDDEN` when trying to use blacklist (deleted user's) JWT 
