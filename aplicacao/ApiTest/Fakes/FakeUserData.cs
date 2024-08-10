using System;
using System.Collections.Generic;
using api.Domain.Classes;

namespace ApiTest.Fakes
{
    public static class FakeUserData
    {
    public static User GetFakeUser()
        {
            return new User
            {
                Id = Guid.Parse("507d8447-316e-4736-8c0f-6fbe3cb4158d"),
                Email = "testuser@example.com",
                Password = "hashedpassword123",
                UserType = "User",
                Role = "Developer",
                TeamId = Guid.Parse("9fc0062b-7a41-4a81-b934-74a3fb9e2b25"),
                TeamName = "Development Team",
                Name = "John Doe",
                Stars = 10,
                RecommendsReceived = new List<Recommend>(),
                FeedbacksReceived = new List<Feedback>(),
                Publishes = new List<Publish>(),
                Photo = "image"
            };
        }

        public static List<User> GetFakeUsers(int count)
        {
            var users = new List<User>();

            for (int i = 0; i < count; i++)
            {
                users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    Email = $"testuser{i}@example.com",
                    Password = "hashedpassword123",
                    UserType = "User",
                    Role = "Developer",
                    TeamId = Guid.NewGuid(),
                    TeamName = $"Team {i}",
                    Name = $"User {i}",
                    Stars = i * 10,
                    RecommendsReceived = new List<Recommend>(),
                    FeedbacksReceived = new List<Feedback>(),
                    Publishes = new List<Publish>(),
                    Photo = $"image"
                });
            }

            return users;
        }
    }
}
