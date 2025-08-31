using Microsoft.AspNetCore.Identity;
using TodoApi.Models;

namespace TodoApi.Data;

public static class Seed
{
    public static async Task SeedData(DataContext context, UserManager<User> userManager)
    {
        if (!userManager.Users.Any())
        {
            var users = new List<User>
            {
                new(){FirstName="Erik", LastName="Gustavsson", UserName="Erik@gmail.com", Email="Erik@gmail.com", RoleName="Admin" },
                new(){FirstName="Eva", LastName="Eriksson",   UserName="eva@gmail.com",  Email="eva@gmail.com",  RoleName="User"  },
                new(){FirstName="Charlotte", LastName="Magnusson", UserName="lotta@gmail.com", Email="lotta@gmail.com", RoleName="User" },
            };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }

        if (context.ToDos.Any()) return;

        var toDos = new List<ToDo>
        {
            new(){Title="Städa",   UserId = userManager.Users.First().Id },
            new(){Title="Handla",  UserId = userManager.Users.First().Id },
            new(){Title="Laga mat",UserId = userManager.Users.Skip(1).First().Id },
            new(){Title="Diska",   UserId = userManager.Users.Skip(1).First().Id },
            new(){Title="Tvätta",  UserId = userManager.Users.Skip(2).First().Id },
            new(){Title="Stryka",  UserId = userManager.Users.Skip(2).First().Id },
        };

        context.ToDos.AddRange(toDos);
        await context.SaveChangesAsync();
    }
}
