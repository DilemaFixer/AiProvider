using AIs;

namespace AiServise 
{
    public class Program
    {
        private const string API_TOKEN = "token";
        
        static void Main(string[] args)
        {
            IAiProvider aiProvider = new DeepseekAi(DeepseekAi.Models.BaseModel , API_TOKEN);

            var msgs = new Message[]
            {
                new Message{ Role = "system", Content = "You are a helpful assistant." },
                new Message { Role = "user", Content = "How write hello world to console in c# ?" }
            };

           string response = aiProvider.GetResponseAsync(msgs).GetAwaiter().GetResult();
           Console.WriteLine(response);
           Console.ReadLine();
        }
    }
    
    /*          RESPONSE
     
     In C#, you can write "Hello, World!" to the console using the following code:

```csharp
using System;

class Program {
    static void Main() {
        Console.WriteLine("Hello, World!");
    }
}
```

This code first includes the `System` namespace, which is needed for the `Console.WriteLine` method. The `Console.WriteLine` method is a method in the `Console` class that writes a new line to the console.
 The string "Hello, World!" is passed as an argument to `Console.WriteLine`.

     
     */
}