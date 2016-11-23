using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_MemoryObject : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		MemoryObject data = (MemoryObject)obj;
        if (obj.GetType() == typeof(SecretAreaTriggerMemory))//2016-11-23: copied from ES2UserType_SavableObject.Write(..)
        {
            SecretAreaTriggerMemory satm = (SecretAreaTriggerMemory)obj;
            writer.Write("SecretAreaTriggerMemory");
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
        if (objType == "SecretAreaTriggerMemory")//2016-11-23: copied from ES2UserType_SavableObject.Read(.)
        {
            data = new SecretAreaTriggerMemory();
           
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