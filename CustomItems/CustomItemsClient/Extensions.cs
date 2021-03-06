//using System;
//using System.Reflection;
//using UnityEngine;
//
//namespace CustomItemsClient
//{
//    public static class Extensions
//    {
//        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
//        {
//            Type type = comp.GetType();
//            if (type.FullName != other.GetType().FullName) return null; // type mis-match
//            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
////            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
////                                 BindingFlags.Default;
//            PropertyInfo[] pinfos = type.GetProperties(flags);
//            foreach (var pinfo in pinfos)
//            {
//                if (pinfo.CanWrite)
//                {
//                    try
//                    {
//                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
//                    }
//                    catch
//                    {
//                    } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
//                }
//            }
//
//            FieldInfo[] finfos = type.GetFields(flags);
//            foreach (var finfo in finfos)
//            {
//                try
//                {
//                    finfo.SetValue(comp, finfo.GetValue(other));
//                }
//                catch
//                {
//                }
//            }
//
//            return comp as T;
//        }
//
//        public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
//        {
//            return go.AddComponent<T>().GetCopyOf(toAdd) as T;
//        }
//        
//        public static T AddComponentWithInit<T>(this GameObject obj, System.Action<T> onInit) where T : Component
//        {
//            bool oldState = obj.activeSelf;
//            obj.SetActive(false);
//            T comp = obj.AddComponent<T>();
//            if (onInit != null)
//                onInit(comp);
//            obj.SetActive(oldState);
//            return comp;
//        }
//    }
//}