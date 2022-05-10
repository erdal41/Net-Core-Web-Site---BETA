using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MySite.Entities.Dtos
{
    public class TagUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Etiket Adı")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(70, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        public string Name { get; set; }

        [DisplayName("Etiket Açıklaması")]
        [MaxLength(500, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        public string Note { get; set; }

        [DisplayName("Aktif Mi?")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        public bool IsActive { get; set; }

        [DisplayName("Silindi Mi?")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        public bool IsDeleted { get; set; }
    }
}