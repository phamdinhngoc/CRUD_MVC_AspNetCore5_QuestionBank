using System;
using System.Collections.Generic;

#nullable disable

namespace CRUD_AspNetCore5.Models
{
    public partial class Question
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
