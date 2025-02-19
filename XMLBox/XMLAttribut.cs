namespace XMLBox
{
	/// <summary>
	/// Representiert eine Attribut eines Knoten in einer XML Datei
	/// </summary>
	public class XMLAttribut
	{
		public XMLAttribut(string name, string value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; set; }
		public string Value { get; set; }
	}
}
