namespace Gimp
  {
    public enum AddMaskType
      {
	ADD_WHITE_MASK,
	ADD_BLACK_MASK,
	ADD_ALPHA_MASK,
	ADD_ALPHA_TRANSFER_MASK,
	ADD_SELECTION_MASK,
	ADD_COPY_MASK
      }

    public enum BlendMode
      {
	FG_BG_RGB_MODE,
	FG_BG_HSV_MODE,
	FG_TRANSPARENT_MODE,
	CUSTOM_MODE
      }

    public enum BrushApplicationMode
      {
	BRUSH_HARD,
	BRUSH_SOFT
      }

    public enum BucketFillMode
      {
	FG_BUCKET_FILL,
	BG_BUCKET_FILL,
	PATTERN_BUCKET_FILL
      }

    public enum ChannelOps
      {
	ADD,
	SUBTRACT,
	REPLACE,
	INTERSECT
      }

    public enum FillType
      {
	FOREGROUND_FILL,
	BACKGROUND_FILL,
	WHITE_FILL,
	TRANSPARENT_FILL,
	PATTERN_FILL
      }

    public enum RunMode
      {
	INTERACTIVE,
	NONINTERACTIVE,
	WITH_LAST_VALS
      }

    public enum PDBStatusType
      {
	PDB_EXECUTION_ERROR,
	PDB_CALLING_ERROR,
	PDB_PASS_THROUGH,
	PDB_SUCCESS,
	PDB_CANCEL
      }

    public enum PDBProcType
      {
	INTERNAL,
	PLUGIN,
	EXTENSION,
	TEMPORARY
      }
  
    public enum ImageBaseType
      {
	RGB,    
	RGBA,
	GRAY,
	GRAYA,
	INDEXED,
	INDEXEDA
      }

    public enum PDBArgType
      {
	INT32,
	INT16,
	INT8,
	FLOAT,
	STRING,
	INT32ARRAY,
	INT16ARRAY,
	INT8ARRAY,
	FLOATARRAY,
	STRINGARRAY,
	COLOR,
	REGION,
	DISPLAY,
	IMAGE,
	LAYER,
	CHANNEL,
	DRAWABLE,
	SELECTION,
	BOUNDARY,
	PATH,
	PARASITE,
	STATUS,
	END
      }

    public enum ConvertDitherType
      {
	NO_DITHER,
	FS_DITHER,
	FSLOWBLEED_DITHER,
	FIXED_DITHER
      }

    public enum ConvertPaletteType
      {
	MAKE_PALETTE,
	REUSE_PALETTE,
	WEB_PALETTE,
	MONO_PALETTE,
	CUSTOM_PALETTE
      }

    public enum ImageType
      {
	RGB_IMAGE,
	RGBA_IMAGE,
	GRAY_IMAGE,
	GRAYA_IMAGE,
	INDEXED_IMAGE,
	INDEXEDA_IMAGE
      }
  
    public enum LayerModeEffects
      {
	NORMAL_MODE,
	DISSOLVE_MODE,
	BEHIND_MODE,
	MULTIPLY_MODE,
	SCREEN_MODE,
	OVERLAY_MODE,
	DIFFERENCE_MODE,
	ADDITION_MODE,
	SUBTRACT_MODE,
	DARKEN_ONLY_MODE,
	LIGHTEN_ONLY_MODE,
	HUE_MODE,
	SATURATION_MODE,
	COLOR_MODE,
	VALUE_MODE,
	DIVIDE_MODE,
	DODGE_MODE,
	BURN_MODE,
	HARDLIGHT_MODE,
	SOFTLIGHT_MODE,
	GRAIN_EXTRACT_MODE,
	GRAIN_MERGE_MODE,
	COLOR_ERASE_MODE
      }

    public enum MergeType
      {
	EXPAND_AS_NECESSARY,
	CLIP_TO_IMAGE,
	CLIP_TO_BOTTOM_LAYER,
	FLATTEN_IMAGE
      }

    public enum MaskApplyMode
      {
	APPLY,
	DISCARD
      }

    public enum OrientationType
      {
	HORIZONTAL,
	VERTICAL,
	UNKNOWN
      }

    public enum RotationType
      {
	ROTATE_90,
	ROTATE_180,
	ROTATE_270
      }

    public enum Transparency
      {
	KEEP_ALPHA,
	SMALL_CHECKS,
	LARGE_CHECKS
      }

    public enum Unit
      {
	PIXEL   = 0,
	INCH    = 1,
	MM      = 2,
	POINT   = 3,
	PICA    = 4,
	END     = 5,

	PERCENT = 65536 
      }
  }
