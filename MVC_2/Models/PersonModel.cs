using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace MVC_2.Models
{
    public class PersonModel
    {
        public PersonModel() { }
        public int id { set; get; }
        [Required, DisplayName("Tên")]
        public string firstName { set; get; }
        [Required, DisplayName("Họ")]
        public string lastName { set; get; }
        [Required, DisplayName("Ngày Sinh")]
        public DateTime dateOfBirth { set; get; }
        [Required, DisplayName("Giới Tính")]
        public string gender { set; get; }
        [Required, DisplayName("Số Điện Thoại")]
        public int phoneNumber { set; get; }
        [Required, DisplayName("Nơi Sinh")]
        public string birthPlace { set; get; }
        [Required, DisplayName("Tuổi")]
        public int age { set; get; }
        [Required, DisplayName("Tình Trạng Tốt Nghiệp")]
        public bool isGraduated { set; get; }
        [Required, DisplayName("Email")]
        public string email { set; get; }

    }
}
