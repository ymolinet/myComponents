/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace Gaia.WebWidgets
{
    sealed class Reflection
    {
        /// <summary>
        /// Provides abstraction for property value access
        /// </summary>
        /// <typeparam name="TTarget">Target object type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        public interface IGetter<in TTarget, out TValue>
        {
            TValue GetValue(TTarget obj);
        }

        public delegate TReturn Func<in T1, out TReturn>(T1 param1);

        private static readonly bool CanSkipVisibilityChecks = SecurityManager.IsGranted(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));
        private static readonly MethodInfo WeakDelegateMethod1 = typeof(Reflection).GetMethod("CreateWeakDelegate1", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding);
        private static readonly MethodInfo WeakDelegateMethod2 = typeof(Reflection).GetMethod("CreateWeakDelegate2", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding);
        private static readonly MethodInfo WeakDelegateMethod3 = typeof(Reflection).GetMethod("CreateWeakDelegate3", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding);
        private static readonly MethodInfo WeakDelegateMethod4 = typeof(Reflection).GetMethod("CreateWeakDelegate4", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding);

        /// <summary>
        /// Creates IGetter for the property on the type TTarget
        /// having the specified name
        /// </summary>
        /// <typeparam name="TTarget">Target object type</typeparam>
        /// <typeparam name="TValue">Property value type</typeparam>
        /// <param name="propertyName">Property name</param>
        /// <returns>IGetter for the property</returns>
        public static IGetter<TTarget, TValue> Property<TTarget, TValue>(string propertyName)
        {
            return Property<TTarget, TValue>(typeof(TTarget).GetProperty(propertyName, 
                BindingFlags.ExactBinding | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
        }

        /// <summary>
        /// Creates IGetter for the property based on the specified PropertyInfo.
        /// Conversion from real object/value types to the specified TTarget/TValue types
        /// are made automatically. May throw if not assignable.
        /// </summary>
        /// <typeparam name="TTarget">Target object type</typeparam>
        /// <typeparam name="TValue">Property value</typeparam>
        /// <param name="propertyInfo">PropertyInfo to use for property access</param>
        /// <returns>IGetter for the property</returns>
        public static IGetter<TTarget, TValue> Property<TTarget, TValue>(PropertyInfo propertyInfo)
        {
            MethodInfo info = propertyInfo.GetGetMethod(true);
            return CanSkipVisibilityChecks || info.IsPublic || info.IsFamily || info.IsFamilyOrAssembly ||
                    info.DeclaringType.Assembly.Equals(Assembly.GetExecutingAssembly()) ?
                (IGetter<TTarget, TValue>)new OptimizedPropertyGetter<TTarget, TValue>(info) : new ReflectionPropertyGetter<TTarget, TValue>(info);
        }

        private sealed class ReflectionPropertyGetter<TTarget, TValue> : IGetter<TTarget, TValue>
        {
            private readonly MethodInfo _info;

            public ReflectionPropertyGetter(MethodInfo info)
            {
                _info = info;
            }

            public TValue GetValue(TTarget obj)
            {
                return (TValue)_info.Invoke(obj, null);
            }
        }

        private sealed class OptimizedPropertyGetter<TTarget, TValue> : IGetter<TTarget, TValue>
        {
            private readonly Func<TTarget, TValue> _getter;

            public OptimizedPropertyGetter(MethodInfo info)
            {
                Type strongType = typeof(Func<,>).MakeGenericType(info.DeclaringType, info.ReturnType);
                _getter = CreateValueGetter<TTarget, TValue>(info.DeclaringType, info.ReturnType, Delegate.CreateDelegate(strongType, info));
            }

            public TValue GetValue(TTarget obj)
            {
                return _getter(obj);
            }
        }
        
        public static Func<TTarget, TReturn> CreateWeakDelegate1<TTarget, TReturn>(Delegate strongDelegate)
        {
            return (Func<TTarget, TReturn>)strongDelegate;
        }

        public static Func<TTarget2, TReturn1> CreateWeakDelegate2<TTarget1, TReturn1, TTarget2>(Delegate strongDelegate)
        {
            var callback = (Func<TTarget1, TReturn1>)strongDelegate;
            return target => callback((TTarget1) (object) target);
        }

        public static Func<TTarget1, TReturn2> CreateWeakDelegate3<TTarget1, TReturn1, TReturn2>(Delegate strongDelegate)
        {
            var callback = (Func<TTarget1, TReturn1>)strongDelegate;
            return target => (TReturn2) (object) callback(target);
        }

        public static Func<TTarget2, TReturn2> CreateWeakDelegate4<TTarget1, TReturn1, TTarget2, TReturn2>(Delegate strongDelegate)
        {
            var callback = (Func<TTarget1, TReturn1>)strongDelegate;
            return target => (TReturn2) (object) callback((TTarget1) (object) target);
        }

        private static Func<TTarget, TValue> CreateValueGetter<TTarget, TValue>(Type targetType, Type returnType, Delegate callback)
        {
            bool sameTargetType = targetType == typeof(TTarget);
            bool sameReturnType = returnType == typeof(TValue);
            
            var arguments = new object[] { callback };
            
            if (sameTargetType && sameReturnType)
                return (Func<TTarget, TValue>)WeakDelegateMethod1.MakeGenericMethod(targetType, returnType).Invoke(null, arguments);
            if (sameReturnType)
                return (Func<TTarget, TValue>)WeakDelegateMethod2.MakeGenericMethod(targetType, returnType, typeof(TTarget)).Invoke(null, arguments);
            if (sameTargetType)
                return (Func<TTarget, TValue>)WeakDelegateMethod3.MakeGenericMethod(targetType, returnType, typeof(TValue)).Invoke(null, arguments);

            return (Func<TTarget, TValue>)WeakDelegateMethod4.MakeGenericMethod(targetType, returnType, typeof(TTarget), typeof(TValue)).Invoke(null, arguments);
        }

        /// <summary>
        /// Cache of members (properties or methods) having specific custom attribute
        /// </summary>
        /// <typeparam name="TCustomAttribute">Custom attribute type for member filtering</typeparam>
        internal sealed class Cache<TCustomAttribute> where TCustomAttribute : Attribute
        {
            public sealed class CachedProperty
            {
                readonly string _name;
                readonly TCustomAttribute _attribute;
                readonly IGetter<object, object> _getter;

                public string Name
                {
                    get { return _name; }
                }

                public TCustomAttribute Attribute
                {
                    get { return _attribute; }
                }

                public IGetter<object, object> Getter
                {
                    get { return _getter; }
                }

                public CachedProperty(PropertyInfo info, TCustomAttribute attribute)
                {
                    _name = info.Name;
                    _attribute = attribute;
                    _getter = Property<object, object>(info);
                }
            }

            private sealed class CacheKey
            {
                private readonly string _methodName;
                private readonly RuntimeTypeHandle _typeHandle;

                public CacheKey(RuntimeTypeHandle typeHandle, string methodName)
                {
                    _typeHandle = typeHandle;
                    _methodName = methodName;
                }

                public override bool Equals(object obj)
                {
                    var key = obj as CacheKey;
                    return key != null && key._methodName == _methodName && _typeHandle.Equals(key._typeHandle);
                }

                public override int GetHashCode()
                {
                    return _methodName.GetHashCode() ^ _typeHandle.GetHashCode();
                }
            }

            private static readonly Dictionary<CacheKey, MethodInfo> MethodCache = new Dictionary<CacheKey, MethodInfo>();
            private static readonly Dictionary<RuntimeTypeHandle, List<CachedProperty>> PropertyCache = new Dictionary<RuntimeTypeHandle, List<CachedProperty>>();

            /// <summary>
            /// Returns cached MethodInfo for the specified method
            /// </summary>
            /// <param name="obj">Object to find method on</param>
            /// <param name="methodName">Method name to search for</param>
            /// <param name="throwOnAbsence">Specifies if should throw exception if the method is absent</param>
            /// <returns>Cached MethodInfo</returns>
            public static MethodInfo GetMethod(object obj, string methodName, bool throwOnAbsence)
            {
                MethodInfo info;
                RuntimeTypeHandle typeHandle = Type.GetTypeHandle(obj);
                var key = new CacheKey(typeHandle, methodName);

                if (!MethodCache.TryGetValue(key, out info))
                {
                    Type type = Type.GetTypeFromHandle(typeHandle);
                    Type customAttributeType = typeof(TCustomAttribute);

                    info = type.GetMethod(methodName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.ExactBinding | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    if (info == null || !Attribute.IsDefined(info, customAttributeType, true))
                    {
                        if (!throwOnAbsence) return null;

                        throw new ArgumentException(string.Format("Type {0} does not contain method with name {1} and custom attribute {2}",
                            type.FullName, methodName, customAttributeType.FullName));
                    }

                    MethodCache[key] = info;
                }

                return info;
            }

            /// <summary>
            /// Returns list of cached properties having 
            /// specific attribute on the specified object
            /// </summary>
            /// <param name="obj">Object to search for properties</param>
            /// <returns>Cached lists of properties</returns>
            public static List<CachedProperty> GetProperties(object obj)
            {
                List<CachedProperty> items;
                RuntimeTypeHandle key = Type.GetTypeHandle(obj);

                if (!PropertyCache.TryGetValue(key, out items))
                {
                    items = new List<CachedProperty>();
                    Type customAttributeType = typeof(TCustomAttribute);

                    Array.ForEach(Type.GetTypeFromHandle(key).GetProperties(BindingFlags.Instance | BindingFlags.ExactBinding | BindingFlags.Public | BindingFlags.NonPublic),
                        delegate(PropertyInfo info)
                        {
                            Attribute attr = Attribute.GetCustomAttribute(info, customAttributeType, true);
                            if (attr != null) items.Add(new CachedProperty(info, attr as TCustomAttribute));
                        });
                    PropertyCache[key] = items;
                }

                return items;
            }
        }
    }
}
