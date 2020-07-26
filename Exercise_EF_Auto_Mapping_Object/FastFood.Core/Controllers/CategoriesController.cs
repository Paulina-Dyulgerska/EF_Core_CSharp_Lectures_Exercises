namespace FastFood.Core.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using FastFood.Models;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Categories;

    public class CategoriesController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public CategoriesController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryInputModel model)
        {
            if (!ModelState.IsValid)  
                //ako modela ima nevalidni danni spored moite constraints v class CreateCategoryInputModel
                //togawa shte hvyrli error:
            {
                return this.RedirectToAction("Error", "Home");
            }

            var category = this.mapper.Map<Category>(model);

            this.context.Categories.Add(category); 

            this.context.SaveChanges(); //save-vam promenite v DB-a, tova mi e Post zaqwkata na praktika!!!!

            //return this.RedirectToAction("All", "Categories");
            //ne e nujno da pisha che sym v Categories, zashtoto sym
            //v contexta na obekt ot type CategoriesController i e qsno che sym v Categories. Zatowa moje i taka:
            return this.RedirectToAction("All");
        }

        public IActionResult All()
        {
            List<CategoryAllViewModel> categories = this.context.Categories
                                            .ProjectTo<CategoryAllViewModel>(mapper.ConfigurationProvider)
                                            .ToList();

            //ako otida vyv Views->Categories->All.cshtml shte vidq kakwo shte se generira ot towa:
            return this.View(categories);
        }
    }
}
