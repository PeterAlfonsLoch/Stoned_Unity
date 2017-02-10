using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Rigidbody2DLockSavable : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Rigidbody2DLockSavable data = (Rigidbody2DLockSavable)obj;
        // Add your writer.Write calls here.
        writer.Write("Rigidbody2DLockSavable");

    }
	
	public override object Read(ES2Reader reader)
	{
		Rigidbody2DLockSavable data = new Rigidbody2DLockSavable();
		Read(reader, data);
		return data;
	}

	public override void Read(ES2Reader reader, object c)
	{
		Rigidbody2DLockSavable data = (Rigidbody2DLockSavable)c;
        // Add your reader.Read calls here to read the data into the object.
        string type = reader.Read<string>();
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_Rigidbody2DLockSavable():base(typeof(Rigidbody2DLockSavable)){}
}