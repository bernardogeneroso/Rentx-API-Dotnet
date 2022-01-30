<p align="center">
   <img src="https://raw.githubusercontent.com/bernardogeneroso/RentX-Rocketseat/main/readme-assets/logo.png" alt="RenteX" height="260"/>
</p>

<p align="center">
   <a href="https://www.linkedin.com/in/bernardo-generoso-829ba81b0">
      <img alt="Bernardo Generoso" src="https://img.shields.io/badge/-Bernardo%20Generoso-DC1637?style=flat&logo=Linkedin&logoColor=white" />
   </a>

  <img alt="License" src="https://img.shields.io/badge/license-MIT-DC1637">
</p>

> This project already exist, but this time has been created with Web Api ASP.Net Core using language of programming C#

> Link for the other project [RentX](https://github.com/bernardogeneroso/RentX-Rocketseat);

> Content created by [Rocketseat](https://github.com/Rocketseat);

## Start project

Setup appsettings.Development.json or appsettings.json

1. Setting Environment variables -> /API/appsettings.json
    ```json
    {
       ...other code,
       "ConnectionStrings": {
          "DefaultConnection": "Server=localhost; Port=5432; User Id=postgres; Password=pass; Database=RentX"
       },
       "Mail": {
          "Host": "smtp.gmail.com",
          "Email": "example@gmail.com",
          "Port": 587,
          "User": "example@gmail.com",
          "Password": "pass"
       },
       "TokenKey": "super secret key"
    }
    ```
2. API routes
    1. Users
        - [GET] user -> /account
            - [Authorize] -> Bearer token
        - [GET] resend email confirmation link -> /account/resendEmailConfirmationLink
            - Params -> email
        - [POST] login -> /account/login
            - Body -> email, password
        - [POST] register -> /account/register
            - Body -> displayName, username, email, password
        - [POST] upload avatar -> /account/image
            - [Authorize] -> Bearer token
            - Body(form-data) -> File
        - [POST] verify email -> /account/verifyEmail
            - Params -> token, email
        - [POST] refresh token -> /account/refreshToken
            - [Authorize] -> Bearer token
            - Headers -> Cookie -> refreshToken=...
    2. Cars
         1. Car /car
            - [GET] cars -> /
                  - Params -> search[^1]
            - [POST] create car -> /
                  - [Authorize] -> Bearer token
                  - Body -> plate, brand, model, color, year, fuel, transmission, doors, seats, pricePerDay, detail - maxSpeed, topSpeed, acceleration, weight, hp
            - [PUT] update car -> /{plate}
                  - [Authorize] -> Bearer token
                  - Body -> brand, model, color, year, fuel, transmission, doors, seats, pricePerDay
            - [DELETE] delete car -> /{plate}
                  - [Authorize] -> Bearer token
            - [GET] user favorite car -> /favorite
         2. Car details -> /details
            - [GET] details -> /{plate}
            - [PUT] update details -> /{plate}
                  - [Authorize] -> Bearer token
                  - Body -> maxSpeed, topSpeed, acceleration, weight, hp
         3. Car images -> /image
            - [POST] upload image -> /{plate}
                  - [Authorize] -> Bearer token
                  - Body(form-data) -> File
            - [POST] set main image -> /{plate}/setMain
                  - [Authorize] -> Bearer token
                  - Params -> imageName
            - [DELETE] delete image -> /{plate}
                  - [Authorize] -> Bearer token
                  - Params -> imageName
         4. Car appointments -> /appointments
            - [POST] create appointment -> /{plate}
                  - [Authorize] -> Bearer token
                  - Body -> startDate, endDate
            - [GET] user scheduled -> /
            - [GET] between dates -> /between-dates
                  - Params -> startDate, endDate, startPricePerDay, endPricePerDay, fuel, transmission
            - [DELETE] delete appointment -> /{plate}

# :computer: Authors

<table>
  <tr>
    <td align="center">
      <a href="http://github.com/bernardogeneroso">
        <img src="https://avatars.githubusercontent.com/u/58465456?v=4" width="100px;" alt="Bernardo Generoso"/>
        <br />
        <sub>
          <b>Bernardo Generoso</b>
        </sub>
       </a>
       <br />
       <a href="https://www.linkedin.com/in/bernardo-generoso-829ba81b0" title="Linkedin">@bernardogeneroso</a>
       <br />
       <a href="https://github.com/bernardogeneroso/RentX-Rocketseat/commits/main" title="Code">💻</a>
    </td>
    <td align="center">
      <a href="https://github.com/Rocketseat">
        <img src="https://avatars0.githubusercontent.com/u/28929274?s=200&v=4" width="100px;" alt="Rocketseat"/>
        <br />
        <sub>
          <b>Rocketseat</b>
        </sub>
       </a>
       <br />
       <a href="https://www.linkedin.com/school/rocketseat" title="Linkedin">@Rocketseat</a>
       <br />
       <a href="https://rocketseat.com.br" title="Content creators">🚀</a>
    </td>
  </tr>
</table>

# :closed_book: License

This project is under a license [MIT](./LICENSE).

[^1]: This search work with brand and model
