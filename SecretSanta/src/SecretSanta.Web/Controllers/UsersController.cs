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
                using (HttpClient httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    SecretSantaClient secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    ViewBag.Users = await secretSantaClient.GetAllUsersAsync().ConfigureAwait(false);
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

            if (string.IsNullOrWhiteSpace(viewModel.LastName))
            {
                ModelState.AddModelError("LastName", "Last Name field is required.");
            }

            if (ModelState.IsValid)
            {
                using (HttpClient httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    try
                    {
                        SecretSantaClient secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                        await secretSantaClient.CreateUserAsync(viewModel).ConfigureAwait(false);

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
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel viewModel)
        {
            IActionResult result = View();

            if (string.IsNullOrWhiteSpace(viewModel.FirstName))
            {
                ModelState.AddModelError("FirstName", "First Name field is required.");
            }

            if (string.IsNullOrWhiteSpace(viewModel.LastName))
            {
                ModelState.AddModelError("LastName", "Last Name field is required.");
            }

            if (ModelState.IsValid)
            {
                using (HttpClient httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    try
                    {
                        SecretSantaClient secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                        await secretSantaClient.UpdateUserAsync(viewModel.Id, Mapper.Map<UserInputViewModel>(viewModel)).ConfigureAwait(false);

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

            using (HttpClient httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    SecretSantaClient secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    await secretSantaClient.DeleteUserAsync(id).ConfigureAwait(false);

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
