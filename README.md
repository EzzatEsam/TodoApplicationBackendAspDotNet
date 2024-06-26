# TodoProj API Dotnet

This is an ASP.NET API for a Todo application supporting multiple projects and multiple Todos for each project with user management and authorization. It utilizes JWT (JSON Web Tokens) for authentication.

This  only provides the API endpoints for the Todo application.

A wokring implementation of the frontend made with react and tailwind can be found in this repo [TodoAppFrontendReact
](https://github.com/EzzatEsam/TodoAppFrontendReact)


![](Github/Screenshots/2024-06-26-21-38-48.png)
## Getting Started

To get started with this project, follow the steps below:

1. Clone the repository.
2. Install the required dependencies.
3. Configure the database connection.
4. Build and run the project.

## Dependencies

This project has the following dependencies:

- DotNet 8+
- ASP.NET Core
- Entity Framework Core (Dotnet library)
- JWT (JSON Web Tokens) (Dotnet library)
- Sqlite (Dotnet library)

Make sure to have these dependencies installed before running the project.

## Configuration

To configure the database connection, update the `appsettings.json` file with your database credentials.

## Usage

Once the project is up and running, you can use the API endpoints to manage your Todo application. Here are some of the available endpoints:




### Base URL:
** `${serverAddr}` (replace with actual server address)

### Authentication Format


**Authorization Type:** Bearer Token

**Token Location:** Authorization Header

**Header Format:**

```
Authorization: Bearer {token}
```

* Replace `{token}` with your valid JWT token obtained from the `/api/UserAccount/login` endpoint.

**Example:**

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkxvcmVtIpsumIiwiZXhwIjoxNjU3MjM4NDgwfQ.sNzbAfAZKibFsztJrYvSUe29555L0xo-zKRsK_4g-4U
```

**Notes:**

* Include the Authorization header in all requests that require authentication.
* The API will return a 401 (Unauthorized) status code if a valid token is not provided.


### User Account

* **POST /api/UserAccount/register**
  * Request Body: `SignUpModel`
    * `userName` (string): Username for signup
    * `email` (string): User email for signup
    * `password` (string): User password for signup
    * `firstName` (string): User first name
    * `lastName` (string): User last name
  * Response:
    * Success (200): Empty response array (may change based on actual implementation)
    * Failure: Array of `SignUpErrors`
      * `code` (string): Error code
      * `description` (string): Error description
* **POST /api/UserAccount/login**
  * Request Body: `LoginModel`
    * `email` (string): User email for login
    * `password` (string): User password for login
  * Response:
    * Success (200): `LoginReponse` object
      * `token` (string): JWT token for authentication
    * Failure (including bad credentials): Array containing error message (may change based on actual implementation)
* **GET /api/UserAccount/me**
  * Authorization: Required
  * Response:
    * Success (200): `User` object
      * `firstName` (string): User first name
      * `lastName` (string): User last name
      * `email` (string): User email
      * `username` (string): Username
    * Failure (401/unauthorized): Null

### Todo Groups

* **GET /api/TodoGroups**
  * Authorization: Required
  * Response:
    * Success (200): Array of `TodoGroup` objects
      * `id` (number): Group ID
      * `name` (string): Group name
      * `TodoItems` (array of `TodoItem` objects): May be empty (backend populates on retrieval)
    * Failure: Null
* **POST /api/TodoGroups**
  * Authorization: Required
  * Request Body: Partial `TodoGroup` object
    * `name` (string): Name of the new group
  * Response:
    * Success (201): Created `TodoGroup` object with populated ID
    * Failure: Null
* **PUT /api/TodoGroups/:id**
  * Authorization: Required
  * Path Variable: `id` (number): ID of the group to modify
  * Request Body: `TodoGroup` object
    * `id` (number): ID of the group (should match path variable)
    * `name` (string): Updated group name
  * Response:
    * Success (204): No content 
    * Failure: Null
* **DELETE /api/TodoGroups/:id**
  * Authorization: Required
  * Path Variable: `id` (number): ID of the group to delete
  * Response:
    * Success (204): No content 
    * Failure: False

### Todo Items

* **GET /api/TodoItems/ParentGroup/:groupId**
  * Authorization: Required
  * Path Variable: `groupId` (number): ID of the parent group
  * Response:
    * Success (200): Array of `TodoItem` objects within the group
      * `id` (number): Item ID
      * `groupId` (number): Parent group ID
      * `name` (string): Item name
      * `isDone` (boolean): Indicates item completion status
      * `description` (string): Item description
      * `dueDate` (Date): Due date of the item
      * `createdDate` (Date): Creation date of the item
    * Failure: Null
* **POST /api/TodoItems**
  * Authorization: Required
  * Request Body: `TodoItem` object
    * All fields from `GET /api/TodoItems/ParentGroup/:groupId` response
  * Response:
    * Success (201): Created `TodoItem` object with populated ID
    * Failure: Null
* **PUT /api/TodoItems/:id**
  * Authorization: Required
  * Path Variable: `id` (number): ID of the item to modify
  * Request Body: `TodoItem` object (all fields are optional for update)
  * Response:
    * Success (204): No content 
    


## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvements, please open an issue or submit a pull request.

## License

This project is licensed under the [Apache 2.0](LICENSE).


