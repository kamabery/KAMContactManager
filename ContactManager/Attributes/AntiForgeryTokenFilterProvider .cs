using System.Collections.Generic;
using System.Web.Mvc;

namespace ContactManager.Attributes
{
    public class AntiForgeryTokenFilterProvider : IFilterProvider
    {
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if (controllerContext.HttpContext.Request.HttpMethod.ToUpper() == "POST")
            {
                yield return new Filter(new ValidateAntiForgeryTokenAttribute(), FilterScope.Global, null);
            }
        }
    }
}