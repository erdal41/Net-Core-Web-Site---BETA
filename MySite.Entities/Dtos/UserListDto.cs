using MySite.Entities.Concrete;
using MySite.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace MySite.Entities.Dtos
{
    public class UserListDto : DtoGetBase
    {
        public IList<User> Users { get; set; }
    }
}