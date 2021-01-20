using System.Security;

namespace WorldEditor
{
    public class Prefab
    {
        public string Name;
        public string Bundle;
        
        public Prefab(string name, string bundle)
        {
            this.Name = name;
            this.Bundle = bundle;
        }
    }
}