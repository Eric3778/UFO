using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private List<UfoProperty> used = new List<UfoProperty>();
    private List<UfoProperty> free = new List<UfoProperty>();

    public GameObject Produce(int round)
    {                     
        GameObject ufo_object = null;
        float y = -10f;
        float[][] property = new float[3][]; //每个round中不同种类飞碟的比例
        property[0] = new float[3] { 0.7f, 0.9f, 1f };
        property[1] = new float[3] { 0.5f, 0.8f, 1f };
        property[2] = new float[3] { 0.3f, 0.7f, 1f };
        int temp_result = Random.Range(0, 100);
        float choice_result = (float)temp_result * 0.01f;
        int ufo_kind = 0;
        for (int i = 0; i < 3; i++)
        {
            if (choice_result <= property[round - 1][i]) {
                ufo_kind = i;
                break;
            }
        }
        for (int i=0;i<free.Count;i++)
        {
            if (free[i].gameObject.GetComponent<UfoProperty>().ufo_kind == ufo_kind)
            {
                ufo_object = free[i].gameObject;
                free.Remove(free[i]);
                break;
            }
        }
        if (ufo_object == null)
        {
            ufo_object = Instantiate(Resources.Load<GameObject>("Prefabs/ufo"), new Vector3(0, y, 0), Quaternion.identity);
            ufo_object.GetComponent<UfoProperty>().ufo_kind = ufo_kind;
            float x = Random.Range(-1f, 1f) < 0 ? -1 : 1;
            ufo_object.GetComponent<UfoProperty>().score = ufo_kind + 1;
            ufo_object.GetComponent<UfoProperty>().direction = new Vector3(x, y, 0);
            if (ufo_kind == 0)
            {
                ufo_object.GetComponent<Renderer>().material.color = Color.blue;
                ufo_object.transform.localScale = new Vector3(2f, 0.25f, 2f);
            }
            else if (ufo_kind == 1)
            {
                ufo_object.GetComponent<Renderer>().material.color = Color.yellow;
                ufo_object.transform.localScale = new Vector3(1.5f, 0.25f, 1.5f);
            }
            else
            {
                ufo_object.GetComponent<Renderer>().material.color = Color.red;
                ufo_object.transform.localScale = new Vector3(1f, 0.25f, 1f);
            }
        }
        used.Add(ufo_object.GetComponent<UfoProperty>());
        return ufo_object;
    }
    
    public void Recover(GameObject disk)
    {
        for(int i = 0;i < used.Count; i++)
        {
            if (disk.GetInstanceID() == used[i].gameObject.GetInstanceID())
            {
                used[i].gameObject.SetActive(false);
                free.Add(used[i]);
                used.Remove(used[i]);
                break;
            }
        }
    }
}
