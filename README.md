# GF Machining Solutions Technical Challenge

This project is a solution to the technical challenge presented given by GF Machining Solutions. The challenge involves interacting with a secret API to obtain magic numbers, calculating the sum of these numbers, and passing the result to a POST endpoint. If the solution is correct, a key will be received through a provided callback.


## Solution behavior

Every minute a background service will query the magic numbers, do the sum and give the result providing a Rest API endpoint as a callback. If the result is correct, it will log the result which should reveal a hidden key.

## DevOps
The provided azure pipeline trigger when a new commit on main it pushed. It will first run the tests and if passed it will produce a docker image on azure container registry. As soon as it has been pushed it will generate a release on Azure DevOps which will deploy it on an azure app service.

## Getting Started

### Prerequisites

Before running the solution, ensure that you have the following prerequisites set up:

- [.NET SDK](https://dotnet.microsoft.com/download) installed
- Access to the secret API with a valid token

### Configuration

The solution requires three parameters for configuration:

1. **SecretAPI:Url**: The endpoint where the magic numbers are obtained.
2. **SecretAPI:Key**: The API token for accessing the secret API.
3. **SecretAPI:CallbackUrl**: The callback URL for the service's REST API.

These parameters can be configured in three ways: via `appsettings.json`, using secrets, or through environment variables. When setting them through environment variables, replace `:` with `__` and add `GF_` before each parameter.

Example : **SecretAPI:Url** become **GF_SecretAPI__Url**