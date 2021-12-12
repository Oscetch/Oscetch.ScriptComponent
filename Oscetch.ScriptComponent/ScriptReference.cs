namespace Oscetch.ScriptComponent
{
    public class ScriptReference
    {
        /// <summary>
        /// The path to the dll that contains the script class
        /// </summary>
        public string DllPath { get; }
        /// <summary>
        /// The complete class name of the class this instance references
        /// </summary>
        public string ScriptClassName { get; }

        public ScriptReference(string dllPath, string scriptClassName)
        {
            DllPath = dllPath;
            ScriptClassName = scriptClassName;
        }

        public override string ToString()
        {
            return $"{ScriptClassName[(ScriptClassName.LastIndexOf('.') + 1)..]} -> {DllPath}";
        }

        public override bool Equals(object obj)
        {
            return obj is ScriptReference other
                && other.DllPath == DllPath
                && other.ScriptClassName == ScriptClassName;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(DllPath, ScriptClassName);
        }

        public static bool operator ==(ScriptReference left, ScriptReference right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }

        public static bool operator !=(ScriptReference left, ScriptReference right)
        {
            if (left is null)
            {
                return !(right is null);
            }

            return !left.Equals(right);
        }
    }
}
