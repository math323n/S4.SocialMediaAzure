using System;
using System.Collections.Generic;

#nullable disable

namespace S4.SocialMediaAzure.Entities.Models
{
    public partial class AspNetPost
    {
        public AspNetPost()
        {
            AspNetComments = new HashSet<AspNetComment>();
        }

        public int PkId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsEdited { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string FkUserId { get; set; }

        public virtual AspNetUser FkUser { get; set; }
        public virtual ICollection<AspNetComment> AspNetComments { get; set; }
    }
}
