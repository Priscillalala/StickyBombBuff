using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StickyBombBuff
{

    public class ImplicitArray<T> : IEnumerable
    {
        private T[] values;
        public ImplicitArray(T value)
        {
            values = new T[1];
            values[0] = value;
        }
        public ImplicitArray(params T[] values)
        {
            this.values = values;
        }
        public T this[int index] => values[index];
        public int Length => values.Length;

        public static implicit operator ImplicitArray<T>(T value) => new ImplicitArray<T>(value);
        public static implicit operator ImplicitArray<T>(T[] values) => new ImplicitArray<T>(values);
        public IEnumerator GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }
}
