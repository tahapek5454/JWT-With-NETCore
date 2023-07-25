using Microsoft.AspNetCore.Authorization;

namespace MiniApp1.API.Requirements
{
    public class BirthdatRequirement: IAuthorizationRequirement
    {
        // for make authorization with based policay must implementation

        public int Age { get; set; }

        public BirthdatRequirement(int age)
        {
            Age = age;
        }
    }

    //now write business

    public class BirthdayRequirementHandler : AuthorizationHandler<BirthdatRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BirthdatRequirement requirement)
        {
            var birtDate = context.User.Claims.FirstOrDefault(c => c.Type == "birth-date");
            // var birtDate = context.User.FindFirst("birth-date"); alternative

            if(birtDate == null)
            {
                context.Fail(); // break evertying forbidden
                return Task.CompletedTask;
            }

          
            var today = DateTime.UtcNow;
            var age = today.Year - Convert.ToDateTime(birtDate.Value).Year;
            

            if(requirement.Age <= age)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;

            
        }
    }
}
