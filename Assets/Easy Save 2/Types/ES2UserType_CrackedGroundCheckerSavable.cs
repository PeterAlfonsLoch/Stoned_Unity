using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_CrackedGroundCheckerSavable : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		CrackedGroundCheckerSavable data = (CrackedGroundCheckerSavable)obj;
		// Add your writer.Write calls here.
        writer.Write("CrackedGroundCheckerSavable");
        writer.Write(data.cracked);

	}
	
	public override object Read(ES2Reader reader)
	{
		CrackedGroundCheckerSavable data = new CrackedGroundCheckerSavable();
		Read(reader, data);
		return data;
	}

	public override void Read(ES2Reader reader, object c)
	{
		CrackedGroundCheckerSavable data = (CrackedGroundCheckerSavable)c;
        // Add your reader.Read calls here to read the data into the object.
        string type = reader.Read<string>();
		data.cracked = reader.Read<System.Boolean>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_CrackedGroundCheckerSavable():base(typeof(CrackedGroundCheckerSavable)){}
}