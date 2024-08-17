using Micro.Web.Models;
using Micro.Web.Service;
using Micro.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Micro.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var responce = await _productService.GetAllAsync();
            if(responce != null&&responce.Success)
            {
                var list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(responce.Result));
                return View(list);
            }
            return View(new List<ProductDTO>());
        }
		public async Task<IActionResult> Create()
		{
			return View(new ProductDTO());
		}
        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO productDTO)
        {
            var responce = await _productService.CreateAsync(productDTO);
            if(responce != null && responce.Success)
            {
                TempData["success"] = "Created succesfuly";
            }
            else
            {
                TempData["error"] = responce.Message;
            }
            return RedirectToAction("Index");
        }
		public async Task<IActionResult> Update(int id)
		{
			var responce = await _productService.GetAsync(id);
            if(responce!=null && responce.Success)
            {
				var item = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(responce.Result));
				return View(item);
			}
			TempData["error"] = responce.Message;
			return RedirectToAction("Index");
		}
		[HttpPost]
		public async Task<IActionResult> Update(ProductDTO productDTO)
		{
			var responce = await _productService.UpdateAsync(productDTO);
			if (responce != null && responce.Success)
			{
				TempData["success"] = "Updated succesfuly";
			}
			else
			{
				TempData["error"] = responce.Message;
			}
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Delete(int id)
		{
			var responceDTO = await _productService.DeleteAsync(id);
			TempData["success"] = "Deleted successfuly";
			return RedirectToAction("Index");
		}
	}
}
