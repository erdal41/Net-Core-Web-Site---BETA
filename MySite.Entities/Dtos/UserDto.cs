using MySite.Entities.Concrete;
using MySite.Shared.Entities.Abstract;

namespace MySite.Entities.Dtos
{
    public class UserDto : DtoGetBase
    {
        public User User { get; set; }
    }
}