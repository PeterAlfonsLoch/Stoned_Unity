using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_CheckPointCheckerMemory : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		CheckPointCheckerMemory data = (CheckPointCheckerMemory)obj;
        // Add your writer.Write calls here.
        writer.Write("CheckPointCheckerMemory");
        writer.Write(data.found);
		writer.Write(data.objectName);
		writer.Write(data.sceneName);

	}
	
	public override object Read(ES2Reader reader)
	{
		CheckPointCheckerMemory data = new CheckPointCheckerMemory();
		Read(reader, data);
		return data;
	}

	public override void Read(ES2Reader reader, object c)
	{
		CheckPointCheckerMemory data = (CheckPointCheckerMemory)c;
		// Add your reader.Read calls here to read the data into the object.
		data.found = reader.Read<System.Boolean>();
		data.objectName = reader.Read<System.String>();
		data.sceneName = reader.Read<System.String>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_CheckPointCheckerMemory():base(typeof(CheckPointCheckerMemory)){}
}