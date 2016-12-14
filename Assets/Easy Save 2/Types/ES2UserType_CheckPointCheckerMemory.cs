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
        ///writer.Write(data.ghostSprite);
        //string filename = "" + data.sceneName + "_" + data.objectName + ".png";
        //Debug.Log("ES2_CPCM: Write: filename: " + filename);
        //ES2.SaveImage(data.ghostSprite.texture, filename);
        //writer.WriteRaw(data.ghostSprite.texture.EncodeToPNG());
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
        //data.ghostSprite = reader.Read<UnityEngine.Sprite>();
        data.found = reader.Read<System.Boolean>();
		data.objectName = reader.Read<System.String>();
		data.sceneName = reader.Read<System.String>();
        //string filename = "" + data.sceneName + "_" + data.objectName + ".png";
        //Debug.Log("ES2_CPCM: Read: filename: " + filename);
        //Texture2D t2d = ES2.LoadImage(filename);
        //data.ghostSprite = Sprite.Create(t2d,new Rect(0,0,t2d.width,t2d.height), new Vector2(0.5f, 0.5f));

    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_CheckPointCheckerMemory():base(typeof(CheckPointCheckerMemory)){}
}