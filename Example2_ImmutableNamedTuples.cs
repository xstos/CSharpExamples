using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples
{
    public static class Person
    {
        public delegate void PersonDelegate(string name, int age);
        public static Action<PersonDelegate> New(string name, int age)
        {
            return (method) => method(name, age);
        }
    }

    public static partial class Examples
    {
        public static void Example2()
        {
            Console.WriteLine("Example2");
            var jimmy = Person.New(name: "Jimmy", age: 5);

            //do something with jimmy
            jimmy((name, age) =>
            {
                //prints name:Jimmy age:5
                Console.WriteLine($"name:{name} age:{age}"); 
            });
        }
    }
}
