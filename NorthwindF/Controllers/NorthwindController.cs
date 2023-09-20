using Microsoft.AspNetCore.Mvc;
using NorthwindF.Model;

namespace NorthwindF.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NorthwindController : ControllerBase
    {

        private readonly ILogger<NorthwindController> _logger;

        public NorthwindController(ILogger<NorthwindController> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Alap termékek lekérdezése egy táblázatba
        /// </summary>
        /// <returns>Termékek listája</returns>
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            ODataWebExperimental.Northwind.Model.NorthwindEntities nw =
                        new ODataWebExperimental.Northwind.Model.NorthwindEntities(new Uri("http://services.odata.org/V4/Northwind/Northwind.svc/"));
            var products = nw.Products.ToList().Select(x => new Product
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                QuantityPerUnit = x.QuantityPerUnit,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                UnitsOnOrder = x.UnitsOnOrder,
                ReorderLevel = x.ReorderLevel,
                Discontinued = x.Discontinued
            });

            return products;

            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
        }

        /// <summary>
        /// Egy input mezõben megadott szöveg alapján szûr a db-ben a termék nevét használva
        /// Lehetne külön a táblázatba is megoldani kliens oldalon, de mivel React volt a feladat
        /// ezért ott inkább a db-be keres vissza jelenleg.
        /// </summary>
        /// <param name="name">A string ami alapján a db-ben szûrünk</param>
        /// <returns>Json az eredménnyel</returns>
        [HttpPost]
        [Route("getFilteredProducts")]
        public JsonResult getFilteredProducts([FromForm] string name)
        {
            ODataWebExperimental.Northwind.Model.NorthwindEntities nw =
                        new ODataWebExperimental.Northwind.Model.NorthwindEntities(new Uri("http://services.odata.org/V4/Northwind/Northwind.svc/"));
            var products = nw.Products.Where(x => x.ProductName.Contains(name)).ToList().Select(x => new Product
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                QuantityPerUnit = x.QuantityPerUnit,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                UnitsOnOrder = x.UnitsOnOrder,
                ReorderLevel = x.ReorderLevel,
                Discontinued = x.Discontinued
            });

            return new JsonResult(products);

        }

        /// <summary>
        /// Az értékesítõk termék summáját számolja ki
        /// </summary>
        /// <returns>Json az eredménnyel</returns>
        [HttpPost]
        [Route("getSumSupplier")]
        public JsonResult getSumSupplier()
        {
            ODataWebExperimental.Northwind.Model.NorthwindEntities nw =
                        new ODataWebExperimental.Northwind.Model.NorthwindEntities(new Uri("http://services.odata.org/V4/Northwind/Northwind.svc/"));


            var Suppliers = nw.Suppliers.ToList();
            var Products = nw.Products.ToList();
            var Order_Details = nw.Order_Details.ToList();

            var res = Suppliers.Join(Products, x => x.SupplierID, y => y.SupplierID,
                (x, y) => new
                {
                    x.SupplierID,
                    y.ProductID,
                    y.ProductName,
                    x.CompanyName,
                    y.UnitPrice
                }).Join(Order_Details, x => x.ProductID, y => y.ProductID,
                (x, y) => new
                {
                    x.SupplierID,
                    x.ProductID,
                    y.OrderID,
                    x.ProductName,
                    x.CompanyName,
                    x.UnitPrice,
                    y.Quantity,
                    y.Discount
                }).GroupBy(x => new { x.SupplierID, x.ProductID, x.CompanyName, x.ProductName }).Select(x => new AggregateSuppliers
                {
                    SupplierID = x.Key.SupplierID,
                    ProductID = x.Key.ProductID,
                    CompanyName = x.Key.CompanyName,
                    ProductName = x.Key.ProductName,
                    SumPrice = (int)x.Sum(y => (int)y.Quantity * y.UnitPrice * (1 - (decimal)y.Discount))
                }).ToList();

            return new JsonResult(res);

        }
    }
}