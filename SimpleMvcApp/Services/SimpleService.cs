using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleMvcApp.Services
{
    public class SimpleService : ISimpleService
    {
        public string GetMessage()
        {
            return "Hello World, I've been injected!";
        }
    }
}