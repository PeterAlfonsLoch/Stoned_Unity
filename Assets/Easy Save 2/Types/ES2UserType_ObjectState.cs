using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_ObjectState : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		ObjectState data = (ObjectState)obj;
		// Add your writer.Write calls here.
		writer.Write(data.position);
		writer.Write(data.localScale);
		writer.Write(data.rotation);
		writer.Write(data.velocity);
		writer.Write(data.angularVelocity);
		writer.Write(data.soList);
		writer.Write(data.objectName);
		writer.Write(data.sceneName);

	}
	
	public override object Read(ES2Reader reader)
	{
		ObjectState data = new ObjectState();
		Read(reader, data);
		return data;
	}

	public override void Read(ES2Reader reader, object c)
	{
		ObjectState data = (ObjectState)c;
		// Add your reader.Read calls here to read the data into the object.
		data.position = reader.Read<UnityEngine.Vector3>();
		data.localScale = reader.Read<UnityEngine.Vector3>();
		data.rotation = reader.Read<UnityEngine.Quaternion>();
		data.velocity = reader.Read<UnityEngine.Vector2>();
		data.angularVelocity = reader.Read<System.Single>();
		data.soList = reader.ReadList<SavableObject>();
		data.objectName = reader.Read<System.String>();
		data.sceneName = reader.Read<System.String>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_ObjectState():base(typeof(ObjectState)){}
}