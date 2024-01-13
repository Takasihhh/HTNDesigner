
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HTNDesigner.Utilies
{

    public static class ReflectionMethodExtension
    {
        public static object CreateInstance(string className)
        {
            Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Type type = assembly.GetType(className);
            if (type != null)
            {
                object res = Activator.CreateInstance(type);
                return res;
            }

            return null;
        }
        
              /// <summary>
        /// Gets all fields from an object and its hierarchy inheritance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>All fields of the type.</returns>
        public static List<FieldInfo> GetAllFields(this Type type, BindingFlags flags)
        {
            // Early exit if Object type
            if (type == typeof(object))
            {
                return new List<FieldInfo>();
            }

            // Recursive call
            var fields = type.BaseType.GetAllFields(flags);
            fields.AddRange(type.GetFields(flags | BindingFlags.DeclaredOnly));
            return fields;
        }

        /// <summary>
        /// Perform a deep copy of the class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>A deep copy of obj.</returns>
        /// <exception cref="ArgumentNullException">Object cannot be null</exception>
        public static T DeepCopy<T>(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }
            return (T)DoCopy(obj);
        }


        /// <summary>
        /// Does the copy.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Unknown type</exception>
        private static object DoCopy(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            // Value type
            var type = obj.GetType();
            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }

            // Array
            else if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                var array = obj as Array;
                Array copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(DoCopy(array.GetValue(i)), i);
                }
                return Convert.ChangeType(copied, obj.GetType());
            }

            // Unity Object
            else if (typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                return obj;
            }

            // Class -> Recursion
            else if (type.IsClass)
            {
                var copy = Activator.CreateInstance(obj.GetType());

                var fields = type.GetAllFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    var fieldValue = field.GetValue(obj);
                    if (fieldValue != null)
                    {
                        field.SetValue(copy, DoCopy(fieldValue));
                    }
                }

                return copy;
            }

            // Fallback
            else
            {
                throw new ArgumentException("Unknown type");
            }
        }
        
        
        
        /// <summary>
        /// 得到所有的子类
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static IEnumerable<Type> FindSubclassesOf(Type baseType)
        {
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前执行的程序集
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(baseType))
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// 得到项目中的所有枚举类型
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> FindEnumTypes()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsEnum)
                {
                    yield return type;
                }
            }
        }
        
        public static Type CreateEnum(string enumName, IEnumerable<string> enumElements)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;

            // Create a dynamic assembly in the current application domain,
            // and allow it to be executed and saved to disk.
            AssemblyName aName = Assembly.GetExecutingAssembly().GetName();
            AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(aName,AssemblyBuilderAccess.Run);
            ModuleBuilder mb = ab.DefineDynamicModule(aName.Name);
            EnumBuilder eb = mb.DefineEnum(enumName, TypeAttributes.Public, typeof(int));
            int value = 0;
            foreach (var element in enumElements.Distinct())
            {
                eb.DefineLiteral(element, value++);
            }

            return eb.CreateTypeInfo();
        }

    }
}