using System;
using System.Collections.Generic;

#nullable disable

namespace S4.SocialMediaAzure.Entities.Models
{
    public partial class AspNetComment
    {
        public int PkId { get; set; }
        public string FkUserId { get; set; }
        public int FkPostId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsEdited { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public virtual AspNetPost FkPost { get; set; }
        public virtual AspNetUser FkUser { get; set; }
    }
}
