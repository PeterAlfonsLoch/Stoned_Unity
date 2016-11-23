using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_CrackedGroundChecker : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		CrackedGroundChecker data = (CrackedGroundChecker)obj;
		// Add your writer.Write calls here.
		writer.Write(data.cracked);

	}
	
	public override object Read(ES2Reader reader)
	{
		CrackedGroundChecker data = GetOrCreate<CrackedGroundChecker>();
		Read(reader, data);
		return data;
	}

	public override void Read(ES2Reader reader, object c)
	{
		CrackedGroundChecker data = (CrackedGroundChecker)c;
		// Add your reader.Read calls here to read the data into the object.
		data.cracked = reader.Read<System.Boolean>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_CrackedGroundChecker():base(typeof(CrackedGroundChecker)){}
}