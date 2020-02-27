using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public GameObject panel;

    public static SceneTransition instance = null;

    public Animator sceneTransitionAnim;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        StartCoroutine(StartScene());
    }
    void Start()
    {

    }

    public IEnumerator StartScene()
    {
        yield return new WaitForSecondsRealtime(1.5f);
    }
    public IEnumerator EndScene(string SceneName)
    {
        print("end called");
        sceneTransitionAnim.SetTrigger("end");

        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadScene(SceneName);
    }

    void Update()
    {
        
    }
}
