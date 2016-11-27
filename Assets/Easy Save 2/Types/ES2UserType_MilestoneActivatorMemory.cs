using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_MilestoneActivatorMemory : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		MilestoneActivatorMemory data = (MilestoneActivatorMemory)obj;
        // Add your writer.Write calls here.
        writer.Write("MilestoneActivatorMemory");
        writer.Write(data.found);
		writer.Write(data.objectName);
		writer.Write(data.sceneName);

	}
	
	public override object Read(ES2Reader reader)
	{
		MilestoneActivatorMemory data = new MilestoneActivatorMemory();
		Read(reader, data);
		return data;
	}

	public override void Read(ES2Reader reader, object c)
	{
		MilestoneActivatorMemory data = (MilestoneActivatorMemory)c;
		// Add your reader.Read calls here to read the data into the object.
		data.found = reader.Read<System.Boolean>();
		data.objectName = reader.Read<System.String>();
		data.sceneName = reader.Read<System.String>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_MilestoneActivatorMemory():base(typeof(MilestoneActivatorMemory)){}
}