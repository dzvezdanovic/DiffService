# DiffService
Welcome to the DiffService, a service that allows you to compare binary data and identify differences between them. This README provides essential information on how to use and integrate the DiffService into your projects.

## Overview

The DiffService provides HTTP endpoints for comparing binary data and retrieving diff results in JSON format. It consists of three main endpoints:

1. **Left Endpoint:** `/v1/diff/<ID>/left`
2. **Right Endpoint:** `/v1/diff/<ID>/right`
3. **Diff Endpoint:** `/v1/diff/<ID>`

## Features

### 1. HTTP Endpoints

- **Left Endpoint:** Accepts JSON containing base64 encoded binary data.
- **Right Endpoint:** Accepts JSON containing base64 encoded binary data.
- **Diff Endpoint:** Returns comparison results in JSON format.

### 2. Comparison Results

The diff results provided by the Diff Endpoint include:

- If the data is equal, it returns that they are equal.
- If the data is not of equal size, it returns that they are not equal.
- If the data is of the same size, it provides insights into where the differences are, without providing the actual diffs. It includes offsets and lengths of the differences.

## Getting Started

To integrate the Binary Diff Tool into your project, follow these steps:

1. **Installation:** Clone the repository from [GitHub Repository](https://github.com/dzvezdanovic/DiffService.git).
2. **Dependencies:** Ensure you have the required dependencies installed. (List dependencies if applicable)
3. -Xunit/NewtonSoft.JSON
4. **Usage:** Use the provided HTTP endpoints to send binary data for comparison and retrieve diff results.

## API Usage

### Left Endpoint

Send base64 encoded binary data to the Left Endpoint:
POST v1/diff/<ID>/left
```json
{
"data": "base64_encoded_data"
}
```
### Left Endpoint

Send base64 encoded binary data to the Right Endpoint:
POST v1/diff/<ID>/right
```json
{
"data": "base64_encoded_data"
}
```
### Diff Endpoint

Retrieve comparison results using the Diff Endpoint:
GET v1/diff/<ID>
```json
{
  "result": "equal",
  "message": "Data is equal."
}
```
##Contributing
We welcome contributions from the community! If you find any issues or have suggestions for improvements, please feel free to submit a pull request or open an issue on GitHub.

##Contact
For any inquiries or support, please contact me at dzvezdanovic@outlook.com.

Thank you for reading this!
