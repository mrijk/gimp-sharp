namespace Gimp
  {
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
  }
