using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecretSanta.Web.ApiModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Web.Controllers
{
    public class UsersController : Controller
    {
        private IHttpClientFactory ClientFactory { get; }
        private IMapper Mapper { get; }
        public UsersController(IHttpClientFactory clientFactory, IMapper mapper)
        {
            ClientFactory = clientFactory;
            Mapper = mapper;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            try
            {
                using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    ViewBag.Users = await secretSantaClient.GetAllUsersAsync();
                }
            }
            catch (SwaggerException se)
            {
                ModelState.AddModelError("", se.Message);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserInputViewModel viewModel)
        {
            IActionResult result = View();

            if (ModelState.IsValid)
            {
                using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    try
                    {
                        var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                        await secretSantaClient.CreateUserAsync(viewModel);

                        result = RedirectToAction(nameof(Index));
                    }
                    catch (SwaggerException se)
                    {
                        ModelState.AddModelError("", se.Message);
                    }
                }
            }

            return result;
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            //UserViewModel fetchedUser = null;

            //using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            //{
            //    try
            //    {
            //        var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
            //        fetchedUser = await secretSantaClient.GetUserAsync(id);
            //    }
            //    catch (SwaggerException se)
            //    {
            //        ModelState.AddModelError("", se.Message);
            //    }

            //    return View(fetchedUser);
            //}

            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel viewModel)
        {
            IActionResult result = View();

            if (ModelState.IsValid)
            {
                using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    try
                    {
                        var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                        await secretSantaClient.UpdateUserAsync(viewModel.Id, Mapper.Map<UserInputViewModel>(viewModel));

                        result = RedirectToAction(nameof(Index));
                    }
                    catch (SwaggerException se)
                    {
                        ModelState.AddModelError("", se.Message);
                    }
                }
                
            }
            //return View()
            return result;
        }

        //[HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            IActionResult result = View();

            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    await secretSantaClient.DeleteUserAsync(id);

                    result = RedirectToAction(nameof(Index));
                }
                catch (SwaggerException se)
                {
                    ModelState.AddModelError("", se.Message);
                }
            }
            
            return result;
        }
    }
}
