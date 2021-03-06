using System;

namespace MySite.Shared.Entities.Abstract
{
    public abstract class EntityUserBase
    {
        public virtual int Id { get; set; }
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now; // override CreatedDate = new DateTime(2020/01/01);
        public virtual DateTime ModifiedDate { get; set; } = DateTime.Now;
        public virtual string CreatedByName { get; set; } = "Admin";
        public virtual string ModifiedByName { get; set; } = "Admin";
        public virtual string Note { get; set; }
    }
}