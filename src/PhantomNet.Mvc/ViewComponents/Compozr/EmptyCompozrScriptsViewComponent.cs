﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PhantomNet.Mvc.ViewComponents.Compozr
{
    public class EmptyCompozrScriptsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.FromResult(0);
            return View();
        }
    }
}
