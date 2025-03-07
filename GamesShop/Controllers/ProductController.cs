﻿using GamesShop.Core.Contracts;
using GamesShop.Infrastructure.Data.Entities;
using GamesShop.Models.Category;
using GamesShop.Models.Genre;
using GamesShop.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GamesShop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IGenreService _genreService;

        public ProductController(IProductService productService, ICategoryService categoryService, IGenreService genreService)
        {
            this._productService = productService;
            this._categoryService = categoryService;
            this._genreService = genreService;
        }
        // GET: ProductController
        [AllowAnonymous]
        public ActionResult Index(string searchStringCategoryName, string searchStringGenreName)
        {
            List<ProductIndexVM> products = _productService.GetProducts(searchStringCategoryName, searchStringGenreName)
                .Select(product => new ProductIndexVM
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    GenreId = product.GenreId,
                    GenreName = product.Genre?.GenreName,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category?.CategoryName, 
                    Producer = product.Producer, 
                    Picture = product.Picture,
                    Description = product.Description, 
                    Quantity = product.Quantity,
                    Price = product.Price,
                    Discount = product.Discount

                }).ToList();

            return this.View(products);
        }


        // GET: ProductController/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            Product item = _productService.GetProductById(id);
            if (item == null)
            {
                return NotFound();
            }

            ProductDetailsVM product = new ProductDetailsVM()
            {
                Id = item.Id,
                ProductName = item.ProductName,
                GenreId = item.GenreId,
                GenreName = item.Genre.GenreName,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.CategoryName,
                Producer = item.Producer,
                Picture = item.Picture,
                Description = item.Description,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount
            };

            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            var product = new ProductCreateVM();
            product.Genres = _genreService.GetGenres()
                .Select(x => new GenrePairVM()
                {
                    Id = x.Id,
                    Name = x.GenreName
                }).ToList();

            product.Categories = _categoryService.GetCategories()
                .Select(x => new CategoryPairVM()
                {
                    Id = x.Id,
                    Name = x.CategoryName
                }).ToList();

            return View(product);
        }


        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] ProductCreateVM product)
        {
            if (ModelState.IsValid)
            {
                var createdId = _productService.Create(product.ProductName, product.GenreId,
                    product.CategoryId,product.Producer, product.Picture,product.Description, product.Quantity, product.Price, product.Discount);

                if (createdId)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
        }


        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            ProductEditVM updatedProduct = new ProductEditVM()
            {
                Id = product.Id,
                ProductName = product.ProductName,
                GenreId = product.GenreId,
                // GenreName = product.Genre.GenreName,
                CategoryId = product.CategoryId,
                // CategoryName = product.Category.CategoryName,
                Producer = product.Producer,
                Picture = product.Picture,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price,
                Discount = product.Discount
            };

            updatedProduct.Genres = _genreService.GetGenres()
                .Select(g => new GenrePairVM()
                {
                    Id = g.Id,
                    Name = g.GenreName
                })
                .ToList();

            updatedProduct.Categories = _categoryService.GetCategories()
                .Select(c => new CategoryPairVM()
                {
                    Id = c.Id,
                    Name = c.CategoryName
                })
                .ToList();

            return View(updatedProduct);
        }


        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductEditVM product)
        {
            if (ModelState.IsValid)
            {
                var updated = _productService.Update(id, product.ProductName, product.GenreId,
                    product.CategoryId, product.Producer, product.Picture, product.Description,
                    product.Quantity, product.Price, product.Discount);

                if (updated)
                {
                    return this.RedirectToAction("Index");
                }
            }

            return View(product);
        }


        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            Product item = _productService.GetProductById(id);
            if (item == null)
            {
                return NotFound();
            }

            ProductDeleteVM product = new ProductDeleteVM()
            {
                Id = item.Id,
                ProductName = item.ProductName,
                GenreId = item.GenreId,
                GenreName = item.Genre.GenreName,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.CategoryName,
                Producer = item.Producer,
                Picture = item.Picture,
                Description = item.Description,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount
            };

            return View(product);
        }


        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var deleted = _productService.RemoveById(id);

            if (deleted)
            {
                return this.RedirectToAction("Success");
            }
            else
            {
                return View();
            }
        }
        public IActionResult Success()
        {
            return View();
        }
    }
}
