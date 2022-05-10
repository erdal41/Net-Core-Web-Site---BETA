using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MySite.Entities.Concrete
{
    public class ContactPageInfo
    {
        [DisplayName("Başlık")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir.")]
        [MaxLength(150, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        [MinLength(5, ErrorMessage = "{0} alanı {1} karakterden küçük olmamalıdır.")]
        public string Title { get; set; }

        [DisplayName("Seo Başlığı")]
        public string SeoTitle { get; set; }

        [DisplayName("Seo Yazarı")]
        public string SeoAuthor { get; set; }

        [DisplayName("Seo Etiketleri")]
        public string SeoTags { get; set; }

        [DisplayName("Seo Açıklaması")]
        public string SeoDescription { get; set; }
    }
}