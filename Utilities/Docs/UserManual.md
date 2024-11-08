# My Service API Documentation

Welcome to the API UserManual for the **Service**. This service allows you to interact with mock **data**. Below is an overview of the available endpoints.

## Endpoints
### GET /api/xxx/all
- **Description**: Retrieve all items from the list.

### GET /api/xxx/id or name
- **Description**: Retrieve the item from the list, prioritizing the `id`, the `name` field is protected from script injection.
- **Parameters**:
    - `id` (optional): The unique identifier of the item to fetch.
    - `name` (optional): The identifier of the item to fetch, it utilizes a pattern matching function.

- **Notes**: 
    - This endpoint returns a single item based on their unique `id` or `name`.
    - If the item is not found, the entire list will appear with the prompt to pick someone from it.

    
### DELETE /api/xxx/id or name
- **Description**: Delete an item in the list, prioritizing the `id`, the `name` field is protected from script injection.
- **Parameters**:
    - `id` (optional): The unique identifier of the item to fetch.
    - `name` (optional): The identifier of the item to fetch.

- **Notes**: 
    - This endpoint deletes a single item based on their unique `id` or unique `name`.
    - If the item is not found, the entire list will appear with the prompt to pick someone from it.

    
### PUT /api/User/{id/}
- **Description**: Update an user in the list, chosen by the `id`.
- **Parameters**:
    - `id` (required): The unique identifier of the user to fetch.

- **Notes**: 
    - This endpoint updates a single user chosen by their unique `id`, by implementing changes to the json object schema.
    - If the `id` of the user is not found or the `email`  format is wrong, the entire list will appear with the prompt to check the `email` format and verify if the `id` is correct.

### PUT /api/Product/{id\}
- **Description**: Update a product in the list, chosen by the `id`.
- **Parameters**:
    - `name` (required): The unique identifier of the product to fetch.

- **Notes**: 
    - This endpoint updates a single product chosen by their unique `id`, by implementing changes to the json object schema.
    - If the `id` of the product is not found, the entire list will appear with the prompt to verify if the `id` is correct.
    - If the `price` of the product is not a number, an error 400 will appear.

    
### POST /api/User/{name\}/{email\}
- **Description**: Create a new user to add to the list, both fields are protected from script injection and the `email` field must abide to the following format: **example@domain.com**.
- **Parameters**:
    - `name` (required): The name field of the user to insert.
    - `email` (required): The email field of the user to insert.

- **Notes**: 
    - This endpoint creates a single user based on the `name` and `email` you insert, the `id` is auto-generated.
    - If the `email` is not valid, the endpoint will prompt to check if the `email` abides to the correct format.

### POST /api/Product/{name\}/{price\}
- **Description**: Create a new product to add to the list, the `name` field is protected from script injection.
- **Parameters**:
    - `name` (required): The name field of the user to insert.
    - `price` (required): The price field of the user to insert.

- **Notes**: 
    - This endpoint creates a single product based on the `name` and `price` you insert, the `id` is auto-generated.
