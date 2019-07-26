using System;
using System.Reflection;

namespace DelegatesAndOtherStuff
{

    public class ActionDelegate
    {
        public static ActionDelegate Create(object o, string name)
        {
            return new ActionDelegate(o, name);
        }

        public static ActionDelegate Create<T0>(object o, string name)
        {
            return new ActionDelegate(o, name, typeof(T0));
        }

        public static ActionDelegate Create<T0, T1>(object o, string name)
        {
            return new ActionDelegate(o, name, typeof(T0), typeof(T1));
        }

        public ActionDelegate(object target, string name, params Type[] argumentTypes)
        {
            Target = target;
            Method = target.GetType().GetMethod(name, argumentTypes);
        }

        public object Target { get; }

        public MethodInfo Method { get; }

        public void Invoke(params object[] arguments)
        {
            Method.Invoke(Target, arguments);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var anonymousPerson = new
            {
                Name = "Nikhil",
                SayHello = new Action( () => Console.WriteLine("Nikhil says hello!!!") )
            };
            Console.WriteLine(anonymousPerson.GetType().Name);
            anonymousPerson.SayHello();
            var person = new Person { Name = "Nikhil" };
            var del1 = ActionDelegate.Create(person, "SayHello");
            var del2 = ActionDelegate.Create<string>(person, "SayHelloWithGreeting");
            del1.Invoke();
            del2.Invoke("Namaste");

            Action act1 = person.SayHello;
            Action<string> act2 = person.SayHelloWithGreeting;
            
            act1.Invoke();
            act2.Invoke("Namaste");
            
            Console.ReadKey(true);
        }

        static void ReflectionSample()
        {
            var typeName = "DelegatesAndOtherStuff.Person, DelegatesAndOtherStuff";
            var type = Type.GetType(typeName); // var type = typeof(Person);
            var hopefullyAPerson = Activator.CreateInstance(type);
            var nameProperty = type.GetProperty("Name");
            nameProperty.SetValue(hopefullyAPerson, "Jane Doe");

            var method = type.GetMethod("SayHello");
            method.Invoke(hopefullyAPerson, null);
            Console.ReadKey(true);
            
        }
    }


    public class Person
    {
        public string Name { get; set; }

        
        public void SayHello()
        {
            Console.WriteLine($"{Name} says Hello!!!");
        }

        public void SayHelloWithGreeting(string greeting)
        {
            Console.WriteLine($"{Name} says {greeting}!!!");
        }
    }
}
