using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PwProblem.Controllers
{
    public class ResultController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            var files = HttpContext.Request.Files;
            if (files.Count != 1)
                throw new ArgumentNullException("Hiányzó file!");

            var file = files[0];
            var name = file.FileName.Split('.');
            var extension = name[name.Count() - 1];
            if (extension != "txt")
                throw new ArgumentNullException("Rossz fájl formátum!");

            List<string> lines = new List<string>();

            using (StreamReader reader = new StreamReader(file.InputStream))
            {
                do
                {
                    string textLine = reader.ReadLine();
                    lines.Add(textLine);
                }
                while (reader.Peek() != -1);
            }

            int validPasswordNumber = 0;

            foreach(var line in lines)
            {
                var lineContent = line.Split(' ');
                string pw = lineContent[2];
                var character = lineContent[1].Remove(1);
                var r = lineContent[0].Split('-');
                var min = int.Parse(r[0]);
                var max = int.Parse(r[1]);

                int n = 0;

                foreach (var c in pw)
                {
                    if (character.Contains(c))
                        n++;
                }

                if (n >= min && n <= max)
                    validPasswordNumber++;
            }

            ViewBag.ValidPasswordNumber = validPasswordNumber;

            return View();
        }
    }
}