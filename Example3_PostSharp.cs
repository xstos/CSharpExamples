using System;
using System.IO;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Reflection;

namespace CSharpExamples
{
    [Serializable]
    public class DeltaAspect : LocationInterceptionAspect
    {
        //public static StreamWriter Writer = new StreamWriter(@"c:\deltaaspect.txt");

        private static Action<string> log = (text) =>
        {
            // Writer.WriteLine(text);
            // Writer.Flush();
            Console.WriteLine(text);
        };
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            object oldValue = args.GetCurrentValue();
            object newValue = args.Value;

            if (args.Location.LocationKind == LocationKind.Property)
            {
                if (args.Location.PropertyInfo.PropertyType.IsValueType &&
                    !args.Location.Name.EndsWith("__BackingField"))
                {
                    if (!oldValue.Equals(newValue))
                    {
                        var classInstance = args.Instance; //this is the class who's property changed
                        var text = $"{args.LocationFullName} changed {oldValue} -> {newValue}";
                        log(text);
                    }
                }
            }

            base.OnSetValue(args);
        }
    }

    [Serializable]
    public sealed class TraceAspect : OnMethodBoundaryAspect
    {
        //public static StreamWriter Writer = new StreamWriter(@"c:\methodaspect.txt");

        static Action<string> log = (text) =>
        {
            // Writer.WriteLine(text);
            // Writer.Flush();
            Console.WriteLine(text);
        };

        [NonSerialized] private string enteringMessage;
        [NonSerialized] private string exitingMessage;

        public TraceAspect() { }

        public override void RuntimeInitialize(MethodBase method)
        {
            var methodName = method.DeclaringType.FullName + "." + method.Name;
            enteringMessage = "Method Enter: "+ methodName;
            exitingMessage = "Method Exit: " + methodName;
        }

        public override void OnEntry(MethodExecutionArgs args) => log(enteringMessage);

        public override void OnExit(MethodExecutionArgs args) => log(exitingMessage);
    }
    [DeltaAspect]
    [TraceAspect]
    class ClassIWantToTrack
    {
        public int Number { get; set; }
        public void Dog() { }
        public void Cat() { }
    }

    public static partial class Examples
    {
        public static void Example3()
        {
            Console.WriteLine("Example3");
            ClassIWantToTrack foo = new ClassIWantToTrack();
            foo.Number = 1;
            foo.Number = 2;
            foo.Dog();
            foo.Cat();

            //prints:

            //Method Enter: CSharpExamples.ClassIWantToTrack..ctor
            //Method Exit: CSharpExamples.ClassIWantToTrack..ctor
            //Method Enter: CSharpExamples.ClassIWantToTrack.set_Number
            //CSharpExamples.ClassIWantToTrack.Number changed 0 -> 1
            //Method Exit: CSharpExamples.ClassIWantToTrack.set_Number
            //Method Enter: CSharpExamples.ClassIWantToTrack.set_Number
            //CSharpExamples.ClassIWantToTrack.Number changed 1 -> 2
            //Method Exit: CSharpExamples.ClassIWantToTrack.set_Number
            //Method Enter: CSharpExamples.ClassIWantToTrack.Dog
            //Method Exit: CSharpExamples.ClassIWantToTrack.Dog
            //Method Enter: CSharpExamples.ClassIWantToTrack.Cat
            //Method Exit: CSharpExamples.ClassIWantToTrack.Cat

        }
    }
}
