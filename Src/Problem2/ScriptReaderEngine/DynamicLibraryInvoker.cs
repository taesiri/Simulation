using System;
using System.Collections;
using System.Reflection;

namespace Problem2.ScriptReaderEngine
{
    public class DynamicLibraryInvoker
    {
        // this way of invoking a function
        // is slower when making multiple calls
        // because the assembly is being instantiated each time.
        // But this code is clearer as to what is going on

        public static Hashtable AssemblyReferences = new Hashtable();
        public static Hashtable ClassReferences = new Hashtable();

        public static Object InvokeMethodSlow(string assemblyName, string className, string methodName, Object[] args)
        {
            // load the assembly
            Assembly assembly = Assembly.LoadFrom(assemblyName);

            // Walk through each type in the assembly looking for our class
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass)
                {
                    if (type.FullName.EndsWith("." + className))
                    {
                        // create an instance of the object
                        object classObj = Activator.CreateInstance(type);

                        // Dynamically Invoke the method
                        object result = type.InvokeMember(methodName, BindingFlags.Default | BindingFlags.InvokeMethod,
                                                          null,
                                                          classObj,
                                                          args);
                        return (result);
                    }
                }
            }
            throw (new Exception("Could not invoke method"));
        }

        public static InvokerClassInfo GetClassReference(string assemblyName, string className)
        {
            if (ClassReferences.ContainsKey(assemblyName) == false)
            {
                Assembly assembly;
                if (AssemblyReferences.ContainsKey(assemblyName) == false)
                {
                    AssemblyReferences.Add(assemblyName, assembly = Assembly.LoadFrom(assemblyName));
                }
                else
                    assembly = (Assembly) AssemblyReferences[assemblyName];

                // Walk through each type in the assembly
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsClass)
                    {
                        // doing it this way means that you don't have
                        // to specify the full namespace and class (just the class)
                        if (type.FullName.EndsWith("." + className))
                        {
                            var ci = new InvokerClassInfo(type,
                                                          Activator.CreateInstance(type));
                            ClassReferences.Add(assemblyName, ci);
                            return (ci);
                        }
                    }
                }
                throw (new Exception("Could not instantiate class"));
            }
            return ((InvokerClassInfo) ClassReferences[assemblyName]);
        }

        public static Object InvokeMethod(InvokerClassInfo ci, string methodName, Object[] args)
        {
            // Dynamically Invoke the method
            object result = ci.Type.InvokeMember(methodName,
                                                 BindingFlags.Default | BindingFlags.InvokeMethod,
                                                 null,
                                                 ci.ClassObject,
                                                 args);
            return (result);
        }

        // --- this is the method that you invoke ------------
        public static Object InvokeMethod(string assemblyName, string className, string methodName, Object[] args)
        {
            InvokerClassInfo ci = GetClassReference(assemblyName, className);
            return (InvokeMethod(ci, methodName, args));
        }

        #region Nested type: InvokerClassInfo

        public class InvokerClassInfo
        {
            public Object ClassObject;
            public Type Type;

            public InvokerClassInfo()
            {
            }

            public InvokerClassInfo(Type t, Object c)
            {
                Type = t;
                ClassObject = c;
            }
        }

        #endregion
    }
}