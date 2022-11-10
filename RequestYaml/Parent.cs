using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace U
{
    public interface IChild
    {
		string Value { get; }
		string this[int index] { get; }
        string this[string name] { get; }
    }
    
	public class Parent
	{
		public object Value { get; set; }

        public IChild Child
        {
            get 
            {
                if (Value.GetType() == typeof(String))
                    return new ChildS { Value = (String)Value };
                    
                if (Value.GetType() == typeof(List<string>))
                    return new ChildL { List = (List<string>)Value };
                    
                if (Value.GetType() == typeof(Dictionary<string, string>))
                    return new ChildD { Dict = (Dictionary<string, string>)Value } ;
                    
                return null;
            }
        }

		public static implicit operator ChildS(Parent parent)
		{
		    return new ChildS { Value = (String)parent.Value };
		}
		
		public static implicit operator ChildL(Parent parent)
		{
		    return new ChildL { List = (List<string>)parent.Value };
		}
		
		public static implicit operator ChildD(Parent parent)
		{
		    return new ChildD { Dict = (Dictionary<string, string>)parent.Value };
		}
	}

	public class ChildS : IChild
	{
		public string Value { get; set; }
        
        public static implicit operator String(ChildS childS) => childS.Value;
		
		public string this[int index] => null;
		
		public string this[string name] => null;
	}

	public class ChildL : IChild
	{
		public List<string> List { get; set; }
		
		public string Value => null;
        
		public string this[int index] => List[index];
		
		public string this[string name] => null;
	}

	public class ChildD : IChild
	{
		public Dictionary<string, string> Dict { get; set; }
		
		public string Value => null;
		
		public string this[int index] => null;
		
		public string this[string name]  => Dict[name];
	} 

	class Program {
	    
        static void Polimorf() 
        {
            Parent parent = new Parent() { Value = "hello" };
            
            IChild child = parent.Child;
            
            Console.WriteLine($"{child.Value}");
            
            
            
            parent = new Parent() { Value = new List<string> { "value1", "value2", "value3" } };
            
            child = parent.Child;
            
            Console.WriteLine($"{child["fsdf"]}");
            
            
            
            parent = new Parent() 
            { 
                Value = new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" },
                    { "key3", "value3" },
                }
            };
            
            child = parent.Child;
            
            Console.WriteLine("{0}", child[0]);

            string yamlText = @"
requests:
	first:
		url: google.com
		header:
			ref: google
			origin: google1
		body:
			text: hello world
			list:
				- value1 // index: 0
				- value2 // index: 1
				- value3 // index: 2
	second:
		url: ${requests.first.url}
		header:
			ref: ${requests.first.header.ref}
		body: ${requests.first.body}
	third:
		url: ${requests.second.url}
		header:
			ref: yandex
			origin: yandex.ru
		body: ${requests.second.body}
			list:
				- ${requests.first.body.list[1]}
				- ${requests.first.body.list[0]}
				- ${requests.first.body.list[2]}

";
        }
    }
}

//  case typeof(String).GetType(): return new ChildS { Value = (String)Value };
// case typeof(List<string>).GetType(): return new ChildL { List = (List<string>)Value };
// case typeof(Dictionary<string, string>).GetType(): return new ChildD { Dict = (Dictionary<string, string>)Value };
// default: return null;