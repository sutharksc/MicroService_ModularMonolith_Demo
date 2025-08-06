using System.Net;

namespace SharedModule.Shared
{
    public static class ClientErrors
    {
        #region Erros403
        public static readonly Error EmailAlreadyExists = new Error("40301", "Email already exists.", (int)HttpStatusCode.Forbidden);
        public static readonly Error UserAlreadyExists = new Error("40302", "UserName already exists.Please try another", (int)HttpStatusCode.Forbidden);
        public static readonly Error UserAccountNotVerified = new Error("40303", "Your account is not verified yet. Please contact to your administrator", (int)HttpStatusCode.Forbidden);
        #endregion

        #region Erros404
        public static readonly Error UserNotFound = new Error("40401", "User not found.", (int)HttpStatusCode.NotFound);
        public static readonly Error OrderNotFound = new Error("40402", "Order not found.", (int)HttpStatusCode.NotFound);
        #endregion

        #region Errors423
        public static readonly Error UserLockout = new Error("42310", "Your account is locked out. Please try again later.", (int)HttpStatusCode.Locked);
        #endregion

        #region Errors400
        public static readonly Error InvalidCredentials = new Error("40001", "Credentials not valid", (int)HttpStatusCode.BadRequest);
        #endregion

        public static Error InternalServerError(string message, int statusCode = (int)HttpStatusCode.InternalServerError)
            => new Error("50020", $"Unexpected Error :- {message}", statusCode);
    }
}
