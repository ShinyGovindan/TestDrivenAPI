﻿using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI;
using TestDrivenAPI.Models;

namespace TestDrivenAPI.Controllers
{
    public class ProductBusinessRulesController : ApiController
    {
        public ProductBusinessRulesController()
        { }
        List<Product> products = new List<Product>();

        public ProductBusinessRulesController(List<Product> products)
        {
            this.products = products;
        }

        /// <summary>
        /// Return all products
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

        /// <summary>
        /// Get non physical products
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GetAllNonPhysicalProducts()
        {
            return products.Where(x => !x.IsPhysicalProduct).ToList();
        }
        /// <summary>
        /// Return Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult GetProduct(int id)
        {
            var product = products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await Task.FromResult(GetAllProducts());
        }

        public async Task<IHttpActionResult> GetProductAsync(int id)
        {
            return await Task.FromResult(GetProduct(id));
        }

        public async Task<IEnumerable<Product>> GetAllNonPhysicalProductsAsync()
        {
            return await Task.FromResult(GetAllNonPhysicalProducts());
        }

        public Task<HttpResponseMessage> SetProductBusinessRule(ProductType type)
        {
            IEnumerable<Product> lstProduct = GetAllProducts();
            Product physicalProduct = lstProduct.Where(x => x.IsPhysicalProduct && x.Id==type.ProductId).FirstOrDefault();
            Product nonPhysicalProduct = lstProduct.Where(x => !x.IsPhysicalProduct && x.Id == type.ProductId).FirstOrDefault();
            switch (type.Type)
            {
                case "Physical":
                    return GenerateSlip(physicalProduct, type);
                case "Book":
                    // TBD
                    break;
                case "MemberShip":
                    return ActivateMembership(nonPhysicalProduct, type);

                default:
                    // TBD
                    break;
            }
            return GenerateSlip(physicalProduct, type);
        }

        private async Task<HttpResponseMessage> GenerateSlip(Product physicalProduct, ProductType type)
        {
            UpdateProductBusinessRule(type);
            const string StoragePath = @"C:\Project\TestDrivenAPI";
               if (Request.Content.IsMimeMultipartContent())
                {
                    var streamProvider = new MultipartFormDataStreamProvider(Path.Combine(StoragePath, "Upload"));
                    await Request.Content.ReadAsMultipartAsync(streamProvider);
                    foreach (MultipartFileData fileData in streamProvider.FileData)
                    {
                        if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                        {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
                        }
                        string fileName = fileData.Headers.ContentDisposition.FileName;
                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                        {
                            fileName = fileName.Trim('"');
                        }
                        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                        {
                            fileName = Path.GetFileName(fileName);
                        }
                        File.Move(fileData.LocalFileName, Path.Combine(StoragePath, fileName));
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
                }
        }

        public async Task<HttpResponseMessage> ActivateMembership(Product nonPhysicalProduct, ProductType type)
        {
            return await Task.FromResult(UpdateMemberShip(type));
        }

        public HttpResponseMessage UpdateMemberShip(ProductType type)
        {
            if (Request.Content.IsMimeMultipartContent())
            {
                //var user = new User();//TBD
                var ProductBusinessRule = new ProductBusinessRule();
                UpdateProductBusinessRule(type);//Corresponding Business rule
                //AddMembership();
                //return Ok(ProductBusinessRule);//This should return the User object like below  
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        public int UpdateProductBusinessRule(ProductType type)
        {
            int result = 0;
            ProductBusinessRule obProductBusinessRule = new ProductBusinessRule();
            obProductBusinessRule.BusinessRuleName = "Generate Slip";//This can be Global values  and can set globaly.So it can change based on the product Type
            obProductBusinessRule.isActive = true;
            obProductBusinessRule.ProductTypeId = type.ProductTypeId;
            //result = DBNull.ExecuteNonQuery(obProductBusinessRule)//TBD
            return result; 
        }
    }
}
