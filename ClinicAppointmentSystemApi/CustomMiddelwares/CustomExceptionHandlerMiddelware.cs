using Microsoft.AspNetCore.Mvc;

namespace ClinicAppointmentSystemApi.CustomMiddelwares
{
    public class CustomExceptionHandlerMiddelware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddelware> _logger;

        public CustomExceptionHandlerMiddelware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddelware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext Context)
        {

            try
            {
                await _next.Invoke(Context);
                await HandleNotFoundEndPointAsync(Context);
            }


            catch (Exception ex)
            {
                //logging
                _logger.LogError(ex, "Something Went Wrong");
                // return Custom Error Response

                var Problem = new ProblemDetails()
                {
                    Title = "UnExpected Error",

                    Detail = ex.Message,
                    Instance = Context.Request.Path,
                    Status = ex switch
                    {

                        //  NotFoundException => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError
                    }
                };
                Context.Response.StatusCode = Problem.Status.Value;
                await Context.Response.WriteAsJsonAsync(Problem);

            }




   






        }


        private static async Task HandleNotFoundEndPointAsync(HttpContext context)
        {

            if (context.Response.StatusCode == StatusCodes.Status404NotFound && !context.Response.HasStarted)
            {
                var problem = new ProblemDetails()
                {
                    Title = "Error while processing a request, End Point Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"EndPoint{context.Request.Path} Not Found",
                    Instance = context.Request.Path
                };


                await context.Response.WriteAsJsonAsync(problem);


            }
        }
    }
}
