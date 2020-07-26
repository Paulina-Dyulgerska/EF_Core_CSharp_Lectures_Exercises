using System.Collections.Generic;
using System.Linq;

namespace Lecture_EF_Auto_Mapping_Object.Models
{
    interface IArtistsService
    {
        IEnumerable<ArtistWithCountViewModel> GetAllWithCount();
    }

    class ArtistsServise : IArtistsService //tova e Service!!!!
    {
        private readonly MusicXContext context;

        public ArtistsServise(MusicXContext context)
        {
            this.context = context;
        }
        public IEnumerable<ArtistWithCountViewModel> GetAllWithCount()
        {
            return context.Artists.Select(x => new ArtistWithCountViewModel
            {
                Name = x.Name,
                SongArtistsCount = x.SongArtists.Count(),
            }).ToList();
        }
    }
}
