using System.ComponentModel.DataAnnotations.Schema;

namespace R61M9C10_W01.ViewModels
{
    public class PROFILEVM
    {
        public string Userid { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile ProfilePic { get; set; }
    }
}
