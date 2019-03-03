using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.ApiModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Web.Controllers
{
    public class GroupsController : Controller
    {
        private IHttpClientFactory ClientFactory { get; }
        private IMapper Mapper { get; }
        public GroupsController(IHttpClientFactory clientFactory, IMapper mapper)
        {
            ClientFactory = clientFactory;
            Mapper = mapper;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            using (HttpClient httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    SecretSantaClient secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    ViewBag.Groups = await secretSantaClient.GetGroupsAsync().ConfigureAwait(false);
                }
                catch (SwaggerException se)
                {
                    ModelState.AddModelError("", se.Message);
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(GroupInputViewModel viewModel)
        {
            IActionResult result = View();

            if (string.IsNullOrWhiteSpace(viewModel.Name))
            {
                ModelState.AddModelError("Name", "Group Name field is required.");
            }

            if (ModelState.IsValid)
            {
                using (HttpClient httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    try
                    {
                        SecretSantaClient secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                        await secretSantaClient.CreateGroupAsync(viewModel).ConfigureAwait(false);

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
        public async Task<IActionResult> Edit(GroupViewModel viewModel)
        {
            IActionResult result = View();

            if (string.IsNullOrWhiteSpace(viewModel.Name))
            {
                ModelState.AddModelError("Name", "Group Name field is required.");
            }

            if (ModelState.IsValid)
            {
                using (HttpClient httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    try
                    {
                        SecretSantaClient secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                        await secretSantaClient.UpdateGroupAsync(viewModel.Id, Mapper.Map<GroupInputViewModel>(viewModel)).ConfigureAwait(false);

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

        public async Task<IActionResult> Delete(int id)
        {
            IActionResult result = View();

            using (HttpClient httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    SecretSantaClient secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    await secretSantaClient.DeleteGroupAsync(id).ConfigureAwait(false);

                    result = RedirectToAction(nameof(Index));
                }
                catch (SwaggerException se)
                {
                    ModelState.AddModelError("", se.Message);
                }

                return result;
            }
        }
    }
}
