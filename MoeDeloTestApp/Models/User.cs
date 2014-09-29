using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoeDeloTestApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [DisplayName("Имя")]
        [Required(ErrorMessage = "Укажите имя")]
        public string FirstName { get; set; }

        [DisplayName("Фамилия")]
        [Required(ErrorMessage = "Укажите фамилию")]
        public string LastName { get; set; }

        [DisplayName("Должность")]
        public Post Post { get; set; }
    }
}