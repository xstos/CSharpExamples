#r "Newtonsoft.Json.dll"
using System;
using System.IO;
using System.Linq;

// https://github.com/scriptcs/scriptcs
Console.WriteLine("Args you passed in: " + Newtonsoft.Json.JsonConvert.SerializeObject(Env.ScriptArgs));
