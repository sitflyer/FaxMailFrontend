namespace XMLBox
{
	/// <summary>
	/// Representiert einen Knoten in einer XML Datei
	/// </summary>
	public class XMLKnoten
	{
		public string Name { get; set; }
		public string Value { get; set; }

		public List<XMLAttribut> Attributes = new() { };

		public XMLKnoten(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}
}
