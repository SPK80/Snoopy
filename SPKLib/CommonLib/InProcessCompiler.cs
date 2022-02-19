using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace CommonLib.Infrastructure
{
    public class SingleTypeInProcessCompiler
    {
        string compilerVersion = "v4.0";

        public Assembly Compile(string sourceCode)
        {
            var compilerParameters = PrepareCompilerParameters();

            var providerOptions = new Dictionary<string, string>{ ["CompilerVersion"] = compilerVersion };
            var codeProvider = new CSharpCodeProvider(providerOptions);
            var results = codeProvider.CompileAssemblyFromSource(compilerParameters, new[] { sourceCode });

            if (results.Errors.HasErrors)
            {
                var sb = new StringBuilder();
                foreach (var e in results.Errors)
                    sb.AppendLine(e.ToString());
                throw new Exception(sb.ToString());
            }

            return results.CompiledAssembly;
        }

        public Type CompileAndGetType(string sourceCode)
        {
            var ass = Compile(sourceCode);
            var types = ass.GetTypes();
            if (types == null || types.Length == 0)
                throw new Exception("Compiled assembly produced no types");
            else
                return types[0];
        }

        public object CompileAndInstantiate(string sourceCode)
        {
            var mainType = CompileAndGetType(sourceCode);
            return Activator.CreateInstance(mainType);
        }

        private CompilerParameters PrepareCompilerParameters()
        {
            var ps = new CompilerParameters { GenerateInMemory = true, GenerateExecutable = false };

            // add everything in this AppDomain
            foreach (var reference in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    ps.ReferencedAssemblies.Add(reference.Location);
                }
                catch (Exception ex)
                {
                    string s = "Cannot add assembly " + reference.FullName + " as reference.";
                    Debug.WriteLine(s);
                }
            }

            return ps;
        }
    }
}
