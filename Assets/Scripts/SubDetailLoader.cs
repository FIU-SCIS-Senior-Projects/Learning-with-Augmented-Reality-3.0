using UnityEngine;
using System.Collections;

public class SubDetailLoader : MonoBehaviour
{

    public const string path = "SubComponents030517";

    public SubDetailContainer dc;

    // Use this for initialization
    void Awake()
    {
        dc = SubDetailContainer.Load(path);
    }
}
