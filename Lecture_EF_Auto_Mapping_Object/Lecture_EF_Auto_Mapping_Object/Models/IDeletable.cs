using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Lecture_EF_Auto_Mapping_Object.Models
{
    public interface IDeletable 
    {
        public bool IsDeleted { get; set; }

    }
}