using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        func("gjy");
    }
    private void func(string name)
    {
        string s = name + "123";

        if(string.IsInterned(s) == null)
        {
            Debug.Log("is not in stringconstpool");
        }
        string s2 = string.Intern(s);

        if(string.IsInterned(name + "123") != null)
        {
            Debug.Log("in pool");
        }

        string s3 = name + "123";

        Debug.Log(ReferenceEquals(s3, s2));
    }
  
}
