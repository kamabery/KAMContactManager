using System;

namespace ContactManager.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PersistentAttribute : Attribute
    {
    }
}