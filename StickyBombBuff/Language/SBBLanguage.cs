using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using System.Linq;
using System.Collections.Generic;

namespace StickyBombBuff.Language
{

    public static class SBBLanguage
    {
        public static (string, string) nullString = (null, null);
        public static bool ModuleEnabled<T>() where T : BaseModModule
        {
            return StickyBombBuffPlugin.instance.ModuleEnabled<T>();
        }
        public static (string token, string localizedString) Add(string token, string localizedString) => (token, localizedString);
        public static (string token, string localizedString) Add<A>(string token, string localizedString)
            where A : BaseModModule
        {
            if (ModuleEnabled<A>())
            {
                return (token, localizedString);
            }
            return nullString;
        }
        public static (string token, string localizedString) Add<A, B>(string token, string localizedString)
            where A : BaseModModule
            where B : BaseModModule
        {
            if (ModuleEnabled<A>() || ModuleEnabled<B>())
            {
                return (token, localizedString);
            }
            return nullString;
        }
        public static (string token, string localizedString) Add<A, B, C>(string token, string localizedString)
            where A : BaseModModule
            where B : BaseModModule
            where C : BaseModModule
        {
            if (ModuleEnabled<A>() || ModuleEnabled<B>() || ModuleEnabled<C>())
            {
                return (token, localizedString);
            }
            return nullString;
        }

    }
}
