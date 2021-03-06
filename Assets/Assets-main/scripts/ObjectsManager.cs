using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


[System.Serializable]
class SceneMotion
    //the class of the scene motion
{
    public int num_objects;// the number of objects to be manipulated
    public float[] motions;// the motions of the object
    public int[] geom_types;// types of the geoms 0: sphere, 1: box, 2: capsule
    public float[] shapes;

}

public class ObjectMotionGenerator
{
    public float[] motion;
    public float motion_time = 0.0f;
    public float dt = 0.01f;//delta time between two frames
    public ObjectMotionGenerator(float[] data)
    {
        this.motion = data;
        this.motion_time = this.dt * (motion.Length / 7 - 1);
    }
    public (Vector3, Quaternion) generate(float t)
    {
        //compute the motion of the object
        t = t % this.motion_time;
        int t1 = (int)(t / this.dt);
        int t2 = t1 + 1;
        //Debug.Log(this.motion.Length);
        float alpha = (t2 * this.dt - t) / this.dt;
        float[] pos1 = this.motion.Skip(t1 * 7).Take(t1 * 7 + 3).ToArray();
        float[] pos2 = this.motion.Skip(t2 * 7).Take(t2 * 7 + 3).ToArray();

        float[] quat1 = this.motion.Skip(t1 * 7 + 3).Take(t1 * 7 + 7).ToArray();
        float[] quat2 = this.motion.Skip(t2 * 7 + 3).Take(t2 * 7 + 7).ToArray();

        Vector3 pos_start = new Vector3(pos1[0], pos1[1], pos1[2]);
        Vector3 pos_end = new Vector3(pos2[0], pos2[1], pos2[2]);

        Quaternion quat_start = new Quaternion(quat1[1], quat1[2], quat1[3], quat1[0]);
        Quaternion quat_end = new Quaternion(quat2[1], quat2[2], quat2[3], quat2[0]);

        Vector3 pos = alpha * pos_start + (1 - alpha) * pos_end;
        Quaternion quat = Quaternion.Slerp(quat_start, quat_end, alpha);

        return (pos, quat);
    }
}

public class ObjectsManager : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> objects_list = new List<GameObject>();
    private List<ObjectMotionGenerator> object_motion_generator_list = new List<ObjectMotionGenerator>();//motion generator for all objects
    public Material[] material_prefab;
    public TextAsset text_asset;
    private float t = 0;
    void Start()
    {

        Debug.Log(text_asset);
        SceneMotion scene_motion = new SceneMotion();//obj2??????????????????
        JsonUtility.FromJsonOverwrite(text_asset.text, scene_motion);//??json_string????????????obj2??
        for(int i=0; i< scene_motion.num_objects;i++)
        {
            Debug.Log(scene_motion.geom_types[i]);
            int geom_type = scene_motion.geom_types[i];
            float[] shape = { scene_motion.shapes[3 * i], scene_motion.shapes[3 * i + 1], scene_motion.shapes[3 * i + 2] };
            int motion_frames = scene_motion.motions.Length / scene_motion.num_objects;
            //float[] motion = new ArraySegment<float>(scene_motion.motions,  i * motion_frames, (i+1) * motion_frames).Array;
            float[] motion = scene_motion.motions.Skip(i * motion_frames).Take(motion_frames).ToArray();
           
            ObjectMotionGenerator object_motion_generator = new ObjectMotionGenerator(motion);
            object_motion_generator_list.Add(object_motion_generator);
            if(geom_type == 0)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.localScale = new Vector3( 2 * shape[0],  2 * shape[0],  2 * shape[0]);
                Renderer rend = sphere.GetComponent<Renderer>();
                rend.material = material_prefab[UnityEngine.Random.Range(0, material_prefab.Length)];
                objects_list.Add(sphere);
            }
            else if(geom_type ==1)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = new Vector3(2 * shape[0], 2 * shape[2], 2 * shape[1]);
                Renderer rend = cube.GetComponent<Renderer>();
                rend.material = material_prefab[UnityEngine.Random.Range(0, material_prefab.Length)];
                objects_list.Add(cube);

            }
            else if(geom_type == 2)
            {
                GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                capsule.transform.localScale = new Vector3(2 * shape[0], shape[2] + shape[0], 2 * shape[0]);
                Renderer rend = capsule.GetComponent<Renderer>();
                rend.material = material_prefab[UnityEngine.Random.Range(0, material_prefab.Length)];
                objects_list.Add(capsule);
            }
            else
            {
            }
        }
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
        for(int i = 0;i<this.object_motion_generator_list.Count; i++)
        {
            (Vector3 pos, Quaternion quat) = this.object_motion_generator_list[i].generate(t);
            this.objects_list[i].transform.position = pos;
            this.objects_list[i].transform.rotation = quat;
        }
        t += Time.deltaTime * 0.1f;
    }
}
