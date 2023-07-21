using Group4_GlassesShop.Models.AddresManager;
using Group4_GlassesShop.Models.Authentication;
using Group4_GlassesShop.Models.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;

namespace Group4_GlassesShop.Controllers.UserProfileManager
{
    public class AddressController : Controller
    {
        private readonly GlassesShopContext _db = new GlassesShopContext();
        public AddressController(GlassesShopContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Displays the address view.
        /// </summary>
        /// <returns>The address view.</returns>
        [Authentication]
        public IActionResult AddressView()
        {
            if (TempData.ContainsKey("SuccessUpdateAddress"))
            {
                ViewBag.SuccessUpdateAddress = TempData["SuccessUpdateAddress"];
            }

            string userJson = HttpContext.Session.GetString("User");

            // if Session of User is null -> return Home page
            if (userJson == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            // view User's address delivery information
            else
            {
                var accountUser = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Account>(userJson);
                var customer = _db.Customers.FirstOrDefault(c => c.AccountId == accountUser.AccountId);

                ViewBag.CustomerName = customer.Name;
                ViewBag.CustomerAvatar = customer.AvatarUrl;
                ViewBag.AccountEmail = accountUser.Email;

                // Get a list of addresses for the customer
                var addresses = _db.Addresses.Where(a => a.CustomerId == customer.CustomerId).ToList();

                if (addresses.Count > 0)
                {
                    var viewModelList = new List<AddressManager>();

                    foreach (var address in addresses)
                    {
                        var provinceCus = _db.Provinces.FirstOrDefault(p => p.PCode == address.ProvincesCode);
                        var districtCus = _db.Districts.FirstOrDefault(d => d.DCode == address.DistrictsCode);
                        var wardCus = _db.Wards.FirstOrDefault(w => w.WCode == address.WardsCode);

                        // Create an AddressManager object containing the information
                        var viewModel = new AddressManager
                        {
                            AddressID = address.AddressId,
                            AddrDetail = address.AddDetails,
                            Province = provinceCus,
                            District = districtCus,
                            Ward = wardCus,
                            Customer = customer,
                            Account = accountUser
                        };

                        viewModelList.Add(viewModel);
                    }

                    return View(viewModelList);
                }
                else
                {
                    // Create an empty AddressManager object
                    var viewModel = new AddressManager
                    {
                        AddrDetail = null,
                        Province = null,
                        District = null,
                        Ward = null,
                        Customer = customer,
                        Account = accountUser
                    };

                    // Wrap the object in a list and pass it to the view
                    var viewModelList = new List<AddressManager> { viewModel };

                    return View(viewModelList);
                }
            }
        }

        /// <summary>
        /// GET
        /// Displays the update address form.
        /// </summary>
        /// <param name="addressId">The address ID.</param>
        /// <returns>The update address view.</returns>
        [Authentication]
        [HttpGet]
        public IActionResult UpdateAddress(int addressId)
        {
            string userJson = HttpContext.Session.GetString("User");

            if (userJson == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                var accountUser = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Account>(userJson);  // Deserialize the user information from the JSON string
                var customer = _db.Customers.FirstOrDefault(c => c.AccountId == accountUser.AccountId);
                var address = _db.Addresses.FirstOrDefault(a => a.CustomerId == customer.CustomerId && a.AddressId == addressId);
                if (address != null)
                {
                    // Retrieve all provinces, districts, and wards
                    var getAllProvinces = _db.Provinces.ToList();
                    var getAllDistricts = _db.Districts.ToList();
                    var getAllWards = _db.Wards.ToList();

                    // Create an AddressManager object containing the address information and other necessary data
                    var viewModel = new AddressManager
                    {
                        AddrDetail = address.AddDetails,
                        Customer = customer,
                        Account = accountUser,
                        SelectedProvinceId = address.ProvincesCode,
                        SelectedDistrictId = address.DistrictsCode,
                        SelectedWardId = address.WardsCode,
                        Provinces = getAllProvinces,
                        Districts = getAllDistricts,
                        Wards = getAllWards
                    };

                    return View(viewModel);
                }
                else
                {
                    var getAllProvinces = _db.Provinces.ToList();
                    var getAllDistricts = _db.Districts.ToList();
                    var getAllWards = _db.Wards.ToList();

                    // Create an AddressManager object with default values for a new address
                    var viewModel = new AddressManager
                    {
                        Customer = customer,
                        Account = accountUser,
                        AddrDetail = null,
                        SelectedProvinceId = null, 
                        SelectedDistrictId = null, 
                        SelectedWardId = null, 
                        Provinces = getAllProvinces,
                        Districts = getAllDistricts,
                        Wards = getAllWards
                    };

                    return View(viewModel);
                }
            }

        }

        /// <summary>
        /// POST
        /// Updates the address.
        /// </summary>
        /// <param name="viewModel">The AddressManager object containing the updated address information.</param>
        /// <param name="addressId">The address ID.</param>
        /// <returns>The action result.</returns>
        [Authentication]
        [HttpPost]
        public IActionResult UpdateAddress(AddressManager viewModel, int addressId)
        {
            string userJson = HttpContext.Session.GetString("User");
            var accountUser = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Account>(userJson);
            var customer = _db.Customers.FirstOrDefault(c => c.AccountId == accountUser.AccountId);
            var address = _db.Addresses.FirstOrDefault(a => a.CustomerId == customer.CustomerId && a.AddressId == addressId);


            // Check if any required fields are empty or null
            if (string.IsNullOrWhiteSpace(viewModel.AddrDetail) || viewModel.SelectedProvinceId == null || viewModel.SelectedDistrictId == null || viewModel.SelectedWardId == null)
            {
                // If any required fields are empty or null, add corresponding model errors and return the view with the updated AddressManager object
                if (string.IsNullOrWhiteSpace(viewModel.AddrDetail))
                {
                    ModelState.AddModelError("viewModel.AddrDetail", "Specific Address must be required.");
                }
                if (viewModel.SelectedProvinceId == null)
                {
                    ModelState.AddModelError("viewModel.SelectedProvinceId", "Provinces must be selected.");
                }
                if (viewModel.SelectedDistrictId == null)
                {
                    ModelState.AddModelError("viewModel.SelectedDistrictId", "Districts must be selected.");
                }
                if (viewModel.SelectedWardId == null)
                {
                    ModelState.AddModelError("viewModel.SelectedWardId", "Wards must be selected.");
                }

                var getAllProvinces = _db.Provinces.ToList();
                var getAllDistricts = _db.Districts.ToList();
                var getAllWards = _db.Wards.ToList();

                viewModel.Provinces = getAllProvinces;
                viewModel.Districts = getAllDistricts;
                viewModel.Wards = getAllWards;
                viewModel.Customer = customer;
                viewModel.Account = accountUser;


                return View("UpdateAddress", viewModel);
            }
            else
            {
                if (address == null)
                {
                    // Create a new Address object if address is null
                    address = new Group4_GlassesShop.Models.DataModel.Address(viewModel.SelectedWardId, viewModel.SelectedProvinceId, viewModel.SelectedDistrictId, customer.CustomerId, viewModel.AddrDetail);
                    _db.Addresses.Add(address);
                    _db.SaveChanges();
                }
                else
                {
                    // Update the address information from the viewModel
                    address.ProvincesCode = viewModel.SelectedProvinceId;
                    address.DistrictsCode = viewModel.SelectedDistrictId;
                    address.WardsCode = viewModel.SelectedWardId;
                    address.AddDetails = viewModel.AddrDetail;
                    _db.Addresses.Update(address);
                    _db.SaveChanges();
                }
               
                TempData["SuccessUpdateAddress"] = "Update Successfully!";
                return RedirectToAction("AddressView", "Address");
            }

        }

        /// <summary>
        /// GET
        /// Displays the create address form.
        /// </summary>
        /// <returns>The create address view.</returns>
        [Authentication]
        [HttpGet]
        public IActionResult CreateAddress()
        {
            string userJson = HttpContext.Session.GetString("User");
            var accountUser = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Account>(userJson);
            var customer = _db.Customers.FirstOrDefault(c => c.AccountId == accountUser.AccountId);
            var getAllProvinces = _db.Provinces.ToList();
            var getAllDistricts = _db.Districts.ToList();
            var getAllWards = _db.Wards.ToList();

            var viewModel = new AddressManager
            {
                Customer = customer,
                Account = accountUser,
                Provinces = getAllProvinces,
                Districts = getAllDistricts,
                Wards = getAllWards
            };

            return View(viewModel);
        }

        /// <summary>
        /// POST
        /// Creates a new address.
        /// </summary>
        /// <param name="viewModel">The AddressManager object containing the new address information.</param>
        /// <returns>The action result.</returns>
        [Authentication]
        [HttpPost]
        public IActionResult CreateAddress(AddressManager viewModel)
        {
            string userJson = HttpContext.Session.GetString("User");
            var accountUser = JsonConvert.DeserializeObject<Group4_GlassesShop.Models.DataModel.Account>(userJson);
            var customer = _db.Customers.FirstOrDefault(c => c.AccountId == accountUser.AccountId);

            // Check if any required fields are empty or null
            if (string.IsNullOrWhiteSpace(viewModel.AddrDetail) || viewModel.SelectedProvinceId == null || viewModel.SelectedDistrictId == null || viewModel.SelectedWardId == null)
            {
                // If any required fields are empty or null, add corresponding model errors and return the view with the updated AddressManager object
                if (string.IsNullOrWhiteSpace(viewModel.AddrDetail))
                {
                    ModelState.AddModelError("viewModel.AddrDetail", "Specific Address must be required.");
                }
                if (viewModel.SelectedProvinceId == null)
                {
                    ModelState.AddModelError("viewModel.SelectedProvinceId", "Provinces must be selected.");
                }
                if (viewModel.SelectedDistrictId == null)
                {
                    ModelState.AddModelError("viewModel.SelectedDistrictId", "Districts must be selected.");
                }
                if (viewModel.SelectedWardId == null)
                {
                    ModelState.AddModelError("viewModel.SelectedWardId", "Wards must be selected.");
                }

                var getAllProvinces = _db.Provinces.ToList();
                var getAllDistricts = _db.Districts.ToList();
                var getAllWards = _db.Wards.ToList();

                viewModel.Provinces = getAllProvinces;
                viewModel.Districts = getAllDistricts;
                viewModel.Wards = getAllWards;
                viewModel.Customer = customer;
                viewModel.Account = accountUser;

                return View(viewModel);
            }
            else
            {
                // Create a new Address object with the provided informatio
                var address = new Group4_GlassesShop.Models.DataModel.Address(viewModel.SelectedWardId, viewModel.SelectedProvinceId, viewModel.SelectedDistrictId, customer.CustomerId, viewModel.AddrDetail);
                _db.Addresses.Add(address);
                _db.SaveChanges();

                TempData["SuccessCreateAddress"] = "Create Successfully!";
                return RedirectToAction("AddressView", "Address");
            }
        }

        /// <summary>
        /// Retrieves the list of districts based on the selected province.
        /// </summary>
        /// <param name="provinceId">The ID of the selected province.</param>
        /// <returns>The partial view containing the list of districts.</returns>
        [HttpGet]
        public IActionResult LoadDistricts(string provinceId)
        {
            // Retrieve the list of districts based on the selected province
            var districts = _db.Districts.Where(d => d.ProvinceCode == provinceId).ToList();

            // Return the partial view "_Districts" with the list of districts
            return PartialView("_Districts", districts);
        }

        /// <summary>
        /// Retrieves the list of wards based on the selected district.
        /// </summary>
        /// <param name="districtId">The ID of the selected district.</param>
        /// <returns>The partial view containing the list of wards.</returns>
        [HttpGet]
        public IActionResult LoadWards(string districtId)
        {
            // Retrieve the list of wards based on the selected district
            var wards = _db.Wards.Where(w => w.DistrictCode == districtId).ToList();

            // Return the partial view "_Wards" with the list of wards
            return PartialView("_Wards", wards);
        }
    }
}
