using System;

namespace KC.Template.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsDisabled { get; set; }

        public int CreatedUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedUserId { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
