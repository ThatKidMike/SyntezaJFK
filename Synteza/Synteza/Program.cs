namespace Synteza {

    using System;
    using System.IO;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    class Program {


        private static readonly string AssemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

        static void Main(string[] args) {

            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText("Class.cs"));
            var newRoot = new Rewriter().Visit(tree.GetRoot());
           

            File.WriteAllText(@"..\..\Class.changed.cs", newRoot.GetText().ToString());

            var mscorlib = MetadataReference.CreateFromFile(Path.Combine(AssemblyPath, "mscorlib.dll"));
            var system = MetadataReference.CreateFromFile(Path.Combine(AssemblyPath, "System.dll"));
            var systemCore = MetadataReference.CreateFromFile(Path.Combine(AssemblyPath, "System.Core.dll"));

            var compilation = CSharpCompilation.Create("MyCompilation", syntaxTrees: new[] { newRoot.SyntaxTree }, 
                references: new[] { mscorlib, system, systemCore },
                options: new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            foreach (var item in compilation.GetDiagnostics())
                Console.WriteLine($"Diagnostics: {item}");

            var emitResult = compilation.Emit("MyCompilation.exe", "MyCompilation.pdb");

            if (!emitResult.Success)
                foreach (var error in emitResult.Diagnostics)
                    Console.WriteLine(error);

            Console.ReadLine();     

            
            }
        
        }

    }

   


 
