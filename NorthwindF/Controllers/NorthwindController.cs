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
        /// Alap term�kek lek�rdez�se egy t�bl�zatba
        /// </summary>
        /// <returns>Term�kek list�ja</returns>
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            _logger.LogInformation("Started product fetching", DateTime.UtcNow.ToLongTimeString());
            ODataWebExperimental.Northwind.Model.NorthwindEntities nw =
                        new ODataWebExperimental.Northwind.Model.NorthwindEntities(new Uri("http://services.odata.org/V4/Northwind/Northwind.svc/"));

            if (nw != null)
            _logger.LogInformation("OData was accessed", DateTime.UtcNow.ToLongTimeString());

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
        }

        /// <summary>
        /// Egy input mez�ben megadott sz�veg alapj�n sz�r a db-ben a term�k nev�t haszn�lva
        /// Lehetne k�l�n a t�bl�zatba is megoldani kliens oldalon, de mivel React volt a feladat
        /// ez�rt ott ink�bb a db-be keres vissza jelenleg.
        /// </summary>
        /// <param name="name">A string ami alapj�n a db-ben sz�r�nk</param>
        /// <returns>Json az eredm�nnyel</returns>
        [HttpPost]
        [Route("getFilteredProducts")]
        public JsonResult getFilteredProducts([FromForm] string name)
        {
            _logger.LogInformation("Started product filtering", DateTime.UtcNow.ToLongTimeString());
            _logger.LogInformation("Filtering string: {0}", name, DateTime.UtcNow.ToLongTimeString());

            ODataWebExperimental.Northwind.Model.NorthwindEntities nw =
                        new ODataWebExperimental.Northwind.Model.NorthwindEntities(new Uri("http://services.odata.org/V4/Northwind/Northwind.svc/"));

            if (nw != null)
                _logger.LogInformation("OData was accessed", DateTime.UtcNow.ToLongTimeString());

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


            _logger.LogInformation("Number of results: {0}", products.Count().ToString(), DateTime.UtcNow.ToLongTimeString());

            return new JsonResult(products);

        }

        /// <summary>
        /// Az �rt�kes�t�k term�k summ�j�t sz�molja ki
        /// </summary>
        /// <returns>Json az eredm�nnyel</returns>
        [HttpPost]
        [Route("getSumSupplier")]
        public JsonResult getSumSupplier()
        {
            _logger.LogInformation("Started supplier aggregation", DateTime.UtcNow.ToLongTimeString());
            ODataWebExperimental.Northwind.Model.NorthwindEntities nw =
                        new ODataWebExperimental.Northwind.Model.NorthwindEntities(new Uri("http://services.odata.org/V4/Northwind/Northwind.svc/"));
            if (nw != null)
                _logger.LogInformation("OData was accessed", DateTime.UtcNow.ToLongTimeString());

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