using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;





[Serializable]
class SceneMotion
    //the class of the scene motion
{
    public int num_objects;// the number of objects to be manipulated
    public float[] objects_motions;// the motions of the object
    public int[] geom_types;// types of the geoms 0: sphere, 1: box, 2: capsule

}


public class ObjectsManager : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> objects_list = new List<GameObject>();
    public Material[] material_prefab;
    public TextAsset text_asset;
    void Start()
    {

        Debug.Log(text_asset);
        SceneMotion scene_motion = new SceneMotion();//obj2里面是混乱的初始值
        JsonUtility.FromJsonOverwrite(text_asset.text, scene_motion);//把json_string字符串解析到obj2中

        Debug.Log(scene_motion.num_objects);
        Debug.Log(scene_motion.geom_types);
        Debug.Log(scene_motion.objects_motions[3]);
        //for(int i = 0; i<10; i++)
        //{
        //    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    cube.transform.position = new Vector3(0, 0.2f + i * 0.02f, 0);
        //    cube.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        //    Renderer rend = cube.GetComponent<Renderer>();
        //    rend.material = material_prefab[Random.Range(0, material_prefab.Length)];
        //    objects_list.Add(cube);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
