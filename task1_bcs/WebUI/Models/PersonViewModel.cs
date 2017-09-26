using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class PersonViewModel
    {
        [Display(Name = "PersonId")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "Поле ФИО обязательно.")]
        [RegularExpression(@"([A-Za-zА-яа-я\s]+)", ErrorMessage = "Поле может содержать только буквы латинского и русского алфавитов")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Поле Телефон обязательно.")]
        [RegularExpression(@"\+7\d{10}", ErrorMessage = "Поле должно соответствовать формату: +7 xxx xxx xx xx")]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string CityName { get; set; }
    }
}