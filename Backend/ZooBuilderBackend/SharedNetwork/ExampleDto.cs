namespace SharedNetwork
{
    public struct OtherStruct : IStringSerializable
    {
        public int Id { get; set; }
        public float Value { get; set; }

        public void FromString(string dataString)
        {
            var copy = StringSerializer.Deserialize<OtherStruct>(dataString);
            Id = copy.Id;
            Value = copy.Value;
        }

        public override string ToString()
        {
            return StringSerializer.Serialize(this);
        }
    }

    public struct ExampleDto : IStringSerializable
    {
        public int ExampleInt { get; set; }
        public OtherStruct OtherStruct { get; set; }
        public float ExampleFloat { get; set; }

        public void FromString(string dataString)
        {
            var copy = StringSerializer.Deserialize<ExampleDto>(dataString);
            ExampleInt = copy.ExampleInt;
            OtherStruct = copy.OtherStruct;
            ExampleFloat = copy.ExampleFloat;
        }

        public override string ToString()
        {
            return StringSerializer.Serialize(this);
        }
    }
}