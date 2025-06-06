namespace BackendTest.Internal.Exceptions
{
    public enum InternalApiErrorCodes
    {
        ItemNotFound = 101,

        CannotUpdate = 102,

        UserWithGivenEmailAlreadyExists = 103,

        EmptyName = 104,

        EmptyEmail = 105,

        EmptyPassword = 106,

        InvalidEmail = 107,

        BadRequest = 400,

        Unauthorized = 401,

        NotFound = 404,

        Conflict = 409,

        UnhandledException = 500
    }
}
