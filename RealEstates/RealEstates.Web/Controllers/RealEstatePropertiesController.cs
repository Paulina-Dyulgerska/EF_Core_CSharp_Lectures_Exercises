﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstates.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstates.Web.Controllers
{
    public class RealEstatePropertiesController : Controller
    {
        private IRealEstatePropertiesService realEstatePropertiesService;

        public RealEstatePropertiesController(IRealEstatePropertiesService realEstatePropertiesService)
        {
            this.realEstatePropertiesService = realEstatePropertiesService;
        }

        public IActionResult Search()
        {
            var realEstateProperties = this.realEstatePropertiesService.SearchByPrice(0, 100000);
            return this.View(realEstateProperties);
        }

        //[Authorize()] //usera trqbwa da se e lognal,za da vijda tazi stranica.
        //[Authorize(Roles = "Moderator")] //usera trqbwa da se e lognal kato Moderator,za da vijda tazi stranica.
        public IActionResult DoSearch(int minPrice, int maxPrice)
        {
            //if (!this.User.IsInRole("Admin"))
            //{
            //    return this.BadRequest(); //samo Admin user moje da vika tazi stranica!!!
            //}
            var realEstateProperties = this.realEstatePropertiesService.SearchByPrice(minPrice, maxPrice);
            return this.View(realEstateProperties);
        }
    }
}
