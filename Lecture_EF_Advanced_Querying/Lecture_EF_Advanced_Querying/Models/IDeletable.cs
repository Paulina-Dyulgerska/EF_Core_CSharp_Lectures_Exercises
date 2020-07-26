using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Lecture_EF_Advanced_Querying.Models
{
    public interface IDeletable 
    {
        public bool IsDeleted { get; set; }

    }
}