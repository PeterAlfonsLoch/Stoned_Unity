using System.Collections;
using System.Collections.Generic;
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

        return data;
	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_MemoryObject():base(typeof(MemoryObject)){}
}