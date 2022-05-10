using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MySite.Entities.Dtos
{
    public class UploadUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Dosya")]
        [Required(ErrorMessage = "Lütfen, bir {0} seçiniz.")]
        public string FileName { get; set; }

        [DisplayName("Alternatif Metin")]
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        public string AltText { get; set; }

        [DisplayName("Başlık")]
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        public string Title { get; set; }

        [DisplayName("Alt Başlık")]
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        public string SubTitle { get; set; }

        [DisplayName("Açıklama")]
        [MaxLength(500, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        public string Note { get; set; }

        [DisplayName("Dosya Türü")]
        [MaxLength(20, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        public string ContentType { get; set; }

        [DisplayName("Dosya Boyutu")]
        public long Size { get; set; }

        [DisplayName("Yüklenen Tarih")]
        public virtual DateTime CreatedDate { get; set; }

        [DisplayName("Yükleyen Kullanıcı")]
        public virtual string CreatedByName { get; set; }
    }
}