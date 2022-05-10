using System.ComponentModel.DataAnnotations;

namespace MySite.Entities.ComplexTypes
{
    public enum FilterBy
    {
        [Display(Name = "Kategori")]
        Category = 0,
        [Display(Name = "Etiket")]
        Tag = 1,
        [Display(Name = "Tarih")]
        Date = 2,
        [Display(Name = "Okunma Sayısı")]
        ViewCount = 3,
        [Display(Name = "Yorum Sayısı")]
        CommentCount = 4,
    }
}