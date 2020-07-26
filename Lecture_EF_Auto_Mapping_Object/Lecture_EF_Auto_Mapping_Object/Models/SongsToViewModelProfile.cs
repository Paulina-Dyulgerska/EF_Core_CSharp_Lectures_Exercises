//using AutoMapper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Lecture_EF_Auto_Mapping_Object.Models
//{
//    public class SongsToViewModelProfile : Profile //classa Profile idwa ot AutoMapper.
//    {
//        public SongsToViewModelProfile()
//        {
//            this.CreateMap<Songs, SongViewModel>()
//                    .ForMember(
//                        x => x.Artists,
//                        opt => opt.MapFrom(
//                            s => string.Join(", ", s.SongArtists.Select(a => a.Artist.Name))))
//                    .ForMember(
//                        x => x.LastModified,
//                        opt => opt.MapFrom(
//                            s => s.ModifiedOn ?? s.CreatedOn))
//                    .ReverseMap();
//        }
//    }
//}
