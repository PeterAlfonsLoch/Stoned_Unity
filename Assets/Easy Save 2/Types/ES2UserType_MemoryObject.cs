using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ES2UserType_MemoryObject : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		MemoryObject data = (MemoryObject)obj;
        if (obj.GetType() == typeof(HiddenAreaMemory))//2016-11-23: copied from ES2UserType_SavableObject.Write(..)
        {
            HiddenAreaMemory satm = (HiddenAreaMemory)obj;
            writer.Write("HiddenAreaMemory");
        }
        else if (obj.GetType() == typeof(CheckPointCheckerMemory))//2016-11-26
        {
            CheckPointCheckerMemory cpcm = (CheckPointCheckerMemory)obj;
            writer.Write("CheckPointCheckerMemory");
            //writer.Write(cpcm.ghostSprite);
            //writer.Write(cpcm.ghostSprite.texture);
            //writer.WriteRaw(cpcm.ghostSprite.texture.EncodeToPNG());
            //if (cpcm.ghostSprite != null && cpcm.found)
            //{
            //    string filename = "" + cpcm.sceneName + "_" + cpcm.objectName + ".png";
            ////    Debug.Log("ES2_MO: Write: filename: " + filename);
            //    ES2.SaveImage(cpcm.ghostSprite.texture, filename);
            //}
        }
        else if (obj.GetType() == typeof(MilestoneActivatorMemory))//2016-11-26
        {
            MilestoneActivatorMemory mam = (MilestoneActivatorMemory)obj;
            writer.Write("MilestoneActivatorMemory");
        }
        else
        {
            writer.Write("None");
        }
        // Write info for all subtypes
        writer.Write(data.found);
		writer.Write(data.objectName);
		writer.Write(data.sceneName);
        

    }
	
	public override object Read(ES2Reader reader)
	{
        MemoryObject data; 
        string objType = reader.Read<string>();
        if (objType == "HiddenAreaMemory")//2016-11-23: copied from ES2UserType_SavableObject.Read(.)
        {
            data = new HiddenAreaMemory();
        }
        else if (objType == "CheckPointCheckerMemory")//2016-11-26
        {
            data = new CheckPointCheckerMemory();
            //Sprite spr = reader.Read<UnityEngine.Sprite>();
            //Debug.Log("ES2_MO: Read: spr: " + spr.name);
            //((CheckPointCheckerMemory)data).ghostSprite = spr;
            //Texture2D t2d = new Texture2D(0, 0);// reader.Read<Texture2D>();
            //Debug.Log("ES2_MO: Read: t2d: " + t2d.);
            //t2d.LoadImage(reader.ReadArray<byte>());
            //((CheckPointCheckerMemory)data).ghostSprite = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
        }
        else if (objType == "MilestoneActivatorMemory")//2016-11-26
        {
            data = new MilestoneActivatorMemory();
        }
        else
        {
            data = new MemoryObject();
        }
        data.found = reader.Read<System.Boolean>();
        data.objectName = reader.Read<System.String>();
        data.sceneName = reader.Read<System.String>();
        //if (objType == "CheckPointCheckerMemory")
        //{//2016-12-13
        //    string filename = "" + data.sceneName + "_" + data.objectName + ".png";
        //    Debug.Log("ES2_MO: Read: filename: " + filename);
        //    if (File.Exists(filename))
        //    {
        //        Texture2D t2d = ES2.LoadImage(filename);
        //        ((CheckPointCheckerMemory)data).ghostSprite = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
        //    }
        //}
        return data;
	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_MemoryObject():base(typeof(MemoryObject)){}
}