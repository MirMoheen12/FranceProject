using Microsoft.EntityFrameworkCore;

namespace Course_ranking.Models
{
    public class CourseContexte : DbContext
    {
        public CourseContexte(DbContextOptions<CourseContexte> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<AllUsers> AllUsers { get; set; }
        public DbSet<CourseUser> CourseUsers { get; set; }
    }
}
