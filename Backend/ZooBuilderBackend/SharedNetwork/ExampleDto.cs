namespace SharedNetwork
{
    public struct ExampleDto : IStringSerializable
    {
        public int ExampleInt;
        public string ExampleString;
        public float ExampleFloat;
        
        public void FromString(string dataString)
        {
            string cleanedString = dataString.Replace("{", string.Empty).Replace("}", string.Empty);
            string[] values = cleanedString.Split(';');
            ExampleInt = int.Parse(values[0]);
            ExampleString = values[1];
            ExampleFloat = float.Parse(values[2].Replace(",", "."));
        }

        public override string ToString()
        {
            return "{" + ExampleInt + ";" + ExampleString + ";" + ExampleFloat + "}";
        }
    }
}