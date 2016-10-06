﻿using Sdl.Web.Common.Logging;
using Sdl.Web.Common.Models;
using SDL.ECommerce.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SDL.ECommerce.DXA.Controllers
{
    /// <summary>
    /// E-Commerce Search Page Controller
    /// </summary>
    public class SearchPageController : AbstractECommercePageController
    {
        
        public ActionResult SearchCategoryPage(string searchPhrase, string categoryUrl)
        {
            Log.Info("Entering search page controller with search phrase: " + searchPhrase + ", category: "  + categoryUrl);

            // Get facets
            //
            var facets = GetFacetParametersFromRequest();

            PageModel templatePage = null;

            // Build query
            //
            Query query;
            if ( categoryUrl != null )
            {
                var category = ECommerceContext.Client.CategoryService.GetCategoryByPath(categoryUrl);
                if (category != null)
                {
                    query = new Query { SearchPhrase = searchPhrase, Category = category, Facets = facets };
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                query = new Query { SearchPhrase = searchPhrase, Facets = facets };
            } 
            
            templatePage = this.ResolveTemplatePage(this.GetSearchPath());
            // templatePage.Title = ?? TODO: What title to use for search results?
            SetupViewData(templatePage);

            var searchResult = ECommerceContext.Client.QueryService.Query(query);

            if (searchResult.RedirectLocation != null)
            {
                return Redirect(ECommerceContext.LinkResolver.GetLocationLink(searchResult.RedirectLocation));
            }

            ECommerceContext.Set(ECommerceContext.QUERY_RESULT, searchResult);
            ECommerceContext.Set(ECommerceContext.URL_PREFIX, ECommerceContext.LocalizePath("/search/") + searchPhrase);
            ECommerceContext.Set(ECommerceContext.FACETS, facets);

            return View(templatePage);
        }

        /// <summary>
        /// Get search path to find an appropriate CMS template page for current search result
        /// </summary>
        /// <returns></returns>
        protected IList<string> GetSearchPath()
        {
            // TODO: Add more possibilities to override the look&feel for search results based on category and search phrases etc

            var searchPath = new List<string>();
            searchPath.Add(ECommerceContext.LocalizePath("/search-results"));
            return searchPath;
        }
    }
}