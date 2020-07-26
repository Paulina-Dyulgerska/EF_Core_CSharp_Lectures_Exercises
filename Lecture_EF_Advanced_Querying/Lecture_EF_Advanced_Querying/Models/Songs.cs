using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lecture_EF_Advanced_Querying.Models
{
    public partial class Songs
    {
        public Songs()
        {
            SongArtists = new HashSet<SongArtists>();
            SongMetadata = new HashSet<SongMetadata>();
        }

        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        
        [ConcurrencyCheck]
        public string Name { get; set; }
        public int? SourceId { get; set; }
        public string SourceItemId { get; set; }
        public string SearchTerms { get; set; }

        public virtual Sources Source { get; set; }
        public virtual ICollection<SongArtists> SongArtists { get; set; }
        public virtual ICollection<SongMetadata> SongMetadata { get; set; }
    }
}
