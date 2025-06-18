# BookifyAuth API Usage Guide
This document provides instructions on how to use the BookifyAuth API endpoints, specifically for user management operations.

## API Endpoints
The BookifyAuth API offers several endpoints for user management. Below are detailed instructions for using all available endpoints.

### 1. User Registration - `POST /api/members/register`
This endpoint allows you to register new users in the system.

#### Request Details
- **URL**: `/api/members/register`
- **Method**: POST
- **Content-Type**: application/json

#### Request Body
```json
{
  "username": "string",
  "email": "string",
  "password": "string",
  "membershipTier": "string" // Optional, defaults to "Basic" if not provided
}
```

#### Example Request
```json
{
  "username": "johndoe",
  "email": "john.doe@example.com",
  "password": "SecureP@ss123",
  "membershipTier": "Premium" // Optional, can be "Basic" or "Premium"
}
```

#### Successful Response (200 OK)
```json
{
  "statusCode": 200,
  "message": "User registered successfully",
  "data": {
    "username": "johndoe",
    "email": "john.doe@example.com", 
    "membershipTier": "Premium"
  }
}
```

#### Error Response (400 Bad Request)
```json
{
  "statusCode": 400,
  "message": "Registration Failed!",
  "errors": [
    "UserName Is Already Exist!",
    // or any other validation errors
  ]
}
```

### 2. User Login - `POST /api/members/login`
This endpoint authenticates users and allows them to log in.

#### Request Details
- **URL**: `/api/members/login`
- **Method**: POST
- **Content-Type**: application/json

#### Request Body
```json
{
  "usernameOrEmail": "string",
  "password": "string"
}
```

#### Example Request
```json
{
  "usernameOrEmail": "johndoe", // Can use either username or email
  "password": "SecureP@ss123"
}
```

#### Successful Response (200 OK)
```json
{
  "statusCode": 200,
  "message": "User logged in successfully",
  "data": {
    "username": "johndoe",
    "membershipTier": "Premium"
  }
}
```

#### Error Response (400 Bad Request)
```json
{
  "statusCode": 400,
  "message": "Login Failed!",
  "errors": [
    "Invalid username or password"
  ]
}
```

### 3. Get User by ID - `GET /api/members/{userId}`
This endpoint retrieves details for a specific user by their ID.

#### Request Details
- **URL**: `/api/members/{userId}`
- **Method**: GET
- **Parameters**: userId (path parameter)

#### Example Request
```
GET /api/members/123
```

#### Successful Response (200 OK)
```json
{
  "statusCode": 200,
  "message": "User retrieved successfully",
  "data": {
    "userId": 123,
    "username": "johndoe",
    "email": "john.doe@example.com",
    "membershipTier": "Premium",
    "createdAt": "2023-11-15T10:30:00Z"
  }
}
```

#### Error Response (404 Not Found)
```json
{
  "statusCode": 404,
  "message": "User not found",
  "errors": [
    "No user exists with ID: 123"
  ]
}
```

### 4. Get All Users - `GET /api/members`
This endpoint retrieves a list of all registered users.

#### Request Details
- **URL**: `/api/members`
- **Method**: GET

#### Successful Response (200 OK)
```json
{
  "statusCode": 200,
  "message": "Users retrieved successfully",
  "data": [
    {
      "userId": 1,
      "username": "johndoe",
      "email": "john.doe@example.com",
      "membershipTier": "Premium",
      "createdAt": "2023-11-15T10:30:00Z"
    },
    {
      "userId": 2,
      "username": "janedoe",
      "email": "jane.doe@example.com",
      "membershipTier": "Basic",
      "createdAt": "2023-11-16T14:45:00Z"
    }
  ]
}
```

### 5. Upgrade Membership Tier - `PUT /api/members/upgrade`
This endpoint allows users to upgrade their membership tier.

#### Request Details
- **URL**: `/api/members/upgrade`
- **Method**: PUT
- **Content-Type**: application/json

#### Request Body
```json
{
  "email": "string",
  "newMembershipTier": "string"
}
```

#### Example Request
```json
{
  "email": "john.doe@example.com",
  "newMembershipTier": "Premium"
}
```

#### Successful Response (200 OK)
```json
{
  "statusCode": 200,
  "message": "Membership tier updated successfully",
  "data": {
    "message": "Membership tier updated to Premium",
    "membershipTier": "Premium"
  }
}
```

#### Error Response (400 Bad Request)
```json
{
  "statusCode": 400,
  "message": "Upgrade Failed!",
  "errors": [
    "User is already at Premium tier",
    // or other validation errors
  ]
}
```

## Using These Endpoints in Postman
### For Registration:
1. Open Postman and create a new request
2. Set the request type to **POST**
3. Enter the URL: `https://your-api-base-url/api/members/register`
4. Go to the **Headers** tab and add: `Content-Type: application/json`
5. Go to the **Body** tab, select **raw** and choose **JSON** from the dropdown
6. Enter the registration JSON example from above
7. Click **Send** to submit the request

### For Login:
1. Open Postman and create a new request
2. Set the request type to **POST**
3. Enter the URL: `https://your-api-base-url/api/members/login`
4. Go to the **Headers** tab and add: `Content-Type: application/json`
5. Go to the **Body** tab, select **raw** and choose **JSON** from the dropdown
6. Enter the login JSON example from above
7. Click **Send** to submit the request

### For Get User by ID:
1. Open Postman and create a new request
2. Set the request type to **GET**
3. Enter the URL: `https://your-api-base-url/api/members/{userId}` (replace `{userId}` with an actual ID)
4. Click **Send** to submit the request

### For Get All Users:
1. Open Postman and create a new request
2. Set the request type to **GET**
3. Enter the URL: `https://your-api-base-url/api/members`
4. Click **Send** to submit the request

### For Upgrade Membership:
1. Open Postman and create a new request
2. Set the request type to **PUT**
3. Enter the URL: `https://your-api-base-url/api/members/upgrade`
4. Go to the **Headers** tab and add: `Content-Type: application/json`
5. Go to the **Body** tab, select **raw** and choose **JSON** from the dropdown
6. Enter the upgrade membership JSON example from above
7. Click **Send** to submit the request

## Using These Endpoints in Swagger
1. Navigate to `https://your-api-base-url/swagger` in your browser
2. Find the endpoint you want to use in the list
3. Click on the endpoint to expand it
4. Click the **Try it out** button
5. Enter the required information in the request body or parameters
6. Click **Execute**
7. View the response below

## Notes
- All password fields should follow standard security practices (minimum 8 characters, mix of uppercase, lowercase, numbers, and symbols)
- The API uses in-memory storage in this implementation, so data will be lost when the application restarts
- Available membership tiers are "Basic" and "Premium" only (Basic is the default)
- For security reasons, certain user fields like password hashes are never returned in responses