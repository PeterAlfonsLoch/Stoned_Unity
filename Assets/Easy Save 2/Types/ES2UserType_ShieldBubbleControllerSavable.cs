using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_ShieldBubbleControllerSavable : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		ShieldBubbleControllerSavable data = (ShieldBubbleControllerSavable)obj;
        // Add your writer.Write calls here.
        writer.Write("ShieldBubbleControllerSavable");
        writer.Write(data.range);
		writer.Write(data.energy);

	}
	
	public override object Read(ES2Reader reader)
	{
		ShieldBubbleControllerSavable data = new ShieldBubbleControllerSavable();
		Read(reader, data);
		return data;
	}

	public override void Read(ES2Reader reader, object c)
	{
		ShieldBubbleControllerSavable data = (ShieldBubbleControllerSavable)c;
        // Add your reader.Read calls here to read the data into the object.
        string type = reader.Read<string>();
        data.range = reader.Read<System.Single>();
		data.energy = reader.Read<System.Single>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_ShieldBubbleControllerSavable():base(typeof(ShieldBubbleControllerSavable)){}
}