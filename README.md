# JWT-With-NETCore
 The purpose of this project is to experience the JWT structuring on NETCore and to prepare a template

 ## Token-Based Authentication (JWT)
- Cookie-Based Auth
  - Can be used if not fed from an external API.
  - Can be exemplified by MVC.
- Token-Based Auth
  - Preferred if fed from an API.
  - Token is a meaningful string expression.
  - A separate server and API can be written to generate tokens.
  - Data is carried within the token, but passwords should not be included.

### JWT
- Header: Encryption information.
- Payload: Data
  - key-value pairs (e.g., sub for user ID).
- Signature: Where the signing information is entered (encryption).

**Note:** The Payload and Header sections are encoded with base64Url, making it accessible to anyone (**passwords should not be added to the payload**). However, without the signature, no changes can be made to the token. If any changes are attempted, the token will be considered invalid.

**Note:** So, even if you obtain the token, you cannot manipulate it to access someone else's data. To enable changes to the token, knowledge of the security password used during signing is required.

**Note:** To protect the user's information when obtaining the token, we set the token's lifespan to be short.

### Security
- Token expiration plays an important role in security.
- Decodable sensitive data, such as important passwords, should not be carried in the payload.
  - Roles can be carried.

**Note:** The data in the payload is referred to as **Claims**.

- The Identity library can be used for protocols.

### Access Token and Refresh Token
- Access token is used for accessing.
- Refresh token is used to retrieve the access token when it expires.
  - It can be any value.
  - It does not contain any data. It is a random string value.
  - Refresh token is only sent to the API that distributes tokens.
  - Access token should have a short lifespan, while refresh token should have a longer lifespan.

**Note:** Access Token and Refresh Token are sent together. When the access token expires, it is renewed through the refresh token without the user noticing. The refresh token also has an expiration. If it expires, the user is returned to the login screen. For example, if we set the access token lifespan to 7 days and the refresh token lifespan to 10 days, even if we log in on the 10th day, both the Access and Refresh tokens are renewed.

**Note:** As a best practice, Access Token and Refresh Token should be used together, and the Access Token should have a short lifespan.
- The Refresh Token is stored in the database per user, while the Access Token is not stored.

**Note:** In decoding, the token's signature, lifespan, and the server it was generated from are important.

## Identity
- Microsoft Identity, a ready-to-use library for performing identity operations, is utilized in the project.
- Major services included in the library:
  - UserManager: For user-related operations.
  - SignInManager: For login processes.
  - RoleManager: For managing user roles.

## Sign
- Asymmetric signing is used for signing processes nowadays.
- It involves both a Private key and a Public key.
- The Authentication server signs with the private key, while APIs verify with the public key.

## Options Pattern - Token Options
- Audience: Determines which APIs the token can request.
- Issuer: Specifies who issued the token. The service that distributes the token.
- AccessTokenExpiration: The lifespan of the Access Token.
- RefreshTokenExpiration: The lifespan of the Refresh Token.
- SecurityKey: The symmetric key used for token signing.

