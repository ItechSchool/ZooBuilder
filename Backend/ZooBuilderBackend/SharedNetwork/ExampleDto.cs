namespace SharedNetwork
{
    public struct ExampleDto : IStringSerializable
    {
        public int ExampleInt { get; set; }
        public string ExampleString { get; set; }
        public float ExampleFloat { get; set; }
        
        public void FromString(string dataString)
        {
            var copy = StringSerializer.Deserialize<ExampleDto>(dataString);
            ExampleInt = copy.ExampleInt;
            ExampleString = copy.ExampleString;
            ExampleFloat = copy.ExampleFloat;
        }

        public override string ToString()
        {
            return StringSerializer.Serialize(this);
        }
    }
}