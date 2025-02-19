using System.Xml;

namespace XMLBox
{
	/// <summary>
	/// Representiert die Struktur einer XML Datei in Knoten und Attributen
	/// </summary>
	public class XMLKollector
	{
		public List<XMLKnoten> Knoten;

		public XMLKollector(XMLFile xdoc)
		{
			Knoten = new() { };
			foreach (XmlNode node in xdoc._XMLFile.SelectNodes("//*"))
			{
				XMLKnoten xnode = new(node.Name, node.InnerText);
				List<XMLAttribut> attribs = new() { };
				foreach (XmlAttribute xatt in node.Attributes)
				{
					attribs.Add(new(xatt.Name, xatt.Value));
				}
				xnode.Attributes = attribs;
				Knoten.Add(xnode);
			}
		}

	}
}
