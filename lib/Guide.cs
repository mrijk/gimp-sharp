using System;
using System.Runtime.InteropServices;

namespace Gimp
{
	public class Guide
	{
		public enum OrientationType
		{
			HORIZONTAL,
			VERTICAL,
			UNKNOWN
		}

		protected Int32 _imageID;
		protected Int32 _guideID;

		public Guide(Image image, Int32 guideID)
		{
			_imageID = image.ID;
			_guideID = guideID;
		}

		Guide(Int32 imageID, Int32 guideID)
		{
			_imageID = imageID;
			_guideID = guideID;
		}

		[DllImport("libgimp-2.0.so")]
		static extern bool gimp_image_delete_guide(Int32 image_ID, Int32 guide_ID);

		public bool Delete()
		{
			return gimp_image_delete_guide(_imageID, _guideID);
		}

		[DllImport("libgimp-2.0.so")]
		static extern int gimp_image_get_guide_position (Int32 image_ID,
			Int32 guide_ID);

		public int Position
		{
			get {return gimp_image_get_guide_position(_imageID, _guideID);}
		}

		[DllImport("libgimp-2.0.so")]
		static extern Int32 gimp_image_find_next_guide (Int32 image_ID,
			Int32 guide_ID);

		public Guide FindNext()
		{
			Int32 next = gimp_image_find_next_guide(_imageID, _guideID);
			return (next == 0) ? null : new Guide(_imageID, next);
		}

		[DllImport("libgimp-2.0.so")]
		static extern OrientationType gimp_image_get_guide_orientation (Int32 image_ID,
			Int32 guide_ID);

		public OrientationType Orientation
		{
			get {return gimp_image_get_guide_orientation(_imageID, _guideID);}
		}
	}

	public class Vguide : Guide
	{
		[DllImport("libgimp-2.0.so")]
		static extern Int32 gimp_image_add_vguide (Int32 image_ID,
			int xposition);

		public Vguide(Image image, int xposition) : base(image, gimp_image_add_vguide(image.ID, xposition))
		{
		}
	}

	public class Hguide : Guide
	{
		[DllImport("libgimp-2.0.so")]
		static extern Int32 gimp_image_add_hguide (Int32 image_ID,
			int yposition);

		public Hguide(Image image, int yposition) : base(image, gimp_image_add_hguide(image.ID, yposition))
		{
		}
	}
}
