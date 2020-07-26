namespace FastFood.Core.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using FastFood.Models;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Positions;

    public class PositionsController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public PositionsController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //ako e bez attribut, to znachi che e po default HttpGet tozi method. [HttpGet]
        //tova shte stane Get zaqwka ot neshto kato takyv route: /{controllerName}/{actionName} i v konkretnata
        //zaqwka towa shte e neshto kato -> myApp.com/Positions/Create
        //zatowa e vajno da si imenuvam prvilno methodite i ot tam se prawi routing-a.
        //na praktika kato izbrah Positions ot home page, otidoh tuk: https://localhost:44331/Positions/Create
        public IActionResult Create()
        {
            return this.View();
        }

        //pri toq post se pravi Positions/Create -> kogato e clicknat Submmit button na formata ot page Positions.
        [HttpPost]
        public IActionResult Create(CreatePositionInputModel model)
        {
            if (!ModelState.IsValid)
            //proverqwa dali validiraneto na dannite ot input formata e stanalo i e validno
            //v input formata trqbwa da sa mi syshtite imenata na poletata, kakvito shte otidat v Modela v DB-a, kakto i da
            //imam syshtite validacii, kato tezi v DB-a!!!!
            //tuk vajat validaciite i constraints, koito az sym zadala za konkretnite propertyta na konkretniq model
            //togawa shte hvyrli error:
            {
                return this.RedirectToAction("Error", "Home");
            }

            var position = this.mapper.Map<Position>(model); //mappvam model-a, kojto mi e doshyl ot input formata, kym
            //DB modela (classa v DB-a). Configuriraneto na tozi mapping e opisano v papka MappingConfiguration ->
            //FastFoodProfile.cs. tam imam dvata vida mappvane:
            ////this.CreateMap<CreatePositionInputModel, Position>()
            ////    .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));
            ////this.CreateMap<Position, PositionsAllViewModel>()
            ////    .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

            this.context.Positions.Add(position); //addvam novata position

            this.context.SaveChanges(); //save-vam promenite v DB-a, tova mi e Post zaqwkata na praktika!!!!

            return this.RedirectToAction("All", "Positions"); //redirectvam kym Method All() ot class PositionsController!!!!!
            //All() e HttpGet action.
        }

        public IActionResult All()
        {
            List<PositionsAllViewModel> positions = this.context.Positions
                                                        .ProjectTo<PositionsAllViewModel>(mapper.ConfigurationProvider)
                                                        .ToList();

            //ako otida vyv Views->Positions->All.cshtml shte vidq kakwo shte se generira ot towa:
            return this.View(positions);
        }
    }
}
