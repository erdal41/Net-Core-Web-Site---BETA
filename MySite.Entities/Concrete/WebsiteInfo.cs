using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MySite.Entities.Concrete
{
    public class WebsiteInfo
    {
        [DisplayName("Site Adı/Başlığı")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir.")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        [MinLength(5, ErrorMessage = "{0} alanı {1} karakterden küçük olmamalıdır.")]
        public string Title { get; set; }
        [DisplayName("Menü Üzerindeki Site Adı/Başlığı")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string MenuTitle { get; set; }
        [DisplayName("Seo Açıklama")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string SeoTitle { get; set; }
        [DisplayName("Seo Başlığı/Site Genel Başlık")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string SeoDescription { get; set; }
        [DisplayName("Seo Etiketleri")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string SeoTags { get; set; }
        [DisplayName("Seo Yazar")]
        [MaxLength(60, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string SeoAuthor { get; set; }
    }
}
