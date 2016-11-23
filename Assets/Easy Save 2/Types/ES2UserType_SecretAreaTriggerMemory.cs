using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_SecretAreaTriggerMemory : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		SecretAreaTriggerMemory data = (SecretAreaTriggerMemory)obj;
        // Add your writer.Write calls here.
        writer.Write("SecretAreaTriggerMemory");
        writer.Write(data.found);
		writer.Write(data.objectName);
		writer.Write(data.sceneName);

	}
	
	public override object Read(ES2Reader reader)
	{
		SecretAreaTriggerMemory data = new SecretAreaTriggerMemory();
		Read(reader, data);
		return data;
	}

	public override void Read(ES2Reader reader, object c)
	{
		SecretAreaTriggerMemory data = (SecretAreaTriggerMemory)c;
		// Add your reader.Read calls here to read the data into the object.
		data.found = reader.Read<System.Boolean>();
		data.objectName = reader.Read<System.String>();
		data.sceneName = reader.Read<System.String>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_SecretAreaTriggerMemory():base(typeof(SecretAreaTriggerMemory)){}
}