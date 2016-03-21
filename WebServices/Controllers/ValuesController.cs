using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServices.Models;

namespace WebServices.Controllers
{
    public class ValuesController : ApiController
    {
        public IEnumerable<ElementModel> Get()
        {
            return new ElementModel[] { new ElementModel() { Id = 1, Altered = DateTime.Now, Abstract = "Element 1" },
                                        new ElementModel() { Id = 2, Altered = DateTime.Now, Abstract = "Element 2" } };
        }


        
        public IEnumerable<ElementModel> Get(long id)
        {
            int lastDigits = (int)id % 100;
            return new ElementModel[] { new ElementModel() { Id = 1, Altered = DateTime.Now, Abstract = "Element 1" },
                                        new ElementModel() { Id = lastDigits, Altered = DateTime.Now, Abstract = "Element " + (id) } };
        }
    }
}
