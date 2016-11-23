using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_SavableObject : ES2Type
{
    public override void Write(object obj, ES2Writer writer)
    {
        SavableObject data = (SavableObject)obj;
        // Add your writer.Write calls here.
        if (obj.GetType() == typeof(CrackedGroundCheckerSavable))//2016-11-20: copied from a post by Joel: http://www.moodkie.com/forum/viewtopic.php?f=5&t=956&p=2618
        {
            CrackedGroundCheckerSavable cgcs = (CrackedGroundCheckerSavable)obj;
            writer.Write("CrackedGroundCheckerSavable");
            writer.Write(cgcs.cracked);
        }
        else
        {
            writer.Write("None");
        }

    }

    public override object Read(ES2Reader reader)
    {
        string objType = reader.Read<string>();
        if (objType == "CrackedGroundCheckerSavable")//2016-11-20: copied from a post by Joel: http://www.moodkie.com/forum/viewtopic.php?f=5&t=956&p=2618
        {
            CrackedGroundCheckerSavable cgcs = new CrackedGroundCheckerSavable();
            cgcs.cracked = reader.Read<bool>();
            return cgcs;
        }
        return ObjectState.dummySO;
    }

    /* ! Don't modify anything below this line ! */
    public ES2UserType_SavableObject() : base(typeof(SavableObject)) { }
}