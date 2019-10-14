using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IActionManager
{
    void Fly(GameObject ufo, float angle, float v);
}

public interface ISceneController
{
    void LoadResources();
}

public interface IUserAction
{
    void Hit(Vector3 pos);
    int GetScore();
    int GetLife();
    int GetRound();
    int GetAction();
    void ReStart();
    void SetAction(int type);
}

public enum SSActionEventType : int { Started, Competeted }
public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null);
}

public class SSDirector : System.Object
{
    private static SSDirector _instance;      
    public ISceneController CurrentScenceController { get; set; }
    public static SSDirector GetInstance()
    {
        if (_instance == null)
        {
            _instance = new SSDirector();
        }
        return _instance;
    }
}


public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    public IActionManager action_manager1;
    public IActionManager action_manager2;
    public Factory disk_factory;
    public UserGUI user_gui;
    public CCJudgement judgement;

    private Queue<GameObject> disk_queue = new Queue<GameObject>(); 
    private List<GameObject> disk_notshot = new List<GameObject>(); 
    private int curr_round = 1;                                     
    private float speed = 2f;                                     
    private bool playing_game = false;
    private int action_type = 1;

    void Start ()
    {
        SSDirector director = SSDirector.GetInstance();     
        director.CurrentScenceController = this;             
        disk_factory = Singleton<Factory>.Instance;
        judgement = Singleton<CCJudgement>.Instance;
        action_manager1 = gameObject.AddComponent<CCActionManager>() as IActionManager;
        action_manager2 = gameObject.AddComponent<PhysisActionManager>() as IActionManager;
        user_gui = gameObject.AddComponent<UserGUI>() as UserGUI;
    }
	
	void Update ()
    {
        if (judgement.GetSituation()== SituationType.Loss)
        {
            CancelInvoke("LoadResources");
        }
        if (!playing_game)
        {
            InvokeRepeating("LoadResources", 1f, speed);
            playing_game = true;
        }
        SendDisk();
        if (judgement.Get_round() != curr_round)
        {
            curr_round = judgement.Get_round();
            speed = 2 - 0.5f * curr_round;
            CancelInvoke("LoadResources");
            playing_game = false;
        }
    }

    public void LoadResources()
    {
        disk_queue.Enqueue(disk_factory.Produce(judgement.Get_round())); 
    }

    private void SendDisk()
    {
        float position_x = 16;                       
        if (disk_queue.Count != 0)
        {
            GameObject disk = disk_queue.Dequeue();
            disk_notshot.Add(disk);
            disk.SetActive(true);
            float ran_y = Random.Range(1f, 4f);
            float ran_x = Random.Range(-1f, 1f) < 0 ? -1 : 1;
            disk.GetComponent<UfoProperty>().direction = new Vector3(ran_x, ran_y, 0);
            Vector3 position = new Vector3(-disk.GetComponent<UfoProperty>().direction.x * position_x, ran_y, 0);
            disk.transform.position = position;
            float power = Random.Range(10f, 15f);
            float angle = Random.Range(15f, 28f);
            if(action_type==1)
                action_manager1.Fly(disk, angle, power);
            else
                action_manager2.Fly(disk, angle, power);
        }

        for (int i = 0; i < disk_notshot.Count; i++)
        {
            GameObject temp = disk_notshot[i];
            if ((temp.transform.position.x > 16 || temp.transform.position.x < -16 || temp.transform.position.y <= 0.1) && temp.gameObject.activeSelf == true)
            {
                disk_factory.Recover(disk_notshot[i]);
                disk_notshot.Remove(disk_notshot[i]);
                judgement.Miss();
            }
        }
    }

    public void Hit(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        bool not_hit = false;
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.collider.gameObject.GetComponent<UfoProperty>() != null)
            {
                for (int j = 0; j < disk_notshot.Count; j++)
                {
                    if (hit.collider.gameObject.GetInstanceID() == disk_notshot[j].gameObject.GetInstanceID())
                    {
                        not_hit = true;
                    }
                }
                if(!not_hit)
                {
                    return;
                }
                disk_notshot.Remove(hit.collider.gameObject);
                judgement.Record(hit.collider.gameObject);
                hit.collider.gameObject.transform.position = new Vector3(0, -9, 0);
                disk_factory.Recover(hit.collider.gameObject);
                break;
            }
        }
    }
    public int GetScore()
    {
        return judgement.score;
    }
    public int GetLife()
    {
        return judgement.life;
    }
    public int GetRound()
    {
        return curr_round;
    }
    public int GetAction()
    {
        return action_type;
    }
    public void SetAction(int type)
    {
        action_type = type;
    }
    public void ReStart()
    {
        playing_game = false;
        judgement.Reset();
        curr_round = 1;
        speed = 2f;
    }
}
