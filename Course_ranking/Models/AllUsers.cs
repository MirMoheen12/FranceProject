using System.ComponentModel.DataAnnotations;

namespace Course_ranking.Models
{
    public class AllUsers
    {
        [Key]
        public int Uid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string useremail { get; set; }
        public string userePassword { get; set; }
        public string city { get; set; }
        public string userstate { get; set; }
        public string zip { get; set; }

    }
}
