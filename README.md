# Digits To Words Converter

## Overview

DigitsToWords is a web app with interactive UI in React and a .NET 6 API that is designed to convert numeric strings into their word representations. This service is particularly useful for financial appliations, such as cheque writing systems, where numbers need to be represented in words.

## Features

- **Convert Numbers to Words**: Convert any valid positive number, including those with decimal points, into words.
- **Validation**: Validates input to ensure it is a positive number that can be converted.

## Getting Started

### Prerequisites

- .NET 6 SDK
- Visual Studio 2019 or later

### Running Locally

- Clone repository to your local machine.

```bash
git clone https://github.com/akashirani110/DigitsToWords.git
```

- Navigate to the Project Directory:

```bash
cd DigitsToWords
```

- To run the API, open the solution in Visual Studio

- Run the startup project, DigitsToWords.Api. The API will be available at `https://localhost:7138/api/NumberToWords/convert`.

- You can test the API from the Swagger UI as welland can be accessed at `https://localhost:7138/swagger/index.html`

- To run the client app, navigate to the client-app directory

```bash
cd client-app
```

- Install the required packages by using

```bash
npm install
```

- Run the client app by using

```bash
npm run
```

- The client app is listening at port localhost:3000. You can access it at `http://localhost:3000/`

### Swagger Usage

Send a GET request to the endpoint with a numeric input for e.g.

```text
123.45
```

Response

```json
"ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS"
```

### Client App

Enter the number in the input field and you will get the equivalent numeric input converted into currency words. You can add cents as well and the number of digits (excluding cents) a dollar value can have is 14.

## Architecture

### Services

- _NumberValidationService_: Validates the input string to ensure it's a valid numeric value.
- _NumberConversionService_: Converts validated numeric strings into their word representations.

### Controllers

- _NumberToWordsController_: Exposes an endpoint to convert numeric strings to words, utilizing the services mentioned above.

## Testing

Unit tests are provided for both services to ensure reliability and correctness.

## Running Tests

Execute the following command to run the unit tests:

```bash
dotnet test
```

OR

Right click on the NumberToDigits.Api.Tests project and select Run Tests option.
