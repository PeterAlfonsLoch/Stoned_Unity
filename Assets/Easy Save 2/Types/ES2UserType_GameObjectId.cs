using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_GameObjectId : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		GameObjectId data = (GameObjectId)obj;
		// Add your writer.Write calls here.
		writer.Write(data.name);
		writer.Write(data.sceneName);

	}
	
	public override object Read(ES2Reader reader)
	{
		GameObjectId data = new GameObjectId();
		Read(reader, data);
		return data;
	}

	public override void Read(ES2Reader reader, object c)
	{
		GameObjectId data = (GameObjectId)c;
		// Add your reader.Read calls here to read the data into the object.
		data.name = reader.Read<System.String>();
		data.sceneName = reader.Read<System.String>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_GameObjectId():base(typeof(GameObjectId)){}
}