using Bogus;
using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class SeedData
    {
        private static Faker faker = null!;

        public static async Task InitAsync(LmsApiContext db)
        {
            if (await db.Course.AnyAsync()) return;

            faker = new Faker("sv");

            var courses = GetCourses();
            await db.AddRangeAsync(courses);

            //var modules = GetModules(courses);
            //await db.AddRangeAsync(modules);
            try
            {
                await db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                //Do something
                System.Console.WriteLine(ex.ToString());

            }
        }

        private static IEnumerable<Module> GetModules(IEnumerable<Course> courses)
        {
            var modules = new List<Module>();

            foreach (var course in courses)
            {
                var n = faker.Random.Int(0, 5);
                for (int i = 0; i < n; i++)
                {
                    var module = new Module
                    {
                        StartDate = course.StartDate,
                        CourseId = course.Id,
                        Title = faker.Lorem.Sentence(5)

                    };
                    modules.Add(module);
                }

            }
            return modules;
        }

        private static IEnumerable<Module> GetModules()
        {
            var modules = new List<Module>();


            var n = faker.Random.Int(0, 5);
            for (int i = 0; i < n; i++)
            {
                var module = new Module
                {
                    //StartDate = faker.Date.Between(DateTime.Now.AddYears(-3), DateTime.Now),
                    StartDate = faker.Date.Past(3, DateTime.Now),
                    //CourseId = course.Id,
                    Title = faker.Lorem.Text()

                };
                modules.Add(module);
            }


            return modules;
        }
        private static IEnumerable<Course> GetCourses()
        {
            var courses = new List<Course>();

            for (int i = 0; i < 10; i++)
            {
                var course = new Course()
                {
                    Title = faker.Company.CatchPhrase(),
                    StartDate = faker.Date.Between(DateTime.Now.AddYears(-3), DateTime.Now)
                    ,Modules = (ICollection<Module>)GetModules()
                    //Modules = new List<Module> {
                    //    new Module { Title = "TEST Module" }
                    //    }
                };
                courses.Add(course);
            }
            return courses;
        }
    }
}
