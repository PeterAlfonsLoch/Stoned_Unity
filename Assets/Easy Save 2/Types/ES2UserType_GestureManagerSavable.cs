using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_GestureManagerSavable : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		GestureManagerSavable data = (GestureManagerSavable)obj;
        // Add your writer.Write calls here.
        writer.Write("GestureManagerSavable");
        writer.Write(data.holdThresholdScale);
		writer.Write(data.tapCount);

	}
	
	public override object Read(ES2Reader reader)
	{
		GestureManagerSavable data = new GestureManagerSavable();
		Read(reader, data);
		return data;
	}

	public override void Read(ES2Reader reader, object c)
	{
		GestureManagerSavable data = (GestureManagerSavable)c;
        // Add your reader.Read calls here to read the data into the object.
        string type = reader.Read<string>();
        data.holdThresholdScale = reader.Read<System.Single>();
		data.tapCount = reader.Read<System.Int32>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_GestureManagerSavable():base(typeof(GestureManagerSavable)){}
}