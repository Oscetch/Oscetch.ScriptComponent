using System;

namespace Oscetch.ScriptComponent.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ScriptParameter(string name) : Attribute
    {
        public string Name { get; } = name;
    }
}
