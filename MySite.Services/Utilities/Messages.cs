namespace MySite.Services.Utilities
{
    public static class Messages
    {
        public static class General
        {
            public static string ValidationError()
            {
                return "Bir veya daha fazla validasyon hatası ile karşılaşıldı.";
            }
        }

        public static class User
        {
            public static string NotFoundById(int userId)
            {
                return $"{userId} kullanıcı koduna ait bir kullanıcı bulunamadı.";
            }
        }

        public static class Category
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir kategori bulunamadı.";
                return "Böyle bir kategori bulunamadı.";
            }

            public static string NotFoundById(int categoryId)
            {
                return $"{categoryId} kategori koduna ait bir kategori bulunamadı.";
            }

            public static string Add(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla eklenmiştir.";
            }

            public static string Update(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla güncellenmiştir.";
            }

            public static string Delete(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla silinmiştir.";
            }

            public static string HardDelete(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla veritabanından silinmiştir.";
            }

            public static string UndoDelete(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla arşivden geri getirilmiştir.";
            }
        }

        public static class Tag
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir etiket bulunamadı.";
                return "Böyle bir etiket bulunamadı.";
            }

            public static string Add(string tagName)
            {
                return $"{tagName} adlı etiket başarıyla eklenmiştir.";
            }

            public static string Update(string tagName)
            {
                return $"{tagName} adlı etiket başarıyla güncellenmiştir.";
            }

            public static string Delete(string tagName)
            {
                return $"{tagName} adlı etiket başarıyla silinmiştir.";
            }

            public static string HardDelete(string tagName)
            {
                return $"{tagName} adlı etiket başarıyla veritabanından silinmiştir.";
            }

            public static string UndoDelete(string tagName)
            {
                return $"{tagName} adlı etiket başarıyla arşivden geri getirilmiştir.";
            }
        }

        public static class Article
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Makaleler bulunamadı.";
                return "Böyle bir makale bulunamadı.";
            }

            public static string NotFoundById(int articleId)
            {
                return $"{articleId} makale koduna ait bir makale bulunamadı.";
            }

            public static string Add(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla eklenmiştir.";
            }

            public static string Update(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla güncellenmiştir.";
            }

            public static string Delete(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla silinmiştir.";
            }

            public static string HardDelete(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla veritabanından silinmiştir.";
            }

            public static string UndoDelete(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla arşivden geri getirilmiştir.";
            }

            public static string IncreaseViewCount(string articleTitle)
            {
                return $"{articleTitle} başlıklı makalenin okunma sayısı  başarıyla attırılmıştır.";
            }
        }

        public static class Page
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Sayfalar bulunamadı.";
                return "Böyle bir sayfa bulunamadı.";
            }

            public static string Add(string pageTitle)
            {
                return $"{pageTitle} başlıklı sayfa başarıyla eklenmiştir.";
            }

            public static string Update(string pageTitle)
            {
                return $"{pageTitle} başlıklı sayfa başarıyla güncellenmiştir.";
            }

            public static string Delete(string pageTitle)
            {
                return $"{pageTitle} başlıklı sayfa başarıyla silinmiştir.";
            }

            public static string HardDelete(string pageTitle)
            {
                return $"{pageTitle} başlıklı sayfa başarıyla veritabanından silinmiştir.";
            }

            public static string UndoDelete(string pageTitle)
            {
                return $"{pageTitle} başlıklı sayfa başarıyla arşivden geri getirilmiştir.";
            }
        }

        public static class Slider
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Sliderlar bulunamadı.";
                return "Böyle bir slider bulunamadı.";
            }

            public static string Add(string sliderTitle)
            {
                return $"{sliderTitle} başlıklı slider başarıyla eklenmiştir.";
            }

            public static string Update(string sliderTitle)
            {
                return $"{sliderTitle} başlıklı slider başarıyla güncellenmiştir.";
            }

            public static string Delete(string sliderTitle)
            {
                return $"{sliderTitle} başlıklı slider başarıyla silinmiştir.";
            }

            public static string HardDelete(string sliderTitle)
            {
                return $"{sliderTitle} başlıklı slider başarıyla veritabanından silinmiştir.";
            }

            public static string UndoDelete(string sliderTitle)
            {
                return $"{sliderTitle} başlıklı slider başarıyla arşivden geri getirilmiştir.";
            }
        }

        public static class SliderCategory
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir slider kategorisi bulunamadı.";
                return "Böyle bir kategori bulunamadı.";
            }

            public static string Add(string categoryName)
            {
                return $"{categoryName} adlı slider kategorisi başarıyla eklenmiştir.";
            }

            public static string Update(string categoryName)
            {
                return $"{categoryName} adlı slider kategorisi başarıyla güncellenmiştir.";
            }

            public static string Delete(string categoryName)
            {
                return $"{categoryName} adlı slider kategorisi başarıyla silinmiştir.";
            }

            public static string HardDelete(string categoryName)
            {
                return $"{categoryName} adlı slider kategorisi başarıyla veritabanından silinmiştir.";
            }

            public static string UndoDelete(string categoryName)
            {
                return $"{categoryName} adlı slider kategorisi başarıyla çöp kutusundan geri getirilmiştir.";
            }
        }

        public static class Comment
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir yorum bulunamadı.";
                return "Böyle bir yorum bulunamadı.";
            }

            public static string Approve(int commentId)
            {
                return $"{commentId} no'lu yorum başarıyla onaylanmıştır.";
            }

            public static string Add(string createdByName)
            {
                return $"Sayın {createdByName}, yorumunuz başarıyla eklenmiştir.";
            }

            public static string Update(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla güncellenmiştir.";
            }

            public static string Delete(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla silinmiştir.";
            }

            public static string HardDelete(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla veritabanından silinmiştir.";
            }

            public static string UndoDelete(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla arşivden geri getirilmiştir.";
            }
        }

        public static class Upload
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir medya dosyası bulunamadı.";
                return "Böyle bir medya dosyası bulunamadı.";
            }

            public static string Add(string uploadFileName)
            {
                return $"{uploadFileName} adlı medya dosyası başarıyla eklenmiştir.";
            }

            public static string Update(string uploadFileName)
            {
                return $"{uploadFileName} adlı medya dosyası başarıyla güncellenmiştir.";
            }

            public static string Delete(string uploadFileName)
            {
                return $"{uploadFileName} adlı medya dosyası başarıyla silinmiştir.";
            }
        }
    }
}