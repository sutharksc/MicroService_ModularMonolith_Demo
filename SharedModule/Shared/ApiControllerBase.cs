namespace SharedModule.Shared
{
    public abstract class ApiControllerBase
    {
        //protected virtual IActionResult ApiResult(Result result)
        //{
        //    return result.Error.Status switch
        //    {
        //        200 => Results.Ok(result), // 200 OK
        //        400 => BadRequest(result), // 400 Bad Request
        //        404 => NotFound(result),
        //        500 => new ObjectResult(result)
        //        {
        //            StatusCode = 500 // 500 Internal Server Error
        //        },
        //        _ => new ObjectResult(result)
        //        {
        //            StatusCode = result.Error.Status // 500 Internal Server Error
        //        } // Default case for other status codes
        //    };
        //}
    }
}
