using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebInterfaceCRUD.Models;

namespace WebInterfaceCRUD.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public async Task<ActionResult> Index()
        {
            //Hosted web API REST Service base url
            string Baseurl = "http://localhost:64614/";
            List<Product> ProdInfo = new List<Product>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                 HttpResponseMessage Res = await client.GetAsync("api/Product");
                //Checking the response is successful or not which is sent using  HttpClient
            if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Product list
                     ProdInfo = JsonConvert.DeserializeObject<List<Product>>(PrResponse);
                }
                //returning the Product list to view
                return View(ProdInfo);
            }
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product prod)
        {
            try
            {
                // TODO: Add update logic here
                string Baseurl = "http://localhost:64614/";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync(string.Format("api/Product", prod), prod);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Edit");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View();
            }
        }

        // GET: Product/Details/5
        public async Task<ActionResult> Details(int id)
        {
            string Baseurl = "http://localhost:64614/";
            Product prod = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                HttpResponseMessage Res = await client.GetAsync(string.Format("api/Product/{0}", id));

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Product list  
                    prod = JsonConvert.DeserializeObject<Product>(PrResponse);

                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            return View(prod);
        }

        // GET: Product/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Product prod = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:64614/");

                HttpResponseMessage Res = await client.GetAsync(string.Format("api/Product/{0}", id));

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Product list  
                    prod = JsonConvert.DeserializeObject<Product>(PrResponse);

                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            return View(prod);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Product prod)
        {
            try
            {
                // TODO: Add update logic here
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:64614/");
                    HttpResponseMessage Res = await client.GetAsync(string.Format("api/Product/{0}", id));
                    Product serverProd = null;
                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var PrResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Product list  
                        serverProd = JsonConvert.DeserializeObject<Product>(PrResponse);
                    }
                    prod.ProductCategory = serverProd.ProductCategory;
                    //HTTP POST
                    var postTask = client.PutAsJsonAsync<Product>(string.Format("api/Product/{0}", prod.Id), prod);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View();
            }
        }

        // GET: Product/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            string Baseurl = "http://localhost:64614/";
            Product prod = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                HttpResponseMessage Res = await client.GetAsync(string.Format("api/Product/{0}", id));

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Product list  
                    prod = JsonConvert.DeserializeObject<Product>(PrResponse);

                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            return View(prod);

        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Product prod)
        {

            try
            {// TODO: Add update logic here
                string Baseurl = "http://localhost:64614/";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    var deleteTask = client.DeleteAsync(string.Format("api/Product/{0}", id));
                    deleteTask.Wait();
                    //Checking the response is successful or not which is sent using HttpClient  
                    var Res = deleteTask.Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }


                }
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View();
            }

        }
    }
}