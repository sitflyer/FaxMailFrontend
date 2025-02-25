namespace FaxMailFrontend.Data
{
	public class Cascade
	{
		public KanalArt KanalVorgabe { get; set; } = KanalArt.unbestimmt;
		public Ordnungsbegriff OBVorgabe { get; set; } = Ordnungsbegriff.unbestimmt;
		public WeitereOrdnungsbegriffe WOBVorgabe { get; set; } = WeitereOrdnungsbegriffe.unbestimmt;
		public BodyVeraktung BodyVeraktenVorgabe { get; set; } = BodyVeraktung.unbestimmt;

		public Cascade(int kanalart, int ob, int wob, int bv)
		{
			KanalVorgabe = (KanalArt)kanalart;
			OBVorgabe = (Ordnungsbegriff)ob;
			WOBVorgabe = (WeitereOrdnungsbegriffe)wob;
			BodyVeraktenVorgabe = (BodyVeraktung)bv;
		}		
	}
}
