using Course_ranking.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Text;
using System.Linq;

namespace Course_ranking.Controllers
{

    public class DashboardController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly CourseContexte courseContexte;
        public DashboardController(IWebHostEnvironment webHostEnvironment, CourseContexte courseContexte)
        {
            this._webHostEnvironment = webHostEnvironment;
            this.courseContexte = courseContexte;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MyResult()
        {
            string email= HttpContext.Session.GetString("UserEmail");
            var data = courseContexte.CourseUsers.Where(x => x.Email == email).ToList().OrderByDescending(x=>x.Id).Take(1);
            if (data != null)
            {
                ViewBag.data = data.FirstOrDefault();
            }
          
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile File,string fileType)
        {
            if (File.Length > 0)
            {
                var extension = Path.GetExtension(File.FileName);
                string Destinationpath = "";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "Uploadfile"+extension);
                if (fileType == "C")
                {
                    Destinationpath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "CfileTemplate.txt");

                }
                else
                {
                    Destinationpath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "PythonTemlate.txt");
                    //var Destinationpath = "D:\\Course_ranking\\Course_ranking\\wwwroot\\uploads\\CfileTemplate.txt";
                }

                using (var stream = System.IO.File.Create(filePath))
                {
                    await File.CopyToAsync(stream);
                }
                string line = "";
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    line = reader.ReadToEnd();
                }
                string dest = "";
                FileStream Deststream = new FileStream(Destinationpath, FileMode.Open);
                using (StreamReader reader = new StreamReader(Deststream))
                {
                    dest = reader.ReadToEnd();
                }
                var result = CalculateSimilarity(line, dest)*100;
                CourseUser courseUser = new CourseUser();
                courseUser.Email = HttpContext.Session.GetString("UserEmail");
                courseUser.Score = result;
                courseUser.Langage = fileType;
                courseContexte.CourseUsers.Add(courseUser);
                courseContexte.SaveChanges();

            }
            ViewBag.Data = "Success";
            return View();
        }
        private double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }
        int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }

    }
}
