# Login
Returns a token for a registered user.  
URL: `/api/user/login`  
Method: `POST`  

## Example
```json
{
    "email": "exampleEmail@example.com",
    "password": "examplePassword",
}
```

## Succces response
Code: `200 OK`  
response example:  
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbkBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiYWRtaW4iLCJ1c2VyIl0sIm5iZiI6MTYxNjE4MzUzMCwiZXhwIjoxNjE2MTg0NDMwLCJpc3MiOiJ3d3cuZnVya2FuaXNpdGFuLmNvbSIsImF1ZCI6Ind3dy5hdWRpZW5jZS5jb20ifQ.cVqY41hMewic2u2hFrFwfNR9-wGoTjAZ5TcemJuHE6o
```

## Error response
Code: `403 FORBIDDEN`  
Content:  
```
Invalid username or password
```