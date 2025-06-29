namespace Oscetch.ScriptComponent
{
    public class ScriptValueParameter(string name, string value)
    {
        public string Name { get; } = name;
        public string Value { get; } = value;
    }
}
