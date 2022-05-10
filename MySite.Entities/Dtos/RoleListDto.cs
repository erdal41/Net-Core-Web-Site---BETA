using MySite.Entities.Concrete;
using System.Collections.Generic;

namespace MySite.Entities.Dtos
{
    public class RoleListDto
    {
        public IList<Role> Roles { get; set; }
    }
}